using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.Users.FindAsync(u => u.Username == loginDto.Username, cancellationToken);
                var user = users.FirstOrDefault();

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Failed login attempt for username: {Username}", loginDto.Username);
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                user.LastLoginAt = DateTime.UtcNow;
                await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var token = _tokenService.GenerateToken(user);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                _logger.LogInformation("User {Username} logged in successfully", user.Username);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", loginDto.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var usernameExists = await _unitOfWork.Users.ExistsAsync(u => u.Username == registerDto.Username, cancellationToken);
                if (usernameExists)
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                var emailExists = await _unitOfWork.Users.ExistsAsync(u => u.Email == registerDto.Email, cancellationToken);
                if (emailExists)
                {
                    return BadRequest(new { message = "Email already exists" });
                }

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Role = "User",
                    CreatedBy = "System"
                };

                await _unitOfWork.Users.AddAsync(user, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                _logger.LogInformation("New user registered: {Username}", user.Username);

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", registerDto.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
