using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        FileInfoStruct FileInfoStanderd = new FileInfoStruct();
        private long PortedBookID;
        private struct BookInfomation
        {
            public string BookName { get; set; }
            public string ISBN { get; set; }
            public string Price { get; set; }
            public string PublisherName { get; set; }
            public string Description { get; set; }
            public DateTime PublicationDate { get; set; }
            public string FilePath { get; set; }
            public string CoverImagePath { get; set; }
            public string AuthorString { get; set; }
            public string TagsString { get; set; }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (!long.TryParse(Request.QueryString["bookID"], out PortedBookID))
                {
                    Response.Redirect("~/Admin-BookSearch/");
                    return;
                }
                Session["BookID"] = PortedBookID;
                BookInfoInit();
                BookInfoLoad();
                txtEditBookName.Attributes.Add("oninput", "validateInput();");
                txtISBN.Attributes.Add("oninput", "validateInput();");
                txtAuthor.Attributes.Add("oninput", "validateInput();");
                txtPublisher.Attributes.Add("oninput", "validateInput();");
                txtPrice.Attributes.Add("oninput", "validateInput();");
                txtTags.Attributes.Add("oninput", "validateInput();");
                txtDescription.Attributes.Add("oninput", "validateInput();");
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
                    Response.Redirect("~/Admin-BookSearch/");
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
            }
        }

        private void BookInfoLoad()
        {
            txtAuthor.Text = (string)ViewState["AuthorString"];
            txtEditBookName.Text = (string)ViewState["BookName"];
            txtISBN.Text = (string)ViewState["ISBN"];
            txtPrice.Text = (string)ViewState["Price"];
            txtPublisher.Text = (string)ViewState["PublisherName"];
            txtDescription.Text = (string)ViewState["Description"];
            txtTags.Text = (string)ViewState["TagsString"];
            calPublicationDate.Text = ((DateTime)ViewState["PublicationDate"]).ToString("yyyy-MM-dd");
            txtAuthor.Text = (string)ViewState["AuthorString"];
            txtTags.Text = (string)ViewState["TagsString"];
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            #region 验证
            bool isValid = Algo.ValidateBookInfo(
                txtEditBookName, BookNameTip,
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

            BookInfomation newBookInfo = new BookInfomation();
            newBookInfo.BookName = txtEditBookName.Text.Trim();
            newBookInfo.ISBN = txtISBN.Text.Trim();
            newBookInfo.Price = txtPrice.Text.Trim();
            newBookInfo.PublisherName = txtPublisher.Text.Trim();
            newBookInfo.Description = txtDescription.Text.Trim();
            newBookInfo.PublicationDate = DateTime.Parse(calPublicationDate.Text);
            newBookInfo.AuthorString = txtAuthor.Text.Trim();
            newBookInfo.TagsString = txtTags.Text.Trim();

            ValidFileInfo validInfo = new ValidFileInfo
            {
                FileExtensions = FileInfoStanderd.ValidBookExtensions,
                MaxSize = FileInfoStanderd.MaxBookSize,
            };

            { // 进入局部作用域
                // 文件检查
                string errorMsg = string.Empty;
                BookFileTip.InnerHtml = string.Empty;
                CoverImageTip.InnerHtml = string.Empty;

                if (BookFileUploader.HasFile)
                {
                    newBookInfo.FilePath = GetFullPath(FileInfoStanderd.BookFolder, BookFileUploader.FileName);
                    if (!FileService.FileCheck(BookFileUploader, validInfo, newBookInfo.FilePath, out errorMsg))
                    {
                        BookFileTip.InnerText = errorMsg;
                    }
                }
                validInfo.FileExtensions = FileInfoStanderd.ValidImageExtensions;
                validInfo.MaxSize = FileInfoStanderd.MaxImageSize;
                if (CoverImageUploader.HasFile)
                {
                    newBookInfo.CoverImagePath = GetFullPath(FileInfoStanderd.CoverImageFolder, CoverImageUploader.FileName);
                    if (!FileService.FileCheck(CoverImageUploader, validInfo, newBookInfo.CoverImagePath, out errorMsg))
                    {
                        CoverImageTip.InnerText = errorMsg;
                    }
                }
                if (errorMsg != string.Empty)
                {
                    Response.Write("<script>alert('检查文件大小或格式是否正确')</script>");
                    return;
                }
            } // 离开局部作用域
            #endregion 验证

            var bookID = (long)Session["BookID"];

            using (var context = new Entities())
            {
                var newBookEntity = context.Books.Find(bookID);
                if (newBookEntity == null)
                {
                    Response.Write("<script>alert('该书籍不存在！')</script>");
                    return;
                }
                // 事务
                using (var trans = context.Database.BeginTransaction())
                {
                    // 提前声明文件服务类在try和catch块都保持可见
                    FileService FileController = new FileService();
                    try
                    {
                        #region 书信息   
                        newBookEntity.ISBN = newBookInfo.ISBN;
                        newBookEntity.BookName = newBookInfo.BookName;
                        newBookEntity.Price = decimal.Parse(newBookInfo.Price);
                        newBookEntity.Description = newBookInfo.Description;
                        #endregion 书信息
                        // 作者、出版社、出版信息
                        DBService.AuthorInfoInsert(context, Algo.AuthorsInfoPreprocess(newBookInfo.AuthorString), newBookEntity);
                        var publisher = DBService.PublisherInsert(context, newBookInfo.PublisherName);
                        newBookEntity.PublisherID = publisher.PublisherID;
                        newBookEntity.PublicationDate = newBookInfo.PublicationDate;
                        // 标签
                        DBService.TagsInsert(context, Algo.TagsPreprocess(newBookInfo.TagsString), newBookEntity);

                        // 文件区
                        if (BookFileUploader.HasFile)
                        {
                            string currPath = ViewState["FilePath"].ToString();
                            Response.Write("<script>console.log('书籍文件处理！');</script>");
                            FileController.BackupFile(currPath);
                            FileController.DeleteFile(currPath);
                            newBookEntity.FilePath = newBookInfo.FilePath;
                            BookFileUploader.SaveAs(newBookInfo.FilePath);
                            Response.Write("<script>console.log('书籍文件处理成功！');</script>");
                        }
                        if (CoverImageUploader.HasFile)
                        {
                            string currPath = ViewState["CoverImagePath"].ToString();
                            Response.Write("<script>console.log('封面文件处理！');</script>");
                            FileController.BackupFile(currPath);
                            FileController.DeleteFile(currPath);
                            newBookEntity.CoverImagePath = newBookInfo.CoverImagePath;
                            CoverImageUploader.SaveAs(newBookInfo.CoverImagePath);
                            Response.Write("<script>console.log('封面文件处理成功！');</script>");
                        }
                        context.SaveChanges();
                        trans.Commit();
                        FileController.DeleteBackupFiles();
                        Response.Write("<script>alert('修改成功！')</script>");
                    }
                    catch (Exception)
                    {
                        FileController.DeleteFile(newBookInfo.FilePath);
                        FileController.DeleteFile(newBookInfo.CoverImagePath);
                        FileController.RestoreFiles();
                        FileController.DeleteBackupFiles();
                        trans.Rollback();
                        Response.Write("<script>alert('修改失败')</script>");
                    }
                }
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
