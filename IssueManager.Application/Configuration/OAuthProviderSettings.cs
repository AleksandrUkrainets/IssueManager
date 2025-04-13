namespace IssueManager.Application.Configuration
{
    public class OAuthProviderSettings
    {
        public required string ClientId { get; set; }

        public required string ClientSecret { get; set; }

        public required string AuthUrl { get; set; }

        public required string Scope { get; set; }

        public required string RedirectUri { get; set; }

        public required string ApiBaseUrl { get; set; }

        public required string BaseUrl { get; set; }
    }
}
