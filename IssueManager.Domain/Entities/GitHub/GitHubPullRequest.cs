namespace IssueManager.Domain.Entities.GitHub
{
    public class GitHubPullRequest
    {
        public string? Url { get; set; }

        public string? HtmlUrl { get; set; }

        public string? DiffUrl { get; set; }

        public string? PatchUrl { get; set; }
    }
}
