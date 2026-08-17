using WhatsappWeb.Api.Models;

namespace WhatsappWeb.Api.Repositories
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task<IEnumerable<Message>> GetChatHistoryAsync(Guid currentUserId, Guid otherUserId);
        Task<bool> SaveChangesAsync();
    }
}