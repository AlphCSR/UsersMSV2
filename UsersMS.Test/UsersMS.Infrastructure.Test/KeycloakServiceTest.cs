using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using UsersMS.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using UsersMS.Infrastructure.Service;
using UsersMS.Domain.Entities;
using System.Text.Json;
using Moq.Protected;
using System.Text;

namespace UsersMS.Test.UsersMS.Infrastructure.Test
{
    public class KeycloakServiceTest
    {
        private static Mock<IConfiguration> GetMockConfig()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["Keycloak:BaseUrl"]).Returns("http://localhost/");
            config.Setup(c => c["Keycloak:Realm"]).Returns("myrealm");
            config.Setup(c => c["Keycloak:ClientId"]).Returns("clientid");
            config.Setup(c => c["Keycloak:ClientSecret"]).Returns("secret");
            return config;
        }

        private static HttpClient GetHttpClient(HttpMessageHandler handler)
        {
            return new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
        }

        [Fact]
        public async Task GetAdminTokenAsync_Returns_Token_On_Success()
        {
            // Arrange
            var tokenJson = "{\"access_token\":\"tok123\"}";
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(tokenJson, Encoding.UTF8, "application/json")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);

            // Act
            var token = await service.GetAdminTokenAsync();

            // Assert
            Assert.Equal("tok123", token);
        }

        [Fact]
        public async Task GetAdminTokenAsync_Throws_On_Error_Response()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent("error")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);
            await Assert.ThrowsAsync<Exception>(() => service.GetAdminTokenAsync());
        }

        [Fact]
        public async Task CreateUserAsync_Success()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent("")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);
            await service.CreateUserAsync(new { Email = "a@b.com" }, "tok");
        }

        [Fact]
        public async Task CreateUserAsync_Throws_On_Error()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent("fail")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);
            await Assert.ThrowsAsync<Exception>(() => service.CreateUserAsync(new { Email = "a@b.com" }, "tok"));
        }

        [Fact]
        public async Task AssignRoleAsync_Throws_On_User_Not_Found()
        {
            // Simula respuesta vacía de usuario
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);
            await Assert.ThrowsAsync<Exception>(() => service.AssignRoleAsync("nouser", "role", "tok"));
        }

        [Fact]
        public async Task AssignRoleAsync_Throws_On_Client_Not_Found()
        {
            // Simula usuario encontrado, pero cliente vacío
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[{\"id\":\"uid\"}]", Encoding.UTF8, "application/json")
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);
            await Assert.ThrowsAsync<Exception>(() => service.AssignRoleAsync("user", "role", "tok"));
        }

        [Fact]
        public async Task AssignRoleAsync_Throws_On_Role_Not_Found()
        {
            // Simula usuario y cliente encontrados, pero rol no encontrado
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[{\"id\":\"uid\"}]", Encoding.UTF8, "application/json")
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[{\"id\":\"cid\"}]", Encoding.UTF8, "application/json")
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = new StringContent("not found")
                });
            var httpClient = GetHttpClient(handler.Object);
            var config = GetMockConfig();
            var service = new KeycloakService(httpClient, config.Object);
            await Assert.ThrowsAsync<Exception>(() => service.AssignRoleAsync("user", "role", "tok"));
        }
    }
}
