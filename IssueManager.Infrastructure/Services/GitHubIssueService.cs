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
    public class GitHubIssueService : IIssueService
    {
        private readonly string _token;
        public GitHubIssueService(string token) => _token = token;

        public async Task CreateIssueAsync(string userId, string provider, string repo, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");

            var response = await client.PostAsJsonAsync($"https://api.github.com/repos/{repo}/issues", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string userId, string provider, string repo, int issueId, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");

            var response = await client.PatchAsJsonAsync($"https://api.github.com/repos/{repo}/issues/{issueId}", new { title, body });
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteIssueAsync(string userId, string provider, string repo, int issueId)
        {
            await Task.CompletedTask;
        }
    }

}
