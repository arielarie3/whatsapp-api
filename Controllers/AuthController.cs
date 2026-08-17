using Microsoft.AspNetCore.Mvc;
using WhatsappWeb.Api.DTOs;
using WhatsappWeb.Api.Services;

namespace WhatsappWeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (result == null)
            {
                return BadRequest("מספר הטלפון כבר קיים במערכת");
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized("מספר טלפון או סיסמה שגויים.");
            }

            return Ok(result);
        }
    }
}