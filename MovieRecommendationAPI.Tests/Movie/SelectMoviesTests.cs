using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Validators.Movie;

namespace MovieRecommendation.Tests.Movie;

public class SelectMoviesTests
{
    [Fact]
    public async Task CanGetMoviesByCategoryTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var movieService = new DbMovieService(dbContext, mapper);
        var categoryService = new DbCategoryService(dbContext, mapper);

        var horrorCat = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Horror" });
        var comedyCat = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Comedy" });
        var actionCat = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Action" });

        var horrorActionMovie = await movieService.CreateMovieAsync(new CreateMovieDto
        {
            Title = "Horror Action Movie",
            DurationMins = 100,
            ReleaseYear = 2000,
            CategoryIds = [horrorCat.Id, actionCat.Id]
        });
        var comedyMovie = await movieService.CreateMovieAsync(new CreateMovieDto
        {
            Title = "Comedy Movie",
            DurationMins = 90,
            ReleaseYear = 1999,
            CategoryIds = [comedyCat.Id]
        });
        var comedyActionMovie = await movieService.CreateMovieAsync(new CreateMovieDto
        {
            Title = "Comedy Action Movie",
            DurationMins = 120,
            ReleaseYear = 2001,
            CategoryIds = [comedyCat.Id, actionCat.Id]
        });

        var actionMovies = await movieService.SelectMoviesByCategoryAsync(new SelectMoviesDto
        {
            CategoryIds = [actionCat.Id],
            OrderType = MovieOrderType.ByReleaseYear,
            OrderDirection = OrderDirection.Descending,
        });
        Assert.Equal(2, actionMovies.Count);
        Assert.Equal(actionMovies[0].Id, comedyActionMovie.Id); // 2001
        Assert.Equal(actionMovies[1].Id, horrorActionMovie.Id); // 2000
        
        var comedyMovies = await movieService.SelectMoviesByCategoryAsync(new SelectMoviesDto
        {
            CategoryIds = [comedyCat.Id],
            OrderType = MovieOrderType.ByDuration,
            OrderDirection = OrderDirection.Ascending,
            Paginate = new Paginate {Count = 1, Page = 0}
        });
        Assert.Equal(1, comedyMovies.Count);
        Assert.Equal(comedyMovies[0].Id, comedyMovie.Id);
    }

    [Fact]
    public async Task CanValidateSelectMoviesDtoTest()
    {
        var selectValidator = new SelectMoviesDtoValidator();
        var validSelect = new SelectMoviesDto
        {
            CategoryIds = [Guid.NewGuid()]
        };
        var result = selectValidator.TestValidate(validSelect);
        Assert.True(result.IsValid);

        var invalidSelect = new SelectMoviesDto();
        result = selectValidator.TestValidate(invalidSelect);
        Assert.False(result.IsValid);
    }
}