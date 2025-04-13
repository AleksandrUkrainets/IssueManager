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
            var issue = await issueService.CreateIssueAsync(request);
            return issue is null ? NotFound() : Ok(issue);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateIssue([FromBody] IssueUpdateRequest request)
        {
            var issue = await issueService.UpdateIssueAsync(request);
            return issue is null ? NotFound() : Ok(issue);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteIssue([FromQuery] string repo, [FromQuery] int issueNumber)
        {
            var result = await issueService.DeleteIssueAsync(repo, issueNumber);
            return result ? Ok(new { isDeleted = result }) : NotFound(new { isDeleted = result });
        }
    }
}
