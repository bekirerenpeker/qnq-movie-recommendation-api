using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Validators.Movie;

namespace MovieRecommendation.Tests.Movie;

public class CategoryServiceTests
{
    [Fact]
    public async Task CanCreateCategoryTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var categoryService = new DbCategoryService(dbContext, mapper);

        var createCategoryDto = new CreateCategoryDto { Name = "Test" };
        var categoryDto = await categoryService.CreateCategoryAsync(createCategoryDto);
        Assert.Equal("Test", categoryDto.Name);
    }

    [Fact]
    public async Task CanReadCategoryTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var categoryService = new DbCategoryService(dbContext, mapper);

        var createCategoryDto = new CreateCategoryDto { Name = "Test" };
        var categoryDto = await categoryService.CreateCategoryAsync(createCategoryDto);
        Assert.Equal(createCategoryDto.Name, categoryDto.Name);

        var allCategories = await categoryService.GetAllCategorysAsync();
        Assert.Single(allCategories);

        var readCategoryDto = await categoryService.GetCategoryByIdAsync(categoryDto.Id);
        Assert.NotNull(readCategoryDto);
        Assert.Equal(createCategoryDto.Name, readCategoryDto.Name);
    }

    [Fact]
    public async Task CanUpdateCategoryTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var categoryService = new DbCategoryService(dbContext, mapper);

        var createCategoryDto = new CreateCategoryDto { Name = "Test" };
        var categoryDto = await categoryService.CreateCategoryAsync(createCategoryDto);
        Assert.Equal(createCategoryDto.Name, categoryDto.Name);

        var updateCategoryDto = new UpdateCategoryDto { Name = "New Name" };
        var newCategoryDto = await categoryService.UpdateCategoryAsync(categoryDto.Id, updateCategoryDto);
        Assert.NotNull(newCategoryDto);
        Assert.Equal(updateCategoryDto.Name, newCategoryDto.Name);
    }

    [Fact]
    public async Task CanDeleteCategoryTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetMovieMapper();
        var categoryService = new DbCategoryService(dbContext, mapper);

        var createCategoryDto = new CreateCategoryDto { Name = "Test" };
        var categoryDto = await categoryService.CreateCategoryAsync(createCategoryDto);
        Assert.Equal(createCategoryDto.Name, categoryDto.Name);

        await categoryService.DeleteCategoryByIdAsync(categoryDto.Id);

        var allCategories = await categoryService.GetAllCategorysAsync();
        Assert.Empty(allCategories);
    }

    [Fact]
    public async Task CanValidateInputTest()
    {
        var createValidator = new CreateCategoryDtoValidator();
        var validCreate = new CreateCategoryDto() { Name = "Test" };
        var result = createValidator.TestValidate(validCreate);
        Assert.True(result.IsValid);

        var invalidCreate = new CreateCategoryDto() { Name = "" };
        result = createValidator.TestValidate(invalidCreate);
        Assert.False(result.IsValid);
        Assert.Equal(1, result.Errors.Count);
    }
}