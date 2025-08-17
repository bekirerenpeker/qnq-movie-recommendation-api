using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data;
using MovieRecommendation.Data.Movie;
using MovieRecommendation.Dtos.Movie;

namespace MovieRecommendation.Services.Movie;

public class DbCategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public DbCategoryService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetAllCategorysAsync()
    {
        var categoires = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<List<CategoryDto>>(categoires);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        var category = new CategoryData
        {
            Id = Guid.NewGuid(),
            Name = createCategoryDto.Name,
            Movies = []
        };
        
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null) return null;
        
        if (updateCategoryDto.Name != null) category.Name = updateCategoryDto.Name;
        
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteCategoryByIdAsync(Guid id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}