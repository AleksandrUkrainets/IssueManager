using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace IssueManager.Persistance.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }

    public class AesEncryptionService : IEncryptionService
    {
        private readonly string _key;

        public AesEncryptionService(IConfiguration config)
        {
            _key = config["Encryption:Key"]!;
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.GenerateIV();
            var encryptor = aes.CreateEncryptor();
            var inputBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            return $"{Convert.ToBase64String(aes.IV)}:{Convert.ToBase64String(encryptedBytes)}";
        }

        public string Decrypt(string cipherText)
        {
            var parts = cipherText.Split(':');
            var iv = Convert.FromBase64String(parts[0]);
            var encryptedData = Convert.FromBase64String(parts[1]);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
