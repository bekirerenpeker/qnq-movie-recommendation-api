using MovieRecommendation.Services;

namespace MovieRecommendation.Dtos.Movie;

public enum MovieOrderType
{
    ByTitle,
    ByRating,
    ByReleaseYear,
    ByDuration,
}

public class SelectMoviesDto
{
    public List<Guid> CategoryIds { get; set; } = [];
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
    public MovieOrderType OrderType { get; set; } = MovieOrderType.ByTitle;
    public Paginate? Paginate { get; set; } = null;
}

public class PaginatedMoviesDto
{
    public List<MovieDto> Movies { get; set; } = [];
    public Paginate? Paginate { get; set; } = null;
}