namespace IssueManager.Application.DTOs
{
    public class IssueDto
    {
        public long Id { get; set; }
        public long Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Body { get; set; }
        public string State { get; set; } = string.Empty;
    }
}
