using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using UsersMS.Infrastructure.Service;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Enums;
using UsersMS.Commons.Events;
using MassTransit;

namespace UsersMS.Test.UsersMS.Infrastructure.Test
{
    public class EventPublisherTest
    {
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
