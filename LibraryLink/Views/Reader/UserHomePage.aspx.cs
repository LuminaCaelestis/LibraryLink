using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models.DatabaseModel;

namespace LibraryLink.Views.Reader
{
    public partial class UserHomePage : System.Web.UI.Page
    {
        private int PageSize = 6; // 每页显示18个项目
        protected int CurrentPage
        {
            get
            {
                return (int)(ViewState["CurrentPage"] ?? 0);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBooks();
            }
        }

        private void BindBooks()
        {
            using (var context = new Entities())
            {
                // 这里复用了一个很复杂的视图
                var books = context.DetailsView
                    .OrderBy(b => b.BookName)
                    .Skip(CurrentPage * PageSize) // 跳过前几页的记录
                    .Take(PageSize) 
                    .Select(b => new
                    {
                        b.BookID,
                        b.BookName,
                        b.AuthorList,
                        b.CoverImagePath

                    }).ToList();

                // 生成HTML

                string htmlContent = string.Empty;
                string ImageFolderPath = "../../Assets/Resource/CoverImages/";
                foreach (var book in books)
                {
                    string ImageFileName = System.IO.Path.GetFileName(book.CoverImagePath);
                    string EncodedFileName = HttpUtility.UrlEncode(ImageFileName);  // 对文件名进行URL编码,不然特殊字符会导致图片无法显示
                    htmlContent += $@"
                    <div class=""col-4 px-1"">
                        <div class=""book-card card my-2 d-flex flex-row"" style=""height: 12rem;"">
                            <!-- 图片部分 -->
                            <div class=""book-card-img-container"">
                                <img loading=""lazy"" src=""{ImageFolderPath}{EncodedFileName}"" class=""book-card-img"" >
                            </div>
                            <!-- 文字部分 -->
                            <div class=""book-card-info-container card-body"">
                                <h5 class=""card-title"">{book.BookName}</h5>
                                <p class=""card-text"">作者：<br />{book.AuthorList}</p>
                            </div>
                        </div>
                    </div>";
                }

                Literal1.Text += htmlContent;

                // 控制分页按钮的状态
                btnPrev.Enabled = CurrentPage > 0;
                btnNext.Enabled = books.Count == PageSize; // 如果取出的记录数少于PageSize，说明已经是最后一页?
            }
        }

        protected void ClearSearch()
        {
            Literal1.Text = string.Empty;
        }

        protected void ChangePage(object sender, EventArgs e)
        {
            if (sender == btnPrev && CurrentPage > 0)
            {
                CurrentPage--;
            }
            else if (sender == btnNext)
            {
                CurrentPage++;
            }
            ClearSearch();
            BindBooks(); // 重新绑定数据
        }


    }
}