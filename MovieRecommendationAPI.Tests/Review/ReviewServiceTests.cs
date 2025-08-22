using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Services.Review;
using MovieRecommendation.Validators.Review;
using Xunit.Abstractions;

namespace MovieRecommendation.Tests.Review;

public class ReviewServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ReviewServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CanCreateUpdateDeleteReviewTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var reviewService = new DbReviewService(dbContext, TestUtils.GetReviewMapper());
        var userService = new DbUserService(dbContext, TestUtils.GetAuthMapper());
        var movieService = new DbMovieService(dbContext, TestUtils.GetMovieMapper());

        // create mock data required to create a review
        var user = await userService.RegisterAsync(new RegisterDto());
        Assert.NotNull(user);

        var movie = await movieService.CreateMovieAsync(new CreateMovieDto());
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
        var afterDelete = await reviewService.GetReviewByIdAsync(createdReview.Id);
        Assert.Null(afterDelete);
    }

    [Fact]
    public async Task CanValidateInputTest()
    {
        var validator = new CreateReviewValidator();

        var validCreateDto = new CreateReviewDto
        {
            Rating = 5,
            MovieId = Guid.NewGuid(),
        };
        var result = await validator.TestValidateAsync(validCreateDto);
        Assert.True(result.IsValid);

        var invalidCreateDto = new CreateReviewDto
        {
            Rating = 14,
        };
        result = await validator.TestValidateAsync(invalidCreateDto);
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
    }

    [Fact]
    public async Task CanRateMoviesTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var reviewService = new DbReviewService(dbContext, TestUtils.GetReviewMapper());
        var userService = new DbUserService(dbContext, TestUtils.GetAuthMapper());
        var movieService = new DbMovieService(dbContext, TestUtils.GetMovieMapper());

        var user1 = await userService.RegisterAsync(new RegisterDto());
        var user2 = await userService.RegisterAsync(new RegisterDto());
        Assert.NotNull(user1);
        Assert.NotNull(user2);

        var movie1 = await movieService.CreateMovieAsync(new CreateMovieDto());
        var movie2 = await movieService.CreateMovieAsync(new CreateMovieDto());
        Assert.NotNull(movie1);
        Assert.NotNull(movie2);

        var user1Movie1Review = await reviewService.CreateReviewAsync(user1.Id, new CreateReviewDto
        {
            MovieId = movie1.Id, Rating = 4,
        });
        Assert.NotNull(user1Movie1Review);

        var user1Movie2Review = await reviewService.CreateReviewAsync(user1.Id, new CreateReviewDto
        {
            MovieId = movie2.Id, Rating = 10,
        });
        Assert.NotNull(user1Movie2Review);

        var user2Movie1Review = await reviewService.CreateReviewAsync(user2.Id, new CreateReviewDto
        {
            MovieId = movie1.Id, Rating = 2,
        });
        Assert.NotNull(user2Movie1Review);

        var user2Movie2Review = await reviewService.CreateReviewAsync(user2.Id, new CreateReviewDto
        {
            MovieId = movie2.Id, Rating = 7,
        });
        Assert.NotNull(user2Movie2Review);

        _testOutputHelper.WriteLine(user1Movie1Review.Rating.ToString());
        _testOutputHelper.WriteLine(user1Movie2Review.Rating.ToString());
        _testOutputHelper.WriteLine(user2Movie1Review.Rating.ToString());
        _testOutputHelper.WriteLine(user2Movie2Review.Rating.ToString());
        _testOutputHelper.WriteLine(movie1.AverageRating.ToString());
        _testOutputHelper.WriteLine(movie2.AverageRating.ToString());
        _testOutputHelper.WriteLine(movie1.Id.ToString());
        _testOutputHelper.WriteLine(movie2.Id.ToString());

        Assert.Equal((user1Movie1Review.Rating + user2Movie1Review.Rating) / 2.0f, movie1.AverageRating);
        Assert.Equal((user1Movie2Review.Rating + user2Movie2Review.Rating) / 2.0f, movie2.AverageRating);
    }
}