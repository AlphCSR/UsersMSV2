using System;
using UsersMS.Domain.Entities;
using Xunit;

namespace UsersMS.Domain.Test
{
    public class BaseTest
    {
        [Fact]
        public void Properties_SetAndGet_ShouldWorkCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;
            var createdBy = "creator";
            var updatedAt = DateTime.UtcNow.AddDays(1);
            var updatedBy = "updater";
            var entity = new Base
            {
                Id = id,
                CreatedAt = createdAt,
                CreatedBy = createdBy,
                UpdatedAt = updatedAt,
                UpdatedBy = updatedBy
            };

            // Assert
            Assert.Equal(id, entity.Id);
            Assert.Equal(createdAt, entity.CreatedAt);
            Assert.Equal(createdBy, entity.CreatedBy);
            Assert.Equal(updatedAt, entity.UpdatedAt);
            Assert.Equal(updatedBy, entity.UpdatedBy);
        }
    }
}
