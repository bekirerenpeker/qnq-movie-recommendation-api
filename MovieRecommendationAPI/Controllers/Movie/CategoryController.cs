using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Movie;

namespace MovieRecommendation.Controllers.Movie;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categoryDtos = await _categoryService.GetAllCategorysAsync();
        return Ok(categoryDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
        if (categoryDto == null) BadRequest();
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var categoryDto = await _categoryService.CreateCategoryAsync(createCategoryDto);
        return Ok(categoryDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        var categoryDto = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
        if (categoryDto == null) BadRequest();
        return Ok(categoryDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await _categoryService.DeleteCategoryByIdAsync(id);
        return Ok();
    }
}