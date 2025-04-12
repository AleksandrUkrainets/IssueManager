using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Clients
{
    public interface IGitHubOAuthClient
    {
        [Post("/login/oauth/access_token")]
        [Headers("Accept: application/json")]
        Task<Dictionary<string, string>> ExchangeCodeForTokenAsync(
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);

        public class UserResponse
        {
            public int Id { get; set; }
        }
    }
}
