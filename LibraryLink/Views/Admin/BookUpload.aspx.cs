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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtBookName.Attributes.Add("oninput", "validateInput();");
                txtISBN.Attributes.Add("oninput", "validateInput();");
                txtAuthor.Attributes.Add("oninput", "validateInput();");
                txtAuthorNationality.Attributes.Add("oninput", "validateInput();");
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
                return;
            }

            string bookName = txtBookName.Text.Trim();
            string isbn = txtISBN.Text.Trim();

            string authorName = txtAuthor.Text.Trim();
            string authorNationality = txtAuthorNationality.Text.Trim();

            string publisherName = txtPublisher.Text.Trim();
            DateTime publicationDate = DateTime.Parse(calPublicationDate.Text);

            decimal price = decimal.Parse(txtPrice.Text.Trim());

            string description = txtDescription.Text.Trim();
            string[] tags = txtTags.Text.Trim().Split(' ');

            // 上传文件路径管理
            string coverImagePath = SaveFile(fuCoverImage, "~/Assets/Resource/CoverImages/");
            string bookFilePath = SaveFile(fuBookFile, "~/Assets/Resource/BookFiles/");

            bool uploadSuccess = true;

            if(string.IsNullOrEmpty(coverImagePath))
            {
                CoverImageTip.InnerText = "未选择封面图片";
                uploadSuccess = false;
            }
            if (string.IsNullOrEmpty(bookFilePath))
            {
                BookFileTip.InnerText = "未选择上传文件";
                uploadSuccess = false;
            }
            if (!uploadSuccess)
            {
                return;
            }

            // LINQ
            using (var db = new LibraryLinkDBContext())
            {
                // 事务包裹整个操作，方便回滚
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
                            db.SaveChanges(); // 提前保存，生成ID
                        }
                        else
                        {
                            Response.Write("<script>alert('书籍已存在！')</script>");
                        }
                        #endregion 书信息

                        #region 作者信息    
                        var author = db.Authors.FirstOrDefault
                        (
                            a => a.AuthorName == authorName && a.Nationality == authorNationality
                        );
                        if (author == null)
                        {
                            author = new Authors
                            {
                                AuthorName = authorName,
                                Nationality = authorNationality,
                            };
                            db.Authors.Add(author);
                        }
                        book.Authors.Add(author);
                        db.SaveChanges();
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
                        db.SaveChanges();
                        #endregion 出版社

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
                        // 这里没有立刻产生自增ID的需求，所以暂时不用SaveChanges()
                        #endregion 出版信息

                        #region 标签
                        var newTags = new List<Tags>();
                        foreach (string tag in tags)
                        {
                            var tagName = tag.Trim();
                            // 检查标签是否已经存在
                            var tagRecord = db.Tags.FirstOrDefault(t => t.TagName == tagName);
                            if (tagRecord == null)
                            {
                                tagRecord = new Tags
                                {
                                    TagName = tagName,
                                };
                                newTags.Add(tagRecord); // 添加到待插入的列表中
                            }
                            else
                            {
                                book.Tags.Add(tagRecord); // 已存在的标签直接关联
                            }
                        }
                        db.Tags.AddRange(newTags);
                        #endregion 标签      
                        
                        db.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('上传成功！')</script>");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Response.Write("<script>alert('上传失败')</script>");
                    }
                }
            }
        }
        #endregion 按钮事件处理

        #region 验证信息
        private bool IsValidInfo()
        {
            bool hasError = false;

            if (!Regex.IsMatch(txtBookName.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5\(\)][\sa-zA-Z0-9\u4e00-\u9fa5]+$") ||
                txtBookName.Text.Trim() == string.Empty
            )
            {
                BookNameTip.InnerText = "中英文开头，包含字母、数字、汉字、空格、括号";
                hasError = true;
            }

            // 验证ISBN - 假设ISBN为13位数字或带有连接符的格式
            if (!Regex.IsMatch(txtISBN.Text.Trim(), @"^(97(8|9))?\d{9}(\d|X)$") ||
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

            // 验证姓名 - 允许中英文，且非空
            if (!Regex.IsMatch(txtAuthor.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5\s]+$") ||
                txtAuthor.Text.Trim() == string.Empty
            )
            {
                AuthorTip.InnerText = "汉字、字母开头，英文以空格分割";
                hasError = true;
            }

            // 国名，允许汉字
            if (!Regex.IsMatch(txtAuthorNationality.Text.Trim(), @"^[\u4e00-\u9fa5]+$") ||
                txtAuthorNationality.Text.Trim() == string.Empty
            )
            {
                NationalityTip.InnerHtml = "国籍仅允许中文译名";
                hasError = true;
            }

            // 验证出版社名称 - 允许中文、英文，且非空
            if (!Regex.IsMatch(txtPublisher.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5\s]+$") ||
                txtPublisher.Text.Trim() == string.Empty
            )
            {
                PublisherTip.InnerText = "汉字、英文字母开头，单词以空格分割";
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
                if (!Regex.IsMatch(tag.Trim(), @"^[a-zA-Z0-9\u4e00-\u9fa5]+$"))
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
                    sum += (isbn[i] - '0') * 3;
                }
                else
                {
                    sum += isbn[i] - '0';
                }
            }
            return (10 - (sum % 10)) == (isbn[12] - '0');
        }

        private string SaveFile(FileUpload fileUpload, string folderPath)
        {
            if (fileUpload.HasFile)
            {
                string fileName = Path.GetFileName(fileUpload.FileName);
                string filePath = Path.Combine(Server.MapPath(folderPath), fileName);
                fileUpload.SaveAs(filePath);
                return filePath;
            }
            return null;
        }
    }
}