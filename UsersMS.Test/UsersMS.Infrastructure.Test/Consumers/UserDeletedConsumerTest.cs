using System.Threading.Tasks;
using Xunit;
using Moq;
using MassTransit;
using UsersMS.Infrastructure.Messaging.Consumers;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;
using MongoDB.Driver;
using UsersMS.Domain.Entities;

namespace UsersMS.Test.UsersMS.Infrastructure.Test.Consumers
{
    public class UserDeletedConsumerTest
    {
        [Fact]
        public async Task Consume_Should_Delete_User_By_Id()
        {
            var mockMongo = new Mock<MongoDbContext>();
            var mockCollection = new Mock<IMongoCollection<UserReadModel>>();
            mockMongo.Setup(m => m.Users).Returns(mockCollection.Object);
            var consumer = new UserDeletedConsumer(mockMongo.Object);
            var evt = new UserDeletedEvent { Id = Guid.NewGuid() };
            var context = new Mock<ConsumeContext<UserDeletedEvent>>();
            context.SetupGet(c => c.Message).Returns(evt);
            var mockDeleteResult = new Mock<DeleteResult>();
            mockDeleteResult.Setup(r => r.DeletedCount).Returns(1);
            mockDeleteResult.Setup(r => r.IsAcknowledged).Returns(true);
            mockCollection.Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<UserReadModel>>(), It.IsAny<CancellationToken>())).ReturnsAsync((DeleteResult)null);
            await consumer.Consume(context.Object);
            mockCollection.Verify(
                c => c.DeleteOneAsync(It.IsAny<FilterDefinition<UserReadModel>>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
