using IssueManager.Domain.Entities.GitLab;
using Refit;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitLabApiClient
    {
        [Get("/api/v4/user")]
        Task<GitLabUser> GetUserAsync([Header("Authorization")] string authHeader);

        [Post("/api/v4/projects/{projectId}/issues")]
        Task<GitLabIssue> CreateIssueAsync(string projectId, [Body] object issue, [Header("Authorization")] string authHeader);

        [Put("/api/v4/projects/{projectId}/issues/{issueNumber}")]
        Task<GitLabIssue> UpdateIssueAsync(string projectId, int issueNumber, [Body] object issue, [Header("Authorization")] string authHeader);

        [Delete("/api/v4/projects/{projectId}/issues/{issueNumber}")]
        Task<ApiResponse<dynamic>> DeleteIssueAsync(string projectId, int issueNumber, [Header("Authorization")] string authHeader);

    }
}
