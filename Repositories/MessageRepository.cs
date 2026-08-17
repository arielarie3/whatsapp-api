using Microsoft.EntityFrameworkCore;
using WhatsappWeb.Api.Data;
using WhatsappWeb.Api.Models;

namespace WhatsappWeb.Api.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task<IEnumerable<Message>> GetChatHistoryAsync(Guid currentUserId, Guid otherUserId)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.RecipientId == otherUserId) ||
                            (m.SenderId == otherUserId && m.RecipientId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}