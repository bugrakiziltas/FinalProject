using FinalProject.Data;
using FinalProject.Dtos.Chat;
using FinalProject.Entities;
using FinalProject.Helpers;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Stripe;
using System.Diagnostics;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IIdentityService _identityService;
        public ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext, IIdentityService identityService)
        {
            _context = context;
            _hubContext = hubContext;
            _identityService = identityService;
        }

        [HttpGet]
        [Route("/chat")]
        public async Task<IActionResult> Chat(Guid chatWithUserId, Guid productId)
        {   
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var messages = await _context.VoiceMessageModels.Include(x => x.Product).Include(x=>x.Sender).Include(x=>x.Receiver)
                .Where(m => (m.SenderId == userId.ToString() && m.ReceiverId == chatWithUserId.ToString()) && m.ProductId==productId ||
                            (m.SenderId == chatWithUserId.ToString() && m.ReceiverId == userId.ToString() && m.ProductId == productId))
                .OrderBy(m => m.Created)
                .ToListAsync();

            ViewBag.ChatWithUserId = chatWithUserId;
            ViewBag.ProductId = productId;
            return View(messages);
        }



        [HttpPost]
        [Route("/chat/upload")]
        public async Task<IActionResult> UploadVoiceMessage(IFormFile audioFile, Guid receiverId, Guid productId)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("Invalid audio file.");
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var senderId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var tempWebmFileName = $"{Guid.NewGuid()}.webm";
            var tempWebmFilePath = Path.Combine("wwwroot/voice_messages", tempWebmFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(tempWebmFilePath)!);

            using (var stream = new FileStream(tempWebmFilePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }


            var wavFileName = $"{Guid.NewGuid()}.wav";
            var wavFilePath = Path.Combine("wwwroot/voice_messages", wavFileName);

            var ffmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe";

            var arguments = $"-i \"{tempWebmFilePath}\" \"{wavFilePath}\"";

            var processInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    return StatusCode(500, $"Audio conversion failed: {error}");
                }
            }

            System.IO.File.Delete(tempWebmFilePath);

            var voiceMessage = new VoiceMessageModel
            {
                SenderId = senderId,
                ReceiverId = receiverId.ToString(),
                AudioFilePath = $"/voice_messages/{wavFileName}",
                Created = DateTime.UtcNow,
                ProductId = productId
            };
            // If sender is a customer, run Speech Emotion Recognition
            if (await _identityService.IsInRoleAsync(senderId, SD.Role_Cust))
            {
                var client = new SpeechEmotionRecognition("http://127.0.0.1:5001");

                if (await client.IsApiHealthyAsync())
                {
                    Console.WriteLine("API is running and healthy!");
                    // you can debug here to see emotion
                    var emotionResult = await client.RecognizeEmotionAsync(wavFilePath);
                    voiceMessage.Emotion = emotionResult;
                }
                else
                {
                    Console.WriteLine("API is not available. Please check if the Flask service is running.");
                }
            }
            _context.VoiceMessageModels.Add(voiceMessage);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", senderId, voiceMessage.AudioFilePath);


            if (await _identityService.IsInRoleAsync(senderId, SD.Role_Cust))
            {
                return RedirectToAction("Chat", new { chatWithUserId = receiverId, productId = productId });
            }

            return RedirectToAction("ChatDetail", new { userId = receiverId, productId = productId });
        }


        [Authorize(Roles = "CRM")]
        public async Task<IActionResult> CRMChats()
        {
            var crmUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (crmUserId == null)
            {
                return Unauthorized();
            }

            var chats = await _context.VoiceMessageModels
        .Include(x => x.Sender)
        .Include(x => x.Receiver)
        .Include(x => x.Product)
        .Where(m => m.ReceiverId == crmUserId || m.SenderId == crmUserId)
        .GroupBy(m => m.SenderId == crmUserId ? m.ReceiverId : m.SenderId)
        .Select(g => new ChatViewModel
        {
            UserName = g.OrderByDescending(m => m.Created).First().Sender.UserName,
            ProductName = g.OrderByDescending(m => m.Created).First().Product.Name,
            ProductId = g.OrderByDescending(m => m.Created).First().Product.Id.ToString(),
            Created = g.OrderByDescending(m => m.Created).First().Created,
            SenderId = g.OrderByDescending(m => m.Created).First().SenderId,
            ReceiverId = g.OrderByDescending(m => m.Created).First().ReceiverId
        })
        .ToListAsync();

            return View(chats);
        }
        [Authorize(Roles = "CRM")]
        public async Task<IActionResult> ChatDetail(string userId, string productId)
        {
            var crmUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.CrmUserId = crmUserId;
            ViewBag.CustomerId = userId;
            ViewBag.productId = productId;
            if (crmUserId == null)
            {
                return Unauthorized();
            }
            var messages = await _context.VoiceMessageModels.Include(x=>x.Product)
            .Where(m => (m.SenderId == userId && m.ReceiverId == crmUserId && m.ProductId.ToString() == productId) ||
            (m.SenderId == crmUserId && m.ReceiverId == userId && m.ProductId.ToString() == productId)).Include(m => m.Sender).Include(m=>m.Receiver)
            .OrderBy(m => m.Created)
            .ToListAsync();

            return View(messages);
        }

    }
}
