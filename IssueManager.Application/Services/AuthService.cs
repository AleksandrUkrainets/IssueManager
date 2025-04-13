using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace IssueManager.Application.Services
{
    public class AuthService(
        IOAuthProviderFactory providerFactory,
        IUserCredentialRepository credentialRepo,
        ITokenGenerator tokenGenerator, ILogger<AuthService> logger) : IAuthService
    {
        private IOAuthProvider? _provider;

        private bool EnsureProviderInitialized(string provider)
        {
            if (_provider != null) return true;
            _provider = providerFactory.Create(provider);
            return _provider != null;
        }

        public string? GetAuthUrl(string provider)
        {
            if (!EnsureProviderInitialized(provider))
            {
                logger.LogWarning("Unsupported provider: {Provider}", provider);
                return null;
            }

            logger.LogInformation("Generated auth URL for provider: {Provider}", provider);
            return _provider!.GetAuthorizationUrl();
        }

        public async Task<AuthTokenDto> SignIn(string code, string provider)
        {
            if (!EnsureProviderInitialized(provider))
            {
                logger.LogWarning("Unsupported provider during SignIn: {Provider}", provider);
                return new(false, null, "Unsupported provider");
            }

            string? accessToken = null;
            try
            {
                accessToken = await _provider!.ExchangeCodeForTokenAsync(code);
                if (string.IsNullOrEmpty(accessToken))
                {
                    logger.LogWarning("Access token missing for provider: {Provider}", provider);
                    return new(false, null, "Access token missing or invalid");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error exchanging code for access token for provider: {Provider}", provider);
                return new(false, null, "Access token exchange failed");
            }

            string? appUserId = null;
            try
            {
                appUserId = await _provider.GetUserIdAsync(accessToken);
                if (string.IsNullOrEmpty(appUserId))
                {
                    logger.LogWarning("User ID not found for provider: {Provider}", provider);
                    return new(false, null, "Failed to fetch user info");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching user ID for provider: {Provider}", provider);
                return new(false, null, "User info fetch failed");
            }

            try
            {
                var jwt = tokenGenerator.GenerateJwtToken(provider, accessToken, appUserId);
                await credentialRepo.SaveCredentialAsync(appUserId, provider, accessToken, jwt);
                logger.LogInformation("User {UserId} signed in with {Provider}", appUserId, provider);
                return new(true, jwt);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save credentials or generate JWT for user: {UserId}, provider: {Provider}", appUserId, provider);
                return new(false, null, "Failed to finalize login");
            }
        }
    }
}