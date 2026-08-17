using Microsoft.EntityFrameworkCore;
using WhatsappWeb.Api.Data;
using WhatsappWeb.Api.Models;

namespace WhatsappWeb.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UserExistsAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}