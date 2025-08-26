using System;

namespace DemoProject.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime ActivityTime { get; set; }
        public string ActivityPriority { get; set; } 
    }
}
