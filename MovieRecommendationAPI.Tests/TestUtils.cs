using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Dtos.Movie;

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
}