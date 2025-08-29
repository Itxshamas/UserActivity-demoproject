namespace DemoProje.Models.DTOs
{
    public class UserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  
        public string Address { get; set; }
        public string MobileNum { get; set; }
        public string RoleId { get; set; }
    }

    public class UserUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }  
        public string Address { get; set; }
        public string MobileNum { get; set; }
        public string RoleId { get; set; }
    }

    public class AdminSelfUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }  
        public string Address { get; set; }
        public string MobileNum { get; set; }
    }
}