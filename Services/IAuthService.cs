using WhatsappWeb.Api.DTOs;

namespace WhatsappWeb.Api.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<UserResponseDto?> LoginAsync(LoginDto loginDto);
    }
}