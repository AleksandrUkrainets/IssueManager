namespace IssueManager.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateJwtToken(string provider, string accessToken, string appUserId);
    }
}
