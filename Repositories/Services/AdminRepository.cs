using DemoProje.Data;
using DemoProje.Models;
using DemoProje.Models.DTOs;
using DemoProje.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DemoProje.Repositories.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // All users 
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            return await _context.Users
              .Include(u => u.Role)
                .Where(u => u.Role.Name == "User")
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
                .ToListAsync();
        }

        // Add this new method for admin self-update
        public async Task<Users> UpdateAdminSelfAsync(string id, AdminSelfUpdateDto dto)
        {
            var admin = await _context.Users.FindAsync(id);
            if (admin == null) return null;

            // Only allow updating specific fields (not RoleId)
            admin.FirstName = dto.FirstName;
            admin.LastName = dto.LastName;
            admin.Email = dto.Email;
            admin.Address = dto.Address;
            admin.MobileNum = dto.MobileNum;
            
            // Update password only if provided
            if (!string.IsNullOrEmpty(dto.Password))
                admin.PasswordHash = dto.Password;

            _context.Users.Update(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

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

        public async Task<List<UserViewModel>> GetAllAdminsAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == "Admin")
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
                .ToListAsync();
        }

        public async Task<UserViewModel> GetAdminByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Id == id && u.Role.Name == "Admin")
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

        // Keep original entity return for create
        public async Task<Users> CreateUserAsync(UserCreateDto dto)
        {
            try
            {
                var user = new Users
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Address = dto.Address,
                    MobileNum = dto.MobileNum,
                    PasswordHash = dto.Password,
                    RoleId = dto.RoleId
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                // log the inner exception details
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<Users> UpdateUserAsync(string id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Address = dto.Address;
            user.MobileNum = dto.MobileNum;
            user.RoleId = dto.RoleId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}