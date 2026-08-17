using System.ComponentModel.DataAnnotations;

namespace WhatsappWeb.Api.DTOs
{
    public class SendMessageDto
    {
        [Required]
        public Guid ReceiverId { get; set; }

        [Required(ErrorMessage = "תוכן ההודעה אינו יכול להיות ריק")]
        public string Content { get; set; } = string.Empty;
    }
}