using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Helpers
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid senderId, Guid receiverId, string filePath, string emotion, string productImageUrl, string productTitle)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, filePath, emotion, productImageUrl, productTitle);
        }
        public async Task SendTextMessage(Guid senderId, Guid receiverId, string textContent, string productImageUrl, string productTitle, string emotion)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveTextMessage", senderId, textContent, productImageUrl, productTitle, emotion);
        }
    }
}
