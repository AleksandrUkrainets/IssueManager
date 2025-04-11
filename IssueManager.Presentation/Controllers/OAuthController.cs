using IssueManager.Application.Configuration;
using IssueManager.Application.DTOs;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace IssueManager.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly OAuthSettings _settings;
        private readonly ITokenStorageService _tokenStorage;
        private readonly IConfiguration _configuration;

        public OAuthController(IOptions<OAuthSettings> settings, ITokenStorageService tokenStorage, IConfiguration configuration)
        {
            _settings = settings.Value;
            _tokenStorage = tokenStorage;
            _configuration = configuration;
        }

        [HttpPost("authUrl")]
        public IActionResult GetAuthUrl([FromBody] SignInRequest request)
        {
            if (!_settings.Providers.TryGetValue(request.Provider.ToLower(), out var config))
                return BadRequest("Unsupported provider");

            var state = request.Provider.ToLower();

            var queryParams = new Dictionary<string, string>
            {
                { "client_id", config.ClientId },
                { "redirect_uri", config.RedirectUri },
                { "response_type", "code" },
                { "scope", config.Scope },
                { "state", state }
            };

            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var authUrl = $"{config.AuthUrl}?{query}";

            return Ok(new { authUrl });
        }

        [HttpGet("signIn")]
        public async Task<IActionResult> SignIn([FromQuery] string code, [FromQuery] string state)
        {
            var provider = state.ToLower();

            if (!_settings.Providers.TryGetValue(provider, out var config))
                return BadRequest("Unsupported provider");

            using var client = new HttpClient();
            HttpRequestMessage tokenRequest;

            if (provider == "bitbucket")
            {
                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{config.ClientId}:{config.ClientSecret}"));
                tokenRequest = new HttpRequestMessage(HttpMethod.Post, config.TokenUrl)
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_code" },
                        { "code", code },
                        { "redirect_uri", config.RedirectUri }
                    })
                };
                tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            }
            else
            {
                tokenRequest = new HttpRequestMessage(HttpMethod.Post, config.TokenUrl)
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
            }

            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(tokenRequest);
            if (!response.IsSuccessStatusCode)
                return BadRequest("Failed to retrieve access token");

            var content = await response.Content.ReadAsStringAsync();

            string? accessToken = null;
            try
            {
                using var jsonDoc = JsonDocument.Parse(content);
                if (jsonDoc.RootElement.TryGetProperty("access_token", out var accessTokenElement))
                {
                    accessToken = accessTokenElement.GetString();
                }
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON in token response");
            }

            if (string.IsNullOrEmpty(accessToken))
                return BadRequest("Access token not found in response");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");

            string appUserId;
            try
            {
                if (provider == "github")
                {
                    var userResp = await client.GetFromJsonAsync<JsonElement>("https://api.github.com/user");
                    appUserId = userResp.GetProperty("id").ToString();
                }
                else if (provider == "gitlab")
                {
                    var userResp = await client.GetFromJsonAsync<JsonElement>("https://gitlab.com/api/v4/user");
                    appUserId = userResp.GetProperty("id").ToString();
                }
                else if (provider == "bitbucket")
                {
                    var userResp = await client.GetFromJsonAsync<JsonElement>("https://api.bitbucket.org/2.0/user");
                    appUserId = userResp.GetProperty("uuid").GetString() ?? Guid.NewGuid().ToString();
                }
                else return BadRequest("Unsupported provider");
            }
            catch
            {
                return BadRequest("Failed to fetch user info from provider");
            }

            var jwt = GenerateJwtToken(provider, accessToken, appUserId);
            await _tokenStorage.SaveTokenAsync(appUserId, provider, accessToken, jwt);

            return Ok(new { token = jwt });
        }

        private string GenerateJwtToken(string provider, string accessToken, string appUserId)
        {
            var claims = new[]
            {
                new Claim("provider", provider),
                new Claim("external_token", accessToken),
                new Claim(ClaimTypes.NameIdentifier, appUserId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
