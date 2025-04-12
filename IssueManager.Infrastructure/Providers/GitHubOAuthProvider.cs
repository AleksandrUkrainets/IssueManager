using IssueManager.Application.Configuration;
using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;
using Microsoft.Extensions.Options;
using Refit;

namespace IssueManager.Infrastructure.Services.OAuth
{
    public class GitHubOAuthProvider : IOAuthProvider
    {
        private readonly OAuthProviderSettings _config;
        private readonly IGitHubOAuthClient _oauthClient;
        private readonly IGitHubApiClient _apiClient;

        public GitHubOAuthProvider(
            IOptions<OAuthSettings> settings,
            IGitHubOAuthClient oauthClient,
            IGitHubApiClient apiClient)
        {
            _config = settings.Value.Providers["github"];
            _oauthClient = oauthClient;
            _apiClient = apiClient;
        }

        public string GetAuthorizationUrl()
        {
            var queryParams = new Dictionary<string, string>
        {
            { "client_id", _config.ClientId },
            { "redirect_uri", _config.RedirectUri },
            { "response_type", "code" },
            { "scope", _config.Scope },
            { "state", "github" }
        };

            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{_config.AuthUrl}?{query}";
        }

        public async Task<string?> ExchangeCodeForTokenAsync(string code)
        {
            try
            {
                var response = await _oauthClient.ExchangeCodeForTokenAsync(new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret },
                { "code", code },
                { "redirect_uri", _config.RedirectUri }
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
                var user = await _apiClient.GetUserAsync($"Bearer {accessToken}");
                return user.Id.ToString();
            }
            catch
            {
                return null;
            }
        }
    }

}
