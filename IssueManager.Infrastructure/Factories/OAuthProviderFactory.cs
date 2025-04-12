using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Services.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace IssueManager.Infrastructure.Factories
{
    public class OAuthProviderFactory : IOAuthProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public OAuthProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IOAuthProvider? Create(string provider)
        {
            return provider.ToLowerInvariant() switch
            {
                "github" => _serviceProvider.GetService<GitHubOAuthProvider>(),
                "gitlab" => _serviceProvider.GetService<GitLabOAuthProvider>(),
                _ => null
            };
        }
    }
}
