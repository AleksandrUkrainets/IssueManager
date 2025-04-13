using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace IssueManager.Application.Services
{
    public class IssueService(IIssueProviderFactory factory, IMapper mapper, ILogger<IssueService> logger) : IIssueService
    {
        private IIssueProvider? _issueProvider;

        private async Task<bool> EnsureProviderInitializedAsync()
        {
            if (_issueProvider != null) return true;
            _issueProvider = await factory.CreateForCurrentUserAsync();
            if (_issueProvider == null)
            {
                logger.LogWarning("Failed to resolve IIssueProvider for current user.");
            }
            return _issueProvider != null;
        }

        public async Task<IssueDto?> CreateIssueAsync(IssueRequest request)
        {
            if (!await EnsureProviderInitializedAsync()) return null;
            var issue = await _issueProvider!.CreateIssueAsync(request.Repo, request.Title, request.Body);
            if (issue == null) logger.LogWarning($"Failed to Create Issue {request.Title} for {request.Repo}");

            logger.LogInformation("Created issue in repo: {Repo}", request.Repo);
            return issue;
        }

        public async Task<IssueDto?> UpdateIssueAsync(IssueUpdateRequest request)
        {
            if (!await EnsureProviderInitializedAsync()) return null;
            var issue = await _issueProvider!.UpdateIssueAsync(request.Repo, request.IssueId, request.Title, request.Body);
            if (issue == null) logger.LogWarning($"Failed to Update Issue {request.Title} for {request.Repo}");

            logger.LogInformation("Updated issue {IssueId} in repo: {Repo}", request.IssueId, request.Repo);
            return issue;
        }

        public async Task<bool> DeleteIssueAsync(string repo, int issueId)
        {
            if (!await EnsureProviderInitializedAsync()) return false;

            logger.LogInformation("Deleted issue {IssueId} in repo: {Repo}", issueId, repo);
            return await _issueProvider!.DeleteIssueAsync(repo, issueId);
        }
    }
}
