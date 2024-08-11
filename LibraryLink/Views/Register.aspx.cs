using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models;
using System.Data.SqlClient;

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

            string connectionString = DatabaseConfig.ConnectionString;
            bool hasErrors = false;

            if(DatabaseInterface.Is_Record_Exists("Username", username, connectionString))
            {
                UsernameTip.InnerHtml = "用户名已存在";
                hasErrors = true;
            }

            if(DatabaseInterface.Is_Record_Exists("Email", email, connectionString))
            {
                EmailTip.InnerHtml = "邮箱已被使用";
                hasErrors = true;
            }

            if(hasErrors)
            {
                return;
            }

            string password = Password_R.Value;

            


        }
    }
}