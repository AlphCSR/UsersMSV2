using System;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Domain.Test
{
    public class UserReadModelTest
    {
        [Fact]
        public void Properties_SetAndGet_ShouldWorkCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var model = new UserReadModel
            {
                Id = id,
                Email = "test@example.com",
                Name = "TestName",
                LastName = "TestLastName",
                Phone = "1234567890",
                Address = "Test Address",
                Password = "TestPassword",
                Role = UserRole.Administrador,
                State = UserState.Active
            };

            // Assert
            Assert.Equal(id, model.Id);
            Assert.Equal("test@example.com", model.Email);
            Assert.Equal("TestName", model.Name);
            Assert.Equal("TestLastName", model.LastName);
            Assert.Equal("1234567890", model.Phone);
            Assert.Equal("Test Address", model.Address);
            Assert.Equal("TestPassword", model.Password);
            Assert.Equal(UserRole.Administrador, model.Role);
            Assert.Equal(UserState.Active, model.State);
        }
    }
}
