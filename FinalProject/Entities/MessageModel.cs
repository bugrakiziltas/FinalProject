using FiinalProject.Entities;
using FinalProject.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class MessageModel : EntityBase<Guid>
    {
        public string SenderId { get; set; }
        public IdentityUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public IdentityUser Receiver { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public MessageTypeEnum MessageType { get; set; }
        public string? AudioFilePath { get; set; }
        public string? TextContent { get; set; }
        public DateTime Created { get; set; }
        public string? Emotion { get; set; } = null;
    }
}
