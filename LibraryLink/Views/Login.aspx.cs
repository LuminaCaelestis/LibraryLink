using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Reader_Login_Click(object sender, EventArgs e)
        {
            string username = Username_L.Value;
            string password = Password_L.Value;

            if (username == "admin" && password == "123456")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('欢迎！但是这个页面还没做出来');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('无效凭证');", true);
            }
        }


    }
}