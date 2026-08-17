// Models/Message.cs
using System.ComponentModel.DataAnnotations;

namespace WhatsappWeb.Api.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SenderId { get; set; }
        public User? Sender { get; set; }
        public Guid RecipientId { get; set; }
        public User? Recipient { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        //  שדות לקבצים
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
    }
}