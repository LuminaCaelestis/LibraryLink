using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models.DatabaseModel;

namespace LibraryLink.Views.Admin
{
    public partial class BookEdit : System.Web.UI.Page
    {
        FileInfoStruct fileInfo = new FileInfoStruct();
        private long bookID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (!long.TryParse(Request.QueryString["BookID"], out bookID))
                {
                    Response.Redirect("~/Admin-BookSearch/");
                    return;
                }
                txtBookName.Attributes.Add("oninput", "validateInput();");
                txtISBN.Attributes.Add("oninput", "validateInput();");
                txtAuthor.Attributes.Add("oninput", "validateInput();");
                txtPublisher.Attributes.Add("oninput", "validateInput();");
                txtPrice.Attributes.Add("oninput", "validateInput();");
                txtTags.Attributes.Add("oninput", "validateInput();");
                txtDescription.Attributes.Add("oninput", "validateInput();");
            }
        }

        private void BookInfoLoad()
        {
            using (var context = new Entities())
            {
                var targetBook = context.Books.Find(bookID);
                if (targetBook == null)
                {
                    Response.Write("<script>alert('该书籍不存在！')</script>");
                    Response.Redirect("~/Admin-BookSearch/");
                    return;
                }
                using(var trans = context.Database.BeginTransaction()) {

                    txtBookName.Text = targetBook.BookName;
                    txtISBN.Text = targetBook.ISBN;
                    txtPrice.Text = targetBook.Price.ToString();
                    //calPublicationDate.Text = DateTime.Parse(targetBook.Publication).ToShortDateString();

                    //// 通过中间表publication找到publisher

                    //var publisher = context.Publisher.Where(
                    //    targetBook.Publication.Where(p => p.BookID == targetBook.BookID)
                    //    ).FirstOrDefault();

                    var targetPublisher = (from book in context.Books
                                           join publication in context.Publication on book.BookID equals publication.BookID
                                           join publisher in context.Publisher on publication.PublisherID equals publisher.PublisherID
                                           where book.BookID == targetBook.BookID
                                           select new
                                           {
                                               publisher.PublisherName,
                                               book.BookName,
                                               publication.PublicationDate,
                                               book.ISBN,
                                               book.Price,
                                               book.Description,
                                               book.CoverImagePath,
                                               book.FilePath
                                           }).FirstOrDefault();

                    var targetTags

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isValid = Algo.ValidateBookInfo(
                txtBookName, BookNameTip,
                txtISBN, ISBNTip,
                txtAuthor, AuthorTip,
                txtPublisher, PublisherTip,
                calPublicationDate, PublicationDateTip,
                txtPrice, PriceTip,
                txtDescription, DescriptionTip,
                txtTags, TagTip);

            if (!isValid)
            {
                Response.Write("<script>alert('请检查输入信息是否正确！')</script>");
                return;
            }

            string coverImagePath = GetFullPath(fileInfo.CoverImageFolder, CoverImageUploader.FileName);
            string bookFilePath = GetFullPath(fileInfo.BookFolder, BookFileUploader.FileName);
            ValidFileInfo validInfo = new ValidFileInfo
            {
                FileExtensions = fileInfo.ValidBookExtensions,
                MaxSize = fileInfo.MaxBookSize,
            };

            { // 进入局部作用域

                // 文件检查
                string errorMsg = string.Empty;
                BookFileTip.InnerHtml = string.Empty;
                CoverImageTip.InnerHtml = string.Empty;

                if (!Algo.FileCheck(BookFileUploader, validInfo, bookFilePath, out errorMsg))
                {
                    BookFileTip.InnerText = errorMsg;
                }

                validInfo.FileExtensions = fileInfo.ValidImageExtensions;
                validInfo.MaxSize = fileInfo.MaxImageSize;

                if (!Algo.FileCheck(CoverImageUploader, validInfo, coverImagePath, out errorMsg))
                {
                    CoverImageTip.InnerText = errorMsg;
                }

                if (errorMsg != string.Empty)
                {
                    Response.Write("<script>alert('请检查文件大小或格式是否正确！')</script>");
                    return;
                }
            } // 离开局部作用域

            using (var context = new Entities())
            {
                var targetBook = context.Books.Find(bookID);
                if (targetBook == null)
                {
                    Response.Write("<script>alert('该书籍不存在！')</script>");
                    return;
                }

                //targetBook.BookName = txtBookName.Text;
                //targetBook.ISBN = txtISBN.Text;
                //targetBook.Author = txtAuthor.Text;
                //targetBook.Publisher = txtPublisher.Text;
                //targetBook.PublicationDate = calPublicationDate.SelectedDate;
                //targetBook.Price = decimal.Parse(txtPrice.Text);
                //targetBook.Description = txtDescription.Text;
                //targetBook.Tags = txtTags.Text;

                //if (BookFileUploader.HasFile)
                //{
                //    targetBook.BookFilePath = bookFilePath;
                //    BookFileUploader.SaveAs(bookFilePath);
                //}

                //if (CoverImageUploader.HasFile)
                //{
                //    targetBook.CoverImagePath = coverImagePath;
                //    CoverImageUploader.SaveAs(coverImagePath);
                //}

                //context.SaveChanges();

                //Response.Write("<script>alert('修改成功！')</script>");



            }














































        }

        // 计算文件完整路径
        private string GetFullPath(string folder, string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath(folder), fileName);
            return fullPath;
        }

    }
}