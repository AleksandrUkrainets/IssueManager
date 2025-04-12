using IssueManager.Domain.Entities.GitHub;
using Refit;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitHubApiClient
    {
        [Get("/user")]
        Task<GitHubUser> GetUserAsync([Header("Authorization")] string authHeader);

        [Post("/repos/{owner}/{repo}/issues")]
        Task<GitHubIssue> CreateIssueAsync(string owner, string repo, [Body] object issue, [Header("Authorization")] string authHeader);

        [Patch("/repos/{owner}/{repo}/issues/{issue_number}")]
        Task<GitHubIssue> UpdateIssueAsync(string owner, string repo, int issue_number, [Body] object issue, [Header("Authorization")] string authHeader);
    }
}
