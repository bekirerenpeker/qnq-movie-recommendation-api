using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Services.Review;

namespace MovieRecommendation.Tests.Review;

public class ReviewServiceTests
{
    [Fact]
    public async Task CanCreateUpdateDeleteReviewTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var reviewService = new DbReviewService(dbContext, TestUtils.GetReviewMapper());
        var userService = new DbUserService(dbContext, TestUtils.GetAuthMapper());
        var movieService = new DbMovieService(dbContext, TestUtils.GetMovieMapper());
        var categoryService = new DbCategoryService(dbContext, TestUtils.GetMovieMapper());

        // create mock data required to create a review
        var user = await userService.RegisterAsync(new RegisterDto
        {
            Email = "test@mail",
            Password = "password",
            Name = "name",
            Surname = "surname",
        });
        Assert.NotNull(user);

        var category = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Category" });
        Assert.NotNull(category);

        var movie = await movieService.CreateMovieAsync(new CreateMovieDto
        {
            Title = "Test Movie",
            DurationMins = 100,
            CategoryIds = [category.Id]
        });
        Assert.NotNull(movie);

        var createdReview = await reviewService.CreateReviewAsync(user.Id, new CreateReviewDto
        {
            Rating = 5,
            MovieId = movie.Id,
        });
        Assert.NotNull(createdReview);
        Assert.Equal(5, createdReview.Rating);
        Assert.Equal(null, createdReview.Comment);

        var updatedReview = await reviewService.CreateReviewAsync(user.Id, new CreateReviewDto
        {
            Rating = 8,
            Comment = "comment",
            MovieId = movie.Id,
        });
        Assert.NotNull(updatedReview);
        Assert.Equal(createdReview.Id, updatedReview.Id);
        Assert.Equal(8, updatedReview.Rating);
        Assert.Equal("comment", updatedReview.Comment);

        await reviewService.DeleteReviewByIdAsync(createdReview.Id);
        var afterDelete = await  reviewService.GetReviewByIdAsync(createdReview.Id);
        Assert.Null(afterDelete);
    }
}