using IssueManager.Application.Interfaces;
using IssueManager.Application.Services;
using IssueManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace IssueManager.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IOAuthProviderFactory> _providerFactoryMock = new();
        private readonly Mock<IOAuthProvider> _providerMock = new();
        private readonly Mock<IUserCredentialRepository> _credRepoMock = new();
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock = new();
        private readonly Mock<ILogger<AuthService>> _loggerMock = new();

        [Fact]
        public async Task SignIn_ShouldReturnToken_WhenProviderAndTokenValid()
        {
            _providerFactoryMock.Setup(f => f.Create("github")).Returns(_providerMock.Object);
            _providerMock.Setup(p => p.ExchangeCodeForTokenAsync("code")).ReturnsAsync("accessToken");
            _providerMock.Setup(p => p.GetUserIdAsync("accessToken")).ReturnsAsync("user123");
            _tokenGeneratorMock.Setup(t => t.GenerateJwtToken("github", "accessToken", "user123")).Returns("jwt");

            var service = new AuthService(_providerFactoryMock.Object, _credRepoMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);
            var result = await service.SignIn("code", "github");

            Assert.True(result.IsSuccess);
            Assert.Equal("jwt", result.Jwt);
        }

        [Fact]
        public async Task SignIn_ShouldReturnFailure_WhenExchangeTokenFails()
        {
            _providerFactoryMock.Setup(f => f.Create("github")).Returns(_providerMock.Object);
            _providerMock.Setup(p => p.ExchangeCodeForTokenAsync("code")).ReturnsAsync((string?)null);

            var service = new AuthService(_providerFactoryMock.Object, _credRepoMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);
            var result = await service.SignIn("code", "github");

            Assert.False(result.IsSuccess);
            Assert.Null(result.Jwt);
        }
    }
}