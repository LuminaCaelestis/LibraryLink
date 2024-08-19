using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

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

    public class Algo
    {

        // 作者信息提取，去重等预处理
        public static List<(string name, string nation)> AuthorsInfoPreprocess(string authorInfoList)
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


        public static bool FileCheck(FileUpload fileUploader, ValidFileInfo ValidInfo, string fullPath, out string errorMsg)
        {
            errorMsg = string.Empty;

            FileInfoStruct fileInfo = new FileInfoStruct();

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
            if (!fileInfo.ValidMimeTypes.ContainsKey(extention))
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
    }

}

