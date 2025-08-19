namespace MovieRecommendation.Dtos.Movie;

public enum OrderDirection
{
    Ascending,
    Descending
}

public enum MovieOrderType
{
    ByName,
    ByRating,
    ByReleaseYear,
    ByDuration,
}

public class ListMoviesDto
{
    public List<Guid> CategoryIds { get; set; } = [];
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
    public MovieOrderType OrderType { get; set; } = MovieOrderType.ByName;
}