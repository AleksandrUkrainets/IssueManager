namespace IssueManager.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string AppUserId { get; }
        string Provider { get; }
    }
}
