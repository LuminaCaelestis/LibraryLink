﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Win32;
using LibraryLink.Models;

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

            if (!DatabaseInterface.Login_Check(username, password, DatabaseConfig.ConnectionString, out string Msg))
            {
                Response.Write("<script>alert('" + Msg + "');</script>");
            }
            else
            {
                Session["username"] = username;
                Session["privilege"] = 0;
                Response.Write("<script>alert('" + Msg + "');</script>");
                Response.Redirect("/HomePage/");
            }
        }
    }
}