using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IssueManager.Infrastructure.Clients.IGitHubOAuthClient;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitLabApiClient
    {
        [Get("/api/v4/user")]
        Task<UserResponse> GetUserAsync([Header("Authorization")] string authHeader);

        [Post("/api/v4/projects/{projectId}/issues")]
        Task CreateIssueAsync(string projectId, [Body] object issue, [Header("Authorization")] string authHeader);

        [Put("/api/v4/projects/{projectId}/issues/{issueId}")]
        Task UpdateIssueAsync(string projectId, int issueId, [Body] object issue, [Header("Authorization")] string authHeader);

        [Delete("/api/v4/projects/{projectId}/issues/{issueId}")]
        Task DeleteIssueAsync(string projectId, int issueId, [Header("Authorization")] string authHeader);

    }
}
