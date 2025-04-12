using IssueManager.Application.DTOs;

namespace IssueManager.Application.Interfaces
{
    public interface IIssueService
    {
        Task<IssueDto?> CreateIssueAsync(IssueRequest request);
        Task<IssueDto?> UpdateIssueAsync(IssueUpdateRequest request);
        Task<bool> DeleteIssueAsync(string repo, int issueId);
    }
}
