using IssueManager.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IssueManager.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        public string AppUserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found");

        public string Provider => _httpContextAccessor.HttpContext?.User?.FindFirst("provider")?.Value
            ?? throw new UnauthorizedAccessException("Provider not found");
    }
}
