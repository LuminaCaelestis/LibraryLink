using System;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace LibraryLink
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "AdminLogin", // 路由名称
                "Admin-Login", // URL 模式
                "~/Views/AdminLogin.aspx" // 物理文件路径
            );

            routes.MapPageRoute(
                "ReaderLogin",
                "Reader-Login",
                "~/Views/Login.aspx"
            );

        }

    }
}