using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Interfaces
{
    public interface IIssueProvider
    {
        Task CreateIssueAsync(string repo, string title, string body);
        Task UpdateIssueAsync(string repo, int issueId, string title, string body);
        Task DeleteIssueAsync(string repo, int issueId);
    }
}
