namespace DemoProje.Models
{
    public class Roles
    {
        public string Id { get; set; }   // GUID (char(36))
        public string Name { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
