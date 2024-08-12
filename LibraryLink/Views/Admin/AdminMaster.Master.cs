using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["privilege"] == null || Convert.ToInt32(Session["privilege"]) != 1)
            {
                Response.Redirect("/Reader-Login/");
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("/Reader-Login/");
        }
    }
}