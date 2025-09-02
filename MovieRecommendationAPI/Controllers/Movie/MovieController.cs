using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Dtos.Movie;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;
using MovieRecommendation.Services.Review;
using QuestPDF.Fluent;

namespace MovieRecommendation.Controllers.Movie;

[ApiController]
[Route("api/movie")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IUserService _userService;
    private readonly IReviewService _reviewService;
    private readonly ICategoryService _categoryService;

    public MovieController(IMovieService movieService, IUserService userService, IReviewService reviewService,
        ICategoryService categoryService)
    {
        _movieService = movieService;
        _userService = userService;
        _reviewService = reviewService;
        _categoryService = categoryService;
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

    [HttpGet("details")]
    public async Task<IActionResult> GetMovieDetailsById([FromQuery] FetchMovieDetailsDto fetchDto)
    {
        var details = await _movieService.GetMovieDetailsAsync(fetchDto);
        if (details == null) return NotFound();
        return Ok(details);
    }

    [HttpGet("details/pdf")]
    public async Task<IActionResult> GetMovieDetailsPdfById([FromQuery] FetchMovieDetailsDto fetchDto)
    {
        var details = await _movieService.GetMovieDetailsAsync(fetchDto);
        if (details == null) return NotFound();

        var categories = new List<CategoryDto?>();
        foreach (var id in details.CategoryIds)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);
            categories.Add(categoryDto);
        }

        var reviews = new List<ReviewDto?>();
        var reviewUsers = new List<UserDto?>();
        foreach (var id in details.ReviewIds)
        {
            var reviewDto = await _reviewService.GetReviewByIdAsync(id);
            reviews.Add(reviewDto);

            if (reviewDto != null)
            {
                var userDto = await _userService.GetUserByIdAsync(reviewDto.UserId);
                reviewUsers.Add(userDto);
            }
        }

        var document = new MovieDetailsDocument(details, categories, reviews, reviewUsers);
        var pdfBytes = document.GeneratePdf();

        return File(pdfBytes, "application/pdf", "movie_details.pdf");
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