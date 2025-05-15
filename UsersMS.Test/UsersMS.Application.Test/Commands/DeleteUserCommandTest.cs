using Moq;
using UsersMS.Application.Commands;
using UsersMS.Application.Handlers.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Commons.Enums;
using UsersMS.Domain.Entities;
using Xunit;

namespace UsersMS.Test.UsersMS.Application.Test.Commands
{
    public class DeleteUserCommandTest
    {
        private DeleteUserDto GetValidDto(Guid? id = null) => new DeleteUserDto
        {
            UserId = id ?? Guid.NewGuid()
        };

        [Fact]
        public async Task Handle_Should_Delete_User_Successfully()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();

            var userId = Guid.NewGuid();
            var user = new User("test@example.com", "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = userId };
            mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            mockUserRepository.Setup(r => r.DeleteAsync(userId)).Returns(Task.CompletedTask);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.DisableUserAsync(user.Email, "token")).Returns(Task.CompletedTask);

            var handler = new DeleteUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new DeleteUserCommand(GetValidDto(userId));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("User successfully disabled.", result);
            mockUserRepository.Verify(r => r.GetByIdAsync(userId), Times.Once);
            mockUserRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
            mockKeycloakService.Verify(x => x.GetAdminTokenAsync(), Times.Once);
            mockKeycloakService.Verify(x => x.DisableUserAsync(user.Email, "token"), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var userId = Guid.NewGuid();
            mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);
            var handler = new DeleteUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new DeleteUserCommand(GetValidDto(userId));

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Email_Is_Null_Or_Empty()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var userId = Guid.NewGuid();
            var user = new User(null, "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = userId };
            mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            var handler = new DeleteUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new DeleteUserCommand(GetValidDto(userId));

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_KeycloakService_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var userId = Guid.NewGuid();
            var user = new User("test@example.com", "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = userId };
            mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ThrowsAsync(new Exception("Keycloak fail"));
            var handler = new DeleteUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new DeleteUserCommand(GetValidDto(userId));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_DisableUserAsync_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var userId = Guid.NewGuid();
            var user = new User("test@example.com", "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = userId };
            mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.DisableUserAsync(user.Email, "token")).ThrowsAsync(new Exception("Disable fail"));
            var handler = new DeleteUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new DeleteUserCommand(GetValidDto(userId));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_DeleteAsync_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var userId = Guid.NewGuid();
            var user = new User("test@example.com", "pass", "111", "Test", "User", "123", "Address", (UserRole)1, (UserState)1) { Id = userId };
            mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.DisableUserAsync(user.Email, "token")).Returns(Task.CompletedTask);
            mockUserRepository.Setup(r => r.DeleteAsync(userId)).ThrowsAsync(new Exception("Repo fail"));
            var handler = new DeleteUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new DeleteUserCommand(GetValidDto(userId));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
