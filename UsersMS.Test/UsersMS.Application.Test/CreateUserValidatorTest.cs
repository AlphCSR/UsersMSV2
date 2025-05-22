using UsersMS.Application.Validator;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Commons.Enums;
using Xunit;

public class CreateUserValidatorTest
{
    private readonly CreateUserValidator _validator = new CreateUserValidator();

    [Fact]
    public void Should_Fail_When_Email_Is_Null()
    {
        var dto = new CreateUserDto { Email = null };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new CreateUserDto { Email = "no-es-un-email" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Too_Short()
    {
        var dto = new CreateUserDto { Password = "123" };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Null()
    {
        var dto = new CreateUserDto { Name = null };
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Pass_With_Valid_Data()
    {
        var dto = new CreateUserDto
        {
            Email = "test@email.com",
            Password = "Password1",
            Name = "Nombre",
            LastName = "Apellido",
            DocumentId = "123456",
            Role = UsersMS.Commons.Enums.UserRole.Administrador,
            Phone = "12345678",
            Address = "Calle 123",
            State = UsersMS.Commons.Enums.UserState.Active
        };
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
