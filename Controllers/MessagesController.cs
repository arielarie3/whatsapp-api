using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappWeb.Api.DTOs;
using WhatsappWeb.Api.Services;

namespace WhatsappWeb.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // שליחת הודעת טקסט
        [HttpPost]
        public async Task<ActionResult<MessageResponseDto>> SendMessage(CreateMessageDto createDto)
        {
            var senderId = GetCurrentUserId();
            if (senderId == null) return Unauthorized();

            var result = await _messageService.SendTextMessageAsync(senderId.Value, createDto);
            if (result == null) return BadRequest("הנמען לא נמצא או ששליחת ההודעה נכשלה.");

            return Ok(result);
        }

        // שליחת קובץ בצ'אט
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<MessageResponseDto>> UploadFile([FromForm] FileUploadDto dto)
        {
            var senderId = GetCurrentUserId();
            if (senderId == null) return Unauthorized();

            try
            {
                var result = await _messageService.SendFileAsync(senderId.Value, dto);
                if (result == null) return BadRequest("הנמען לא נמצא או ששמירת הקובץ נכשלה.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // שליפת היסטוריית שיחה
        [HttpGet("chat/{otherUserId}")]
        public async Task<ActionResult<IEnumerable<MessageResponseDto>>> GetChatHistory(Guid otherUserId)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null) return Unauthorized();

            var messages = await _messageService.GetChatHistoryAsync(currentUserId.Value, otherUserId);
            return Ok(messages);
        }

        private Guid? GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }
}