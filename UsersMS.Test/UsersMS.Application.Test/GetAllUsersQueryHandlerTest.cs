using System;
using System.Collections.Generic;
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

public class GetAllUsersQueryHandlerTest
{
    [Fact]
    public async Task Handle_Should_Return_All_Users()
    {
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Email = "a@test.com", DocumentId = "1", Name = "A", LastName = "B", Phone = "123", Address = "Dir", Role = UsersMS.Commons.Enums.UserRole.Administrador, State = UsersMS.Commons.Enums.UserState.Active },
            new User { Id = Guid.NewGuid(), Email = "b@test.com", DocumentId = "2", Name = "C", LastName = "D", Phone = "456", Address = "Dir2", Role = UsersMS.Commons.Enums.UserRole.Operador, State = UsersMS.Commons.Enums.UserState.Inactive }
        };
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
        var handler = new GetAllUsersQueryHandler(repoMock.Object);
        var result = await handler.Handle(new GetAllUsersQuery(), CancellationToken.None);
        Assert.Equal(2, result.Count);
        Assert.Equal("a@test.com", result[0].Email);
        Assert.Equal("b@test.com", result[1].Email);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Users_Not_Found()
    {
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync((List<User>?)null);
        var handler = new GetAllUsersQueryHandler(repoMock.Object);
        await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(new GetAllUsersQuery(), CancellationToken.None));
    }
}
