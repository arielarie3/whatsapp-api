using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WhatsappWeb.Api.DTOs
{
    public class FileUploadDto
    {
        [Required(ErrorMessage = "חובה לצרף קובץ")]
        public IFormFile File { get; set; } = null!;

        [Required(ErrorMessage = "מזהה הנמען הוא שדה חובה")]
        public Guid RecipientId { get; set; }
    }
}