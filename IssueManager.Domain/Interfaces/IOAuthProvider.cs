using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Interfaces
{
    public interface IOAuthProvider
    {
        string GetAuthorizationUrl();
        Task<string?> ExchangeCodeForTokenAsync(string code);
        Task<string?> GetUserIdAsync(string accessToken);
    }
}
