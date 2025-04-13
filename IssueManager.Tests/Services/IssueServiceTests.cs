using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Application.Services;
using IssueManager.Tests.FakesObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace IssueManager.Tests.Services
{
    public class IssueServiceTests
    {
        private readonly Mock<IIssueProviderFactory> _factoryMock = new();
        private readonly Mock<ILogger<IssueService>> _loggerMock = new();
        private readonly FakeIssueProvider _fakeProvider = new();

        private readonly IssueRequest _createRequest = new()
        {
            Repo = "user/repo",
            Title = "Issue Title",
            Body = "Issue Body"
        };

        private readonly IssueUpdateRequest _updateRequest = new()
        {
            Repo = "user/repo",
            IssueNumber = 1,
            Title = "Updated Title",
            Body = "Updated Body"
        };

        [Fact]
        public async Task CreateIssueAsync_ShouldReturnIssueDto_WhenProviderExists()
        {
            _factoryMock.Setup(f => f.CreateForCurrentUserAsync()).ReturnsAsync(_fakeProvider);

            var service = new IssueService(_factoryMock.Object, _loggerMock.Object);
            var result = await service.CreateIssueAsync(_createRequest);

            Assert.NotNull(result);
            Assert.Equal(_createRequest.Title, result.Title);
            _factoryMock.Verify(f => f.CreateForCurrentUserAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateIssueAsync_ShouldReturnNull_WhenProviderIsMissing()
        {
            _factoryMock.Setup(f => f.CreateForCurrentUserAsync()).ReturnsAsync((IIssueProvider?)null);

            var service = new IssueService(_factoryMock.Object, _loggerMock.Object);
            var result = await service.CreateIssueAsync(_createRequest);

            Assert.Null(result);
            _factoryMock.Verify(f => f.CreateForCurrentUserAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateIssueAsync_ShouldReturnIssueDto_WhenProviderExists()
        {
            _factoryMock.Setup(f => f.CreateForCurrentUserAsync()).ReturnsAsync(_fakeProvider);

            var service = new IssueService(_factoryMock.Object, _loggerMock.Object);
            var result = await service.UpdateIssueAsync(_updateRequest);

            Assert.NotNull(result);
            Assert.Equal(_updateRequest.Title, result.Title);
            _factoryMock.Verify(f => f.CreateForCurrentUserAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateIssueAsync_ShouldReturnNull_WhenProviderIsMissing()
        {
            _factoryMock.Setup(f => f.CreateForCurrentUserAsync()).ReturnsAsync((IIssueProvider?)null);

            var service = new IssueService(_factoryMock.Object, _loggerMock.Object);
            var result = await service.UpdateIssueAsync(_updateRequest);

            Assert.Null(result);
            _factoryMock.Verify(f => f.CreateForCurrentUserAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteIssueAsync_ShouldReturnTrue_WhenProviderExists()
        {
            _factoryMock.Setup(f => f.CreateForCurrentUserAsync()).ReturnsAsync(_fakeProvider);

            var service = new IssueService(_factoryMock.Object, _loggerMock.Object);
            var result = await service.DeleteIssueAsync("user/repo", 1);

            Assert.True(result);
            _factoryMock.Verify(f => f.CreateForCurrentUserAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteIssueAsync_ShouldReturnFalse_WhenProviderIsMissing()
        {
            _factoryMock.Setup(f => f.CreateForCurrentUserAsync()).ReturnsAsync((IIssueProvider?)null);

            var service = new IssueService(_factoryMock.Object, _loggerMock.Object);
            var result = await service.DeleteIssueAsync("user/repo", 1);

            Assert.False(result);
            _factoryMock.Verify(f => f.CreateForCurrentUserAsync(), Times.Once());
        }
    }
}
