using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Interfaces
{
    public interface IIssueService
    {
        Task CreateIssueAsync(string userId, string provider, string repo, string title, string body);
        Task UpdateIssueAsync(string userId, string provider, string repo, int issueId, string title, string body);
        Task DeleteIssueAsync(string userId, string provider, string repo, int issueId);
    }
}
