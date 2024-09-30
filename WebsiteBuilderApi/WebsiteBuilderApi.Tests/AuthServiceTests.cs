using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using WebsiteBuilderApi.Data;
using WebsiteBuilderApi.Models;
using WebsiteBuilderApi.Services;
using Xunit;

namespace WebsiteBuilderApi.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task AuthenticateGoogleUserAsync_ShouldCreateUserIfNotExists()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(c => c["Jwt:Key"]).Returns("TestKey");
            configMock.SetupGet(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            configMock.SetupGet(c => c["Jwt:Audience"]).Returns("TestAudience");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new ApplicationDbContext(options);

            var authService = new AuthService(configMock.Object, context);

            // Act
            var user = await authService.AuthenticateGoogleUserAsync("test@example.com", "providerId");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("test@example.com", user.Email);
        }
    
    }
}
