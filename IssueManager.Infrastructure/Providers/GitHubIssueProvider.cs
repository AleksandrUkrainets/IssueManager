using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;

namespace IssueManager.Infrastructure.Services
{
    public class GitHubIssueProvider : IIssueProvider
    {
        private readonly IGitHubApiClient _client;
        private readonly string _authToken;

        public GitHubIssueProvider(IGitHubApiClient client, string accessToken)
        {
            _client = client;
            _authToken = $"Bearer {accessToken}";
        }

        public async Task CreateIssueAsync(string repo, string title, string body)
        {
            var parts = repo.Split('/');
            await _client.CreateIssueAsync(parts[0], parts[1], new { title, body }, _authToken);
        }

        public async Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            var parts = repo.Split('/');
            await _client.UpdateIssueAsync(parts[0], parts[1], issueId, new { title, body }, _authToken);
        }

        public Task DeleteIssueAsync(string repo, int issueId) => Task.CompletedTask;
    }
}
