using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Helpers
{
    public class ChatHub : Hub
    {
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task SendMessage(string conversationId, Guid senderId, Guid receiverId, string filePath, string voiceEmotion, string textEmotion, string productImageUrl, string productTitle, double voiceConfidenceRate, double textConfidenceRate)
        {
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", senderId, filePath, voiceEmotion, textEmotion, productImageUrl, productTitle, voiceConfidenceRate, textConfidenceRate);
        }

        public async Task SendTextMessage(string conversationId, Guid senderId, Guid receiverId, string textContent, string productImageUrl, string productTitle, string emotion, double confidenceRate)
        {
            await Clients.Group(conversationId).SendAsync("ReceiveTextMessage", senderId, textContent, productImageUrl, productTitle, emotion, confidenceRate);
        }
    }
}
