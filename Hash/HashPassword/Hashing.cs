using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hash.HashPassword
{
    public class Hashing
    {
        private const int SALT_SIZE = 24;
        private const int HASH_SIZE = 24;
        private const int ITERATIONS = 100000;

        public Hashing() { }    

        public static byte[] CreateHash(string input)
        {
            // 1. Generate a random salt using the new RandomNumberGenerator class
            byte[] salt = new byte[SALT_SIZE];
            RandomNumberGenerator.Fill(salt);

            // 2. Use Rfc2898DeriveBytes with the new constructor
            using var pbkdf2 = new Rfc2898DeriveBytes(
            input,
            salt,
            ITERATIONS,
            HashAlgorithmName.SHA256
            );

            // 3. derive the hash
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

            // 4. Combine salt + hash into one byte array
            byte[] saltAndHash = new byte[SALT_SIZE + HASH_SIZE];
            Buffer.BlockCopy(salt, 0, saltAndHash, 0, SALT_SIZE);
            Buffer.BlockCopy(hash, 0, saltAndHash, SALT_SIZE, HASH_SIZE);

            return saltAndHash;
        }

        /// <summary>
        /// Computes a SHA-256 hash of the input string without adding any salt.
        /// </summary>
        public static byte[] HashWithoutSalt(string input)
        {
            // Convert the string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Create a SHA256 instance
            using var sha256 = SHA256.Create();

            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // Return the resulting hash
            return hashBytes;
        }

    }
}
