using DemoProje.Models;
using DemoProje.Models.DTOs;
using DemoProje.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("CreateNewUserOrAdmin")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            try
            {
                var newUser = await this._repo.CreateUserAsync(dto);
                return Ok(new { Message = "User created successfully", User = newUser });
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

        [HttpDelete("DeleteUserById")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
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
