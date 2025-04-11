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
    public class BitbucketIssueService : IIssueService
    {
        private readonly string _token;

        public BitbucketIssueService(string token) => _token = token;

        public async Task CreateIssueAsync(string userId, string provider, string repo, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.PostAsJsonAsync(
                $"https://api.bitbucket.org/2.0/repositories/{repo}/issues",
                new
                {
                    title,
                    content = new { raw = body }
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string userId, string provider, string repo, int issueId, string title, string body)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.PutAsJsonAsync(
                $"https://api.bitbucket.org/2.0/repositories/{repo}/issues/{issueId}",
                new
                {
                    title,
                    content = new { raw = body }
                });

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteIssueAsync(string userId, string provider, string repo, int issueId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await client.DeleteAsync(
                $"https://api.bitbucket.org/2.0/repositories/{repo}/issues/{issueId}"
            );

            response.EnsureSuccessStatusCode();
        }
    }

}
