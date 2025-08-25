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
    }
}