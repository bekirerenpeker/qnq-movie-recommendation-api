using FluentValidation.TestHelper;
using MovieRecommendation.Dtos.Auth;
using MovieRecommendation.Services.Auth;
using MovieRecommendation.Validators.Auth;

namespace MovieRecommendation.Tests.Auth;

public class RegisterTests
{
    [Fact]
    public async Task RegisterCreatesUserTest()
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

        Assert.Empty(dummyRegisterDtos);

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

        var users = await userService.GetAllUsersAsync();
        Assert.Equal(dummyRegisterDtos.Count, users.Count);
        for (int i = 0; i < users.Count; i++)
        {
            Assert.NotNull(users[i]);
            Assert.Equal(dummyRegisterDtos[i].Name, users[i].Name);
            Assert.Equal(dummyRegisterDtos[i].Surname, users[i].Surname);
            Assert.Equal(dummyRegisterDtos[i].Email, users[i].Email);
        }
    }

    [Fact]
    public void RegisterValidatesInputTest()
    {
        var validator = new RegisterDtoValidator();

        var validDto = new RegisterDto
            { Name = "Name", Surname = "Surname", Email = "test@gmail.com", Password = "123123123", };
        var validationResult = validator.TestValidate(validDto);
        Assert.True(validationResult.IsValid);

        var invalidName = new RegisterDto
            { Name = "", Surname = "Surname", Email = "test@gmail.com", Password = "123123123", };
        validationResult = validator.TestValidate(invalidName);
        Assert.False(validationResult.IsValid);

        var invalidSurname = new RegisterDto
            { Name = "Name", Surname = "", Email = "test@gmail.com", Password = "123123123", };
        validationResult = validator.TestValidate(invalidSurname);
        Assert.False(validationResult.IsValid);

        var invalidMail = new RegisterDto
            { Name = "Name", Surname = "Surname", Email = "testgmail.com", Password = "123123123", };
        validationResult = validator.TestValidate(invalidMail);
        Assert.False(validationResult.IsValid);

        var invalidPassword = new RegisterDto
            { Name = "Name", Surname = "Surname", Email = "test@gmail.com", Password = "12312", };
        validationResult = validator.TestValidate(invalidPassword);
        Assert.False(validationResult.IsValid);
    }
}