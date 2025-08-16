using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Validators.Auth;

namespace MovieRecommendation.Tests.Auth;

public class LoginTests
{
    [Fact]
    public async Task CanLoginTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetAuthMapper();
        var userService = new DbUserService(dbContext, mapper);

        // generate dummy users
        var dummyRegisterDtos = new List<RegisterDto>(10);
        for (int i = 0; i < dummyRegisterDtos.Count; i++)
        {
            var user = new RegisterDto
            {
                Name = $"user name {i}",
                Surname = $"user surname {i}",
                Email = $"user{i}@gmail.com",
                Password = $"user password {i}",
            };
            dummyRegisterDtos[i] = user;
        }

        // register generated users
        for (int i = 0; i < dummyRegisterDtos.Count; i++)
        {
            await userService.RegisterAsync(new RegisterDto
            {
                Name = dummyRegisterDtos[i].Name,
                Surname = dummyRegisterDtos[i].Surname,
                Email = dummyRegisterDtos[i].Email,
                Password = dummyRegisterDtos[i].Password,
            });
        }

        for (int i = 0; i < dummyRegisterDtos.Count; i++)
        {
            var result = await userService.LoginAsync(new LoginDto
            {
                Email = dummyRegisterDtos[i].Email,
                Password = dummyRegisterDtos[i].Password,
            });
            Assert.NotNull(result);
            Assert.Equal(dummyRegisterDtos[i].Name, result.Name);
            Assert.Equal(dummyRegisterDtos[i].Surname, result.Surname);
            Assert.Equal(dummyRegisterDtos[i].Email, result.Email);
        }
    }

    [Fact]
    public async Task DoesntLoginInvalidLoginTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetAuthMapper();
        var userService = new DbUserService(dbContext, mapper);

        await userService.RegisterAsync(new RegisterDto
        {
            Name = "test",
            Surname = "test",
            Email = "test@gmail.com",
            Password = "password",
        });

        var validLoginResult = await userService.LoginAsync(new LoginDto
        {
            Email = "test@gmail.com",
            Password = "password",
        });
        Assert.NotNull(validLoginResult);
        Assert.Equal("test", validLoginResult.Name);
        Assert.Equal("test", validLoginResult.Surname);
        Assert.Equal("test@gmail.com", validLoginResult.Email);

        var invalidEmailLoginResult = await userService.LoginAsync(new LoginDto
        {
            Email = "testgmail.com",
            Password = "password",
        });
        Assert.Null(invalidEmailLoginResult);

        var invalidPasswordLoginResult = await userService.LoginAsync(new LoginDto
        {
            Email = "test@gmail.com",
            Password = "password123",
        });
        Assert.Null(invalidPasswordLoginResult);
    }

    [Fact]
    public void CanValidateLoginDtoTest()
    {
        var validator = new LoginDtoValidator();

        var validDto = new LoginDto { Email = "test@gmail.com", Password = "123123123", };
        var validationResult = validator.TestValidate(validDto);
        Assert.True(validationResult.IsValid);

        var invalidMail = new LoginDto { Email = "testgmail.com", Password = "123123123", };
        validationResult = validator.TestValidate(invalidMail);
        Assert.False(validationResult.IsValid);

        var invalidPassword = new LoginDto { Email = "test@gmail.com", Password = "12312", };
        validationResult = validator.TestValidate(invalidPassword);
        Assert.False(validationResult.IsValid);
    }

    [Fact]
    public async Task RegisterAndLoginWithGoogleTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var mapper = TestUtils.GetAuthMapper();
        var userService = new DbUserService(dbContext, mapper);

        var loginDto = new GoogleLoginDto { Email = "test@mail", Name = "Name", Surname = "Surname"};
        
        // user is created here
        var createdUserDto = await userService.LoginOrCreateGoogleUserAsync(loginDto);
        Assert.NotNull(createdUserDto);
        Assert.Equal(loginDto.Email, createdUserDto.Email);
        Assert.Equal(loginDto.Name, createdUserDto.Name);
        Assert.Equal(loginDto.Surname, createdUserDto.Surname);
        
        // user is already in the database it is returned from there
        var foundUserDto = await userService.LoginOrCreateGoogleUserAsync(loginDto);
        Assert.NotNull(foundUserDto);
        Assert.Equal(loginDto.Email, foundUserDto.Email);
        Assert.Equal(loginDto.Name, foundUserDto.Name);
        Assert.Equal(loginDto.Surname, foundUserDto.Surname);
    }
}