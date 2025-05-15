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
    public class UserCreatedConsumerTest
    {
        [Fact]
        public async Task Consume_Should_Insert_User()
        {
            var mockMongo = new Mock<MongoDbContext>();
            var mockCollection = new Mock<IMongoCollection<UserReadModel>>();
            mockMongo.Setup(m => m.Users).Returns(mockCollection.Object);
            var consumer = new UserCreatedConsumer(mockMongo.Object);
            var evt = new UserCreatedEvent { Id = Guid.NewGuid(), Email = "a@b.com" }; 
            var context = new Mock<ConsumeContext<UserCreatedEvent>>();
            context.SetupGet(c => c.Message).Returns(evt);
            mockCollection.Setup(c => c.InsertOneAsync(It.IsAny<UserReadModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            await consumer.Consume(context.Object);
            mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<UserReadModel>(), null, default), Times.Once);
        }
    }
}
