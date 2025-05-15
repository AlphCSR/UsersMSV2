using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using UsersMS.Application.Commands;
using UsersMS.Application.Handlers.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Exceptions;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Test.UsersMS.Application.Test.Commands
{
    public class UpdateUserCommandTest
    {
        private UpdateUserDto GetValidDto(Guid? id = null) => new UpdateUserDto
        {
            UserId = id ?? Guid.NewGuid(),
            Email = "updateduser@example.com",
            Password = "newpassword",
            DocumentId = "654321",
            Name = "Updated",
            LastName = "User",
            Phone = "0987654321",
            Address = "Updated Address"
        };

        [Fact]
        public async Task Handle_Should_Update_User_Successfully()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var user = new User("old@example.com", "oldpass", "111", "Old", "User", "111", "Old Address", (UserRole)1, (UserState)1) { Id = Guid.NewGuid() };
            mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var handler = new UpdateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new UpdateUserCommand(GetValidDto(user.Id));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("User updated successfully.", result);
            mockUserRepository.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
            mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
            mockKeycloakService.Verify(x => x.GetAdminTokenAsync(), Times.Once);
            mockKeycloakService.Verify(x => x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_UserNotFoundException_When_User_Does_Not_Exist()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            var handler = new UpdateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new UpdateUserCommand(GetValidDto());

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_KeycloakService_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var user = new User("old@example.com", "oldpass", "111", "Old", "User", "111", "Old Address", (UserRole)1, (UserState)1) { Id = Guid.NewGuid() };
            mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ThrowsAsync(new Exception("Keycloak fail"));
            var handler = new UpdateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new UpdateUserCommand(GetValidDto(user.Id));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_UpdateUserAsync_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var user = new User("old@example.com", "oldpass", "111", "Old", "User", "111", "Old Address", (UserRole)1, (UserState)1) { Id = Guid.NewGuid() };
            mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>())).ThrowsAsync(new Exception("Update fail"));
            var handler = new UpdateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new UpdateUserCommand(GetValidDto(user.Id));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_UpdateAsync_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var user = new User("old@example.com", "oldpass", "111", "Old", "User", "111", "Old Address", (UserRole)1, (UserState)1) { Id = Guid.NewGuid() };
            mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Repo fail"));
            var handler = new UpdateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new UpdateUserCommand(GetValidDto(user.Id));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
