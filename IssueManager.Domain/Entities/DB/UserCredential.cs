using IssueManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Entities.DB
{
    public class UserCredential
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public ProviderType Provider { get; set; }
        public string AccessTokenEncrypted { get; set; } = string.Empty;
        public string JwtTokenEncrypted { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
