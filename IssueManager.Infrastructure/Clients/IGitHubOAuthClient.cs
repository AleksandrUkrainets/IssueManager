using Refit;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitHubOAuthClient
    {
        [Post("/login/oauth/access_token")]
        [Headers("Accept: application/json")]
        Task<Dictionary<string, string>> ExchangeCodeForTokenAsync(
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);
    }
}
