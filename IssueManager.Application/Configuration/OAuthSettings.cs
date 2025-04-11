namespace IssueManager.Application.Configuration
{
    public class OAuthSettings
    {
        public Dictionary<string, OAuthProviderSettings> Providers { get; set; } = new();
    }
}
