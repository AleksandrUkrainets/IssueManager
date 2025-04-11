using IssueManager.Application.Configuration;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http;

namespace IssueManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly OAuthSettings _settings;
        private readonly IUserCredentialRepository _credentialRepo;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(
            IOptions<OAuthSettings> settings,
            IUserCredentialRepository credentialRepo,
            ITokenGenerator tokenGenerator,
            IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _credentialRepo = credentialRepo;
            _tokenGenerator = tokenGenerator;
            _httpClientFactory = httpClientFactory;
        }

        public string? GetAuthUrl(string provider)
        {
            var state = provider.ToLower();
            if (!_settings.Providers.TryGetValue(state, out var config))
                return null;

            var queryParams = new Dictionary<string, string>
            {
                { "client_id", config.ClientId },
                { "redirect_uri", config.RedirectUri },
                { "response_type", "code" },
                { "scope", config.Scope },
                { "state", state }
            };

            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{config.AuthUrl}?{query}";
        }

        public async Task<AuthTokenResult> SignIn(string code, string provider)
        {
            if (!_settings.Providers.TryGetValue(provider.ToLower(), out var config))
                return new(false, null, "Unsupported provider");

            var accessToken = await GetAccessTokenAsync(code, config);
            if (string.IsNullOrEmpty(accessToken))
                return new(false, null, "Access token missing or invalid");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");

            var appUserId = await GetAppUserIdAsync(provider, client);
            if (string.IsNullOrEmpty(appUserId))
                return new(false, null, "Failed to fetch user info");

            var jwt = _tokenGenerator.GenerateJwtToken(provider, accessToken, appUserId);
            await _credentialRepo.SaveCredentialAsync(appUserId, provider, accessToken, jwt);

            return new(true, jwt);
        }

        private async Task<string?> GetAccessTokenAsync(string code, OAuthProviderSettings config)
        {
            var client = _httpClientFactory.CreateClient();

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, config.TokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", config.ClientId },
                    { "client_secret", config.ClientSecret },
                    { "code", code },
                    { "redirect_uri", config.RedirectUri },
                    { "grant_type", "authorization_code" }
                })
            };

            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(tokenRequest);
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                using var jsonDoc = JsonDocument.Parse(content);
                if (jsonDoc.RootElement.TryGetProperty("access_token", out var accessTokenElement))
                {
                    return accessTokenElement.GetString();
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private async Task<string?> GetAppUserIdAsync(string provider, HttpClient client)
        {
            try
            {
                if (provider == "github")
                {
                    var userResp = await client.GetFromJsonAsync<JsonElement>("https://api.github.com/user");
                    return userResp.GetProperty("id").ToString();
                }
                else if (provider == "gitlab")
                {
                    var userResp = await client.GetFromJsonAsync<JsonElement>("https://gitlab.com/api/v4/user");
                    return userResp.GetProperty("id").ToString();
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}
