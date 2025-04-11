using IssueManager.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Application.DROs
{
    public class IssueUpdateRequest : IssueRequest
    {
        public int IssueId { get; set; }
    }
}
