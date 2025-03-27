namespace SymmetricEncryptionAPI.Services.CryptoServices
{
    public interface ICryptoService
    {
        
        string Name { get; }
        // A name or identifier for this encryption method (e.g. "AES-128", "AES-192")
        (string key, string iv) GenerateKeyIV();
        // Generate a Key and IV for this particular encryption method
        string Encrypt(string plainText, string base64Key, string base64IV);
        // Encrypt a plain text using the provided base64-encoded key and IV

        string Decrypt(string cipherText, string base64Key, string base64IV);
        // Decrypt a cipher text using the provided base64-encoded key and IV
    }
}
