namespace IssueManager.Application.DTOs
{
    public record AuthTokenDto(bool IsSuccess, string? Jwt = null, string? Error = null);
}
