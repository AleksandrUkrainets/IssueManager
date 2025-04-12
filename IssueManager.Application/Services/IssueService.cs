using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Application.Interfaces;

namespace IssueManager.Application.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueProviderFactory _factory;
        private readonly IMapper _mapper;
        private IIssueProvider? _issueProvider;

        public IssueService(IIssueProviderFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        private async Task<bool> EnsureProviderInitializedAsync()
        {
            if (_issueProvider != null) return true;
            _issueProvider = await _factory.CreateForCurrentUserAsync();
            return _issueProvider != null;
        }

        public async Task<IssueDto?> CreateIssueAsync(IssueRequest request)
        {
            if (!await EnsureProviderInitializedAsync()) return null;
            var issue = await _issueProvider!.CreateIssueAsync(request.Repo, request.Title, request.Body);
            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IssueDto?> UpdateIssueAsync(IssueUpdateRequest request)
        {
            if (!await EnsureProviderInitializedAsync()) return null;
            var issue = await _issueProvider!.UpdateIssueAsync(request.Repo, request.IssueId, request.Title, request.Body);
            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<bool> DeleteIssueAsync(string repo, int issueId)
        {
            if (!await EnsureProviderInitializedAsync()) return false;
            return await _issueProvider!.DeleteIssueAsync(repo, issueId);
        }
    }
}
