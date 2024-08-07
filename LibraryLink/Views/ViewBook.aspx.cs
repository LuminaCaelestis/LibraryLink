using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views
{
    public partial class ViewBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 测试
            pdfViewer.Attributes["src"] = "~/Assets/Books/托马斯微积分第十版中文版.pdf";
        }

    }
}