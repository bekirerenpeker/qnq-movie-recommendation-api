using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Services.Review;

namespace MovieRecommendation.Tests;

public static class TestUtils
{
    public static AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    public static IMapper GetAuthMapper()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AuthMappingProfile>(); });
        return config.CreateMapper();
    }

    public static IMapper GetMovieMapper()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<MovieMappingProfile>(); });
        return config.CreateMapper();
    }

    public static IMapper GetReviewMapper()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        return config.CreateMapper();
    }

    private static int _userCount = 0;
    public static async Task<UserDto> CreateUser(IUserService userService)
    {
        var user = await userService.CreateUserAsync(new CreateUserDto
        {
            Name = "TestUserName" + _userCount.ToString(),
            Surname = "TestUserSurname" + _userCount.ToString(),
            Email = "TestUser@Email" + _userCount.ToString(),
            Password = "TestUserPassword" + _userCount.ToString(),
        });
        _userCount++;
        return user;
    }

    private static int _categoryCount = 0;
    public static async Task<CategoryDto> CreateCategory(ICategoryService categoryService)
    {
        var cat = await categoryService.CreateCategoryAsync(new CreateCategoryDto
        {
            Name =  "TestCategoryName" + _categoryCount.ToString(),
        });
        _categoryCount++;
        return cat;
    }
    
    private static int _movieCount = 0;
    public static async Task<MovieDto> CreateMovie(IMovieService movieService, List<CategoryDto> categories)
    {
        var movie = await movieService.CreateMovieAsync(new CreateMovieDto
        {
            Title = "TestMovieTitle"  + _movieCount.ToString(),
            DurationMins = 50 + _movieCount * 10,
            Description = "TestMovieDescription" + _movieCount.ToString(),
            ReleaseYear = 1980 +  _movieCount * 10,
            CategoryIds = categories.Select(c => c.Id).ToList(),
        });
        _movieCount++;
        return movie;
    }
    
    private static int _reviewCount = 0;
    public static async Task<ReviewDto> CreateReview(IReviewService reviewService, Guid userId, Guid movieId)
    {
        var review = await reviewService.CreateReviewAsync(userId, new CreateReviewDto
        {
            MovieId = movieId,
            Rating = (_reviewCount * 3) % 11,
            Comment = "TestComment" + _reviewCount.ToString(),
        });
        _reviewCount++;
        Assert.NotNull(review);
        return review;
    }
}