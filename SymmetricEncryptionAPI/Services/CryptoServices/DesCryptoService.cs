using System.Security.Cryptography;
using System.Text;

namespace SymmetricEncryptionAPI.Services.CryptoServices
{
    public class DesCryptoService : ICryptoService
    {
        public string Name => "DES 56 bit";

        public (string key, string iv) GenerateKeyIV()
        {
            using var des = DES.Create();
            
            des.GenerateKey();
            des.GenerateIV();

            // Return Base64 strings
            return (
                Convert.ToBase64String(des.Key),
                Convert.ToBase64String(des.IV)
            );
        }
       
        public string Encrypt(string plainText, string key, string iv)
        {
            using var des = DES.Create();
            des.Key = Convert.FromBase64String(key);
            des.IV = Convert.FromBase64String(iv);

            using var encryptor = des.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        public string Decrypt(string cipherText, string key, string iv)
        {
            using var des = DES.Create();
            des.Key = Convert.FromBase64String(key);
            des.IV = Convert.FromBase64String(iv);

            using var decryptor = des.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
