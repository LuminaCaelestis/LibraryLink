using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace LibraryLink.Models
{
    public class Hash
    {

        public static string GenerateSalt(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char[] saltChars = new char[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                for (int i = 0; i < saltChars.Length; i++)
                {
                    saltChars[i] = validChars[randomBytes[i] % validChars.Length];
                }
            }
            return new string(saltChars);
        }

        // 接收明文和盐值，返回哈希后的字节数组
        public static byte[] HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
                return sha256.ComputeHash(bytes);
            }
        }

    }
}