using IssueManager.Application.DTOs;

namespace IssueManager.Application.Interfaces
{
    public interface IAuthService
    {
        string? GetAuthUrl(string provider);
        Task<AuthTokenDto> SignIn(string code, string provider);
    }
}
