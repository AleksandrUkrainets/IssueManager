using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IssueManager.Application.DTOs
{
    public class AuthUrlRequest
    {
        public string Provider { get; set; } = string.Empty;
    }
}
