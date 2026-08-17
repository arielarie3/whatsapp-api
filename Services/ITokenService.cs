using WhatsappWeb.Api.Models;

namespace WhatsappWeb.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}