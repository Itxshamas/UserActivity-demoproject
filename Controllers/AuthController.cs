using DemoProje.Models;
using DemoProje.Models.DTOs;
using DemoProje.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoProje.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // SIGNUP
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var user = await _repo.RegisterAsync(dto);
                return Ok(new { Message = "User registered successfully", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // SIGNIN
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _repo.LoginAsync(dto);
            if (user == null)
                return Unauthorized("Invalid email or password");

            // JWT Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                Message = "Login successful",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = user.Role?.Name ?? "User",
                UserId = user.Id,
                Email = user.Email
            });
        }

        // LOGOUT (for JWT: client deletes token, but we provide endpoint for consistency)
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _repo.LogoutAsync(userId);

            return Ok(new { Message = "Logout successful (client should discard JWT token)" });
        }
    }
}
