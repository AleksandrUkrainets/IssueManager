using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IssueManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateIssue([FromBody] IssueRequest request)
        {
            var result = await _issueService.CreateIssueAsync(request);
            if (!result) return Unauthorized();

            return Ok("Issue created");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateIssue([FromBody] IssueUpdateRequest request)
        {
            var result = await _issueService.UpdateIssueAsync(request);
            if (!result) return Unauthorized();

            return Ok("Issue updated");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteIssue([FromQuery] string repo, [FromQuery] int issueId)
        {
            var result = await _issueService.DeleteIssueAsync(repo, issueId);
            if (!result) return Unauthorized();

            return Ok("Issue deleted (if supported by provider)");
        }
    }
}
