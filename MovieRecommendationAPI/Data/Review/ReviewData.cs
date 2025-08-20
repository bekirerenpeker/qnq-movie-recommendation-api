namespace MovieRecommendation.Data.Review;

public class ReviewData
{
    public Guid Id { get; set; } = Guid.Empty;
    public int Rating { get; set; } = 0;
    public string Comment { get; set; } = "";

    public Guid UserId { get; set; } = Guid.Empty;
    public Guid MovieId { get; set; } = Guid.Empty;
}