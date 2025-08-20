namespace MovieRecommendation.Dtos.Movie;

public enum OrderDirection
{
    Ascending,
    Descending
}

public enum MovieOrderType
{
    ByTitle,
    ByRating,
    ByReleaseYear,
    ByDuration,
}

public class Paginate
{
    public int Count { get; set; } = 10;
    public int Page { get; set; } = 0;
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