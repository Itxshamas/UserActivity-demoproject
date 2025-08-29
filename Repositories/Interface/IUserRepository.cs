using DemoProje.Models;
using DemoProje.Models.DTOs;

namespace DemoProje.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<Users> RegisterAsync(RegisterDto dto);
        Task<Users> LoginAsync(LoginDto dto);
        Task LogoutAsync(string userId);

        Task<UserViewModel> GetUserByIdAsync(string id);
        Task<UserViewModel> GetDashboardAsync(string id);

        Task<Users> UpdateUserAsync(string id, UserUpdateDto dto);
    }
}