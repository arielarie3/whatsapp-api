using WhatsappWeb.Api.DTOs;

namespace WhatsappWeb.Api.Services
{
    public interface IMessageService
    {
        Task<MessageResponseDto?> SendTextMessageAsync(Guid senderId, CreateMessageDto createDto);
        Task<MessageResponseDto?> SendFileAsync(Guid senderId, FileUploadDto fileDto);
        Task<IEnumerable<MessageResponseDto>> GetChatHistoryAsync(Guid currentUserId, Guid otherUserId);
    }
}