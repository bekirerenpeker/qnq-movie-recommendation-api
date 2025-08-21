using MovieRecommendation.Data.Review;
using MovieRecommendation.Dtos.Review;

namespace MovieRecommendation.Services.Review;

public interface IReviewService
{
    Task<ReviewDto?> GetReviewByIdAsync(Guid id);
    Task<ReviewDto?> GetReviewByUserAndMovieIdAsync(Guid userId, Guid movieId);
    Task<List<ReviewDto>> GetMovieReviewsAsync(Guid movieId);
    Task<List<ReviewDto>> GetUserReviewsAsync(Guid userId);
    Task DeleteReviewByIdAsync(Guid id);
    
    Task<ReviewDto?> CreateReviewAsync(Guid userId, CreateReviewDto createReviewDto);
    Task UpdateMovieRatingAsync(Guid movieId);
}