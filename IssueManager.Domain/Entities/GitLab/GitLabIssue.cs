using System.Text.Json.Serialization;

namespace IssueManager.Domain.Entities.GitLab
{
    public class GitLabIssue
    {
        public int Id { get; set; }

        public GitLabMilestone? Milestone { get; set; }

        public GitLabUser? Author { get; set; }

        public string? Description { get; set; }

        public string? State { get; set; }

        public int Iid { get; set; }

        public List<GitLabUser>? Assignees { get; set; }

        public GitLabUser? Assignee { get; set; }

        public string? Type { get; set; }

        public List<string>? Labels { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public int MergeRequestsCount { get; set; }

        public string? Title { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public GitLabUser? ClosedBy { get; set; }

        public bool Subscribed { get; set; }

        public int UserNotesCount { get; set; }

        public DateTime? DueDate { get; set; }

        public bool Imported { get; set; }

        public string? ImportedFrom { get; set; }

        public string? WebUrl { get; set; }

        public GitLabReferences? References { get; set; }

        public GitLabTimeStats? TimeStats { get; set; }

        public bool Confidential { get; set; }

        public bool DiscussionLocked { get; set; }

        public string? IssueType { get; set; }

        public string? Severity { get; set; }

        public GitLabTaskCompletionStatus? TaskCompletionStatus { get; set; }

        public int? Weight { get; set; }

        public bool HasTasks { get; set; }

        [JsonPropertyName("_links")]
        public GitLabLinks? Links { get; set; }

        public int? MovedToId { get; set; }

        public string? ServiceDeskReplyTo { get; set; }
    }
}
