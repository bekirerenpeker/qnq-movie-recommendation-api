namespace MovieRecommendation.Dtos.Review;

public class CreateReviewDto
{
    public int Rating {get ; set;} = 0;
    public string? Comment { get; set; } = null;
    // public Guid UserId { get; set; } = Guid.Empty;
    public Guid MovieId { get; set; } = Guid.Empty;
}