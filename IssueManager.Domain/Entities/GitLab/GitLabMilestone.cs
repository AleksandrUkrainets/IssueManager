namespace IssueManager.Domain.Entities.GitLab
{
    public class GitLabMilestone
    {
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }

        public string? State { get; set; }

        public string? Description { get; set; }

        public int Iid { get; set; }

        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
    }
}
