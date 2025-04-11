using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IssueManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssuesController(IIssueService issueService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateIssue([FromBody] IssueRequest request)
        {
            var result = await issueService.CreateIssueAsync(request);
            if (!result) return Unauthorized();

            return Ok("Issue created");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateIssue([FromBody] IssueUpdateRequest request)
        {
            var result = await issueService.UpdateIssueAsync(request);
            if (!result) return Unauthorized();

            return Ok("Issue updated");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteIssue([FromQuery] string repo, [FromQuery] int issueId)
        {
            var result = await issueService.DeleteIssueAsync(repo, issueId);
            if (!result) return Unauthorized();

            return Ok("Issue deleted (if supported by provider)");
        }
    }
}
