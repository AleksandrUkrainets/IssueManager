using IssueManager.Domain.Interfaces;

namespace IssueManager.Application.Interfaces
{
    public interface IIssueProviderFactory
    {
        Task<IIssueProvider?> CreateForCurrentUserAsync();
    }
}
