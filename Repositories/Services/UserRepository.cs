using DemoProje.Data;
using DemoProje.Models;
using DemoProje.Models.DTOs;
using DemoProje.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DemoProje.Repositories.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Signup
        public async Task<Users> RegisterAsync(RegisterDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role);
                if (role == null) throw new Exception("Role not found. Please create Admin/User roles first.");

                var user = new Users
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    Address = dto.Address,
                    MobileNum = dto.MobileNum,
                    RoleId = role.Id
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Transaction Failed: " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }

        // Login
        public async Task<Users> LoginAsync(LoginDto dto)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.PasswordHash == dto.Password);
        }

        // Logout
        public async Task LogoutAsync(string userId)
        {
            await Task.CompletedTask;
        }

        // Get profile
        public async Task<UserViewModel> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Id == id)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Address = u.Address,
                    MobileNum = u.MobileNum,
                    RoleName = u.Role.Name
                })
                .FirstOrDefaultAsync();
        }

        // Get dashboard
        public async Task<UserViewModel> GetDashboardAsync(string id)
        {
            return await GetUserByIdAsync(id);
        }

        // Update profile
        public async Task<Users> UpdateUserAsync(string id, UserUpdateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return null;

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;
                user.Address = dto.Address;
                user.MobileNum = dto.MobileNum;
                if (!string.IsNullOrEmpty(dto.Password))
                    user.PasswordHash = dto.Password;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Transaction Failed: " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }
    }
}
