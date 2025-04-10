namespace IssueManager.Domain.Entities.GitHub
{
    public class GitHubIssue
    {
        public int Id { get; set; }

        public string? NodeId { get; set; }

        public string? Url { get; set; }

        public string? RepositoryUrl { get; set; }

        public string? LabelsUrl { get; set; }

        public string? CommentsUrl { get; set; }

        public string? EventsUrl { get; set; }

        public string? HtmlUrl { get; set; }

        public int Number { get; set; }

        public string? State { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public GitHubUser? User { get; set; }

        public List<GitHubLabel>? Labels { get; set; }

        public GitHubUser? Assignee { get; set; }

        public List<GitHubUser>? Assignees { get; set; }

        public GitHubMilestone? Milestone { get; set; }

        public bool Locked { get; set; }

        public string? ActiveLockReason { get; set; }

        public int Comments { get; set; }

        public GitHubPullRequest? PullRequest { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public GitHubUser? ClosedBy { get; set; }

        public string? AuthorAssociation { get; set; }

        public string? StateReason { get; set; }
    }
}
