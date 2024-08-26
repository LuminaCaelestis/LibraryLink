using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using LibraryLink.Models.DatabaseModel;

namespace LibraryLink.Views.Admin
{
    public struct ValidFileInfo
    {
        public string[] FileExtensions { get; set; }
        public int MaxSize { get; set; }
    }

    public class FileInfoStruct
    {
        public string CoverImageFolder { get; } = "~/Assets/Resource/CoverImages/";
        public string BookFolder { get; } = "~/Assets/Resource/BookFiles/";
        public string CoverImageFullPath { get; set; } = string.Empty;
        public string BookFullPath { get; set; } = string.Empty;
        public int MaxImageSize { get; } = 1024 * 1024 * 2;
        public int MaxBookSize { get; } = 1024 * 1024 * 400; // 400mb
        public string[] ValidImageExtensions { get; } = { ".jpg", ".jpeg", ".png" };
        public string[] ValidBookExtensions { get; } = { ".pdf" };

        public Dictionary<string, string[]> ValidMimeTypes { get; } = new Dictionary<string, string[]>
        {
            { ".jpg", new[] { "image/jpeg", "image/jpg" } },
            { ".jpeg", new[] { "image/jpeg", "image/jpg" } },
            { ".png", new[] { "image/png" } },
            { ".pdf", new[] { "application/pdf" } }
        };
    }

    public class DBService
    {
        public static void AuthorInfoInsert(Entities context, List<(string name, string nation)> procAuthors, Books book)
        {
            List<Authors> newAuthor = new List<Authors>();
            foreach (var (name, nation) in procAuthors)
            {
                var author = context.Authors.FirstOrDefault(a => a.AuthorName == name && a.Nationality == nation);
                if (author == null)
                {
                    author = new Authors
                    {
                        AuthorName = name,
                        Nationality = nation,
                    };
                    newAuthor.Add(author);
                }
                if (!book.Authors.Contains(author))
                {
                    book.Authors.Add(author);
                }
            }
        }

        public static Publisher PublisherInsert(Entities context, string publisherName)
        {
            var publisher = context.Publisher.FirstOrDefault(p => p.PublisherName == publisherName);
            if (publisher == null)
            {
                publisher = new Publisher
                {
                    PublisherName = publisherName,
                };
                context.Publisher.Add(publisher);
            }
            return publisher;
        }

        public static void TagsInsert(Entities context, List<string> tagProcessed, Books book)
        {
            List<Tags> newTags = new List<Tags>();
            foreach (string tag in tagProcessed)
            {
                var trimedTag = tag.Trim();
                var tagRecord = context.Tags.FirstOrDefault(t => t.TagName == trimedTag);
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
            context.Tags.AddRange(newTags);
            foreach (var tag in newTags)
            {
                book.Tags.Add(tag);
            }
        }
    }

    public class FileService
    {
        private readonly string backupDirectory;
        private readonly List<(string originalPath, string backupPath)> backupFilePaths;

        public FileService()
        {
            // 获取应用的根目录
            var appRoot = AppDomain.CurrentDomain.BaseDirectory;
            // 构建备份目录路径
            backupDirectory = Path.Combine(appRoot, "Assets", "backup");
            Directory.CreateDirectory(backupDirectory);
            backupFilePaths = new List<(string originalPath, string backupPath)>();
        }

        // 备份文件到临时目录
        public void BackupFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileName = Path.GetFileName(filePath);
                var backupFilePath = Path.Combine(backupDirectory, fileName);
                File.Copy(filePath, backupFilePath);
                backupFilePaths.Add((filePath, backupFilePath));
            }
        }

        // 恢复备份的文件到原始目录
        public void RestoreFiles()
        {
            foreach (var (originalPath, backupPath) in backupFilePaths)
            {
                if (File.Exists(backupPath))
                {
                    File.Copy(backupPath, originalPath, true);
                }
            }
        }

