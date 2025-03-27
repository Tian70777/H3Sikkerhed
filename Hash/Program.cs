using Hash.HashPassword;

Console.WriteLine("Please enter a password to be hashed: ");
string userInput = Console.ReadLine();


if (userInput != null)
    Console.WriteLine(userInput);

var saltAndHash = Hashing.CreateHash(userInput);

byte[] hashOnly = Hashing.HashWithoutSalt(userInput);
// Convert to Base64 for display
string hashBase64 = Convert.ToBase64String(hashOnly);
Console.WriteLine("Hash without salt (SHA-256, Base64): " + hashBase64);
// Base64 Encoding
string base64Representation = Convert.ToBase64String(saltAndHash);
Console.WriteLine("Salt+Hash (Base64): " + base64Representation);

// Hex Encoding
string hexRepresentation = BitConverter.ToString(saltAndHash).Replace("-", "");
Console.WriteLine("Salt+Hash (Hex): " + hexRepresentation);