namespace WhatsappWeb.Api.DTOs
{
    public class MessageResponseDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
    }
}