using MovieRecommendation.Data.Review;
using MovieRecommendation.Dtos.Review;

namespace MovieRecommendation.Services.Review;

public interface IReviewService
{
    Task<ReviewDto> GetReview(Guid id);
    Task<ReviewDto> GetReview(Guid userId, Guid movieId);
    Task<List<ReviewDto>> GetMovieReviews(Guid movieId);
    Task<List<ReviewDto>> GetUserReviews(Guid userId);
}