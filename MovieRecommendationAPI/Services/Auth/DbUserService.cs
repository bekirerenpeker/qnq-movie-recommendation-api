using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Data;
using MovieRecommendation.Data.Auth;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Utils;

namespace MovieRecommendation.Services.Auth;

public class DbUserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public DbUserService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private async Task<UserData?> GetUserDataByIdAsync(Guid id)
    {
        var user = await _dbContext.Users
            .Include(data => data.WatchedMovies)
            .FirstOrDefaultAsync(m => m.Id == id);
        return user;
    }

    public async Task<UserDto?> LoginAsync(LoginDto loginDto)
    {
        var users = await _dbContext.Users.ToListAsync();
        var user = users.FirstOrDefault(u =>
            u.Email == loginDto.Email && HasherUtil.VerifyPassword(loginDto.Password, u.PasswordHash)
        );
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
    {
        var createUserDto = _mapper.Map<CreateUserDto>(registerDto);
        var userDto = await CreateUserAsync(createUserDto);
        return userDto;
    }

    public async Task<UserDto?> LoginOrCreateGoogleUserAsync(GoogleLoginDto loginDto)
    {
        Console.WriteLine(loginDto.Email + " " + loginDto.Name + " " + loginDto.Surname);
        var users = await _dbContext.Users.ToListAsync();
        var user = users.FirstOrDefault(u => u.Email == loginDto.Email);
        if (user != null) Console.WriteLine("found user: " + user.Email);

        if (user == null)
        {
            await CreateUserAsync(new CreateUserDto
                {
                    Name = loginDto.Name,
                    Surname = loginDto.Surname,
                    Email = loginDto.Email,
                    Password = "",
                    SocialLoginProvider = "Google",
                }
            );
            users = await _dbContext.Users.ToListAsync();
            user = users.FirstOrDefault(u => u.Email == loginDto.Email);
            Console.WriteLine("added user: " + user.Email);
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _dbContext.Users.Include(data => data.WatchedMovies).ToListAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await GetUserDataByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<UserData>(createUserDto);
        user.Id = Guid.NewGuid();
        user.PasswordHash = HasherUtil.HashPassword(createUserDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null) return null;

        if (updateUserDto.Name != null) user.Name = updateUserDto.Name;
        if (updateUserDto.Surname != null) user.Surname = updateUserDto.Surname;
        if (updateUserDto.Email != null) user.Email = updateUserDto.Email;
        if (updateUserDto.Password != null) user.PasswordHash = HasherUtil.HashPassword(updateUserDto.Password);
        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<WatchlistDto?> GetWatchedMovieIdsAsync(Guid id)
    {
        var user = await GetUserDataByIdAsync(id);
        if (user == null) return null;
        return new WatchlistDto
        {
            UserId = user.Id,
            WatchedMovieIds = user.WatchedMovies.Select(movie => movie.Id).ToList(),
        };
    }

    public async Task SetMovieWatchedStateAsync(Guid userId, Guid movieId, bool isWatched)
    {
        var user = await GetUserDataByIdAsync(userId);
        if (user == null) return;

        var index = user.WatchedMovies.FindIndex(movie => movie.Id == movieId);
        var inWatchlist = index != -1;

        if (isWatched && !inWatchlist)
        {
            var movie = await _dbContext.Movies.FindAsync(movieId);
            if (movie == null) return;
            user.WatchedMovies.Add(movie);
        }
        else if (!isWatched && inWatchlist)
        {
            user.WatchedMovies.RemoveAt(index);
        }
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> AdminCount()
    {
        var users = await _dbContext.Users.ToListAsync();
        return users.Count(user => user.IsAdmin);
    }
}