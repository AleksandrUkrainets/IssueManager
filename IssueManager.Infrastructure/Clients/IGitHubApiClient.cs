using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitHubApiClient
    {
        [Get("/user")]
        Task<GitHubUserResponse> GetUserAsync([Header("Authorization")] string authHeader);

        [Post("/repos/{owner}/{repo}/issues")]
        Task CreateIssueAsync(string owner, string repo, [Body] object issue, [Header("Authorization")] string authHeader);

        [Patch("/repos/{owner}/{repo}/issues/{issue_number}")]
        Task UpdateIssueAsync(string owner, string repo, int issue_number, [Body] object issue, [Header("Authorization")] string authHeader);
    }

    public class GitHubUserResponse
    {
        public int Id { get; set; }
    }
}
