using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Services;

namespace IssueManager.Infrastructure.Factories
{
    public class IssueProviderFactory(IUserCredentialRepository credentialRepo, ICurrentUserService currentUser) : IIssueProviderFactory
    {
        public async Task<IIssueProvider?> CreateForCurrentUserAsync()
        {
            var userId = currentUser.AppUserId;
            var provider = currentUser.Provider;

            var tokens = await credentialRepo.GetCredentialAsync(userId, provider);
            if (tokens == null) return null;

            return provider.ToLowerInvariant() switch
            {
                "github" => new GitHubIssueProvider(tokens.Value.accessToken),
                "gitlab" => new GitLabIssueProvider(tokens.Value.accessToken),
                _ => null
            };
        }
    }
}
