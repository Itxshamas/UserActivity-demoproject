using DemoProje.Models;
using DemoProje.Models.DTOs;

namespace DemoProje.Repositories.Interface
{
    public interface IActivityRepository
    {
        // User operations
        Task<Activities> UserCreateActivityAsync(ActivityDto dto, string userId);
        Task<List<ActivityResponseDto>> UserGetActivitiesAsync(string userId);
        Task<ActivityResponseDto> GetActivityByIdAsync(string id, string userId, string role);
        Task<Activities> UpdateActivityAsync(string id, ActivityDto dto, string userId, string role);
        Task<bool> DeleteActivityAsync(string id, string userId, string role);

        // Admin operations
       Task<Activities> AdminCreateActivityAsync(ActivityDto dto, string targetUserId);

        Task<List<ActivityAdminResponseDto>> AdminGetAllActivitiesAsync();
        Task<List<ActivityAdminResponseDto>> GetUserActivitiesAsync(string targetUserId);
    }
}