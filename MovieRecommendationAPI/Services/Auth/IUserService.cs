using MovieRecommendation.Dtos.Auth;

namespace MovieRecommendation.Services.Auth;

public interface IUserService
{
    Task<UserDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> LoginOrCreateGoogleUserAsync(GoogleLoginDto loginDto);

    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task DeleteUserByIdAsync(Guid id);

    Task<WatchlistDto?> GetWatchedMovieIdsAsync(Guid id);
    
    Task<int> AdminCount();
};