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
        /// <summary>
        /// 生成随机盐
        /// </summary>
        /// <param name="length">指定盐值的长度</param>
        /// <returns>返回随机盐值</returns>
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

        /// <summary>
        /// 接收明文密码和盐，返回哈希值
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
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