using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using UsersMS.Application.Handlers.Queries;
using UsersMS.Application.Queries;
using UsersMS.Commons.Dtos.Response;
using UsersMS.Core.Repositories;
using UsersMS.Commons.Enums;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Exceptions;
using Xunit;

public class GetUserQueryHandlerTest
{
    [Fact]
    public async Task Handle_Should_Return_User()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@email.com",
            Password = "Password1",
            DocumentId = "123456",
            Name = "Nombre",
            LastName = "Apellido",
            Phone = "12345678",
            Address = "Calle 123",
            Role = UsersMS.Commons.Enums.UserRole.Administrador,
            State = UsersMS.Commons.Enums.UserState.Active
        };
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        var handler = new GetUserQueryHandler(repoMock.Object);
        var result = await handler.Handle(new GetUserQuery(userId), CancellationToken.None);
        Assert.Equal(userId, result.UserId);
        Assert.Equal("test@email.com", result.Email);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Found()
    {
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
        var handler = new GetUserQueryHandler(repoMock.Object);
        await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(new GetUserQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
