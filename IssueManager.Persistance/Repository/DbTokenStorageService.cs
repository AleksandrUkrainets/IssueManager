using IssueManager.Domain.Entities.DB;
using IssueManager.Domain.Enums;
using IssueManager.Domain.Interfaces;
using IssueManager.Persistance.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Persistance.Repository
{
    public class DbTokenStorageService : ITokenStorageService
    {
        private readonly AppDbContext _db;
        private readonly IEncryptionService _encryption;

        public DbTokenStorageService(AppDbContext db, IEncryptionService encryption)
        {
            _db = db;
            _encryption = encryption;
        }

        public async Task SaveTokenAsync(string appUserId, string provider, string accessToken, string jwtToken)
        {
            var type = Enum.Parse<ProviderType>(provider, ignoreCase: true);
            var encryptedAccess = _encryption.Encrypt(accessToken);
            var encryptedJwt = _encryption.Encrypt(jwtToken);

            var existing = await _db.UserCredentials.FirstOrDefaultAsync(x => x.AppUserId == appUserId && x.Provider == type);
            if (existing != null)
            {
                existing.AccessTokenEncrypted = encryptedAccess;
                existing.JwtTokenEncrypted = encryptedJwt;
            }
            else
            {
                _db.UserCredentials.Add(new UserCredential
                {
                    AppUserId = appUserId,
                    Provider = type,
                    AccessTokenEncrypted = encryptedAccess,
                    JwtTokenEncrypted = encryptedJwt,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task<(string accessToken, string jwtToken)?> GetTokenAsync(string appUserId, string provider)
        {
            var type = Enum.Parse<ProviderType>(provider, ignoreCase: true);
            var record = await _db.UserCredentials.FirstOrDefaultAsync(x => x.AppUserId == appUserId && x.Provider == type);

            if (record == null) return null;

            return (
                _encryption.Decrypt(record.AccessTokenEncrypted),
                _encryption.Decrypt(record.JwtTokenEncrypted)
            );
        }
    }
}
