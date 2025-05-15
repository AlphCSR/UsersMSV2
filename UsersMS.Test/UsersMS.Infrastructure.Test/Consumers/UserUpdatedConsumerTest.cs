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
    public class UserUpdatedConsumerTest
    {
        [Fact]
        public async Task Consume_Should_Update_User()
        {
            var mockMongo = new Mock<MongoDbContext>();
            var mockCollection = new Mock<IMongoCollection<UserReadModel>>();
            mockMongo.Setup(m => m.Users).Returns(mockCollection.Object);
            var consumer = new UserUpdatedConsumer(mockMongo.Object);
            var evt = new UserUpdatedEvent { Id = Guid.NewGuid(), Email = "a@b.com" };
            var context = new Mock<ConsumeContext<UserUpdatedEvent>>();
            context.SetupGet(c => c.Message).Returns(evt);
            var mockReplaceResult = new Mock<ReplaceOneResult>();
            mockReplaceResult.Setup(r => r.IsAcknowledged).Returns(true);
            mockReplaceResult.Setup(r => r.ModifiedCount).Returns(1);
            mockCollection.Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<UserReadModel>>(), It.IsAny<UserReadModel>(), It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockReplaceResult.Object);
            await consumer.Consume(context.Object);
            mockCollection.Verify(
                c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<UserReadModel>>(), It.IsAny<UserReadModel>(), It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
