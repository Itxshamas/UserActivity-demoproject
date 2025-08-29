using DemoProje.Data;
using DemoProje.Models;
using DemoProje.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoProje.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ActivityController(ApplicationDbContext context) => _context = context;

        // CREATE ACTIVITY (User Only) 
        [Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ActivityDto dto)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var activity = new Activities
            {
                Id = Guid.NewGuid().ToString(),
                Title = dto.Title,
                ActivityDescription = dto.ActivityDescription,
                ActivityPriority = dto.ActivityPriority,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Activity created successfully", Activity = activity });
        }

        // get my activities (User Only) 
        [Authorize(Roles = "User")]
        [HttpGet("my")]
        public async Task<IActionResult> MyActivities()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var activities = await _context.Activities
                                           .Where(a => a.UserId == userId)
                                           .ToListAsync();
            return Ok(activities);
        }

        // get all activities (Admin Only) 
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> AllActivities()
        {
            var activities = await _context.Activities.Include(a => a.User).ToListAsync();
            return Ok(activities);
        }

        // update activities (User/Admin) 
        [Authorize(Roles = "User,Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ActivityDto dto)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound("Activity not found");

            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (role == "User" && activity.UserId != userId)
                return Forbid("You can only update your own activities");

            activity.Title = dto.Title;
            activity.ActivityDescription = dto.ActivityDescription;
            activity.ActivityPriority = dto.ActivityPriority;
            activity.UpdatedAt = DateTime.UtcNow;

            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Activity updated successfully", Activity = activity });
        }

        // DELETE ACTIVITY (User/Admin) 
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound("Activity not found");

            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (role == "User" && activity.UserId != userId)
                return Forbid("You can only delete your own activities");

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Activity deleted successfully" });
        }
    }

    // DTO for creating/updating activity
    public class ActivityDto
    {
        public string Title { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityPriority { get; set; }
    }
}