        // 删除备份的文件
        public void DeleteBackupFiles()
        {
            foreach (var (_, backupPath) in backupFilePaths)
            {
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
            }
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static bool IsFileUploaded(FileUpload fileUploader, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (!fileUploader.HasFile)
            {
                errorMsg = "未选择文件";
                return false;
            }
            return true;
        }

        public static bool FileCheck(FileUpload fileUploader, ValidFileInfo ValidInfo, string fullPath, out string errorMsg)
        {
            errorMsg = string.Empty;

            FileInfoStruct fileInfo = new FileInfoStruct();

            if (File.Exists(fullPath))
            {
                errorMsg = "文件重名";
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
            if (!fileInfo.ValidMimeTypes.ContainsKey(extention))
            {
                errorMsg = "文件格式不正确";
                return false;
            }
            if (fileUploader.PostedFile.ContentLength > ValidInfo.MaxSize)
            {
                errorMsg = "文件大小超出限制";
                return false;
            }
            return true;
        }
    }

    public class Algo
    {
        // 标签去重
        public static List<string> TagsPreprocess(string tagList)
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

        // 作者信息提取，去重等预处理
        public static List<(string name, string nation)> AuthorsInfoPreprocess(string authorInfoList)
        {
            List<(string name, string nation)> result = new List<(string name, string nation)> { };
            HashSet<string> authorCheckSet = new HashSet<string>();
            var authorInfoArr = authorInfoList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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

        //// 作者信息格式回滚
        //public static string AuthorsInfoFormat(List<(string name, string nation)> authorsInfoList)
        //{
        //    var formattedAuthors = authorsInfoList
        //        .Select(author => $"{author.name}[{author.nation}]")
        //        .ToList();

        //    return string.Join("; ", formattedAuthors) + ";";
        //}

        public static bool IsValidISBN(string isbn)
        {
            // 验证ISBN的末尾校验码，假设ISBN为13位数字
            // 根据ISBN13的规则
            // ISBN的末尾校验码是通过取前12位，偶数位乘3，然后求和，最后取余10，用10减去余数，结果应该等于最后一位数字
            int sum = 0;
            for (int i = 0; i != 12; ++i)
            {
                if (i % 2 == 0)
                {
                    sum += (isbn[i] - '0');
                }
                else
                {
                    sum += isbn[i] - '0' * 3;
                }
            }
            int res;
            if (sum % 10 == 0)
            {
                res = 0;
            }
            else
            {
                res = 10 - (sum % 10);
            }
            return res == (isbn[12] - '0');
        }


        public static bool ValidateBookInfo(
            TextBox txtBookName, HtmlGenericControl bookNameTip,
            TextBox txtISBN, HtmlGenericControl isbnTip,
            TextBox txtAuthor, HtmlGenericControl authorTip,
            TextBox txtPublisher, HtmlGenericControl publisherTip,
            TextBox calPublicationDate, HtmlGenericControl publicationDateTip,
            TextBox txtPrice, HtmlGenericControl priceTip,
            TextBox txtDescription, HtmlGenericControl descriptionTip,
            TextBox txtTags, HtmlGenericControl tagTip)
        {
            bool hasError = false;
            bookNameTip.InnerHtml = string.Empty;
            isbnTip.InnerHtml = string.Empty;
            authorTip.InnerHtml = string.Empty;
            publisherTip.InnerHtml = string.Empty;
            publicationDateTip.InnerHtml = string.Empty;
            priceTip.InnerHtml = string.Empty;
            descriptionTip.InnerHtml = string.Empty;
            tagTip.InnerHtml = string.Empty;


            if (!Regex.IsMatch(txtBookName.Text.Trim(), @"^[\sa-zA-Z0-9\u4e00-\u9fa5\(\)]+$") ||
                txtBookName.Text.Trim() == string.Empty)
            {
                bookNameTip.InnerHtml = "中英文开头，包含字母、数字、汉字、空格、括号";
                hasError = true;
            }

            if (!Regex.IsMatch(txtISBN.Text.Trim(), @"^\d{13}$") ||
                txtISBN.Text.Trim() == string.Empty)
            {
                isbnTip.InnerHtml = "ISBN必须为13位纯数字";
                hasError = true;
            }

            if (!Regex.IsMatch(txtAuthor.Text.Trim(),
                @"^(?:[\u4e00-\u9fa5A-Za-z\s]+\[[\u4e00-\u9fa5]+\]\s*,\s*)*[\u4e00-\u9fa5A-Za-z\s]+\[[\u4e00-\u9fa5\s]+\]\s*") ||
                txtAuthor.Text.Trim() == string.Empty)
            {
                authorTip.InnerHtml = "汉字、字母开头，英文以空格分割";
                hasError = true;
            }

            if (!Regex.IsMatch(txtPublisher.Text.Trim(), @"^[a-zA-Z\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5\s]+$") ||
                txtPublisher.Text.Trim() == string.Empty)
            {
                publisherTip.InnerHtml = "汉字、英文字母开头，空格分割单词";
                hasError = true;
            }

            if (string.IsNullOrEmpty(calPublicationDate.Text.Trim()))
            {
                publicationDateTip.InnerHtml = "请选择出版日期";
                hasError = true;
            }

            if (txtPrice.Text.Trim() == string.Empty ||
                !decimal.TryParse(txtPrice.Text.Trim(), out decimal price) ||
                price < 0m || price > 99999999.99m)
            {
                priceTip.InnerHtml = "介于0~99999999.99间的阿拉伯数字";
                hasError = true;
            }

            if (txtDescription.Text.Trim().Length > 2000)
            {
                descriptionTip.InnerHtml = "书籍描述不能超过2000字符";
                hasError = true;
            }

            string[] tags = txtTags.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tag in tags)
            {
                if (!Regex.IsMatch(tag.Trim(), @"^((?:[a-zA-Z\u4e00-\u9fa5]+)(?:\s*))+$"))
                {
                    tagTip.InnerHtml = "标签只能包含中文、英文";
                    hasError = true;
                    break;
                }
            }

            return !hasError;
        }

    }

}

