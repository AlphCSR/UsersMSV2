using System.Threading.Tasks;
using UsersMS.Application.Validator;
using UsersMS.Commons.Dtos.Request;
using FluentValidation;
using UsersMS.Infrastructure.Exceptions;
using Xunit;

public class ValidatorBaseTest
{
    private class DummyValidator : ValidatorBase<CreateUserDto>
    {
        public DummyValidator()
        {
            RuleFor(x => x.Email).NotNull();
        }
    }

    [Fact]
    public async Task ValidateRequest_Should_Throw_When_Invalid()
    {
        var validator = new DummyValidator();
        var dto = new CreateUserDto { Email = null };
        await Assert.ThrowsAsync<ValidatorException>(() => validator.ValidateRequest(dto));
    }

    [Fact]
    public async Task ValidateRequest_Should_Return_True_When_Valid()
    {
        var validator = new DummyValidator();
        var dto = new CreateUserDto { Email = "test@email.com" };
        var result = await validator.ValidateRequest(dto);
        Assert.True(result);
    }
}
