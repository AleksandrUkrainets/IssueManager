using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IssueManager.Infrastructure.Factories
{
    public class IssueProviderFactory(
        IUserCredentialRepository credentialRepo,
        ICurrentUserService currentUser,
        IServiceProvider serviceProvider) : IIssueProviderFactory
    {
        public async Task<IIssueProvider?> CreateForCurrentUserAsync()
        {
            var userId = currentUser.AppUserId;
            var provider = currentUser.Provider;

            var tokens = await credentialRepo.GetCredentialAsync(userId, provider);
            if (tokens == null) return null;

            return provider.ToLowerInvariant() switch
            {
                "github" => ActivatorUtilities.CreateInstance<GitHubIssueProvider>(serviceProvider, tokens.Value.accessToken),
                "gitlab" => ActivatorUtilities.CreateInstance<GitLabIssueProvider>(serviceProvider, tokens.Value.accessToken),
                _ => null
            };
        }
    }
}
