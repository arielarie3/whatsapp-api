using BCrypt.Net;
using WhatsappWeb.Api.DTOs;
using WhatsappWeb.Api.Models;
using WhatsappWeb.Api.Repositories;

namespace WhatsappWeb.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<UserResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            // 1. בדיקה אם המשתמש כבר קיים
            if (await _userRepository.UserExistsAsync(registerDto.PhoneNumber))
            {
                return null;
            }

            // 2. הצפנת הסיסמה ב-BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // 3. יצירת המשתמש
            var user = new User
            {
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName,
                PasswordHash = passwordHash
            };

            // 4. שמירה במסד הנתונים
            await _userRepository.AddAsync(user);
            if (!await _userRepository.SaveChangesAsync())
            {
                return null;
            }

            // 5. החזרת תשובה נקייה כולל הטוקן
            return new UserResponseDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                DisplayName = user.DisplayName,
                ProfileImageUrl = user.ProfileImageUrl,
                About = user.About,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<UserResponseDto?> LoginAsync(LoginDto loginDto)
        {
            // 1. שליפת המשתמש לפי מספר טלפון
            var user = await _userRepository.GetByPhoneNumberAsync(loginDto.PhoneNumber);
            if (user == null)
            {
                return null;
            }

            // 2. אימות הסיסמה
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return null;
            }

            // 3. החזרת ה-DTO עם הטוקן
            return new UserResponseDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                DisplayName = user.DisplayName,
                ProfileImageUrl = user.ProfileImageUrl,
                About = user.About,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}