using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data;
using MovieRecommendation.Data.Review;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Movie;

namespace MovieRecommendation.Services.Review;

public class DbReviewService : IReviewService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public DbReviewService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ReviewDto?> GetReviewByIdAsync(Guid id)
    {
        var review = await _dbContext.Reviews.FindAsync(id);
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<ReviewDto?> GetReviewByUserAndMovieIdAsync(Guid userId, Guid movieId)
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<List<ReviewDto>> GetMovieReviewsAsync(Guid movieId)
    {
        var reviews = await _dbContext.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
        return _mapper.Map<List<ReviewDto>>(reviews);
    }

    public async Task<List<ReviewDto>> GetUserReviewsAsync(Guid userId)
    {
        var reviews = await _dbContext.Reviews.Where(r => r.UserId == userId).ToListAsync();
        return _mapper.Map<List<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto?> CreateReviewAsync(Guid userId, CreateReviewDto createReviewDto)
    {
        var movieExists = await _dbContext.Movies.AnyAsync(m => m.Id == createReviewDto.MovieId);
        var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
        if (!movieExists || !userExists) return null;

        var review = await _dbContext.Reviews.FirstOrDefaultAsync(r =>
            r.MovieId == createReviewDto.MovieId && r.UserId == userId
        );

        if (review == null)
        {
            review = new ReviewData
            {
                Id = Guid.NewGuid(),
                Rating = createReviewDto.Rating,
                Comment = createReviewDto.Comment,
                UserId = userId,
                MovieId = createReviewDto.MovieId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _dbContext.Reviews.AddAsync(review);
        }
        else
        {
            review.Rating = createReviewDto.Rating;
            review.Comment = createReviewDto.Comment;
            review.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
        await UpdateMovieRatingAsync(review.MovieId);
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task DeleteReviewByIdAsync(Guid id)
    {
        var review = await _dbContext.Reviews.FindAsync(id);
        if (review != null)
        {
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();
            await UpdateMovieRatingAsync(review.MovieId);
        }
    }

    public async Task UpdateMovieRatingAsync(Guid movieId)
    {
        var movieReviews = await GetMovieReviewsAsync(movieId);
        var ratingSum = movieReviews.Sum(r => r.Rating);

        var movie = await _dbContext.Movies.FindAsync(movieId);
        if (movie != null) movie.AverageRating = (float)ratingSum / movieReviews.Count;

        await _dbContext.SaveChangesAsync();
    }
}