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
    public class GitLabIssueService : IIssueService
    {
        private readonly string _token;

        public GitLabIssueService(string token) => _token = token;

        public async Task CreateIssueAsync(string userId, string provider, string repo, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var projectId = Uri.EscapeDataString(repo); // repo = "namespace/project"
            var response = await client.PostAsJsonAsync(
                $"https://gitlab.com/api/v4/projects/{projectId}/issues",
                new { title, description = body }
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string userId, string provider, string repo, int issueId, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var projectId = Uri.EscapeDataString(repo);
            var response = await client.PutAsJsonAsync(
                $"https://gitlab.com/api/v4/projects/{projectId}/issues/{issueId}",
                new { title, description = body }
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteIssueAsync(string userId, string provider, string repo, int issueId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var projectId = Uri.EscapeDataString(repo);
            var response = await client.DeleteAsync(
                $"https://gitlab.com/api/v4/projects/{projectId}/issues/{issueId}"
            );

            response.EnsureSuccessStatusCode();
        }
    }

}
