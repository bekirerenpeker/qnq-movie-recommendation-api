using MovieRecommendation.Dtos.Movie;

namespace MovieRecommendation.Services.Movie;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategorysAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryDto> AddCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto);
    Task DeleteCategoryByIdAsync(Guid id);
}