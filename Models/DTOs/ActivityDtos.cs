namespace DemoProje.Models.DTOs
{
    public class ActivityDto
    {
        public string Title { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityPriority { get; set; }
    }

    public class ActivityResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityPriority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ActivityAdminResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityPriority { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AdminActivityCreateDto
    {
        public ActivityDto Activity { get; set; }
        public string TargetUserId { get; set; }
    }
}