namespace WhatsappWeb.Api.DTOs
{
    public class CreateMessageDto
    {
        public Guid RecipientId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}