/*
using MovieRecommendationAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

public class InMemUserService : IUserService
{
    private readonly List<UserData> _users = new List<UserData>();

    public Task<UserData?> LoginAsync(LoginRequest loginRequest)
    {
        var user = _users.FirstOrDefault(u =>
        {
            return u.Email == loginRequest.Email && HasherUtil.VerifyPassword(loginRequest.Password, u.PasswordHash);
        });

        return Task.FromResult(user);
    }

    public Task<UserData?> RegisterAsync(RegisterDto registerRequest)
    {
        var user = new UserData
        {
            Id = Guid.NewGuid(),
            Name = registerRequest.Name,
            Surname = registerRequest.Surname,
            Email = registerRequest.Email,
            PasswordHash = HasherUtil.HashPassword(registerRequest.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _users.Add(user);

        return Task.FromResult<UserData?>(user);
    }

    public Task<List<UserData>> GetAllUsersAsync()
    {
        return Task.FromResult(_users);
    }

    public Task<UserData?> GetUserByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<UserData> CreateUserAsync(UserData user)
    {
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<UserData?> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        int index = _users.FindIndex(0, _users.Count, u => u.Id == id);
        if (index != -1)
        {
            _users[index].Name = updateUserDto.Name;
            _users[index].Surname = updateUserDto.Surname;
            _users[index].Email = updateUserDto.Email;
            _users[index].PasswordHash = HasherUtil.HashPassword(updateUserDto.Password);
            _users[index].UpdatedAt = DateTime.UtcNow;
            return Task.FromResult<UserData?>(_users[index]);
        }
        else
        {
            return Task.FromResult<UserData?>(null);
        }
    }

    public void DeleteUserByIdAsync(Guid id)
    {
        _users.RemoveAll(u => u.Id == id);
    }
};
*/