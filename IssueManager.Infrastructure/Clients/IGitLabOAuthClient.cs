using Refit;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitLabOAuthClient
    {

        [Post("/oauth/token")]
        [Headers("Accept: application/json")]
        Task<Dictionary<string, string>> ExchangeCodeForTokenAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);
    }
}
