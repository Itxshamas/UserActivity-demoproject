using DemoProje.Data;
using DemoProje.Models;
using DemoProje.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoProje.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (userExists != null)
                return BadRequest("User already exists!");

            // Ensure role exists
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role);
            if (role == null)
            {
                role = new Roles
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = dto.Role
                };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }

            // Create new user and assign role directly
            var newUser = new Users
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), 
                Address = dto.Address,
                MobileNum = dto.MobileNum,
                RoleId = role.Id 
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully", role = dto.Role });
        }
    }
}
