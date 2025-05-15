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
    public class GetAllUsersQueryTest
    {
        private User GetUser(Guid? id = null) => new User("test@example.com", "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = id ?? Guid.NewGuid() };

        [Fact]
        public async Task Handle_Should_Return_All_Users()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetAllUsersQueryHandler(mockUserRepository.Object);
            var query = new GetAllUsersQuery();
            var users = new List<User>
            {
                GetUser(),
                GetUser()
            };
            mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
            foreach (var dto in result)
            {
                Assert.NotEqual(Guid.Empty, dto.UserId);
                Assert.False(string.IsNullOrEmpty(dto.Email));
            }
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Users_Exist()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetAllUsersQueryHandler(mockUserRepository.Object);
            var query = new GetAllUsersQuery();
            mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_Should_Throw_UserNotFoundException_When_Repository_Returns_Null()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetAllUsersQueryHandler(mockUserRepository.Object);
            var query = new GetAllUsersQuery();
            mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync((List<User>)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Repository_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var handler = new GetAllUsersQueryHandler(mockUserRepository.Object);
            var query = new GetAllUsersQuery();
            mockUserRepository.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Repo fail"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
