namespace MovieRecommendation.Dtos.Movie;

public class MovieDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = "";
    public string? Description { get; set; } = null;
    public float AverageRating { get; set; } = 0;
    public int DurationMins { get; set; } = 0;
    public int? ReleaseYear { get; set; } = null;

    public List<Guid> CategoryIds { get; set; } = [];
}

public class CreateMovieDto
{
    public string Title { get; set; } = "";
    public string? Description { get; set; } = null;
    public int DurationMins { get; set; } = 0;
    public int? ReleaseYear { get; set; } = null;

    public List<Guid> CategoryIds { get; set; } = [];
}

public class UpdateMovieDto
{
    public string? Title { get; set; } = null;
    public string? Description { get; set; } = null;
    public int? DurationMins { get; set; } = null;
    public int? ReleaseYear { get; set; } = null;

    public List<Guid>? CategoryIds { get; set; } = null;
}