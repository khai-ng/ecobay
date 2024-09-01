using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Identity.API.Application.Common.Extensions
{
    public class GeneratePassword
    {
        public required string Password { get; set; }
        public required string PasswordSalt { get; set; }
    }
    public static class PasswordExtension
    {
        public static GeneratePassword GeneratePassword(string password, string? passwordSalt = null)
        {
            byte[] salt;

            if (string.IsNullOrEmpty(passwordSalt))
                salt = RandomNumberGenerator.GetBytes(128 / 8);
            else
                salt = Convert.FromBase64String(passwordSalt);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return new GeneratePassword
            {
                Password = hashed,
                PasswordSalt = Convert.ToBase64String(salt)
            };
        }
    }
}
