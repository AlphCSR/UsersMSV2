using System;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Commons.Enums;
using Xunit;

namespace UsersMS.Test.UsersMS.Commons.Test.Dtos.Request
{
    public class UpdateUserDtoTest
    {
        [Fact]
        public void Properties_Work()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateUserDto
            {
                UserId = id,
                Email = "test@mail.com",
                Password = "pass",
                DocumentId = "123",
                Name = "John",
                LastName = "Doe",
                Phone = "123456",
                Address = "Main St",
                Role = UserRole.Administrador,
                State = UserState.Active
            };
            Assert.Equal(id, dto.UserId);
            Assert.Equal("test@mail.com", dto.Email);
            Assert.Equal("pass", dto.Password);
            Assert.Equal("123", dto.DocumentId);
            Assert.Equal("John", dto.Name);
            Assert.Equal("Doe", dto.LastName);
            Assert.Equal("123456", dto.Phone);
            Assert.Equal("Main St", dto.Address);
            Assert.Equal(UserRole.Administrador, dto.Role);
            Assert.Equal(UserState.Active, dto.State);
        }
    }
}
