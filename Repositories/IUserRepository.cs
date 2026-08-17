using WhatsappWeb.Api.Models;

namespace WhatsappWeb.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> UserExistsAsync(string phoneNumber);
        Task AddAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}