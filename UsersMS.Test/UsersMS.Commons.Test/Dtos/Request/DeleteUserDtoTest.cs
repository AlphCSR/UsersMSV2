using System;
using UsersMS.Commons.Dtos.Request;
using Xunit;

namespace UsersMS.Test.UsersMS.Commons.Test.Dtos.Request
{
    public class DeleteUserDtoTest
    {
        [Fact]
        public void Properties_Work()
        {
            var id = Guid.NewGuid();
            var dto = new DeleteUserDto { UserId = id };
            Assert.Equal(id, dto.UserId);
        }
    }
}
