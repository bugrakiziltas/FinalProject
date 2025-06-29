using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Helpers
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid senderId, Guid receiverId, string filePath, string voiceEmotion, string textEmotion,string productImageUrl, string productTitle, double voiceConfidenceRate, double textConfidenceRate)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, filePath, voiceEmotion, textEmotion, productImageUrl, productTitle, voiceConfidenceRate, textConfidenceRate);
        }
        public async Task SendTextMessage(Guid senderId, Guid receiverId, string textContent, string productImageUrl, string productTitle, string emotion, double confidenceRate)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveTextMessage", senderId, textContent, productImageUrl, productTitle, emotion, confidenceRate);
        }
    }
}
