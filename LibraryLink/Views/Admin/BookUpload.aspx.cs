
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models;
using LibraryLink.Models.DatabaseModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace LibraryLink.Views.Admin
{

    public partial class BookUpload : System.Web.UI.Page
    {
        FileInfoStruct fileInfo = new FileInfoStruct();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtBookName.Attributes.Add("oninput", "validateInput();");
                txtISBN.Attributes.Add("oninput", "validateInput();");
                txtAuthor.Attributes.Add("oninput", "validateInput();");
                txtPublisher.Attributes.Add("oninput", "validateInput();");
                txtPrice.Attributes.Add("oninput", "validateInput();");
                txtTags.Attributes.Add("oninput", "validateInput();");
                txtDescription.Attributes.Add("oninput", "validateInput();");
            }
        }

        #region 按钮事件处理
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
                string NameError = string.Empty;
                BookFileTip.InnerHtml = string.Empty;
                CoverImageTip.InnerHtml = string.Empty;

                if (!FileService.IsFileUploaded(BookFileUploader, out errorMsg)) {
                    BookFileTip.InnerText = errorMsg;
                }
                if (!FileService.IsFileUploaded(CoverImageUploader, out errorMsg)){
                    CoverImageTip.InnerText = errorMsg;
                }

                if (!FileService.FileCheck(BookFileUploader, validInfo, bookFilePath, out errorMsg))
                {
                    BookFileTip.InnerText = errorMsg;
                }

                validInfo.FileExtensions = fileInfo.ValidImageExtensions;
                validInfo.MaxSize = fileInfo.MaxImageSize;

                if (!FileService.FileCheck(CoverImageUploader, validInfo, coverImagePath, out errorMsg))
                {
                    CoverImageTip.InnerText = errorMsg;
                }

                if (errorMsg != string.Empty || NameError != string.Empty)
                {
                    Response.Write("<script>alert('检查文件大小或格式是否正确')</script>");
                    return;
                }
            } // 离开局部作用域

            string bookName = txtBookName.Text.Trim();
            string isbn = txtISBN.Text.Trim();
            string authorInfoList = txtAuthor.Text.Trim();
            string publisherName = txtPublisher.Text.Trim();
            DateTime publicationDate = DateTime.Parse(calPublicationDate.Text);
            decimal price = decimal.Parse(txtPrice.Text.Trim());
            string description = txtDescription.Text.Trim();
            string tags = txtTags.Text.Trim();

            using (var context = new Entities())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    { 
                        var book = context.Books.FirstOrDefault(b => b.ISBN == isbn);
                        if (book != null)
                        {
                            Response.Write("<script>alert('书籍已存在！')</script>");
                            return;
                        }
                        book = new Books
                        {
                            BookName = bookName,
                            ISBN = isbn,
                            Description = description,
                            BookRating = 0,  // 初始评分为0
                            Price = price,
                            CoverImagePath = coverImagePath,
                            FilePath = bookFilePath,
                        };
                        context.Books.Add(book);
                        // 作者信息
                        DBService.AuthorInfoInsert(context, Algo.AuthorsInfoPreprocess(authorInfoList), book);
                        // 出版社和出版信息
                        var publisher = DBService.PublisherInsert(context, publisherName);
                        book.PublicationDate = publicationDate;
                        book.PublisherID = publisher.PublisherID;
                        // 标签信息
                        DBService.TagsInsert(context, Algo.TagsPreprocess(tags), book);
                        // 保存文件
                        CoverImageUploader.SaveAs(coverImagePath);
                        BookFileUploader.SaveAs(bookFilePath);
                        context.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('上传成功！')</script>");
                    }
                    catch (Exception)
                    {
                        if (File.Exists(coverImagePath))
                        {
                            File.Delete(coverImagePath);
                        }
                        if (File.Exists(bookFilePath))
                        {
                            File.Delete(bookFilePath);
                        }
                        trans.Rollback();

                        Response.Write($"<script>alert('上传失败!')</script>");
                    }
                }
            }
        }
        #endregion 按钮事件处理

        // 计算文件完整路径
        private string GetFullPath(string folder, string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath(folder), fileName);
            return fullPath;
        }
    }
}