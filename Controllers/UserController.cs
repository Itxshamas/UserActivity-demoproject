using DemoProje.Models;
using DemoProje.Models.DTOs;
using DemoProje.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoProje.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        // User dashboard
        [HttpGet("user-dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _repo.GetDashboardAsync(userId);

            if (user == null) return NotFound("User not found");

            return Ok(new { Message = "Welcome User! This is your dashboard.", UserInfo = user });
        }

        // User Profile 
        [HttpGet("user-profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _repo.GetUserByIdAsync(userId);

            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        // Update profile
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedUser = await _repo.UpdateUserAsync(userId, dto);

            if (updatedUser == null) return NotFound("User not found");

            return Ok(new { Message = "Profile updated successfully", User = updatedUser });
        }
    }
}
