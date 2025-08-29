using DemoProje.Data;
using DemoProje.Models;
using DemoProje.Models.DTOs;
using DemoProje.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DemoProje.Repositories.Services
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // User: Create activity
        public async Task<Activities> UserCreateActivityAsync(ActivityDto dto, string userId)
        {
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
            return activity;
        }

        // User: Get user's activities
        public async Task<List<ActivityResponseDto>> UserGetActivitiesAsync(string userId)
        {
            return await _context.Activities
                .Where(a => a.UserId == userId)
                .Select(a => new ActivityResponseDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    ActivityDescription = a.ActivityDescription,
                    ActivityPriority = a.ActivityPriority,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();
        }

        // Get activity by ID with ownership check
        public async Task<ActivityResponseDto> GetActivityByIdAsync(string id, string userId, string role)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return null;

            // User can only access their own activities
            if (role == "User" && activity.UserId != userId)
                return null;

            return new ActivityResponseDto
            {
                Id = activity.Id,
                Title = activity.Title,
                ActivityDescription = activity.ActivityDescription,
                ActivityPriority = activity.ActivityPriority,
                CreatedAt = activity.CreatedAt,
                UpdatedAt = activity.UpdatedAt
            };
        }


   // Admin: Create activity for any user
        public async Task<Activities> AdminCreateActivityAsync(ActivityDto dto, string targetUserId)
        {
            // Verify the target user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == targetUserId);
            if (!userExists)
            {
                throw new Exception("Target user not found");
            }

            var activity = new Activities
            {
                Id = Guid.NewGuid().ToString(),
                Title = dto.Title,
                ActivityDescription = dto.ActivityDescription,
                ActivityPriority = dto.ActivityPriority,
                UserId = targetUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
            return activity;
        } 

        // Update activity with ownership check
        public async Task<Activities> UpdateActivityAsync(string id, ActivityDto dto, string userId, string role)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return null;

            // User can only update their own activities
            if (role == "User" && activity.UserId != userId)
                return null;

            activity.Title = dto.Title;
            activity.ActivityDescription = dto.ActivityDescription;
            activity.ActivityPriority = dto.ActivityPriority;
            activity.UpdatedAt = DateTime.UtcNow;

            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        // Delete activity with ownership check
        public async Task<bool> DeleteActivityAsync(string id, string userId, string role)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            // User can only delete their own activities
            if (role == "User" && activity.UserId != userId)
                return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Admin: Get all activities
        public async Task<List<ActivityAdminResponseDto>> AdminGetAllActivitiesAsync()
        {
            return await _context.Activities
                .Include(a => a.User)
                .Select(a => new ActivityAdminResponseDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    ActivityDescription = a.ActivityDescription,
                    ActivityPriority = a.ActivityPriority,
                    UserId = a.UserId,
                    UserName = a.User.FirstName + " " + a.User.LastName,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();
        }

        // Admin: Get activities by user ID
        public async Task<List<ActivityAdminResponseDto>> GetUserActivitiesAsync(string targetUserId)
        {
            return await _context.Activities
                .Include(a => a.User)
                .Where(a => a.UserId == targetUserId)
                .Select(a => new ActivityAdminResponseDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    ActivityDescription = a.ActivityDescription,
                    ActivityPriority = a.ActivityPriority,
                    UserId = a.UserId,
                    UserName = a.User.FirstName + " " + a.User.LastName,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();
        }
    }
}