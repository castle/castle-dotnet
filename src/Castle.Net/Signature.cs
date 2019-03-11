using System;
using System.Security.Cryptography;
using System.Text;

namespace Castle
{
    public static class Signature
    {
        public static string Compute(string key, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashed = hmac.ComputeHash(messageBytes);
                return string.Concat(Array.ConvertAll(hashed, x => x.ToString("x2")));
            }
        }
    }
}
