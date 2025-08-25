namespace MovieRecommendation.Dtos.Auth;

public class CreateUserDto
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string SocialLoginProvider { get; set; } = "Local";
}

public class UpdateUserDto
{
    public string? Name { get; set; } = null;
    public string? Surname { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Password { get; set; } = null;
}

public class UserDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsAdmin { get; set; } = false;

    public List<Guid> WatchedMovieIds { get; set; } = [];
}

public class WatchlistDto
{
    public Guid UserId { get; set; } = Guid.Empty;
    public List<Guid> WatchedMovieIds { get; set; } = [];
}