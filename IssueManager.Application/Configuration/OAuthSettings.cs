using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Configuration
{
    public class OAuthSettings
    {
        public Dictionary<string, OAuthProviderSettings> Providers { get; set; } = new();
    }
}
