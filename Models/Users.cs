using System.Text.Json.Serialization;


namespace DemoProje.Models
{
    public class Users
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MobileNum { get; set; }
        public string RoleId { get; set; }
        public string PasswordHash { get; set; }



        [JsonIgnore] // Add this

        public Roles Role { get; set; }
        public List<Activities> Activities { get; set; }


    }
}