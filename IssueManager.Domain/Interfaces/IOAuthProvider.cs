namespace IssueManager.Domain.Interfaces
{
    public interface IOAuthProvider
    {
        string GetAuthorizationUrl();
        Task<string?> ExchangeCodeForTokenAsync(string code);
        Task<string?> GetUserIdAsync(string accessToken);
    }
}
