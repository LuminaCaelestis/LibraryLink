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


namespace LibraryLink.Views.Admin
{
    public partial class BookSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void BindBooksSearchView()
        {
            var query = "SELECT b.BookID, BookName, ISBN, STRING_AGG(a.AuthorName, ', ') AS AuthorName, PublisherName, Price, b.Available " +
                        "FROM Books b " +
                        "INNER JOIN Writes w ON b.BookID = w.BookID " +
                        "INNER JOIN Authors a ON w.AuthorID = a.AuthorID " +
                        "INNER JOIN Publication p ON b.BookID = p.BookID " +
                        "INNER JOIN Publisher per ON p.PublisherID = per.PublisherID " +
                        "GROUP BY b.BookID, BookName, ISBN, PublisherName, Price, Available;";

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
            ViewState["BookName"] = txtBookName.Text.Trim();
            ViewState["ISBN"] = txtISBN.Text.Trim();
            ViewState["MinPrice"] = txtMinPrice.Text.Trim();
            ViewState["MaxPrice"] = txtMaxPrice.Text.Trim();
            ViewState["AuthorName"] = txtAuthorName.Text.Trim();
            ViewState["Publisher"] = txtPublisher.Text.Trim();
            ViewState["Available"] = txtFilterAvailiable.SelectedValue;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var connectionString = Models.DatabaseConfig.ConnectionString;
            var query = "SELECT b.BookID, b.BookName, b.ISBN, STRING_AGG(a.AuthorName, ', ') AS AuthorName, per.PublisherName, b.Price, b.Available " +
                        "FROM Books b  " +
                        "INNER JOIN Writes w ON b.BookID = w.BookID  " +
                        "INNER JOIN Authors a ON w.AuthorID = a.AuthorID  " +
                        "INNER JOIN Publication p ON b.BookID = p.BookID  " +
                        "INNER JOIN Publisher per ON p.PublisherID = per.PublisherID  " +
                        "WHERE 1=1 ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (ViewState["BookName"] != null && !string.IsNullOrEmpty(ViewState["BookName"].ToString()))
            {
                query += "AND BookName LIKE @BookName ";
                parameters.Add(new SqlParameter("@BookName", $"%{ViewState["BookName"].ToString()}%"));
            }

            if (ViewState["ISBN"] != null && !string.IsNullOrEmpty(ViewState["ISBN"].ToString()))
            {
                query += "AND ISBN = @ISBN ";
                parameters.Add(new SqlParameter("@ISBN", ViewState["ISBN"].ToString()));
            }

            if (ViewState["MinPrice"] != null && decimal.TryParse(ViewState["MinPrice"].ToString(), out decimal minPrice))
            {
                query += "AND Price >= @MinPrice ";
                parameters.Add(new SqlParameter("@MinPrice", minPrice));
            }

            if (ViewState["MaxPrice"] != null && decimal.TryParse(ViewState["MaxPrice"].ToString(), out decimal maxPrice))
            {
                query += "AND Price <= @MaxPrice ";
                parameters.Add(new SqlParameter("@MaxPrice", maxPrice));
            }

            if (ViewState["AuthorName"] != null && !string.IsNullOrEmpty(ViewState["AuthorName"].ToString()))
            {
                query += "AND AuthorName LIKE @AuthorName ";
                parameters.Add(new SqlParameter("@AuthorName", $"%{ViewState["AuthorName"].ToString()}%"));
            }

            if (ViewState["Publisher"] != null && !string.IsNullOrEmpty(ViewState["Publisher"].ToString()))
            {
                query += "AND PublisherName LIKE @PublisherName ";
                parameters.Add(new SqlParameter("@PublisherName", $"%{ViewState["Publisher"].ToString()}%"));
            }

            if (ViewState["Available"] != null && !string.IsNullOrEmpty(ViewState["Available"].ToString()))
            {
                query += "AND Available = @Available ";
                parameters.Add(new SqlParameter("@Available", ViewState["Available"].ToString()));
            }

            query += "GROUP BY b.BookID, b.BookName, b.ISBN, per.PublisherName, b.Price, b.Available";

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
                long bookID = Convert.ToInt32(e.CommandArgument);  // 获取 BookID
                Response.Redirect($"BookDetails.aspx?BookID={bookID}");  // 重定向到详细信息页面
            }

        }

        protected void BookSearchView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long bookID = Convert.ToInt32(e.Keys[0]);  // 获取 BookID
            using(var Context = new Entities())
            {
                var book = Context.Books.Find(bookID);
                if(book == null)
                {
                    Response.Write("<script>alert('状态切换失败')</script>");
                    return;
                }
                book.Available = !book.Available;
               
                Context.SaveChanges();
                Response.Write("<script>alert('状态切换成功')</script>");
            }
            ApplyFilters();
        }

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
            ApplyFilters();
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
                    ApplyFilters();
                }
            }
        }


    }
}