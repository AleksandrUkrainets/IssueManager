using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Domain.Interfaces
{
    public interface ITokenStorageService
    {
        Task SaveTokenAsync(string appUserId, string provider, string accessToken, string jwtToken);
        Task<(string accessToken, string jwtToken)?> GetTokenAsync(string appUserId, string provider);
    }
}
