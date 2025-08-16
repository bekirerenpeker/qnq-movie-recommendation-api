using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Services.Auth;

namespace MovieRecommendation.Controllers.Auth;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
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

    private async Task<bool> IsAuthorized(Guid id)
    {
        var userId = GetCurrentUserId();
        return (userId != null) && (userId == id || await IsAdmin());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        if (!(await IsAdmin())) return Unauthorized();
        var userDtos = await _userService.GetAllUsersAsync();
        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        if (!(await IsAuthorized(id))) return Unauthorized();
        var userDto = await _userService.GetUserByIdAsync(id);
        if (userDto is null) return NotFound();
        return Ok(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(CreateUserDto createUserDto)
    {
        if (!(await IsAdmin())) return Unauthorized();
        var userDto = await _userService.AddUserAsync(createUserDto);
        return Ok(userDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
    {
        if (!(await IsAuthorized(id))) return Unauthorized();
        var userDto = await _userService.UpdateUserAsync(id, updateUserDto);
        if (userDto is null) return NotFound();
        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserById(Guid id)
    {
        if (!(await IsAuthorized(id))) return Unauthorized();

        if (await IsAdmin() && (await _userService.AdminCount() == 1))
        {
            return BadRequest("The user you trying to delete is the only admin. Please create another admin account to delete this");
        }
        
        await _userService.DeleteUserByIdAsync(id);
        return NoContent();
    }
}