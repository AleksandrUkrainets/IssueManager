using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;

namespace IssueManager.Tests.FakesObjects
{
    public class FakeIssueProvider : IIssueProvider
    {

        public async Task<IssueDto> CreateIssueAsync(string repo, string title, string body)
        {
            return await Task.FromResult(new IssueDto { Title = title, Body = body });
        }

        public Task<IssueDto> UpdateIssueAsync(string repo, int issueId, string title, string body)
        {
            return Task.FromResult(new IssueDto { Title = title, Body = body });
        }

        public Task<bool> DeleteIssueAsync(string repo, int issueId)
        {
            return Task.FromResult(true);
        }
    }
}
