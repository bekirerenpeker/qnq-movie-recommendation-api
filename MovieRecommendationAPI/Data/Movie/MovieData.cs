namespace MovieRecommendation.Data.Movie;

public class MovieData
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = "";
    public string? Description { get; set; } = null;
    public int DurationMins  { get; set; } = 0;
    public int? ReleaseYear { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    
    public List<CategoryData> Categories { get; set; } = [];
}