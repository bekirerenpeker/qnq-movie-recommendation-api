using MovieRecommendation.Services;

namespace MovieRecommendation.Dtos.Movie;

public enum ReviewOrderType
{
    ByRating,
    CreatedAt,
}

public class FetchMovieDetailsDto
{
    public Guid Id { get; set; } = Guid.Empty;

    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
    public ReviewOrderType OrderType { get; set; } = ReviewOrderType.ByRating;
    public Paginate? Paginate { get; set; } = null;
};

public class MovieDetailsDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = "";
    public string? Description { get; set; } = null;
    public float AverageRating { get; set; } = -1;
    public int DurationMins { get; set; } = -1;
    public int? ReleaseYear { get; set; } = null;

    public List<Guid> CategoryIds { get; set; } = [];

    public Paginate? Paginate { get; set; } = null;
    public List<Guid> ReviewIds { get; set; } = [];
}