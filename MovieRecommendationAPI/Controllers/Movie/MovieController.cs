using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Movie;

namespace MovieRecommendation.Controllers.Movie;

[ApiController]
[Route("api/movie")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        var movieDtos = await _movieService.GetAllMoviesAsync();
        return Ok(movieDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(Guid id)
    {
        var movieDto = await _movieService.GetMovieByIdAsync(id);
        if(movieDto == null)  return NotFound();
        return Ok(movieDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(CreateMovieDto createMovieDto)
    {
        var movieDto = await _movieService.CreateMovieAsync(createMovieDto);
        return Ok(movieDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updateMovieDto)
    {
        var movieDto = await _movieService.UpdateMovieAsync(id,  updateMovieDto);
        if(movieDto == null) return NotFound();
        return Ok(movieDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        await _movieService.DeleteMovieByIdAsync(id);
        return NoContent();
    }
}