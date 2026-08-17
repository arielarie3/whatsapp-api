// Services/IFileService.cs
namespace WhatsappWeb.Api.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}