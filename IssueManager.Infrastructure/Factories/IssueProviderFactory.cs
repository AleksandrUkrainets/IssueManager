using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Factories
{
    public class IssueProviderFactory : IIssueProviderFactory
    {
        private readonly IUserCredentialRepository _credentialRepo;
        private readonly ICurrentUserService _currentUser;

        public IssueProviderFactory(IUserCredentialRepository credentialRepo, ICurrentUserService currentUser)
        {
            _credentialRepo = credentialRepo;
            _currentUser = currentUser;
        }

        public async Task<IIssueProvider?> CreateForCurrentUserAsync()
        {
            var userId = _currentUser.AppUserId;
            var provider = _currentUser.Provider;

            var tokens = await _credentialRepo.GetCredentialAsync(userId, provider);
            if (tokens == null) return null;

            return provider.ToLowerInvariant() switch
            {
                "github" => new GitHubIssueProvider(tokens.Value.accessToken),
                "gitlab" => new GitLabIssueProvider(tokens.Value.accessToken),
                "bitbucket" => new BitbucketIssueProvider(tokens.Value.accessToken),
                _ => null
            };
        }
    }
}
