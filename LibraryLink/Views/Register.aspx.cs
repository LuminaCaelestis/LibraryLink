using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models;
using System.Text.RegularExpressions;

namespace LibraryLink.Views
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Register_Clicked(object sender, EventArgs e)
        {
            string username = Username_R.Value;
            string email = Email_R.Value;


            if (!Regex.IsMatch(username, @"^[a-zA-Z]\w{5,29}$"))
            {
                UsernameTip.InnerHtml = "用户名只能包含字母、数字、下划线";
                return;
            }
            if (!Regex.IsMatch(email, @"^[\w-_]+\@[\w-_]+\.{1}[\w-_]{2,8}$") || email.Length > 50)
            {
                EmailTip.InnerHtml = "邮箱格式不正确";
                return;
            }


            string connectionString = DatabaseConfig.ConnectionString;
            bool hasErrors = false;

            if(DatabaseInterface.Is_Record_Exists("Users", "Username", username, connectionString))
            {
                UsernameTip.InnerHtml = "用户名已存在";
                hasErrors = true;
            }
            if(DatabaseInterface.Is_Record_Exists("Users", "Email", email, connectionString))
            {
                EmailTip.InnerHtml = "邮箱已被使用";
                hasErrors = true;
            }
            if(hasErrors)
            {
                return;
            }

            string password = Password_R.Value;
            // 调用注册方法
            string Salt = Hash.GenerateSalt(32);
            byte[] hashedPassword = Hash.HashPassword(password, Salt);
            bool isSuccess = DatabaseInterface.Register_User(username, hashedPassword, email, Salt, connectionString);

            if (isSuccess)
            {
                Response.Write("<script>alert('注册成功!')</script>");
            }
            else
            {
                Response.Write("<script>alert('注册失败!')</script>");
            }
        }
    }
}