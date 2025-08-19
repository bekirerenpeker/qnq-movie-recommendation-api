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

public class SelectMoviesDto
{
    public List<Guid> CategoryIds { get; set; } = [];
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
    public MovieOrderType OrderType { get; set; } = MovieOrderType.ByTitle;
    public int? Count { get; set; } = null;
}