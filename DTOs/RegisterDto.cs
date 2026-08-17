using System.ComponentModel.DataAnnotations;

namespace WhatsappWeb.Api.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "מספר טלפון הוא שדה חובה")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "שם תצוגה הוא שדה חובה")]
        public string DisplayName { get; set; } = string.Empty;

        [Required(ErrorMessage = "סיסמה היא שדה חובה")]
        [MinLength(6, ErrorMessage = "הסיסמה חייבת להכיל לפחות 6 תווים")]
        public string Password { get; set; } = string.Empty;
    }
}