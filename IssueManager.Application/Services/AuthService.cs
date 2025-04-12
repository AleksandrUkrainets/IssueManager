using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace IssueManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOAuthProviderFactory _providerFactory;
        private readonly IUserCredentialRepository _credentialRepo;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<AuthService> _logger;

        private IOAuthProvider? _provider;

        public AuthService(
            IOAuthProviderFactory providerFactory,
            IUserCredentialRepository credentialRepo,
            ITokenGenerator tokenGenerator, ILogger<AuthService> logger)
        {
            _providerFactory = providerFactory;
            _credentialRepo = credentialRepo;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        private bool EnsureProviderInitialized(string provider)
        {
            if (_provider != null) return true;
            _provider = _providerFactory.Create(provider);
            return _provider != null;
        }

        public string? GetAuthUrl(string provider)
        {
            if (!EnsureProviderInitialized(provider))
            {
                _logger.LogWarning("Unsupported provider: {Provider}", provider);
                return null;
            }

            _logger.LogInformation("Generated auth URL for provider: {Provider}", provider);
            return _provider!.GetAuthorizationUrl();
        }

        public async Task<AuthTokenResult> SignIn(string code, string provider)
        {
            if (!EnsureProviderInitialized(provider))
            {
                _logger.LogWarning("Unsupported provider during SignIn: {Provider}", provider);
                return new(false, null, "Unsupported provider");
            }

            string? accessToken = null;
            try
            {
                accessToken = await _provider!.ExchangeCodeForTokenAsync(code);
                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogWarning("Access token missing for provider: {Provider}", provider);
                    return new(false, null, "Access token missing or invalid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exchanging code for access token for provider: {Provider}", provider);
                return new(false, null, "Access token exchange failed");
            }

            string? appUserId = null;
            try
            {
                appUserId = await _provider.GetUserIdAsync(accessToken);
                if (string.IsNullOrEmpty(appUserId))
                {
                    _logger.LogWarning("User ID not found for provider: {Provider}", provider);
                    return new(false, null, "Failed to fetch user info");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user ID for provider: {Provider}", provider);
                return new(false, null, "User info fetch failed");
            }

            try
            {
                var jwt = _tokenGenerator.GenerateJwtToken(provider, accessToken, appUserId);
                await _credentialRepo.SaveCredentialAsync(appUserId, provider, accessToken, jwt);
                _logger.LogInformation("User {UserId} signed in with {Provider}", appUserId, provider);
                return new(true, jwt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save credentials or generate JWT for user: {UserId}, provider: {Provider}", appUserId, provider);
                return new(false, null, "Failed to finalize login");
            }
        }
    }
}