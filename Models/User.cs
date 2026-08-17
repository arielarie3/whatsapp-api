using System.ComponentModel.DataAnnotations;

namespace WhatsappWeb.Api.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }

        [MaxLength(150)]
        public string? About { get; set; } = "Hey there! I am using WhatsApp.";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // קשרי גומלין (Navigation Properties)
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}