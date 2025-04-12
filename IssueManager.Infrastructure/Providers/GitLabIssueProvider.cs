using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Domain.Entities.GitHub;
using IssueManager.Domain.Entities.GitLab;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;

namespace IssueManager.Infrastructure.Services
{
    public class GitLabIssueProvider : IIssueProvider
    {
        private readonly IGitLabApiClient _client;
        private readonly string _authToken;
        private readonly IMapper _mapper;

        public GitLabIssueProvider(IGitLabApiClient client, IMapper mapper, string accessToken)
        {
            _client = client;
            _authToken = $"Bearer {accessToken}";
            _mapper = mapper;
        }

        public async Task<IssueDto> CreateIssueAsync(string repo, string title, string body)
        {
            var projectId = Uri.EscapeDataString(repo);
            var issue = await _client.CreateIssueAsync(projectId, new { title, description = body }, _authToken);

            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto> UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            var projectId = Uri.EscapeDataString(repo);
            var issue = await _client.UpdateIssueAsync(projectId, issueId, new { title, description = body }, _authToken);

            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<bool> DeleteIssueAsync(string repo, int issueId)
        {
            var projectId = Uri.EscapeDataString(repo);
            var result = await _client.DeleteIssueAsync(projectId, issueId, _authToken);

            return result.IsSuccessStatusCode;
        }
    }
}
