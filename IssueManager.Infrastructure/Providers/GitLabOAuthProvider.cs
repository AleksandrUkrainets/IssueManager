using IssueManager.Application.Configuration;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;
using Microsoft.Extensions.Options;

namespace IssueManager.Infrastructure.Services.OAuth
{
    public class GitLabOAuthProvider(IOptions<OAuthSettings> settings, IGitLabApiClient apiClient, IGitLabOAuthClient oAuthClient) : IOAuthProvider
    {
        private readonly OAuthProviderSettings _config = settings.Value.Providers["gitlab"];

        public string GetAuthorizationUrl()
        {
            var queryParams = new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "redirect_uri", _config.RedirectUri },
                { "response_type", "code" },
                { "scope", _config.Scope },
                { "state", "gitlab" }
            };

            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{_config.AuthUrl}?{query}";
        }

        public async Task<string?> ExchangeCodeForTokenAsync(string code)
        {
            try
            {
                var response = await oAuthClient.ExchangeCodeForTokenAsync(new Dictionary<string, string>
                {
                    { "client_id", _config.ClientId },
                    { "client_secret", _config.ClientSecret },
                    { "code", code },
                    { "redirect_uri", _config.RedirectUri },
                    { "grant_type", "authorization_code" }
                });

                return response.TryGetValue("access_token", out var token) ? token : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> GetUserIdAsync(string accessToken)
        {
            try
            {
                var user = await apiClient.GetUserAsync($"Bearer {accessToken}");
                return user.Id.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
