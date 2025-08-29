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
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        // CREATE ACTIVITY (User Only) 
        [Authorize(Roles = "User")]
        [HttpPost("UserCreateActivity")]
        public async Task<IActionResult> Create([FromBody] ActivityDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var activity = await _activityRepository.UserCreateActivityAsync(dto, userId);
            return Ok(new { Message = "Activity created successfully", Activity = activity });
        } 


                [Authorize(Roles = "Admin")]
        [HttpPost("AdminCreateActivity")]
        public async Task<IActionResult> AdminCreateActivity([FromBody] AdminActivityCreateDto dto)
{
            try
            {
                var activity = await _activityRepository.AdminCreateActivityAsync(dto.Activity, dto.TargetUserId);
                return Ok(new { 
                    Message = "Activity created successfully for user", 
                    Activity = activity 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Get my activities (User Only) 
        [Authorize(Roles = "User")]
        [HttpGet("UserActivities")]
        public async Task<IActionResult> MyActivities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activities = await _activityRepository.UserGetActivitiesAsync(userId);
            return Ok(activities);
        }

        // GET: Get single activity by ID 
        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetActivityById")]
        public async Task<IActionResult> GetActivity(string id)
        {
            // CHANGE THIS LINE
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var activity = await _activityRepository.GetActivityByIdAsync(id, userId, role);
            if (activity == null) return NotFound("Activity not found or access denied");

            return Ok(activity);
        }

        // GET: Get all activities (Admin Only) 
        [Authorize(Roles = "Admin")]
        [HttpGet("AdminGetAllActivities")]
        public async Task<IActionResult> AllActivities()
        {
            var activities = await _activityRepository.AdminGetAllActivitiesAsync();
            return Ok(activities);
        }

        // UPDATE activities (User/Admin) 
        [Authorize(Roles = "User,Admin")]
        [HttpPut("UpdateActivities")]
        public async Task<IActionResult> Update(string id, [FromBody] ActivityDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var activity = await _activityRepository.UpdateActivityAsync(id, dto, userId, role);
            if (activity == null) return NotFound("Activity not found or access denied");

            return Ok(new { Message = "Activity updated successfully", Activity = activity });
        }

        // DELETE ACTIVITY (User/Admin) 
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("DeleteActivities")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var deleted = await _activityRepository.DeleteActivityAsync(id, userId, role);
            if (!deleted) return NotFound("Activity not found or access denied");

            return Ok(new { Message = "Activity deleted successfully" });
        }



        // Get activities by user 
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserActivities")]
        public async Task<IActionResult> GetUserActivities(string userId)
        {
            var activities = await _activityRepository.GetUserActivitiesAsync(userId);
            return Ok(activities);
        }
    }
}