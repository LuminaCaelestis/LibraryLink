using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models.DatabaseModel;
using System.Data.Entity;
using System.Security.Policy;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;

namespace LibraryLink.Views.Admin
{
    public partial class BookSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBooksSearchView();
            }
        }

        protected void BindBooksSearchView()
        {
            var query = "SELECT b.BookID, BookName, ISBN, STRING_AGG(a.AuthorName, ', ') AS AuthorName, PublisherName, Price " +
                        "FROM Books b " +
                        "INNER JOIN Writes w ON b.BookID = w.BookID " +
                        "INNER JOIN Authors a ON w.AuthorID = a.AuthorID " +
                        "INNER JOIN Publication p ON b.BookID = p.BookID " +
                        "INNER JOIN Publisher per ON p.PublisherID = per.PublisherID " +
                        "GROUP BY b.BookID, BookName, ISBN, PublisherName, Price;";

            using (SqlConnection conn = new SqlConnection(Models.DatabaseConfig.ConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                BookSearchView.DataSource = dt;
                BookSearchView.DataBind();
            }
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var connectionString = Models.DatabaseConfig.ConnectionString;

            var query = "SELECT BookID, BookName, ISBN, STRING_AGG(a.AuthorName, ', ') AS AuthorName, PublisherName, Price " +
                        "FROM Books b " +
                        "INNER JOIN Writes w ON b.BookID = w.BookID " +
                        "INNER JOIN Authors a ON w.AuthorID = a.AuthorID " +
                        "INNER JOIN Publication p ON b.BookID = p.BookID " +
                        "INNER JOIN Publisher per ON p.PublisherID = per.PublisherID " +
                        "WHERE 1=1 ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(txtBookName.Text.Trim()))
            {
                query += "AND BookName LIKE @BookName ";
                parameters.Add(new SqlParameter("@BookName", $"%{txtBookName.Text.Trim()}%"));
            }

            if (!string.IsNullOrEmpty(txtISBN.Text.Trim()))
            {
                query += "AND ISBN = @ISBN ";
                parameters.Add(new SqlParameter("@ISBN", txtISBN.Text.Trim()));
            }

            if (decimal.TryParse(txtMinPrice.Text.Trim(), out decimal minPrice))
            {
                query += "AND Price >= @MinPrice ";
                parameters.Add(new SqlParameter("@MinPrice", minPrice));
            }

            if (decimal.TryParse(txtMaxPrice.Text.Trim(), out decimal maxPrice))
            {
                query += "AND Price <= @MaxPrice ";
                parameters.Add(new SqlParameter("@MaxPrice", maxPrice));
            }

            if (!string.IsNullOrEmpty(txtAuthorName.Text.Trim()))
            {
                query += "AND AuthorName LIKE @AuthorName ";
                parameters.Add(new SqlParameter("@AuthorName", $"%{txtAuthorName.Text.Trim()}%"));
            }

            if (!string.IsNullOrEmpty(txtPublisher.Text.Trim()))
            {
                query += "AND PublisherName LIKE @PublisherName ";
                parameters.Add(new SqlParameter("@PublisherName", $"%{txtPublisher.Text.Trim()}%"));
            }

            query += "GROUP BY BookName, ISBN, PublisherName, Price";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            BookSearchView.DataSource = dt;
            BookSearchView.DataBind();
        }


        protected void BookSearchView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Details")
            {
                long bookId = Convert.ToInt32(e.CommandArgument);  // 获取 BookID
                Response.Redirect($"BookDetails.aspx?BookID={bookId}");  // 重定向到详细信息页面
            }
            else if (e.CommandName == "Delete")
            {
                DeleteBook(Convert.ToInt32(e.CommandArgument));
            }

        }

        protected void DeleteBook(long bookID)
        {
            using(var context = new LibraryLinkDBContext())
            {

            }
        }

        // 翻页
        protected void BooksSearchView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int newPageIndex = e.NewPageIndex;
            if (newPageIndex < 0)
            {
                newPageIndex = 0;
            }
            else if (newPageIndex >= BookSearchView.PageCount)
            {
                newPageIndex = BookSearchView.PageCount - 1;
            }
            BookSearchView.PageIndex = newPageIndex;
            BindBooksSearchView();
        }

        protected void btnJumpToPage_Click(object sender, EventArgs e)
        {
            TextBox txtJumpToPage = (TextBox)BookSearchView.BottomPagerRow.FindControl("txtJumpToPage");

            if (txtJumpToPage != null)
            {
                int pageNumber;
                if (int.TryParse(txtJumpToPage.Text.Trim(), out pageNumber))
                {
                    pageNumber = pageNumber - 1;

                    if (pageNumber < 0)
                    {
                        pageNumber = 0;
                    }
                    else if (pageNumber >= BookSearchView.PageCount)
                    {
                        pageNumber = BookSearchView.PageCount - 1;
                    }
                    BookSearchView.PageIndex = pageNumber;
                    BindBooksSearchView();
                }
            }
        }


    }
}