using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryLink.Models
{
    public class DatabaseInterface
    {
        // 通用方法, 接受字段名、值、连接字符串，返回是否存在记录
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

        // 登录检查, 接受用户名、密码、连接字符串,返回是否存在用户名和密码的组合

        public static bool Login_Check(string username, string password, string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }
    }
}


