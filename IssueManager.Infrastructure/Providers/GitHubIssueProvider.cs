using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Domain.Entities.GitHub;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;

namespace IssueManager.Infrastructure.Services
{
    public class GitHubIssueProvider(IGitHubApiClient client, IMapper mapper, string accessToken) : IIssueProvider
    {
        private readonly string _authToken = $"Bearer {accessToken}";

        public async Task<IssueDto> CreateIssueAsync(string repo, string title, string body)
        {
            var parts = repo.Split('/');
            var issue = await client.CreateIssueAsync(parts[0], parts[1], new { title, body }, _authToken);
            return mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            var parts = repo.Split('/');
            var issue = await client.UpdateIssueAsync(parts[0], parts[1], issueId, new { title, body }, _authToken);
            return mapper.Map<IssueDto>(issue);
        }

        public Task<bool> DeleteIssueAsync(string repo, int issueId) => Task.FromResult(false);
    }

}
