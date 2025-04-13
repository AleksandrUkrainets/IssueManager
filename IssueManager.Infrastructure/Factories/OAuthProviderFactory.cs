using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace IssueManager.Infrastructure.Factories
{
    public class OAuthProviderFactory(IServiceProvider serviceProvider) : IOAuthProviderFactory
    {
        public IOAuthProvider? Create(string provider)
        {
            return provider.ToLowerInvariant() switch
            {
                "github" => serviceProvider.GetService<GitHubOAuthProvider>(),
                "gitlab" => serviceProvider.GetService<GitLabOAuthProvider>(),
                _ => null
            };
        }
    }
}
