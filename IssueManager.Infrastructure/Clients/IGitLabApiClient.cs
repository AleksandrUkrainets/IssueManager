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

        [Put("/api/v4/projects/{projectId}/issues/{issueId}")]
        Task<GitLabIssue> UpdateIssueAsync(string projectId, int issueId, [Body] object issue, [Header("Authorization")] string authHeader);

        [Delete("/api/v4/projects/{projectId}/issues/{issueId}")]
        Task<ApiResponse<dynamic>> DeleteIssueAsync(string projectId, int issueId, [Header("Authorization")] string authHeader);

    }
}
