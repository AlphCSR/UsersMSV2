using System.Threading.Tasks;
using Moq;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Service;
using MassTransit;
using UsersMS.Commons.Events;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Test.UsersMS.Infrastructure.Test.Service
{
    public class EventPublisherTest
    {
        [Fact]
        public async Task PublishUserCreatedAsync_Should_Call_Publish_With_UserCreatedEvent()
        {
            // Arrange
            var publishEndpointMock = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(publishEndpointMock.Object);
            var user = new User
            {
                Id = System.Guid.NewGuid(),
                Email = "test@mail.com",
                Name = "Test",
                LastName = "User",
                Phone = "123456789",
                Address = "Test Address",
                Password = "pass",
                Role = UserRole.Administrador,
                State = UserState.Active
            };

            // Act
            await publisher.PublishUserCreatedAsync(user);

            // Assert
            publishEndpointMock.Verify(p => p.Publish(It.Is<UserCreatedEvent>(e =>
                e.Id == user.Id &&
                e.Email == user.Email &&
                e.Name == user.Name &&
                e.LastName == user.LastName &&
                e.Phone == user.Phone &&
                e.Address == user.Address &&
                e.Password == user.Password &&
                e.Role.Equals(user.Role) &&
                e.State.Equals(user.State)
            ), default), Times.Once);
        }

        [Fact]
        public async Task PublishUserUpdatedAsync_Should_Call_Publish_With_UserUpdatedEvent()
        {
            // Arrange
            var publishEndpointMock = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(publishEndpointMock.Object);
            var user = new User
            {
                Id = System.Guid.NewGuid(),
                Email = "test@mail.com",
                Name = "Test",
                LastName = "User",
                Phone = "123456789",
                Address = "Test Address",
                Password = "pass",
                Role = UserRole.Administrador,
                State = UserState.Active
            };

            // Act
            await publisher.PublishUserUpdatedAsync(user);

            // Assert
            publishEndpointMock.Verify(p => p.Publish(It.Is<UserUpdatedEvent>(e =>
                e.Id == user.Id &&
                e.Email == user.Email &&
                e.Name == user.Name &&
                e.LastName == user.LastName &&
                e.Phone == user.Phone &&
                e.Address == user.Address &&
                e.Password == user.Password &&
                e.Role.Equals(user.Role) &&
                e.State.Equals(user.State)
            ), default), Times.Once);
        }

        [Fact]
        public async Task PublishUserDeletedAsync_Should_Call_Publish_With_UserDeletedEvent()
        {
            // Arrange
            var publishEndpointMock = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(publishEndpointMock.Object);
            var user = new User
            {
                Id = System.Guid.NewGuid(),
                Email = "test@mail.com",
                Name = "Test",
                LastName = "User",
                Phone = "123456789",
                Address = "Test Address",
                Password = "pass",
                Role = UserRole.Administrador,
                State = UserState.Active
            };

            // Act
            await publisher.PublishUserDeletedAsync(user);

            // Assert
            publishEndpointMock.Verify(p => p.Publish(It.Is<UserDeletedEvent>(e =>
                e.Id == user.Id &&
                e.Email == user.Email &&
                e.Name == user.Name &&
                e.LastName == user.LastName &&
                e.Phone == user.Phone &&
                e.Address == user.Address &&
                e.Password == user.Password &&
                e.Role.Equals(user.Role) &&
                e.State.Equals(user.State)
            ), default), Times.Once);
        }

        [Fact]
        public async Task PublishUserCreatedAsync_Should_Publish_Event_Successfully()
        {
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(mockEndpoint.Object);
            var user = new User("a@b.com", "p", "d", "n", "l", "t", "dir", UserRole.Administrador, UserState.Active);
            mockEndpoint.Setup(e => e.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            await publisher.PublishUserCreatedAsync(user);
            mockEndpoint.Verify(e => e.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PublishUserCreatedAsync_Should_Throw_On_Bus_Exception()
        {
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(mockEndpoint.Object);
            var user = new User("a@b.com", "p", "d", "n", "l", "t", "dir", UserRole.Administrador, UserState.Active);
            mockEndpoint.Setup(e => e.Publish(It.IsAny<UserCreatedEvent>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("bus fail"));
            await Assert.ThrowsAsync<Exception>(() => publisher.PublishUserCreatedAsync(user));
        }

        [Fact]
        public async Task PublishUserUpdatedAsync_Should_Publish_Event_Successfully()
        {
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(mockEndpoint.Object);
            var user = new User("a@b.com", "p", "d", "n", "l", "t", "dir", UserRole.Administrador, UserState.Active);
            mockEndpoint.Setup(e => e.Publish(It.IsAny<UserUpdatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            await publisher.PublishUserUpdatedAsync(user);
            mockEndpoint.Verify(e => e.Publish(It.IsAny<UserUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PublishUserDeletedAsync_Should_Publish_Event_Successfully()
        {
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new EventPublisher(mockEndpoint.Object);
            var user = new User("a@b.com", "p", "d", "n", "l", "t", "dir", UserRole.Administrador, UserState.Active);
            mockEndpoint.Setup(e => e.Publish(It.IsAny<UserDeletedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
            await publisher.PublishUserDeletedAsync(user);
            mockEndpoint.Verify(e => e.Publish(It.IsAny<UserDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
