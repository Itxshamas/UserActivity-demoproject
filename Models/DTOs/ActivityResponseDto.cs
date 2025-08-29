public class ActivityResponseDto
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