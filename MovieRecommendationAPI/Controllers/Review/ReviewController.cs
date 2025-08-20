using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Review;

namespace MovieRecommendation.Controllers.Review;

[ApiController]
[Route("api/review")]
public class ReviewController : ControllerBase
{
    private readonly  IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(Guid id)
    {
        var reviewDto = await _reviewService.GetReviewByIdAsync(id);
        if (reviewDto == null) return NotFound();
        return Ok(reviewDto);
    }

    [HttpGet("by-movie-and-user-id")]
    public async Task<IActionResult> GetReviewByUserAndMovieIdAsync(Guid userId, Guid movieId)
    {
        var reviewDto = await _reviewService.GetReviewByUserAndMovieIdAsync(userId, movieId);
        if (reviewDto == null) return NotFound();
        return Ok(reviewDto);
    }

    [HttpGet("by-movie-id")]
    public async Task<IActionResult> GetMovieReviewsAsync(Guid movieId)
    {
        var reviewDto = await _reviewService.GetMovieReviewsAsync(movieId);
        return Ok(reviewDto);
    }

    [HttpGet("by-user-id")]
    public async Task<IActionResult> GetUserReviewsAsync(Guid userId)
    {
        var reviewDto = await _reviewService.GetUserReviewsAsync(userId);
        return Ok(reviewDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReviewAsync(CreateReviewDto createReviewDto)
    {
        var reviewDto = await _reviewService.CreateReviewAsync(createReviewDto);
        if(reviewDto == null) return NotFound();
        return Ok(reviewDto);
    }
}