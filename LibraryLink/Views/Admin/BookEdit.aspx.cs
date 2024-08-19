using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views.Admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        FileInfoStruct fileInfo = new FileInfoStruct();

        protected void Page_Load(object sender, EventArgs e)
        {

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



        }


        // 计算文件完整路径
        private string GetFullPath(string folder, string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath(folder), fileName);
            return fullPath;
        }

    }
}