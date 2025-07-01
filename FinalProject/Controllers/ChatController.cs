using FinalProject.Data;
using FinalProject.Dtos.Chat;
using FinalProject.Entities;
using FinalProject.Helpers;
using FinalProject.Interfaces;
using FinalProject.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using Stripe;
using Stripe.Climate;
using System.Diagnostics;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IIdentityService _identityService;
        private readonly IProductService _productService;
        public ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext, IIdentityService identityService, IProductService productService)
        {
            _context = context;
            _hubContext = hubContext;
            _identityService = identityService;
            _productService = productService;
        }

        [HttpGet]
        [Route("/chat")]
        public async Task<IActionResult> Chat(Guid chatWithUserId, Guid productId, string imageUrl, string productName)
        {   
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var user = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
            var userId = user.Id.ToString();

            var messages = await _context.MessageModels.Include(x => x.Product).Include(x=>x.Sender).Include(x=>x.Receiver)
                .Where(m => (m.SenderId == userId.ToString() && m.ReceiverId == chatWithUserId.ToString()) && m.ProductId==productId ||
                            (m.SenderId == chatWithUserId.ToString() && m.ReceiverId == userId.ToString() && m.ProductId == productId))
                .OrderBy(m => m.Created)
                .ToListAsync();

            ViewBag.ChatWithUserId = chatWithUserId;
            ViewBag.CrmUserId=chatWithUserId.ToString();
            ViewBag.CustomerId=userId;
            ViewBag.productId = productId.ToString();
            ViewBag.ProductId = productId.ToString();
            ViewBag.ProductTitle = productName;
            ViewBag.ProductImageUrl= "/Images/" + imageUrl;
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
            var sender = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
            var senderId = sender.Id.ToString();
            var product = await _productService.GetProductByIdAsync(productId);

            var tempWebmFileName = $"{Guid.NewGuid()}.webm";
            var tempWebmFilePath = Path.Combine("wwwroot/voice_messages", tempWebmFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(tempWebmFilePath)!);

            using (var stream = new FileStream(tempWebmFilePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }


            var wavFileName = $"{Guid.NewGuid()}.wav";
            var wavFilePath = Path.Combine("wwwroot/voice_messages", wavFileName);

            var ffmpegPath = @"C:\ffmpeg\ffmpeg-7.1.1-essentials_build\bin\ffmpeg.exe";

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

            var voiceMessage = new MessageModel
            {
                SenderId = senderId,
                ReceiverId = receiverId.ToString(),
                MessageType = MessageTypeEnum.Voice,
                AudioFilePath = $"/voice_messages/{wavFileName}",
                Created = DateTime.Now,
                ProductId = productId
            };
            
            if (await _identityService.IsInRoleAsync(senderId, SD.Role_Cust))
            {
                var client = new EmotionAnalysis("http://127.0.0.1:5001");

                if (await client.IsApiHealthyAsync())
                {
                    var voiceEmotion = await client.RecognizeEmotionAsync(wavFilePath);
                    var TextOfVoice = await client.TranscribeAudioAsync(wavFilePath);
                    voiceMessage.TextContent= TextOfVoice;
                    var textEmotion = await client.RecognizeTextEmotionAsync(TextOfVoice);
                    bool isFirstMessageOfCustomer = IsFirstMessageFromCustomerForProduct(voiceMessage.SenderId, product.Id);
                    switch (textEmotion.predictedEmotion)
                    {
                        case 0:
                            voiceMessage.TextEmotion = "sad";
                            if (isFirstMessageOfCustomer)
                            {
                                if (product.problematicComments >= 0)
                                {
                                    product.isProblematic = true;
                                }
                                else
                                {
                                    product.problematicComments++;
                                }
                            }
                            break;
                        case 1:
                            voiceMessage.TextEmotion = "joy";
                            break;
                        case 2:
                            voiceMessage.TextEmotion = "love";
                            break;
                        case 3:
                            voiceMessage.TextEmotion = "angry";
                            if (isFirstMessageOfCustomer)
                            {
                                if (product.problematicComments >= 0)
                                {
                                    product.isProblematic = true;
                                }
                                else
                                {
                                    product.problematicComments++;
                                }
                            }
                            break;
                        case 4:
                            voiceMessage.TextEmotion = "fear";
                            if (isFirstMessageOfCustomer)
                            {
                                if (product.problematicComments >= 0)
                                {
                                    product.isProblematic = true;
                                }
                                else
                                {
                                    product.problematicComments++;
                                }
                            }
                            break;
                        case 5:
                            voiceMessage.TextEmotion = "surprise";
                            break;
                    }
                if (new[] { "fear", "disgust", "anger", "angry", "sad" }.Contains(voiceEmotion.predictedEmotion))
                    {
                        if (isFirstMessageOfCustomer)
                        {
                            if (product.problematicComments >= 0)
                            {
                                product.isProblematic = true;
                            }
                            else
                            {
                                product.problematicComments++;
                            }
                        }
                    }
                    voiceMessage.VoiceEmotion = voiceEmotion.predictedEmotion;
                    voiceMessage.voiceConfidenceRate=voiceEmotion.confidenceRate;
                    voiceMessage.textConfidenceRate=textEmotion.confidenceRate;
                }
                else
                {
                    Console.WriteLine("API is not available. Please check if the Flask service is running.");
                }
            }
            _context.MessageModels.Add(voiceMessage);
            await _context.SaveChangesAsync();
            if (await _identityService.IsInRoleAsync(senderId, SD.Role_Cust))
            {
                var conversationId = $"{receiverId.ToString()}_{senderId}_{productId.ToString()}";
                await _hubContext.Clients.Group(conversationId)
                    .SendAsync("ReceiveMessage", senderId, voiceMessage.AudioFilePath, voiceMessage.VoiceEmotion, voiceMessage.TextEmotion, "/Images/" + product.ImageUrl, product.Name, voiceMessage.voiceConfidenceRate, voiceMessage.textConfidenceRate);
                return Ok(new { success = true });
            }
            var conversationIdx = $"{senderId}_{receiverId.ToString()}_{productId.ToString()}";
            await _hubContext.Clients.Group(conversationIdx)
                   .SendAsync("ReceiveMessage", senderId, voiceMessage.AudioFilePath, "", "", "/Images/" + product.ImageUrl, product.Name);
            return Ok(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> SendTextMessage(string textContent, Guid receiverId, Guid productId)
        {
            if (string.IsNullOrWhiteSpace(textContent))
            {
                return BadRequest("Message content cannot be empty.");
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var sender = await _identityService.GetUserByEmailAsync(claimsIdentity.Name);
            var senderId = sender.Id.ToString();
            var product = await _productService.GetProductByIdAsync(productId);

            var message = new MessageModel
            {
                SenderId = senderId,
                ReceiverId = receiverId.ToString(),
                MessageType = MessageTypeEnum.Text,
                TextContent = textContent,
                Created = DateTime.Now,
                ProductId = productId
            };
            if (await _identityService.IsInRoleAsync(senderId, SD.Role_Cust))
            {
                var client = new EmotionAnalysis("http://127.0.0.1:5001");

                if (await client.IsApiHealthyAsync())
                {
                    var emotionResult = await client.RecognizeTextEmotionAsync(textContent);
                    message.textConfidenceRate = emotionResult.confidenceRate;
                    bool isFirstMessageOfCustomer = IsFirstMessageFromCustomerForProduct(message.SenderId, product.Id);
                    switch (emotionResult.predictedEmotion)
                    {
                        case 0:
                            message.TextEmotion = "sad";
                            if (isFirstMessageOfCustomer)
                            {
                                if (product.problematicComments >= 0)
                                {
                                    product.isProblematic = true;
                                }
                                else
                                {
                                    product.problematicComments++;
                                }
                            }
                            break;
                        case 1:
                            message.TextEmotion = "joy";
                            break;
                        case 2:
                            message.TextEmotion = "love";
                            break;
                        case 3:
                            message.TextEmotion = "angry";
                            if (isFirstMessageOfCustomer)
                            {
                                if (product.problematicComments >= 0)
                                {
                                    product.isProblematic = true;
                                }
                                else
                                {
                                    product.problematicComments++;
                                }
                            }
                            break;
                        case 4:
                            message.TextEmotion = "fear";
                            if (isFirstMessageOfCustomer)
                            {
                                if (product.problematicComments >= 0)
                                {
                                    product.isProblematic = true;
                                }
                                else
                                {
                                    product.problematicComments++;
                                }
                            }
                            break;
                        case 5:
                            message.TextEmotion = "surprise";
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("API is not available. Please check if the Flask service is running.");
                }
            }
            _context.MessageModels.Add(message);
            await _context.SaveChangesAsync();
            if (message.textConfidenceRate != null)
            {
                var conversationId = $"{receiverId.ToString()}_{senderId}_{productId.ToString()}";//9fd crm e75 cust f2 pro
                await _hubContext.Clients.Group(conversationId)
       .SendAsync("ReceiveTextMessage", senderId, message.TextContent, "/Images/" + product.ImageUrl, product.Name, message.TextEmotion, message.textConfidenceRate);

                return Ok(new { success = true });
            }
            var conversationIdx = $"{senderId}_{receiverId.ToString()}_{productId.ToString()}";
            await _hubContext.Clients.Group(conversationIdx)
        .SendAsync("ReceiveTextMessage", senderId, message.TextContent, "/Images/" + product.ImageUrl, product.Name, message.TextEmotion);

            return Ok(new { success = true });
        }

        [Authorize(Roles = "CRM")]
        public async Task<IActionResult> CRMChats()
        {
            var crmUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (crmUserId == null)
            {
                return Unauthorized();
            }

            var messages = await _context.MessageModels
            .Include(x => x.Sender)
            .Include(x => x.Receiver)
            .Include(x => x.Product)
            .Where(m => m.ReceiverId == crmUserId || m.SenderId == crmUserId)
            .ToListAsync();

            var chats = messages
            .GroupBy(m => new
            {
                OtherUserId = m.SenderId == crmUserId ? m.ReceiverId : m.SenderId,
                m.ProductId
            })
            .Select(g =>
            {
                var custId = g.OrderByDescending(m => m.Created).Last().SenderId;
                var lastMessage = g.Where(x=>x.SenderId == custId).OrderByDescending(m => m.Created).First();
                // Little Chat Report
                var allEmotions = g.Where(x => x.SenderId == custId).OrderByDescending(x => x.Created).SelectMany(x =>
                {
                    var emotions = new List<string>();
                    if (x.VoiceEmotion != null)
                    {
                        emotions.Add(x.TextEmotion);
                        emotions.Add(x.VoiceEmotion);
                    }
                    else
                    {
                        emotions.Add(x.TextEmotion);
                    }

                    return emotions;
                })
                      .ToArray();
                var lastThreeEmotions = new List<string>();
                for(int i=0; i<3; i++)
                {
                    if (i >= allEmotions.Count())
                    {
                        lastThreeEmotions.Add("unknown");
                    }
                    else
                    {
                        lastThreeEmotions.Add(allEmotions[i]);
                    }
                }
                return new ChatViewModel
                {
                    UserName = lastMessage.Sender.UserName,
                    ProductName = lastMessage.Product.Name,
                    ProductId = lastMessage.Product.Id.ToString(),
                    Created = lastMessage.Created,
                    SenderId = g.OrderByDescending(m => m.Created).Last().SenderId,
                    ReceiverId = g.OrderByDescending(m => m.Created).Last().ReceiverId,
                    Emotion = lastMessage.TextEmotion,
                    LastEmotions = lastThreeEmotions,
                };
            })
            .OrderByDescending(c => new[] { "angry", "sad", "disgust", "fear" }.Contains(c.Emotion?.ToLower()))
            .ThenByDescending(c => c.Created)
            .ToList();

            return View(chats);

        }
        [Authorize(Roles = "CRM")]
        public async Task<IActionResult> ChatDetail(string userId, string productId)
        {
            var crmUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.CrmUserId = crmUserId;//9fd crm e75 cust f2 pro
            ViewBag.CustomerId = userId;
            ViewBag.productId = productId;
            if (crmUserId == null)
            {
                return Unauthorized();
            }
            var messages = await _context.MessageModels.Include(x=>x.Product)
            .Where(m => (m.SenderId == userId && m.ReceiverId == crmUserId && m.ProductId.ToString() == productId) ||
            (m.SenderId == crmUserId && m.ReceiverId == userId && m.ProductId.ToString() == productId)).Include(m => m.Sender).Include(m=>m.Receiver)
            .OrderBy(m => m.Created)
            .ToListAsync();

            //Checking if the product is problematic
            ViewBag.IsProblematic=messages.First().Product.isProblematic;

            //Deciding the Customer Type
            var allMessagesSentByCust = await _context.MessageModels.Include(x=>x.Sender).Where(x=>x.SenderId==userId).ToListAsync();
            var negativeMessages = allMessagesSentByCust.Where(x => new[] { "fear", "disgust", "anger", "angry", "sad" }.Contains(x.TextEmotion?.ToLower())).Count();
            var positiveMessages = allMessagesSentByCust.Where(x => new[] { "happy", "joy", "neutral", "love", "surprise", "neutral","ps" }.Contains(x.TextEmotion?.ToLower())).Count();
            if (positiveMessages >= negativeMessages) {
                ViewBag.UserType = "pleased"; 
            }
            else
            {
                ViewBag.UserType = "Stressful";
            }
            ViewBag.ProductImageUrl = messages.First().Product.ImageUrl;
            ViewBag.ProductTitle = messages.First().Product.Name;
            return View(messages);
        }

        private bool IsFirstMessageFromCustomerForProduct(string customerId, Guid productId)
        {
            int messageCount = _context.MessageModels
                .Count(m => m.SenderId == customerId && m.ProductId == productId);
            return messageCount == 0;
        }
    }
}
