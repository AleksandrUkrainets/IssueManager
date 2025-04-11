using IssueManager.Application.DROs;
using IssueManager.Application.Factories;
using IssueManager.Application.Services;
using IssueManager.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IssueManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssuesController : ControllerBase
    {
        private readonly IssueServiceFactory _factory;
        private readonly ITokenStorageService _tokenStorage;

        public IssuesController(IssueServiceFactory factory, ITokenStorageService tokenStorage)
        {
            _factory = factory;
            _tokenStorage = tokenStorage;
        }

        private string? AppUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? Provider => User.FindFirstValue("provider");

        [HttpPost("create")]
        public async Task<IActionResult> CreateIssue([FromBody] IssueRequest request)
        {
            if (AppUserId == null || Provider == null) return Unauthorized();

            var tokens = await _tokenStorage.GetTokenAsync(AppUserId, Provider);
            if (tokens == null) return Unauthorized();

            var service = _factory.CreateFromToken(Provider, tokens.Value.accessToken);
            if (service == null) return Unauthorized();

            await service.CreateIssueAsync(AppUserId, Provider, request.Repo, request.Title, request.Body);
            return Ok("Issue created");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateIssue([FromBody] IssueUpdateRequest request)
        {
            if (AppUserId == null || Provider == null) return Unauthorized();

            var tokens = await _tokenStorage.GetTokenAsync(AppUserId, Provider);
            if (tokens == null) return Unauthorized();

            var service = _factory.CreateFromToken(Provider, tokens.Value.accessToken);
            if (service == null) return Unauthorized();

            await service.UpdateIssueAsync(AppUserId, Provider, request.Repo, request.IssueId, request.Title, request.Body);
            return Ok("Issue updated");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteIssue([FromQuery] string repo, [FromQuery] int issueId)
        {
            if (AppUserId == null || Provider == null) return Unauthorized();

            var tokens = await _tokenStorage.GetTokenAsync(AppUserId, Provider);
            if (tokens == null) return Unauthorized();

            var service = _factory.CreateFromToken(Provider, tokens.Value.accessToken);
            if (service == null) return Unauthorized();

            await service.DeleteIssueAsync(AppUserId, Provider, repo, issueId);
            return Ok("Issue deleted (if supported by provider)");
        }
    }
}
