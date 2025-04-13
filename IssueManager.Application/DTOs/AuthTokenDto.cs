using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.DTOs
{
    public record AuthTokenDto(bool IsSuccess, string? Jwt = null, string? Error = null);
}
