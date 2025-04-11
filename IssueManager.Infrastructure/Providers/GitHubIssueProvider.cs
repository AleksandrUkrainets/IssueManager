using IssueManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Services
{
    public class GitHubIssueProvider : IIssueProvider
    {
        private readonly string _token;
        public GitHubIssueProvider(string token) => _token = token;

        public async Task CreateIssueAsync(string repo, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");

            var response = await client.PostAsJsonAsync($"https://api.github.com/repos/{repo}/issues", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
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
