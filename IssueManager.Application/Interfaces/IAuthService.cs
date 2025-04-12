namespace IssueManager.Application.Interfaces
{
    public interface IAuthService
    {
        string? GetAuthUrl(string provider);
        Task<AuthTokenResult> SignIn(string code, string provider);
    }

    public record AuthTokenResult(bool IsSuccess, string? Jwt = null, string? Error = null);
}
