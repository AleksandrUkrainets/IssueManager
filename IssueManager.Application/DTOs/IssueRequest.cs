namespace IssueManager.Application.DTOs
{
    public class IssueRequest
    {
        public string Repo { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
