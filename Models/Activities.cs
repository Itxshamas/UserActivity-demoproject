using System.Text.Json.Serialization;


namespace DemoProje.Models
{
    public class Activities
    {
    
        public string Id { get; set; }
        public string Title { get; set; }
        public string ActivityDescription { get; set; }  

        public string ActivityPriority { get; set; }
        public string UserId { get; set; } 

            [JsonIgnore] // Add this

        public Users User { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }  // Optional field
    }
}