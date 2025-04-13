namespace IssueManager.Domain.Entities.GitHub
{
    public class GitHubLabel
    {
        public long? Id { get; set; }

        public string? NodeId { get; set; }

        public string? Url { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Color { get; set; }

        public bool? Default { get; set; }
    }
}
