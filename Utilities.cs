using System;
using System.Security.Cryptography;
using System.Text;

namespace AlgoApp
{
    public static class Utilities
    {
        public static string HashPassword(string password)
        {
            password += "AlgoApp.Salt";
            using var hasher = SHA1.Create();
            var hashedPasswordBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "");

            return hashedPassword;
        }
    }
}
