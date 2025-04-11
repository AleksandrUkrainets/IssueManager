using IssueManager.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Interfaces
{
    public interface IIssueService
    {
        Task<bool> CreateIssueAsync(IssueRequest request);
        Task<bool> UpdateIssueAsync(IssueUpdateRequest request);
        Task<bool> DeleteIssueAsync(string repo, int issueId);
    }
}
