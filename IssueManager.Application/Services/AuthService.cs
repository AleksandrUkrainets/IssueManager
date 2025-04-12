using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;

namespace IssueManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOAuthProviderFactory _providerFactory;
        private readonly IUserCredentialRepository _credentialRepo;
        private readonly ITokenGenerator _tokenGenerator;

        private IOAuthProvider? _provider;

        public AuthService(
            IOAuthProviderFactory providerFactory,
            IUserCredentialRepository credentialRepo,
            ITokenGenerator tokenGenerator)
        {
            _providerFactory = providerFactory;
            _credentialRepo = credentialRepo;
            _tokenGenerator = tokenGenerator;
        }

        private bool EnsureProviderInitialized(string provider)
        {
            if (_provider != null) return true;
            _provider = _providerFactory.Create(provider);
            return _provider != null;
        }

        public string? GetAuthUrl(string provider)
        {
            return EnsureProviderInitialized(provider) ? _provider!.GetAuthorizationUrl() : null;
        }

        public async Task<AuthTokenResult> SignIn(string code, string provider)
        {
            if (!EnsureProviderInitialized(provider))
                return new(false, null, "Unsupported provider");

            var accessToken = await _provider!.ExchangeCodeForTokenAsync(code);
            if (string.IsNullOrEmpty(accessToken))
                return new(false, null, "Access token missing or invalid");

            var appUserId = await _provider.GetUserIdAsync(accessToken);
            if (string.IsNullOrEmpty(appUserId))
                return new(false, null, "Failed to fetch user info");

            var jwt = _tokenGenerator.GenerateJwtToken(provider, accessToken, appUserId);
            await _credentialRepo.SaveCredentialAsync(appUserId, provider, accessToken, jwt);

            return new(true, jwt);
        }
    }
}
