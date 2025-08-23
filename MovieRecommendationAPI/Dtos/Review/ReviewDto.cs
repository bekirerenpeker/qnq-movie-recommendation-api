namespace MovieRecommendation.Dtos.Review;

public class ReviewDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public int Rating { get; set; } = 0;
    public string? Comment { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; } = Guid.Empty;
    public Guid MovieId { get; set; } = Guid.Empty;
}