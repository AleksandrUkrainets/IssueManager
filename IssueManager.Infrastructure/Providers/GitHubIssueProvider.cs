using IssueManager.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IssueManager.Infrastructure.Services
{
    public class GitHubIssueProvider : IIssueProvider
    {
        private readonly HttpClient _client;

        public GitHubIssueProvider(HttpClient client, string token)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.github.com/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");
        }

        public async Task CreateIssueAsync(string repo, string title, string body)
        {
            var response = await _client.PostAsJsonAsync($"repos/{repo}/issues", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            var response = await _client.PatchAsJsonAsync($"repos/{repo}/issues/{issueId}", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public Task DeleteIssueAsync(string repo, int issueId) => Task.CompletedTask;
    }
}
