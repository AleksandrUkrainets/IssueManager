using IssueManager.Application.DTOs;

namespace IssueManager.Application.Interfaces
{
    public interface IIssueService
    {
        Task<bool> CreateIssueAsync(IssueRequest request);
        Task<bool> UpdateIssueAsync(IssueUpdateRequest request);
        Task<bool> DeleteIssueAsync(string repo, int issueId);
    }
}
