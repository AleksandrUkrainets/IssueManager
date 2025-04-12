using IssueManager.Domain.Entities.DB;
using IssueManager.Domain.Enums;
using IssueManager.Domain.Interfaces;
using IssueManager.Persistance.Security;
using Microsoft.EntityFrameworkCore;

namespace IssueManager.Persistance.Repository
{
    public class UserCredentialRepository(AppDbContext db, IEncryptionService encryption) : IUserCredentialRepository
    {
        public async Task SaveCredentialAsync(string appUserId, string provider, string accessToken, string jwtToken)
        {
            var type = Enum.Parse<ProviderType>(provider, ignoreCase: true);
            var encryptedAccess = encryption.Encrypt(accessToken);
            var encryptedJwt = encryption.Encrypt(jwtToken);

            var existing = await db.UserCredentials.FirstOrDefaultAsync(x => x.AppUserId == appUserId && x.Provider == type);
            if (existing != null)
            {
                existing.AccessTokenEncrypted = encryptedAccess;
                existing.JwtTokenEncrypted = encryptedJwt;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                db.UserCredentials.Add(new UserCredential
                {
                    AppUserId = appUserId,
                    Provider = type,
                    AccessTokenEncrypted = encryptedAccess,
                    JwtTokenEncrypted = encryptedJwt,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await db.SaveChangesAsync();
        }

        public async Task<(string accessToken, string jwtToken)?> GetCredentialAsync(string appUserId, string provider)
        {
            var type = Enum.Parse<ProviderType>(provider, ignoreCase: true);
            var userCredentialResult = await db.UserCredentials.FirstOrDefaultAsync(x => x.AppUserId == appUserId && x.Provider == type);

            if (userCredentialResult == null) return null;

            return (
                encryption.Decrypt(userCredentialResult.AccessTokenEncrypted),
                encryption.Decrypt(userCredentialResult.JwtTokenEncrypted)
            );
        }
    }
}
