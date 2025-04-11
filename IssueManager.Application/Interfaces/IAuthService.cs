using IssueManager.Application.DTOs;
using IssueManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Interfaces
{
    public interface IAuthService
    {
        string? GetAuthUrl(string provider);
        Task<AuthTokenResult> SignIn(string code, string provider);
    }

    public record AuthTokenResult(bool IsSuccess, string? Jwt = null, string? Error = null);
}
