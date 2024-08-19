
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

namespace LibraryLink.Views.Admin
{
    public partial class BookUpload : System.Web.UI.Page
    {
        private string CoverImageFolder { get; } = "~/Assets/Resource/CoverImages/";
        private string BookFolder { get; } = "~/Assets/Resource/BookFiles/";
        private string CoverImageFullPath { get; set; } = string.Empty;
        private string BookFullPath { get; set; } = string.Empty;
        private int MaxImageSize { get; } = 1024 * 1024 * 2;
        private int MaxBookSize { get; } = 1024 * 1024 * 400; // 400mb
        private string[] ValidImageExtensions { get; } = { ".jpg", ".jpeg", ".png" };
        private string[] ValidBookExtensions { get; } = { ".pdf"};

        Dictionary<string, string[]> ValidMimeTypes { get; } = new Dictionary<string, string[]>
        {
            { ".jpg", new[] { "image/jpeg", "image/jpg" } },
            { ".jpeg", new[] { "image/jpeg", "image/jpg" } },
            { ".png", new[] { "image/png" } },
            { ".pdf", new[] { "application/pdf" } }
        };

        struct ValidFileInfo
        {
            public string[] FileExtensions { get; set; }
            public int MaxSize { get; set; }
        }
        
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
            if (!IsValidInfo())
            {
                Response.Write("<script>alert('请检查输入信息是否正确！')</script>");
                return;
            }

            string coverImagePath = GetFullPath(CoverImageFolder, CoverImageUploader.FileName);
            string bookFilePath = GetFullPath(BookFolder, BookFileUploader.FileName);
            ValidFileInfo validInfo = new ValidFileInfo
            {
                FileExtensions = ValidBookExtensions,
                MaxSize = MaxBookSize,
            };

            // 文件检查
            string errorMsg = string.Empty;
            if (!FileCheck(BookFileUploader, validInfo, bookFilePath,out errorMsg))
            {
                BookFileTip.InnerText = errorMsg;
            }
            else
            {
                BookFileTip.InnerHtml = string.Empty;
            }
            validInfo.FileExtensions = ValidImageExtensions;
            validInfo.MaxSize = MaxImageSize;
            if (!FileCheck(CoverImageUploader, validInfo, coverImagePath, out errorMsg))
            {
                CoverImageTip.InnerText = errorMsg;
            }
            else
            {
                CoverImageTip.InnerHtml = string.Empty;
            }
            if(errorMsg!= string.Empty)
            { 
                Response.Write("<script>alert('请检查文件大小或格式是否正确！')</script>");
                return; 
            }
            string bookName = txtBookName.Text.Trim();
            string isbn = txtISBN.Text.Trim();
            string authorInfoList = txtAuthor.Text.Trim();
            string publisherName = txtPublisher.Text.Trim();
            DateTime publicationDate = DateTime.Parse(calPublicationDate.Text);
            decimal price = decimal.Parse(txtPrice.Text.Trim());
            string description = txtDescription.Text.Trim();
            string tags = txtTags.Text.Trim();

            using (var db = new Entities())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region 书信息   
                        var book = db.Books.FirstOrDefault(b => b.ISBN == isbn);
                        if (book == null)
                        {
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
                            db.Books.Add(book);
                        }
                        else
                        {
                            Response.Write("<script>alert('书籍已存在！')</script>");
                            return;
                        }
                        #endregion 书信息

                        #region 作者信息    
                        { // 进入局部作用域

                            // 去重
                            var processedAuthInfo = AuthorsInfoPreprocess(authorInfoList);
                            
                            // DB操作
                            List<Authors> newAuthors = new List<Authors>();

                            foreach (var (name, nation) in processedAuthInfo)
                            {
                                var author = db.Authors.FirstOrDefault(a => a.AuthorName == name && a.Nationality == nation);
                                if (author == null)
                                {
                                    author = new Authors
                                    {
                                        AuthorName = name,
                                        Nationality = nation,
                                    };
                                    newAuthors.Add(author);
                                }
                                if (!book.Authors.Contains(author))
                                {
                                    book.Authors.Add(author);
                                }
                            }
                        }
                        #endregion 作者信息

