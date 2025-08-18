using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Validators.Movie;

namespace MovieRecommendation.Tests.Movie;

public class MovieServiceTests
{
    [Fact]
    public async Task CanCreateMovieTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var movieService = new DbMovieService(dbContext, mapper);

        var createMovieDto = new CreateMovieDto
        {
            Title = "Test Movie",
            Description = "Test Movie",
            DurationMins = 100,
            ReleaseYear = 2021,
            CategoryIds = [Guid.NewGuid()] // should not add this since it is invalid
        };

        var movieDto = await movieService.CreateMovieAsync(createMovieDto);
        Assert.Equal(createMovieDto.Title, movieDto.Title);
        Assert.Equal(createMovieDto.Description, movieDto.Description);
        Assert.Equal(createMovieDto.DurationMins, movieDto.DurationMins);
        Assert.Equal(createMovieDto.ReleaseYear, movieDto.ReleaseYear);
        Assert.Empty(movieDto.CategoryIds);
    }

    [Fact]
    public async Task CanAddCategoriesToMovieTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var movieService = new DbMovieService(dbContext, mapper);
        var categoryService = new DbCategoryService(dbContext, mapper);

        var horrorCategoryDto = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Horror" });
        var actionCategoryDto = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Action" });
        var comedyCategoryDto = await categoryService.CreateCategoryAsync(new CreateCategoryDto { Name = "Comedy" });

        var createMovieDto = new CreateMovieDto
        {
            Title = "Test Movie",
            Description = "Test Movie",
            DurationMins = 100,
            ReleaseYear = 2021,
            CategoryIds =
            [
                horrorCategoryDto.Id,
                comedyCategoryDto.Id,
                actionCategoryDto.Id,
                Guid.NewGuid(), // should ignore this
            ]
        };

        var movieDto = await movieService.CreateMovieAsync(createMovieDto);
        Assert.Equal(3, movieDto.CategoryIds.Count);
        
        horrorCategoryDto = await categoryService.GetCategoryByIdAsync(horrorCategoryDto.Id);
        Assert.NotNull(horrorCategoryDto);
        Assert.Single(horrorCategoryDto.MovieIds);
        
        actionCategoryDto = await categoryService.GetCategoryByIdAsync(actionCategoryDto.Id);
        Assert.NotNull(actionCategoryDto);
        Assert.Single(actionCategoryDto.MovieIds);
        
        comedyCategoryDto = await categoryService.GetCategoryByIdAsync(comedyCategoryDto.Id);
        Assert.NotNull(comedyCategoryDto);
        Assert.Single(comedyCategoryDto.MovieIds);
        
    }

    [Fact]
    public async Task CanGetMovieTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var movieService = new DbMovieService(dbContext, mapper);

        var createMovieDto = new CreateMovieDto
        {
            Title = "Test Movie",
            Description = "Test Movie",
            DurationMins = 100,
            ReleaseYear = 2021,
            CategoryIds = [Guid.NewGuid()] // should not add this since it is invalid
        };

        var movieDto = await movieService.CreateMovieAsync(createMovieDto);

        var allMovieDtos = await movieService.GetAllMoviesAsync();
        Assert.Single(allMovieDtos);

        var readMovieDto = await movieService.GetMovieByIdAsync(movieDto.Id);
        Assert.NotNull(readMovieDto);
        Assert.Equal(readMovieDto.Title, movieDto.Title);
        Assert.Equal(readMovieDto.Description, movieDto.Description);
        Assert.Equal(readMovieDto.DurationMins, movieDto.DurationMins);
        Assert.Equal(readMovieDto.ReleaseYear, movieDto.ReleaseYear);
        Assert.Empty(readMovieDto.CategoryIds);
    }

    [Fact]
    public async Task CanUpdateMovieTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var movieService = new DbMovieService(dbContext, mapper);

        var createMovieDto = new CreateMovieDto
        {
            Title = "Test Movie",
            Description = "Test Movie",
            DurationMins = 100,
            ReleaseYear = 2021,
            CategoryIds = [Guid.NewGuid()] // should not add this since it is invalid
        };

        var movieDto = await movieService.CreateMovieAsync(createMovieDto);

        var updateMovieDto = new UpdateMovieDto
        {
            Title = "New Title",
            Description = "New Description",
            ReleaseYear = 2025,
            DurationMins = 130,
        };

        var updatedMovieDto = await movieService.UpdateMovieAsync(movieDto.Id, updateMovieDto);
        Assert.NotNull(updatedMovieDto);
        Assert.Equal(updateMovieDto.Title, updatedMovieDto.Title);
        Assert.Equal(updateMovieDto.Description, updatedMovieDto.Description);
        Assert.Equal(updateMovieDto.DurationMins, updatedMovieDto.DurationMins);
        Assert.Equal(updateMovieDto.ReleaseYear, updatedMovieDto.ReleaseYear);
    }

    [Fact]
    public async Task CanDeleteMovieTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var movieService = new DbMovieService(dbContext, mapper);

        var createMovieDto = new CreateMovieDto
        {
            Title = "Test Movie",
            Description = "Test Movie",
            DurationMins = 100,
            ReleaseYear = 2021,
            CategoryIds = [Guid.NewGuid()] // should not add this since it is invalid
        };

        var movieDto = await movieService.CreateMovieAsync(createMovieDto);

        await movieService.DeleteMovieByIdAsync(movieDto.Id);

        var allMovieDtos = await movieService.GetAllMoviesAsync();
        Assert.Empty(allMovieDtos);
    }

    [Fact]
    public async Task CanValidateInputTest()
    {
        var createValidator = new CreateMovieDtoValidator();
        var validCreate = new CreateMovieDto
        {
            Title = "Test Movie",
            Description = "Test Movie",
            DurationMins = 100,
            ReleaseYear = 2021,
            CategoryIds = [Guid.NewGuid()] // should not add this since it is invalid
        };
        var result = createValidator.TestValidate(validCreate);
        Assert.True(result.IsValid);

        var invalidCreate = new CreateMovieDto { };
        result = createValidator.TestValidate(invalidCreate);
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
    }
}