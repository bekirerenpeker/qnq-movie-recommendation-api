using MovieRecommendation.Data.Movie;

namespace MovieRecommendation.Data.Auth;

public class UserData
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string SocialLoginProvider { get; set; } = "Local";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsAdmin { get; set; } = false;

    public List<MovieData> WatchedMovies { get; set; } = [];
};