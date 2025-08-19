using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data;
using MovieRecommendation.Data.Movie;
using MovieRecommendation.Dtos.Movie;

namespace MovieRecommendation.Services.Movie;

public class DbMovieService : IMovieService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public DbMovieService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private async Task<MovieData?> GetMovieDataByIdAsync(Guid id)
    {
        var movie = await _dbContext.Movies.Include(data => data.Categories).FirstOrDefaultAsync(m => m.Id == id);
        return movie;
    }

    public async Task<List<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await _dbContext.Movies.Include(data => data.Categories).ToListAsync();
        return _mapper.Map<List<MovieDto>>(movies);
    }

    public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
    {
        var movie = await GetMovieDataByIdAsync(id);
        return _mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto createMovieDto)
    {
        var movie = new MovieData
        {
            Id = Guid.NewGuid(),
            Title = createMovieDto.Title,
            Description = createMovieDto.Description,
            DurationMins = createMovieDto.DurationMins,
            ReleaseYear = createMovieDto.ReleaseYear,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Categories = createMovieDto.CategoryIds
                .Select(id => _dbContext.Categories.Find(id))
                .Where(c => c != null)
                .ToList()!
        };

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDto?> UpdateMovieAsync(Guid id, UpdateMovieDto updateMovieDto)
    {
        var movie = await GetMovieDataByIdAsync(id);
        if (movie == null) return null;

        if (updateMovieDto.Title != null) movie.Title = updateMovieDto.Title;
        if (updateMovieDto.Description != null) movie.Description = updateMovieDto.Description;
        if (updateMovieDto.DurationMins != null) movie.DurationMins = (int)updateMovieDto.DurationMins;
        if (updateMovieDto.ReleaseYear != null) movie.ReleaseYear = updateMovieDto.ReleaseYear;
        if (updateMovieDto.CategoryIds != null)
        {
            movie.Categories.Clear();
            movie.Categories = updateMovieDto.CategoryIds
                .Select(id => _dbContext.Categories.Find(id))
                .Where(c => c != null)
                .ToList()!;
        }

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<MovieDto>(movie);
    }

    public async Task DeleteMovieByIdAsync(Guid id)
    {
        var movie = await _dbContext.Movies.FindAsync(id);
        if (movie != null)
        {
            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<MovieDto>> SelectMoviesByCategoryAsync(SelectMoviesDto selectMoviesDto)
    {
        var movies = await _dbContext.Movies.Include(data => data.Categories).ToListAsync();

        // filter for category
        movies = movies
            .Where(m => m.Categories.Select(e => e.Id).Intersect(selectMoviesDto.CategoryIds).Any())
            .ToList();

        // sort 
        switch (selectMoviesDto.OrderType)
        {
            case MovieOrderType.ByTitle:
                movies.Sort((a, b) =>
                    selectMoviesDto.OrderDirection == OrderDirection.Ascending
                        ? String.Compare(a.Title, b.Title, StringComparison.Ordinal)
                        : String.Compare(b.Title, a.Title, StringComparison.Ordinal)
                );
                break;
            case MovieOrderType.ByDuration:
                movies.Sort((a, b) =>
                    selectMoviesDto.OrderDirection == OrderDirection.Ascending
                        ? a.DurationMins.CompareTo(b.DurationMins)
                        : b.DurationMins.CompareTo(a.DurationMins)
                );
                break;
            case MovieOrderType.ByRating:
                movies.Sort((a, b) =>
                    selectMoviesDto.OrderDirection == OrderDirection.Ascending
                        ? a.AvarageRating.CompareTo(b.AvarageRating)
                        : b.AvarageRating.CompareTo(a.AvarageRating)
                );
                break;
            case MovieOrderType.ByReleaseYear:
                movies.Sort((a, b) =>
                    selectMoviesDto.OrderDirection == OrderDirection.Ascending
                        ? (a.ReleaseYear ?? 0).CompareTo(b.ReleaseYear ?? 0)
                        : (b.ReleaseYear ?? 0).CompareTo(a.ReleaseYear ?? 0)
                );
                break;
        }

        // paginate
        if (selectMoviesDto.Count != null)
        {
            movies =  movies.Take((int)selectMoviesDto.Count).ToList();
        }

        return _mapper.Map<List<MovieDto>>(movies);
    }
}