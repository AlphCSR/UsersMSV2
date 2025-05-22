using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using UsersMS.Infrastructure.Repositories;
using Xunit;

namespace UsersMS.Test.UsersMS.Infrastructure.Test.Repositories
{
    public class KeycloakServiceTest
    {
        [Fact]
        public async Task UpdateUserAsync_Should_Send_Put_Request_And_Return_Success()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                // Primera llamada: GET usuario
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"id\":\"user-id-123\"}]")
                })
                // Segunda llamada: PUT actualización
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Keycloak:BaseUrl"]).Returns("http://localhost/");
            configMock.Setup(c => c["Keycloak:Realm"]).Returns("testrealm");
            configMock.Setup(c => c[It.IsNotIn("Keycloak:BaseUrl", "Keycloak:Realm")]).Returns("dummy");
            var service = new KeycloakService(httpClient, configMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(() => service.UpdateUserAsync("testuser", new { name = "Test" }, "token"));

            // Assert
            Assert.Null(exception);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task UpdateUserAsync_Should_Throw_On_Failure_Response()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("error")
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Keycloak:BaseUrl"]).Returns("http://localhost/");
            configMock.Setup(c => c["Keycloak:Realm"]).Returns("testrealm");
            configMock.Setup(c => c[It.IsNotIn("Keycloak:BaseUrl", "Keycloak:Realm")]).Returns("dummy");
            var service = new KeycloakService(httpClient, configMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.UpdateUserAsync("testuser", new { name = "Test" }, "token"));
        }
    }
}
