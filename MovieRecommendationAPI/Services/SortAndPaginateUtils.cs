using MovieRecommendation.Data.Movie;
using MovieRecommendation.Data.Review;
using MovieRecommendation.Dtos.Movie;

namespace MovieRecommendation.Services;

public static class SortAndPaginateUtils
{
    public static List<T> Paginate<T>(List<T> movies, Paginate paginate)
    {
        return movies.Skip(paginate.Count * paginate.Page).Take(paginate.Count).ToList();
    }

    public static void SortMoviesBy(List<MovieData> movies, MovieOrderType orderType, OrderDirection direction)
    {
        switch (orderType)
        {
            case MovieOrderType.ByTitle:
                movies.Sort((a, b) =>
                    direction == OrderDirection.Ascending
                        ? String.Compare(a.Title, b.Title, StringComparison.Ordinal)
                        : String.Compare(b.Title, a.Title, StringComparison.Ordinal)
                );
                break;
            case MovieOrderType.ByDuration:
                movies.Sort((a, b) =>
                    direction == OrderDirection.Ascending
                        ? a.DurationMins.CompareTo(b.DurationMins)
                        : b.DurationMins.CompareTo(a.DurationMins)
                );
                break;
            case MovieOrderType.ByRating:
                movies.Sort((a, b) =>
                    direction == OrderDirection.Ascending
                        ? a.AverageRating.CompareTo(b.AverageRating)
                        : b.AverageRating.CompareTo(a.AverageRating)
                );
                break;
            case MovieOrderType.ByReleaseYear:
                movies.Sort((a, b) =>
                    direction == OrderDirection.Ascending
                        ? (a.ReleaseYear ?? 0).CompareTo(b.ReleaseYear ?? 0)
                        : (b.ReleaseYear ?? 0).CompareTo(a.ReleaseYear ?? 0)
                );
                break;
        }
    }

    public static void SortReviewsBy(List<ReviewData> reviews, ReviewOrderType orderType, OrderDirection direction)
    {
        
    }
}

public enum OrderDirection
{
    Ascending,
    Descending
}

public class Paginate
{
    public int Count { get; set; } = 10;
    public int Page { get; set; } = 0;
}