                        #region 出版社
                        var publisher = db.Publisher.FirstOrDefault(p => p.PublisherName == publisherName);
                        if (publisher == null)
                        {
                            publisher = new Publisher
                            {
                                PublisherName = publisherName,
                            };
                            db.Publisher.Add(publisher);
                        }
                        #endregion 出版社
                        db.SaveChanges();
                        #region 出版信息
                        var publication = db.Publication.FirstOrDefault
                        (
                            p => p.BookID == book.BookID && p.PublisherID == publisher.PublisherID
                        );
                        if (publication == null)
                        {
                            publication = new Publication
                            {
                                PublicationDate = publicationDate,
                                PublisherID = publisher.PublisherID,
                                BookID = book.BookID,
                            };
                        }
                        db.Publication.Add(publication);
                        #endregion 出版信息

                        #region 标签
                        {
                            var tagProcessed = TagPreprocess(tags);
                            List<Tags> newTags = new List<Tags>();
                            foreach (string tag in tagProcessed)
                            {
                                var trimedTag = tag.Trim();
                                var tagRecord = db.Tags.FirstOrDefault(t => t.TagName == trimedTag);
                                if (tagRecord == null)
                                {
                                    tagRecord = new Tags
                                    {
                                        TagName = trimedTag,
                                    };
                                    newTags.Add(tagRecord);
                                }
                                else
                                {
                                    book.Tags.Add(tagRecord);
                                }
                            }
                            db.Tags.AddRange(newTags);
                            foreach (var tag in newTags)
                            {
                                book.Tags.Add(tag);
                            }
                        }
                        #endregion 标签      
                        // 保存文件
                        CoverImageUploader.SaveAs(coverImagePath);
                        BookFileUploader.SaveAs(bookFilePath);
                        db.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('上传成功！')</script>");
                    }
                    catch (Exception ex)
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

