using System;
using UsersMS.Commons.Enums;
using UsersMS.Commons.Events;
using Xunit;

namespace UsersMS.Test.UsersMS.Commons.Test.Events
{
    public class UserCreatedEventTest
    {
        [Fact]
        public void ConstructorAndProperties_Work()
        {
            var id = Guid.NewGuid();
            var evt = new UserCreatedEvent
            {
                Id = id,
                Email = "test@mail.com",
                Name = "John",
                LastName = "Doe",
                Phone = "123456",
                Address = "Main St",
                Password = "pass",
                Role = UserRole.Administrador,
                State = UserState.Active
            };
            Assert.Equal(id, evt.Id);
            Assert.Equal("test@mail.com", evt.Email);
            Assert.Equal("John", evt.Name);
            Assert.Equal("Doe", evt.LastName);
            Assert.Equal("123456", evt.Phone);
            Assert.Equal("Main St", evt.Address);
            Assert.Equal("pass", evt.Password);
            Assert.Equal(UserRole.Administrador, evt.Role);
            Assert.Equal(UserState.Active, evt.State);
        }
    }
}
