using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Interfaces
{
    public interface IUserCredentialRepository
    {
        Task SaveCredentialAsync(string appUserId, string provider, string accessToken, string jwtToken);
        Task<(string accessToken, string jwtToken)?> GetCredentialAsync(string appUserId, string provider);
    }
}
