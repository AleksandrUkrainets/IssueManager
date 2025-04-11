using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Configuration
{
    public class OAuthProviderSettings
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string AuthUrl { get; set; }
        public required string TokenUrl { get; set; }
        public required string Scope { get; set; }
        public required string RedirectUri { get; set; }
    }
}
