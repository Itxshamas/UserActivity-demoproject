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
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;

        public AdminController(IAdminRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repo.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetSingleUserById")]
        public async Task<IActionResult> GetUser(string id)
        {
            UserViewModel user = await _repo.GetUserByIdAsync(id); 
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _repo.GetAllAdminsAsync();
            return Ok(admins);
        }

        [HttpGet("GetAdminById")]
        public async Task<IActionResult> GetAdminById(string id)
        {
            UserViewModel admin = await this._repo.GetAdminByIdAsync(id); 
            if (admin == null)
                return NotFound("Admin not found");

            return Ok(admin);
        }

        // Get current admin's own profile
        [HttpGet("GetMyProfile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var adminId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var admin = await _repo.GetAdminByIdAsync(adminId);
            
            if (admin == null)
                return NotFound("Admin profile not found");

            return Ok(admin);
        }

        [HttpPost("CreateNewUserOrAdmin")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            try
            {
                var newUser = await _repo.CreateUserAsync(dto);
                
                return Ok(new { 
                    Message = "User created successfully", 
                    UserId = newUser.Id,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUserById")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto dto)
        {
            try
            {
                var targetUser = await _repo.GetUserByIdAsync(id);
                if (targetUser == null)
                    return NotFound("User not found");

                if (targetUser.RoleName == "Admin")
                    return Forbid("Admins cannot update other admin accounts");

                var updatedUser = await _repo.UpdateUserAsync(id, dto);
                if (updatedUser == null)
                    return NotFound("User not found");
                    
                return Ok(new { Message = "User updated successfully", User = updatedUser });
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
        }

        // Admin updates his own profile only
        [HttpPut("AdminUpdateHisOwnProfile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] AdminSelfUpdateDto dto)
        {
            try
            {
                var adminId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var updatedAdmin = await _repo.UpdateAdminSelfAsync(adminId, dto);
                
                if (updatedAdmin == null)
                    return NotFound("Admin not found");

                return Ok(new { 
                    Message = "Your profile updated successfully", 
                    FirstName = updatedAdmin.FirstName,
                    LastName = updatedAdmin.LastName,
                    Email = updatedAdmin.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUserById")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var targetUser = await _repo.GetUserByIdAsync(id);
                if (targetUser == null)
                    return NotFound("User not found");

                if (targetUser.RoleName == "Admin")
                    return Forbid("Admins cannot delete other admin accounts");

                var deleted = await _repo.DeleteUserAsync(id);
                if (!deleted)
                    return NotFound("User not found");
                    
                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
        }

        
    }
}