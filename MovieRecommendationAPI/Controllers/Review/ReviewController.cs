using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Review;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Review;

namespace MovieRecommendation.Controllers.Review;

[ApiController]
[Route("api/review")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IUserService _userService;

    public ReviewController(IReviewService reviewService , IUserService userService)
    {
        _reviewService = reviewService;
        _userService = userService;
    }

    private Guid? GetCurrentUserId()
    {
        var userIdString = base.User.FindFirstValue(ClaimTypes.NameIdentifier);
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReviewAsync(CreateReviewDto createReviewDto)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var reviewDto = await _reviewService.CreateReviewAsync((Guid)userId, createReviewDto);
        if (reviewDto == null) return BadRequest();
        return Ok(reviewDto);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReviewAsync(Guid id)
    {
        var reviewDto = await _reviewService.GetReviewByIdAsync(id);
        if (reviewDto == null) return NotFound();
        if (GetCurrentUserId() != reviewDto.UserId && !(await IsAdmin())) return Unauthorized();

        await _reviewService.DeleteReviewByIdAsync(id);
        return NoContent();
    }
}