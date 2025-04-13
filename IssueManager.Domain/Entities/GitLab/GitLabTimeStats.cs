namespace IssueManager.Domain.Entities.GitLab
{
    public class GitLabTimeStats
    {
        public int? TimeEstimate { get; set; }

        public int? TotalTimeSpent { get; set; }

        public string? HumanTimeEstimate { get; set; }

        public string? HumanTotalTimeSpent { get; set; }
    }
}
