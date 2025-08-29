

using DemoProje.Models;
using DemoProje.Models.DTOs;

namespace DemoProje.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<UserViewModel> GetUserByIdAsync(string id);
        Task<List<UserViewModel>> GetAllAdminsAsync();
        Task<UserViewModel> GetAdminByIdAsync(string id);

        Task<Users> CreateUserAsync(UserCreateDto dto);
        Task<Users> UpdateUserAsync(string id, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(string id);
        Task<Users> UpdateAdminSelfAsync(string id, AdminSelfUpdateDto dto);

    }
}