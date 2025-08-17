using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;

namespace MovieRecommendation.Controllers.Movie;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public CategoryController(ICategoryService categoryService,  IUserService userService)
    {
        _categoryService = categoryService;
        _userService = userService;
    }

    private Guid? GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null) return null;
        return Guid.Parse(userIdString);
    }

    private async Task<bool> IsAdmin()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return false;
        var user = await _userService.GetUserByIdAsync((Guid)userId);
        return user != null && user.IsAdmin;
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
        if (categoryDto == null) NotFound();
        return Ok(categoryDto);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        if(!(await IsAdmin())) return Unauthorized();
        var categoryDto = await _categoryService.CreateCategoryAsync(createCategoryDto);
        return Ok(categoryDto);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        if(!(await IsAdmin())) return Unauthorized();
        var categoryDto = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
        if (categoryDto == null) NotFound();
        return Ok(categoryDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        if(!(await IsAdmin())) return Unauthorized();
        await _categoryService.DeleteCategoryByIdAsync(id);
        return NoContent();
    }
}