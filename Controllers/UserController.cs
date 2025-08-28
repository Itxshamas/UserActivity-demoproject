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
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var user = await _context.Users.Include(u => u.Role)
                                           .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                Message = "Welcome User! This is your dashboard.",
                UserInfo = new
                {
                    user.Id,
                    user.Email,
                    Role = role
                }
            });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var user = await _context.Users.Include(u => u.Role)
                                           .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                Profile = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Address,
                    user.MobileNum,
                    Role = role
                }
            });
        }
    }
}
