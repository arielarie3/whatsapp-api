using Microsoft.AspNetCore.SignalR;
using WhatsappWeb.Api.DTOs;
using WhatsappWeb.Api.Hubs;
using WhatsappWeb.Api.Models;
using WhatsappWeb.Api.Repositories;

namespace WhatsappWeb.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageService(
            IMessageRepository messageRepository,
            IUserRepository userRepository,
            IFileService fileService,
            IHubContext<ChatHub> hubContext)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _fileService = fileService;
            _hubContext = hubContext;
        }

        public async Task<MessageResponseDto?> SendTextMessageAsync(Guid senderId, CreateMessageDto createDto)
        {
            var recipient = await _userRepository.GetByIdAsync(createDto.RecipientId);
            if (recipient == null) return null;

            var message = new Message
            {
                SenderId = senderId,
                RecipientId = createDto.RecipientId,
                Content = createDto.Content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(message);
            if (!await _messageRepository.SaveChangesAsync()) return null;

            var responseDto = MapToResponseDto(message);

            // שידור בזמן אמת ב-SignalR
            await SendSignalRNotificationAsync(createDto.RecipientId, responseDto);

            return responseDto;
        }

        public async Task<MessageResponseDto?> SendFileAsync(Guid senderId, FileUploadDto fileDto)
        {
            var recipient = await _userRepository.GetByIdAsync(fileDto.RecipientId);
            if (recipient == null) return null;

            // שמירת הקובץ על הדיסק לקבלת ה-URL
            string fileUrl = await _fileService.SaveFileAsync(fileDto.File);

            var message = new Message
            {
                SenderId = senderId,
                RecipientId = fileDto.RecipientId,
                Content = fileDto.File.FileName,
                FileUrl = fileUrl,
                FileName = fileDto.File.FileName,
                FileType = fileDto.File.ContentType,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(message);
            if (!await _messageRepository.SaveChangesAsync()) return null;

            var responseDto = MapToResponseDto(message);

            // שידור בזמן אמת ב-SignalR
            await SendSignalRNotificationAsync(fileDto.RecipientId, responseDto);

            return responseDto;
        }

        public async Task<IEnumerable<MessageResponseDto>> GetChatHistoryAsync(Guid currentUserId, Guid otherUserId)
        {
            var messages = await _messageRepository.GetChatHistoryAsync(currentUserId, otherUserId);
            return messages.Select(MapToResponseDto);
        }

        private static MessageResponseDto MapToResponseDto(Message message)
        {
            return new MessageResponseDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                RecipientId = message.RecipientId,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead,
                FileUrl = message.FileUrl,
                FileName = message.FileName,
                FileType = message.FileType
            };
        }

        private async Task SendSignalRNotificationAsync(Guid recipientId, MessageResponseDto responseDto)
        {
            var recipientConnectionId = ChatHub.GetConnectionIdForUser(recipientId.ToString());
            if (!string.IsNullOrEmpty(recipientConnectionId))
            {
                await _hubContext.Clients.Client(recipientConnectionId)
                    .SendAsync("ReceiveMessage", responseDto);
            }
        }
    }
}