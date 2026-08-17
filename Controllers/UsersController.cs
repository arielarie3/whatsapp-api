using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappWeb.Api.DTOs;
using WhatsappWeb.Api.Hubs;
using WhatsappWeb.Api.Repositories;

namespace WhatsappWeb.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var users = await _userRepository.GetAllAsync();

            var userDtos = users
                .Where(u => u.Id.ToString() != currentUserId)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    PhoneNumber = u.PhoneNumber,
                    DisplayName = u.DisplayName,
                    ProfileImageUrl = u.ProfileImageUrl,
                    About = u.About,
                    //  בדיקה האם המשתמש מחובר כרגע ב-SignalR
                    IsOnline = !string.IsNullOrEmpty(ChatHub.GetConnectionIdForUser(u.Id.ToString()))
                });

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("המשתמש לא נמצא");
            }

            return Ok(new UserResponseDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                DisplayName = user.DisplayName,
                ProfileImageUrl = user.ProfileImageUrl,
                About = user.About,
                IsOnline = !string.IsNullOrEmpty(ChatHub.GetConnectionIdForUser(user.Id.ToString()))
            });
        }
    }
}