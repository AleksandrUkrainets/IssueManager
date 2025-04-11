using IssueManager.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IssueManager.Infrastructure.Services
{
    public class GitHubIssueProvider(string token) : IIssueProvider
    {
        public async Task CreateIssueAsync(string repo, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");

            var response = await client.PostAsJsonAsync($"https://api.github.com/repos/{repo}/issues", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");

            var response = await client.PatchAsJsonAsync($"https://api.github.com/repos/{repo}/issues/{issueId}", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteIssueAsync(string repo, int issueId)
        {
            await Task.CompletedTask;
        }
    }

}
