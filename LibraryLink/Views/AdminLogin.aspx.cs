using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using LibraryLink.Models;
using static LibraryLink.Models.DatabaseInterface;

namespace LibraryLink.Views
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Admin_Login_Click(object sender, EventArgs e)
        {
            string username = Request.Form["Username_Admin_Login"];
            string password = Request.Form["Password_Admin_Login"];

            string connectStr= DatabaseConfig.ConnectionString;
            bool hasError = false;

            if (!Is_Admin_Exists(username, connectStr))
            {
                Response.Write("<script>alert('管理员用户名或密码错误');</script>");
                hasError = true;
            }
            if (!Login_Check(username, password, connectStr, out string Msg))
            {
                Response.Write("<script>alert('" + Msg + "');</script>");
                hasError = true;
            }
            if(hasError)
            {
                return;
            }
            Session["username"] = username;
            Session["privilege"] = 1;
            Response.Write("<script>alert('欢迎管理员'); window.location.href='/Admin-UsersManagement/';</script>");

        }
    }
}