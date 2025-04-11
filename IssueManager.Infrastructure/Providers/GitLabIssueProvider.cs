using IssueManager.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IssueManager.Infrastructure.Services
{
    public class GitLabIssueProvider : IIssueProvider
    {
        private readonly HttpClient _client;

        public GitLabIssueProvider(HttpClient client, string token)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://gitlab.com/api/v4/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task CreateIssueAsync(string repo, string title, string body)
        {
            var projectId = Uri.EscapeDataString(repo);
            var response = await _client.PostAsJsonAsync(
                $"projects/{projectId}/issues",
                new { title, description = body }
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            var projectId = Uri.EscapeDataString(repo);
            var response = await _client.PutAsJsonAsync(
                $"projects/{projectId}/issues/{issueId}",
                new { title, description = body }
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteIssueAsync(string repo, int issueId)
        {
            var projectId = Uri.EscapeDataString(repo);
            var response = await _client.DeleteAsync(
                $"projects/{projectId}/issues/{issueId}"
            );

            response.EnsureSuccessStatusCode();
        }
    }
}
