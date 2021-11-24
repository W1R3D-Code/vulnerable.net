
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Vulnerable.MVC.Areas.Identity
{
    public class WeakPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public string HashPassword(TUser user, string password)
            => Md5Hash(password);

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
            => string.Equals(hashedPassword, Md5Hash(providedPassword), StringComparison.Ordinal)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;

        private string Md5Hash(string plaintext)
        {
            using var hash = MD5.Create();

            var plaintextBytes = Encoding.ASCII.GetBytes(plaintext);
            var hashBytes = hash.ComputeHash(plaintextBytes);

            var sb = new StringBuilder();
            
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            //  Introduce potential for timing attacks
            Thread.Sleep(1500);

            return sb.ToString();
        }
    }
}
