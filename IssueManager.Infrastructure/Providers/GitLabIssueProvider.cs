using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IssueManager.Infrastructure.Services
{
    public class GitLabIssueProvider : IIssueProvider
    {
        private readonly IGitLabApiClient _client;
        private readonly string _authToken;

        public GitLabIssueProvider(IGitLabApiClient client, string accessToken)
        {
            _client = client;
            _authToken = $"Bearer {accessToken}";
        }

        public Task CreateIssueAsync(string repo, string title, string body)
        {
            var projectId = Uri.EscapeDataString(repo);
            return _client.CreateIssueAsync(projectId, new { title, description = body }, _authToken);
        }

        public Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            var projectId = Uri.EscapeDataString(repo);
            return _client.UpdateIssueAsync(projectId, issueId, new { title, description = body }, _authToken);
        }

        public Task DeleteIssueAsync(string repo, int issueId)
        {
            var projectId = Uri.EscapeDataString(repo);
            return _client.DeleteIssueAsync(projectId, issueId, _authToken);
        }
    }
}
