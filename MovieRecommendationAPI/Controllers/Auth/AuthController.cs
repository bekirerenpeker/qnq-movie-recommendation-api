using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Services.Auth;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MovieRecommendation.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public AuthController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var userDto = await _userService.LoginAsync(loginDto);
        if (userDto == null) return NotFound();

        var jwtToken = GenerateJwtToken(userDto);

        return Ok(new { Token = jwtToken });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var userDto = await _userService.RegisterAsync(registerDto);
        if (userDto == null) return NotFound();

        var jwtToken = GenerateJwtToken(userDto);

        return Ok(new { Token = jwtToken });
    }

    [HttpGet("login/google")]
    public IActionResult GoogleLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback", "Auth", null, Request.Scheme)
        };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("login/google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded) return BadRequest("Google authentication failed");

        var claims = result.Principal.Claims.ToList();
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (email == null || name == null) return BadRequest("Couldn't get profile info from Google");

        var userDto = await _userService.LoginOrCreateGoogleUserAsync(new GoogleLoginDto
        {
            Email = email,
            Name = name
        });
        if (userDto == null) return BadRequest("Couldn't create user");

        var jwtToken = GenerateJwtToken(userDto);
        return Ok(new { Token = jwtToken });
    }

    private string GenerateJwtToken(UserDto userDto)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userDto.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userDto.Email),
            new Claim(JwtRegisteredClaimNames.Name, userDto.Name + " " + userDto.Surname),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserData()
    {
        var userId = base.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
        return user == null ? NotFound() : Ok(user);
    }
};