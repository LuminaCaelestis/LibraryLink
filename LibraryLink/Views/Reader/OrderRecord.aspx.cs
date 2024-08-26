using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views.Reader
{
    public partial class OrderRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string orderNumber = txtOrderNumber.Text.Trim();
            //string isbn = txtISBN.Text.Trim();
            //string bookName = txtBookName.Text.Trim();
            //string author = txtAuthor.Text.Trim();
            ////string startDate = txtStartDate.Text.Trim();
            ////string endDate = txtEndDate.Text.Trim();

            //// 构建SQL查询语句
            //string query = "SELECT * FROM Orders WHERE 1=1";

            //if (!string.IsNullOrEmpty(orderNumber))
            //    query += " AND OrderNumber LIKE @OrderNumber";

            //if (!string.IsNullOrEmpty(isbn))
            //    query += " AND ISBN LIKE @ISBN";

            //if (!string.IsNullOrEmpty(bookName))
            //    query += " AND BookName LIKE @BookName";

            //if (!string.IsNullOrEmpty(author))
            //    query += " AND Author LIKE @Author";

            //if (!string.IsNullOrEmpty(startDate))
            //    query += " AND OrderDate >= @StartDate";

            //if (!string.IsNullOrEmpty(endDate))
            //    query += " AND OrderDate <= @EndDate";

            //// 使用SQL连接和命令来执行查询
            //using (SqlConnection conn = new SqlConnection("YourConnectionString"))
            //{
            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        if (!string.IsNullOrEmpty(orderNumber))
            //            cmd.Parameters.AddWithValue("@OrderNumber", "%" + orderNumber + "%");

            //        if (!string.IsNullOrEmpty(isbn))
            //            cmd.Parameters.AddWithValue("@ISBN", "%" + isbn + "%");

            //        if (!string.IsNullOrEmpty(bookName))
            //            cmd.Parameters.AddWithValue("@BookName", "%" + bookName + "%");

            //        if (!string.IsNullOrEmpty(author))
            //            cmd.Parameters.AddWithValue("@Author", "%" + author + "%");

            //        if (!string.IsNullOrEmpty(startDate))
            //            cmd.Parameters.AddWithValue("@StartDate", startDate);

            //        if (!string.IsNullOrEmpty(endDate))
            //            cmd.Parameters.AddWithValue("@EndDate", endDate);

            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);

            //        gvOrders.DataSource = dt;
            //        gvOrders.DataBind();
            //    }
            //}
        }

        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // 如果需要自定义行的样式或数据展示，可以在这里处理
        }

    }
}