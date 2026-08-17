// Services/FileService.cs
namespace WhatsappWeb.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("קובץ לא תקין.");
            }

            // 1. יצירת נתיב התיקייה
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // 2. יצירת שם ייחודי ושמירה לדיסק
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 3. בניית ה-URL הציבורי
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";

            return $"{baseUrl}/uploads/{uniqueFileName}";
        }
    }
}