using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using UsersMS.Application.Queries;
using UsersMS.Application.Handlers.Queries;
using UsersMS.Core.Repositories;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Exceptions;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Test.UsersMS.Application.Test.Querys
{
    public class GetUserQueryTest
    {
        private User GetUser(Guid? id = null) => new User("test@example.com", "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = id ?? Guid.NewGuid() };

        [Fact]
        public async Task Handle_Should_Return_User_When_User_Exists()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetUserQueryHandler(mockUserRepository.Object);
            var userId = Guid.NewGuid();
            var user = GetUser(userId);
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            var query = new GetUserQuery { UserId = userId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Phone, result.Phone);
            Assert.Equal(user.Address, result.Address);
            Assert.Equal(user.Role, result.Role);
            Assert.Equal(user.State, result.State);
        }

        [Fact]
        public async Task Handle_Should_Throw_UserNotFoundException_When_User_Does_Not_Exist()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetUserQueryHandler(mockUserRepository.Object);
            var userId = Guid.NewGuid();
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);
            var query = new GetUserQuery { UserId = userId };

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Repository_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetUserQueryHandler(mockUserRepository.Object);
            var userId = Guid.NewGuid();
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ThrowsAsync(new Exception("Repo fail"));
            var query = new GetUserQuery { UserId = userId };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
