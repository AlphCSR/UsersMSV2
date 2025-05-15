using System;
using UsersMS.Commons.Events;
using Xunit;

namespace UsersMS.Test.UsersMS.Commons.Test.Events
{
    public class UserDeletedEventTest
    {
        [Fact]
        public void ConstructorAndProperties_Work()
        {
            var id = Guid.NewGuid();
            var evt = new UserDeletedEvent
            {
                Id = id,
                Email = "test@mail.com",
                Name = "John",
                LastName = "Doe",
                Phone = "123456",
                Address = "Main St",
                Password = "pass",
                Role = null,
                State = null
            };
            Assert.Equal(id, evt.Id);
            Assert.Equal("test@mail.com", evt.Email);
            Assert.Equal("John", evt.Name);
            Assert.Equal("Doe", evt.LastName);
            Assert.Equal("123456", evt.Phone);
            Assert.Equal("Main St", evt.Address);
            Assert.Equal("pass", evt.Password);
            Assert.Null(evt.Role);
            Assert.Null(evt.State);
        }
    }
}
