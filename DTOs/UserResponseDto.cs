namespace WhatsappWeb.Api.DTOs
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? About { get; set; }
        public bool IsOnline { get; set; } 
        public string Token { get; set; } = string.Empty;
    }
}