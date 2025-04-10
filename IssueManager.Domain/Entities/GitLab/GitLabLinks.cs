namespace IssueManager.Domain.Entities.GitLab
{
    public class GitLabLinks
    {
        public string? Self { get; set; }

        public string? Notes { get; set; }

        public string? AwardEmoji { get; set; }

        public string? Project { get; set; }

        public string? ClosedAsDuplicateOf { get; set; }
    }
}
