
using IssueManager.Application.DTOs;

namespace IssueManager.Application.Interfaces
{
    public interface IIssueProvider
    {
        Task<IssueDto> CreateIssueAsync(string repo, string title, string body);
        Task<IssueDto> UpdateIssueAsync(string repo, int issueId, string title, string body);
        Task<bool> DeleteIssueAsync(string repo, int issueId);
    }
}
