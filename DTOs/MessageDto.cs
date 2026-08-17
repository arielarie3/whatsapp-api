namespace WhatsappWeb.Api.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }

    }
}