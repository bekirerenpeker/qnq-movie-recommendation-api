using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Services.Review;

namespace MovieRecommendation.Tests.Movie;

public class MovieDetailsTests
{
    [Fact]
    public async Task CanGetMovieDetailsTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var userService = new DbUserService(dbContext, TestUtils.GetAuthMapper());
        var categoryService = new DbCategoryService(dbContext, TestUtils.GetMovieMapper());
        var movieService = new DbMovieService(dbContext, TestUtils.GetMovieMapper());
        var reviewService = new DbReviewService(dbContext, TestUtils.GetReviewMapper());

        var user1 = await TestUtils.CreateUser(userService);
        var user2 = await TestUtils.CreateUser(userService);

        var cat1 = await TestUtils.CreateCategory(categoryService);
        var cat2 = await TestUtils.CreateCategory(categoryService);

        var movie = await TestUtils.CreateMovie(movieService, [cat1, cat2]);

        var review1 = await TestUtils.CreateReview(reviewService, user1.Id, movie.Id);
        var review2 = await TestUtils.CreateReview(reviewService, user2.Id, movie.Id);

        var movieDetails = await movieService.GetMovieDetailsAsync(new FetchMovieDetailsDto
        {
            Id = movie.Id,
            OrderDirection = OrderDirection.Ascending,
            OrderType = ReviewOrderType.ByRating,
        });
        Assert.NotNull(movieDetails);
        Assert.Equal(movieDetails.Title, movie.Title);
        Assert.Equal(movieDetails.Description, movie.Description);
        Assert.Equal(movieDetails.DurationMins, movie.DurationMins);
        Assert.Equal(movieDetails.ReleaseYear, movie.ReleaseYear);
        Assert.Equal(movieDetails.CategoryIds, movie.CategoryIds);
        Assert.Equal(movieDetails.ReviewIds[0], review1.Id);
        Assert.Equal(movieDetails.ReviewIds[1], review2.Id);
    }
}