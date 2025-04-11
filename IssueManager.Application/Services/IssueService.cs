using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using IssueManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Services
{
    public class IssueService(IIssueProviderFactory factory) : IIssueService
    {
        private readonly IIssueProviderFactory _factory = factory;
        private IIssueProvider? _issueProvider;

        private async Task<bool> EnsureProviderInitializedAsync()
        {
            if (_issueProvider != null) return true;
            _issueProvider = await _factory.CreateForCurrentUserAsync();
            return _issueProvider != null;
        }

        public async Task<bool> CreateIssueAsync(IssueRequest request)
        {
            if (!await EnsureProviderInitializedAsync()) return false;
            await _issueProvider!.CreateIssueAsync(request.Repo, request.Title, request.Body);
            return true;
        }

        public async Task<bool> UpdateIssueAsync(IssueUpdateRequest request)
        {
            if (!await EnsureProviderInitializedAsync()) return false;
            await _issueProvider!.UpdateIssueAsync(request.Repo, request.IssueId, request.Title, request.Body);
            return true;
        }

        public async Task<bool> DeleteIssueAsync(string repo, int issueId)
        {
            if (!await EnsureProviderInitializedAsync()) return false;
            await _issueProvider!.DeleteIssueAsync(repo, issueId);
            return true;
        }
    }
}
