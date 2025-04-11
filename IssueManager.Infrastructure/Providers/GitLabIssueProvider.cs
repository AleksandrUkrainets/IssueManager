using IssueManager.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IssueManager.Infrastructure.Services
{
    public class GitLabIssueProvider(string token) : IIssueProvider
    {
        public async Task CreateIssueAsync(string repo, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var projectId = Uri.EscapeDataString(repo); // repo = "namespace/project"
            var response = await client.PostAsJsonAsync(
                $"https://gitlab.com/api/v4/projects/{projectId}/issues",
                new { title, description = body }
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var projectId = Uri.EscapeDataString(repo);
            var response = await client.PutAsJsonAsync(
                $"https://gitlab.com/api/v4/projects/{projectId}/issues/{issueId}",
                new { title, description = body }
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteIssueAsync(string repo, int issueId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var projectId = Uri.EscapeDataString(repo);
            var response = await client.DeleteAsync(
                $"https://gitlab.com/api/v4/projects/{projectId}/issues/{issueId}"
            );

            response.EnsureSuccessStatusCode();
        }
    }

}
