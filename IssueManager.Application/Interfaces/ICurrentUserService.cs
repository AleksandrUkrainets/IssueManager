using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string AppUserId { get; }
        string Provider { get; }
    }
}
