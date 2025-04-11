using IssueManager.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IssueManager.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public string AppUserId => accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found");

        public string Provider => accessor.HttpContext?.User?.FindFirst("provider")?.Value
            ?? throw new UnauthorizedAccessException("Provider not found");
    }
}
