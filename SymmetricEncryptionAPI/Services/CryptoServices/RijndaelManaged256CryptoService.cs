using System.Text;
using System.Security.Cryptography;

namespace SymmetricEncryptionAPI.Services.CryptoServices
{
    public class RijndaelManaged256CryptoService : ICryptoService
    {
        public string Name => "Rijndael-256";

        public (string key, string iv) GenerateKeyIV()
        {
            // Create a RijndaelManaged instance
            using var rijndael = new RijndaelManaged
            {
                // Set key size to 256 bits
                KeySize = 256
                // (BlockSize defaults to 256, which is fine here)
            };

            // Generate random Key and IV
            rijndael.GenerateKey();
            rijndael.GenerateIV();

            // Return Base64 strings for easy transport
            return (
                Convert.ToBase64String(rijndael.Key),
                Convert.ToBase64String(rijndael.IV)
            );
        }

        public string Encrypt(string plainText, string key, string iv)
        {
            using var rijndael = new RijndaelManaged
            {
                Key = Convert.FromBase64String(key),
                IV = Convert.FromBase64String(iv)
            };

            using var encryptor = rijndael.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(cipherBytes);
        }

        public string Decrypt(string cipherText, string key, string iv)
        {
            using var rijndael = new RijndaelManaged
            {
                Key = Convert.FromBase64String(key),
                IV = Convert.FromBase64String(iv)
            };

            using var decryptor = rijndael.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