        // 作者信息提取，去重等预处理
        private List<(string name, string nation)> AuthorsInfoPreprocess(string authorInfoList)
        {
            List<(string name, string nation)> result = new List<(string name, string nation)> { };
            HashSet<string> authorCheckSet = new HashSet<string>();
            var authorInfoArr = authorInfoList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var authorInfo in authorInfoArr)
            {
                var trimedInfo = authorInfo.Trim();

                if (authorCheckSet.Contains(trimedInfo))
                {
                    continue;
                }
                else
                {
                    authorCheckSet.Add(trimedInfo);
                }

                int startIndex = trimedInfo.IndexOf('[');
                int endIndex = trimedInfo.IndexOf(']');
                var authorName = trimedInfo.Substring(0, startIndex).Trim();
                var authorNationality = trimedInfo.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();

                result.Add((authorName, authorNationality));
            }
            return result;
        }

        // 标签去重
        private List<string> TagPreprocess(string tagList)
        {
            HashSet<string> tagSet = new HashSet<string>();
            var tagArr = tagList.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            foreach (var tag in tagArr)
            {
                var trimedTag = tag.Trim();
                if (tagSet.Contains(trimedTag))
                {
                    continue;
                }
                else
                {
                    tagSet.Add(trimedTag);
                    result.Add(trimedTag);
                }
            }
            return result;
        }

        #region 验证信息
        private bool IsValidInfo()
        {
            bool hasError = false;

            if (!Regex.IsMatch(txtBookName.Text.Trim(), @"^[\sa-zA-Z0-9\u4e00-\u9fa5\(\)]+$") ||
                txtBookName.Text.Trim() == string.Empty
            )
            {
                BookNameTip.InnerText = "中英文开头，包含字母、数字、汉字、空格、括号";
                hasError = true;
            }

            // 验证ISBN - 假设ISBN为13位数字或带有连接符的格式
            if (!Regex.IsMatch(txtISBN.Text.Trim(), @"^\d{13}$") ||
                txtISBN.Text.Trim() == string.Empty
            )
            {
                ISBNTip.InnerText = "ISBN必须为13位纯数字";
                hasError = true;
            }

            //if (!IsValidISBN(txtISBN.Text.Trim()))
            //{
            //    ISBNTip.InnerText += " ISBN校验码不正确";
            //    hasError = true;
            //}

            // 验证姓名国籍，名[国]，人名含中英文字符和空格。国籍是方括号[]内的纯汉字，不含空格。多个作者分号;分隔
            if (!Regex.IsMatch(txtAuthor.Text.Trim(), @"^(?:[\u4e00-\u9fa5A-Za-z\s]+\[[\u4e00-\u9fa5]+\]\s*;\s*)+[\u4e00-\u9fa5A-Za-z\s]+\[[\u4e00-\u9fa5\s]+\]\s*") ||
                txtAuthor.Text.Trim() == string.Empty
            )
            {
                AuthorTip.InnerText = "汉字、字母开头，英文以空格分割";
                hasError = true;
            }

            // 验证出版社名称 - 允许中文、英文，且非空
            if (!Regex.IsMatch(txtPublisher.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5\s]+$") ||
                txtPublisher.Text.Trim() == string.Empty
            )
            {
                PublisherTip.InnerText = "汉字、英文字母开头，空格分割单词";
                hasError = true;
            }

            // 如果没有选择出版日期，则显示提示信息
            if (string.IsNullOrEmpty(calPublicationDate.Text.Trim()))
            {
                PublicationDateTip.InnerText = "请选择出版日期";
                hasError = true;
            }

            // 验证价格 - 检查是否是正数
            if (txtPrice.Text.Trim() == string.Empty ||
                !decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || 
                price < 0m || price > 99999999.99m)
            {
                PriceTip.InnerText = "介于0~99999999.99间的阿拉伯数字";
                hasError = true;
            }

            // 验证书籍描述 - 最多2000字符
            if (txtDescription.Text.Trim().Length > 2000)
            {
                DescriptionTip.InnerText = "书籍描述不能超过2000字符";
                hasError = true;
            }

            // 验证标签 - 中英文
            string[] tags = txtTags.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tag in tags)
            {
                if (!Regex.IsMatch(tag.Trim(), @"^((?:[a-zA-Z\u4e00-\u9fa5]+)(?:\s*))+$"))
                {
                    TagTip.InnerText = "标签只能包含中文、英文";
                    hasError = true;
                    break;
                }
            }
            return !hasError;
        }
        #endregion 验证信息

        private static bool IsValidISBN(string isbn)
        {
            // 验证ISBN的末尾校验码，假设ISBN为13位数字
            // 根据ISBN13的规则
            // ISBN的末尾校验码是通过取前12位，偶数位乘3，然后求和，最后取余10，用10减去余数，结果应该等于最后一位数字
            int sum = 0;
            for (int i = 0; i != 12; ++i)
            {
                if(i % 2 == 0)
                {
                    sum += (isbn[i] - '0');
                }
                else
                {
                    sum += isbn[i] - '0' * 3;
                }
            }
            int res;
            if(sum % 10 == 0)
            {
                res = 0;
            }
            else {
                res = 10 - (sum % 10);
            }
            return res == (isbn[12] - '0');
        }

        private bool FileCheck(FileUpload fileUploader,ValidFileInfo ValidInfo, string fullPath, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (!fileUploader.HasFile)
            {
                errorMsg = "未选择文件";
                return false;
            }
            var extention = Path.GetExtension(fileUploader.FileName).ToLower();
            // 获取文件后缀
            if (!ValidInfo.FileExtensions.Contains(extention))
            {
                errorMsg = "文件格式不正确";
                return false;
            }
            // MimeType校验
            if (!ValidMimeTypes.ContainsKey(extention))
            {
                errorMsg = "文件格式不正确";
                return false;
            }
            // 大小
            if (fileUploader.PostedFile.ContentLength > ValidInfo.MaxSize)
            {
                errorMsg = "文件大小超出限制";
                return false;
            }
            if (File.Exists(fullPath))
            {
                errorMsg = "文件重名";
                return false;
            }
            return true;
        }
        // 计算文件完整路径
        private string GetFullPath(string folder, string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath(folder), fileName);
            return fullPath;
        }
    }
}