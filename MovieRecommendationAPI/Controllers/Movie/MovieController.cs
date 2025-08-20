using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;

namespace MovieRecommendation.Controllers.Movie;

[ApiController]
[Route("api/movie")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IUserService _userService;

    public MovieController(IMovieService movieService, IUserService userService)
    {
        _movieService = movieService;
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
    public async Task<IActionResult> GetAllMovies()
    {
        var movieDtos = await _movieService.GetAllMoviesAsync();
        return Ok(movieDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(Guid id)
    {
        var movieDto = await _movieService.GetMovieByIdAsync(id);
        if (movieDto == null) return NotFound();
        return Ok(movieDto);
    }

    [HttpGet("select")]
    public async Task<IActionResult> GetMoviesByCategory([FromQuery] SelectMoviesDto selectMoviesDto)
    {
        var selectedMovies = await _movieService.SelectMoviesByCategoryAsync(selectMoviesDto);
        return Ok(new PaginatedMoviesDto
        {
            Movies = selectedMovies,
            Paginate = selectMoviesDto.Paginate
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateMovie(CreateMovieDto createMovieDto)
    {
        if (!(await IsAdmin())) return Unauthorized();
        var movieDto = await _movieService.CreateMovieAsync(createMovieDto);
        return Ok(movieDto);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updateMovieDto)
    {
        if (!(await IsAdmin())) return Unauthorized();
        var movieDto = await _movieService.UpdateMovieAsync(id, updateMovieDto);
        if (movieDto == null) return NotFound();
        return Ok(movieDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        if (!(await IsAdmin())) return Unauthorized();
        await _movieService.DeleteMovieByIdAsync(id);
        return NoContent();
    }
}