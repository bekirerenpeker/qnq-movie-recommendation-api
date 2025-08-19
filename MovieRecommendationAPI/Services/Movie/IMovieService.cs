using MovieRecommendation.Dtos.Movie;

namespace MovieRecommendation.Services.Movie;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(Guid id);
    Task<MovieDto> CreateMovieAsync(CreateMovieDto createMovieDto);
    Task<MovieDto?> UpdateMovieAsync(Guid id, UpdateMovieDto updateMovieDto);
    Task DeleteMovieByIdAsync(Guid id);
    Task<List<MovieDto>> ListMoviesByCategoryAsync(SelectMoviesDto selectMoviesDto);
}