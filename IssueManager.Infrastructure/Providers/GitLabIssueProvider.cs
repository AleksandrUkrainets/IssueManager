using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Infrastructure.Clients;

namespace IssueManager.Infrastructure.Providers
{
    public class GitLabIssueProvider(IGitLabApiClient client, IMapper mapper, string accessToken) : IIssueProvider
    {
        private readonly string _authToken = $"Bearer {accessToken}";

        public async Task<IssueDto> CreateIssueAsync(string repo, string title, string body)
        {
            var projectId = repo;
            var issue = await client.CreateIssueAsync(projectId, new { title, description = body }, _authToken);

            return mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> UpdateIssueAsync(string repo, int issueNumber, string title, string body)
        {
            var projectId = repo;
            var issue = await client.UpdateIssueAsync(projectId, issueNumber, new { title, description = body }, _authToken);

            return mapper.Map<IssueDto>(issue);
        }

        public async Task<bool> DeleteIssueAsync(string repo, int issueNumber)
        {
            var projectId = repo;
            var result = await client.DeleteIssueAsync(projectId, issueNumber, _authToken);

            return result.IsSuccessStatusCode;
        }
    }
}
