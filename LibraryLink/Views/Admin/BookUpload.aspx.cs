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

        }

        // ORM！ ！！！！LINQ！！！！ 咋那么方便呢
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string bookName = txtBookName.Text.Trim();
            string isbn = txtISBN.Text.Trim();

            string authorName = txtAuthor.Text.Trim();
            string authorNationality = txtAuthorNationality.Text.Trim();

            string publisherName = txtPublisher.Text.Trim();
            DateTime publicationDate = DateTime.Parse(calPublicationDate.Text);
            
            decimal price = decimal.Parse(txtPrice.Text.Trim());

            string description = txtDescription.Text.Trim();
            string[] tags = txtTags.Text.Trim().Split(',');

            // 上传文件路径管理
            string coverImagePath = SaveFile(fuCoverImage, "~/Assets/Resource/CoverImages/");
            string bookFilePath = SaveFile(fuBookFile, "~/Assets/Resource/BookFiles/");

            // LINQ
            using (var db = new LibraryLinkDBEntities())
            {
                // 事务包裹整个操作，方便回滚
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 先插入书籍信息
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

                        // 作者
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


                        // 出版社
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

                        // 出版信息
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

                        // 标签
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
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Response.Write("<script>alert('上传失败！" + ex.Message + "')</script>");
                    }
                }
                Response.Write("<script>alert('上传成功！')</script>");
            }

            //Response.Redirect("BookManagement.aspx");
        }

        // 验证功能还没完成
        private byte IsValidInfo()
        {
            byte ValidCondition = 255;

            if (!Regex.IsMatch(txtBookName.Text.Trim(), @"^[a-zA-Z0-9\u4e00-\u9fa5]+$"))
            {
                ValidCondition -= 128;
                // 可以在这里添加错误信息或提示用户输入无效
            }

            // 验证ISBN - 假设ISBN为13位数字或带有连接符的格式
            if (!Regex.IsMatch(txtISBN.Text.Trim(), @"^(97(8|9))?\d{9}(\d|X)$"))
            {
                ValidCondition -= 64;
            }

            // 验证作者姓名 - 允许中文、英文，且非空
            if (!Regex.IsMatch(txtAuthor.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5\s]+$"))
            {
                ValidCondition -= 32;
            }

            // 验证作者国籍 - 允许中文、英文，且非空
            if (!Regex.IsMatch(txtAuthorNationality.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5\s]+$"))
            {
                ValidCondition -= 16;
            }

            // 验证出版社名称 - 允许中文、英文，且非空
            if (!Regex.IsMatch(txtPublisher.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5\s]+$"))
            {
                ValidCondition -= 8;
            }

            // 验证出版日期 - 检查是否是有效的日期
            if (!DateTime.TryParse(calPublicationDate.Text, out _))
            {
                ValidCondition -= 4;
            }

            // 验证价格 - 检查是否是正数
            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price <= 0)
            {
                ValidCondition -= 2;
            }

            // 验证书籍描述 - 描述长度是否在合理范围内（假设最多2000字符）
            if (txtDescription.Text.Trim().Length > 2000)
            {
                ValidCondition -= 1;
            }

            // 验证标签 - 检查每个标签是否合法，允许的字符可以是中文、英文、数字、逗号
            string[] tags = txtTags.Text.Trim().Split(',');
            foreach (string tag in tags)
            {
                if (!Regex.IsMatch(tag.Trim(), @"^[a-zA-Z0-9\u4e00-\u9fa5]+$"))
                {
                }
            }
            return ValidCondition;
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