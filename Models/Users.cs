namespace DemoProje.Models
{
    public class Users
    {
        public string Id { get; set; }            // GUID (char(36))
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // capital "P" for convention
        public string Address { get; set; }
        public string MobileNum { get; set; }
        public string RoleId { get; set; }

        public Roles Role { get; set; }
    }
}
