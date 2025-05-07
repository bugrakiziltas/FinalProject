namespace FinalProject.Dtos.Chat
{
    public class ChatViewModel
    {
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public DateTime Created { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string? Emotion { get; set; }
    }
}
