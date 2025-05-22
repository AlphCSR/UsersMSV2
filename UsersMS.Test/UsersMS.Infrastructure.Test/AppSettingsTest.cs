using UsersMS.Infrastructure.Setings;
using Xunit;

namespace UsersMS.Infrastructure.Test
{
    public class AppSettingsTest
    {
        [Fact]
        public void Properties_SetAndGet_ShouldWorkCorrectly()
        {
            // Arrange
            var settings = new AppSettings
            {
                key1 = "value1"
            };

            // Assert
            Assert.Equal("value1", settings.key1);
        }
    }
}
