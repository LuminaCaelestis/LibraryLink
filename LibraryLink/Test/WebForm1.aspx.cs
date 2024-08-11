using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private bool isAdmin(string strAdmin, string strPassword)
        {
            return true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strAdminname = tname.Text;
            string strAdminPW = tpass.Text;

            if (strAdminname == String.Empty || strAdminPW == String.Empty)
            {
                Response.Write("<script>");
                Response.Write("alert('用户名/密码 不能为空!!!');");
                Response.Write("</script>");
                return;
            }


            if (isAdmin(strAdminname, strAdminPW))
            {
                Session["User"] = strAdminname;
                Response.Write("<script>alert('成功登陆');</script>");
                Response.Write("<script>parent.location.href='Test.aspx';</script>");
            }
            else
            {
                Response.Write("<script>");
                Response.Write("alert('用户名/密码 不正确!!!');");
                Response.Write("</script>");
            }

        }
    }
}