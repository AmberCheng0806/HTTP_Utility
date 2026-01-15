using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Utility
{
    public class util
    {
        public static string challengeMethod { get; set; } = "S256";
        public static string CodeVerifier { get; set; }
        public static string GetCodeVerifier()
        {
            Random random = new Random();
            string codeVerifier = "";
            for (int i = 0; i < 42; i++)
            {
                int number = random.Next(65, 123);
                Char numberChar = (Char)number;
                codeVerifier += numberChar.ToString();
            }
            CodeVerifier = codeVerifier;
            return codeVerifier;
        }

        public static string GetCodeChallenge(string codeVerifier)
        {
            SHA256 hash = SHA256Managed.Create();
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(codeVerifier));
            return Convert.ToBase64String(result)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
    }
}
