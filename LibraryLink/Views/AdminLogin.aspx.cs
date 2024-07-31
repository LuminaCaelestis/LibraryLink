using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Admin_Login_Click(object sender, EventArgs e)
        {
            string username = Request.Form["Username_L"];
            string password = Request.Form["Password_L"];

            if (username == "admin" && password == "123456")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('欢迎管理员！但是这个页面还没做出来');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('无效凭证');", true);
            }
        }
    }
}