using System;
using System.Collections.Generic;
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
            if (!IsValidInfo())
            {
                return;
            }



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

    }
}