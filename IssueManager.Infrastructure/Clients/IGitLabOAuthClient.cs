using IssueManager.Domain.Entities.GitLab;
using Refit;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitLabOAuthClient
    {
        [Post("/oauth/token")]
        [Headers("Accept: application/json")]
        Task<GitLabTokenResponse> ExchangeCodeForTokenAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);
    }
}
