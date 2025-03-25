using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Helpers
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid senderId, Guid receiverId, string filePath)
        {
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, filePath);
        }
    }
}
