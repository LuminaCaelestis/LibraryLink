using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models.DatabaseModel;

namespace LibraryLink.Views.Reader
{
    public partial class BookDetails : System.Web.UI.Page
    {

        private string PortedBookID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PortedBookID = Request.QueryString["BookID"];
                ViewState["BookID"] = PortedBookID;
                BookInfoInit();
            }
        }

        private void BookInfoInit()
        {
            using (var context = new Entities())
            {
                var targetBook = context.Books.Find(PortedBookID);
                if (targetBook == null)
                {
                    Response.Write("<script>alert('该书籍不存在！')</script>");
                    //Response.Redirect("~/Admin-BookSearch/");
                    return;
                }

                var BookInfoResult = (from book in context.Books
                                      join publisher in context.Publisher
                                      on book.PublisherID equals publisher.PublisherID
                                      where book.BookID == targetBook.BookID
                                      select new
                                      {
                                          book.BookName,
                                          book.ISBN,
                                          book.Price,
                                          publisher.PublisherName,
                                          book.Description,
                                          book.PublicationDate,
                                          book.FilePath,
                                          book.CoverImagePath,
                                          book.BookRating,
                                      }).FirstOrDefault();

                var authorTempList = context.BookAuthorsView
                    .Where(info => info.BookID == targetBook.BookID)
                    .Select(info => info.AuthorName + "[" + info.Nationality + "]").ToList();

                var tagsTempList = context.BookTagsView
                    .Where(info => info.BookID == targetBook.BookID)
                    .Select(info => info.TagName).ToList();

                ViewState["BookName"] = BookInfoResult.BookName;
                ViewState["ISBN"] = BookInfoResult.ISBN;
                ViewState["Price"] = BookInfoResult.Price.ToString();
                ViewState["PublisherName"] = BookInfoResult.PublisherName;
                ViewState["Description"] = BookInfoResult.Description;
                ViewState["PublicationDate"] = BookInfoResult.PublicationDate;
                ViewState["FilePath"] = BookInfoResult.FilePath;
                ViewState["CoverImagePath"] = BookInfoResult.CoverImagePath;
                ViewState["AuthorString"] = string.Join(" ; ", authorTempList);
                ViewState["TagsString"] = string.Join(" ", tagsTempList);
                ViewState["BookRating"] = BookInfoResult.BookRating;
            }
        }

        protected void InfomationLoad(object sender, EventArgs e)
        {
            
        }

        private void TagsLoad(string TagsString)
        {
            string tagHtml = string.Empty;
            var Tags = TagsString.Split(' ');
            foreach (var tag in Tags)
            { 
                tagHtml += "<span class=\"badge bg-secondary m-1\" style=\"float: left;\">" + tag + "</span>\n";
            }
            TagsBox.InnerHtml = tagHtml;
        }
        

        

    }
}