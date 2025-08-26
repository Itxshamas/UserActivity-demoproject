using DemoProje.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoProje.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var admin = await _context.Users.Include(u => u.Role)
                                            .FirstOrDefaultAsync(u => u.Id == userId);
            if (admin == null) return NotFound("Admin not found");

            return Ok(new
            {
                Message = $"Welcome Admin! This is your dashboard.",
                UserId = admin.Id,
                Email = admin.Email,
                Role = role
            });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var adminId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var admin = await _context.Users.Include(u => u.Role)
                                            .FirstOrDefaultAsync(u => u.Id == adminId);
            if (admin == null) return NotFound("Admin not found");

            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return Ok(new
            {
                Message = $"Admin authorized as {role}",
                AdminInfo = new { admin.Id, admin.Email, admin.Role?.Name },
                Users = users
            });
        }
    }
}
