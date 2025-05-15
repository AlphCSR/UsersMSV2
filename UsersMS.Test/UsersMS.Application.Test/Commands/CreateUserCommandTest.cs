using Moq;
using UsersMS.Application.Commands;
using UsersMS.Application.Handlers.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Test.UsersMS.Application.Test.Commands
{
    public class CreateUserCommandTest
    {
        private CreateUserDto GetValidDto()
        {
            return new CreateUserDto
            {
                Email = "testuser@example.com",
                Password = "password123",
                DocumentId = "123456",
                Name = "Test",
                LastName = "User",
                Phone = "1234567890",
                Address = "Test Address",
                Role = (UserRole)1,
                State = (UserState)1,
            };
        }

        [Fact]
        public async Task Handle_Should_Create_User_Successfully()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();

            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.CreateUserAsync(It.IsAny<object>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockKeycloakService.Setup(x => x.AssignRoleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockEventPublisher.Setup(x => x.PublishUserCreatedAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var handler = new CreateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new CreateUserCommand(GetValidDto());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("User successfully created.", result);
            mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            mockKeycloakService.Verify(s => s.GetAdminTokenAsync(), Times.Once);
            mockKeycloakService.Verify(s => s.CreateUserAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once);
            mockKeycloakService.Verify(s => s.AssignRoleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockEventPublisher.Verify(pub => pub.PublishUserCreatedAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Dto_Is_Null()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();
            var handler = new CreateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new CreateUserCommand(null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_UserRepository_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();

            mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Repo fail"));

            var handler = new CreateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new CreateUserCommand(GetValidDto());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_KeycloakService_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();

            mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ThrowsAsync(new Exception("Keycloak fail"));

            var handler = new CreateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new CreateUserCommand(GetValidDto());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EventPublisher_Fails()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockKeycloakService = new Mock<IKeycloakService>();
            var mockEventPublisher = new Mock<IEventPublisher>();

            mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            mockKeycloakService.Setup(x => x.GetAdminTokenAsync()).ReturnsAsync("token");
            mockKeycloakService.Setup(x => x.CreateUserAsync(It.IsAny<object>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockKeycloakService.Setup(x => x.AssignRoleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            mockEventPublisher.Setup(x => x.PublishUserCreatedAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Publisher fail"));

            var handler = new CreateUserCommandHandler(mockUserRepository.Object, mockKeycloakService.Object, mockEventPublisher.Object);
            var command = new CreateUserCommand(GetValidDto());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
