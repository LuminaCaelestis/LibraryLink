using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryLink.Models
{
    public class DatabaseInterface
    {
        /// <summary>
        /// 检查数据库中是否存在给定的记录。
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="value">值</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns>如果记录存在，返回true；否则返回false。</returns>
        public static bool Is_Record_Exists(string columnName, string value, string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = $"SELECT COUNT(*) FROM Users WHERE {columnName} = @{columnName}";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue($"@{columnName}", value);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }

        /// <summary>
        /// 检查数据库中给定用户的密码与输入值是否匹配
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns>如果用户名和密码的组合存在，返回true；否则返回false。</returns>
        public static bool Login_Check(string username, string password, string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Salt, Password FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string salt = (string)reader["Salt"];
                    byte[] storedHashedPassword = (byte[])reader["Password"];
                    byte[] inputHashedPassword = Hash.HashPassword(password, salt);
                    // 比较计算出的哈希值和数据库中的哈希值
                    Console.WriteLine($"stored password: {storedHashedPassword}");
                    Console.WriteLine($"input password: {inputHashedPassword}");
                    Console.WriteLine($"stored salt: {salt}");
                    if (inputHashedPassword.SequenceEqual(storedHashedPassword))
                    {
                        return true;
                    }
                }
                conn.Close();
                return false;
            }
        }

        /// <summary>
        /// 注册新用户。
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns>如果注册成功，返回true；否则返回false。</returns>
        public static bool Register_User(string username, byte[] password, string email, string salt, string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Users (Username, Password, Email, Salt) VALUES (@Username, @Password, @Email, @Salt)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Salt", salt);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    return false;
                }
            }
        }
    }
}


