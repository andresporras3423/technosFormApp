using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace technosFormApp
{
    public static class Session
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            Console.WriteLine("entered password is: " + enteredPassword);
            Console.WriteLine("Stored hash is: " + storedHash);
            string enteredHash = HashPassword(enteredPassword);
            Console.WriteLine("Entered password after hashing is: " + enteredHash);
            return enteredHash == storedHash;
        }
    }
}
