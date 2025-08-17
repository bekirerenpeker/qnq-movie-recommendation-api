using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Validators.Auth;

namespace MovieRecommendation.Tests.Auth;

public class UserServiceTests
{
    [Fact]
    public async Task CanCreateUserTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetAuthMapper();
        var userService = new DbUserService(dbContext, mapper);

        var createUserDto = new CreateUserDto
        {
            Name = "Name",
            Surname = "Surname",
            Email = "test@gmail.com",
            Password = "123123123",
        };

        var user = await userService.CreateUserAsync(createUserDto);
        Assert.NotNull(user);
        Assert.Equal(createUserDto.Name, user.Name);
        Assert.Equal(createUserDto.Surname, user.Surname);
        Assert.Equal(createUserDto.Email, user.Email);
    }

    [Fact]
    public async Task CanReadUserTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetAuthMapper();
        var userService = new DbUserService(dbContext, mapper);

        var createUserDto = new CreateUserDto
        {
            Name = "Name",
            Surname = "Surname",
            Email = "test@gmail.com",
            Password = "123123123",
        };

        var user = await userService.CreateUserAsync(createUserDto);
        Assert.NotNull(user);

        var users = await userService.GetAllUsersAsync();
        Assert.NotNull(users);
        Assert.Equal(1, users.Count);

        var userById = await userService.GetUserByIdAsync(user.Id);
        Assert.NotNull(userById);
        Assert.Equal(user.Name, userById.Name);
        Assert.Equal(user.Surname, userById.Surname);
        Assert.Equal(user.Email, userById.Email);
    }

    [Fact]
    public async Task CanUpdateUserTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetAuthMapper();
        var userService = new DbUserService(dbContext, mapper);

        var createUserDto = new CreateUserDto
        {
            Name = "Name",
            Surname = "Surname",
            Email = "test@gmail.com",
            Password = "123123123",
        };

        var user = await userService.CreateUserAsync(createUserDto);
        Assert.NotNull(user);

        var updateUserDto = new UpdateUserDto
        {
            Name = "New Name",
            Surname = "new Surname",
            Email = "newtest@gmail.com",
            Password = "122345678",
        };

        var updatedUser = await userService.UpdateUserAsync(user.Id, updateUserDto);
        Assert.NotNull(updatedUser);
        Assert.Equal(updateUserDto.Name, updatedUser.Name);
        Assert.Equal(updateUserDto.Surname, updatedUser.Surname);
        Assert.Equal(updateUserDto.Email, updatedUser.Email);
    }

    [Fact]
    public void CanValidateInputsTest()
    {
        var createValidator = new CreateUserDtoValidator();

        var validCreateDto = new CreateUserDto
            { Name = "Name", Surname = "Surname", Email = "test@gmail.com", Password = "123123123", };
        var createValidationResult = createValidator.TestValidate(validCreateDto);
        Assert.True(createValidationResult.IsValid);

        var invalidCreateDto = new CreateUserDto { Name = "", Surname="",Email = "testgmail.com", Password = "123123", };
        createValidationResult = createValidator.TestValidate(invalidCreateDto);
        Assert.False(createValidationResult.IsValid);
    }
}