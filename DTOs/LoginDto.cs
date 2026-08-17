using System.ComponentModel.DataAnnotations;

namespace WhatsappWeb.Api.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "מספר טלפון הוא שדה חובה")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "סיסמה היא שדה חובה")]
        public string Password { get; set; } = string.Empty;
    }
}