namespace MovieRecommendation.Dtos.Movie;

public enum ReviewOrderType
{
    ByRating,
    CreatedAt,
}

public class MovieDetailsDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = "";
    public string? Description { get; set; } = null;
    public float AverageRating { get; set; } = -1;
    public int DurationMins { get; set; } = -1;
    public int? ReleaseYear { get; set; } = null;

    public List<Guid> CategoryIds { get; set; } = [];
    
    public List<Guid> ReviewIds { get; set; } = [];
}