using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Factories
{
    public class IssueServiceFactory
    {
        private readonly ITokenStorageService _tokenStorage;

        public IssueServiceFactory(ITokenStorageService tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }

        public IIssueService? CreateFromToken(string provider, string accessToken)
        {
            return provider.ToLower() switch
            {
                "github" => new GitHubIssueService(accessToken),
                "gitlab" => new GitLabIssueService(accessToken),
                "bitbucket" => new BitbucketIssueService(accessToken),
                _ => null
            };
        }
    }
}
