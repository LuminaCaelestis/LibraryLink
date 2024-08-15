using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryLink.Models;
using System.Text.RegularExpressions;

namespace LibraryLink.Views.Admin
{
    public partial class TagsManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindTagsGridView();
            }
        }

        private readonly string connStr = DatabaseConfig.ConnectionString;

        private void BindTagsGridView()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT TagID, TagName FROM Tags";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                TagsGridView.DataSource = dt;
                TagsGridView.DataBind();
            }
        }

        private bool IsTagNameValid(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                Response.Write("<script>alert('标签名不能为空！');</script>");
                return false;
            }
            if (!Regex.IsMatch(tagName, @"^[\u4e00-\u9fa5A-Za-z]+$"))
            {
                Response.Write("<script>alert('标签仅支持汉字与英文，不能包含空格、数字、特殊字符！');</script>");
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            txtTagName.Text = string.Empty;
            txtTagID.Text = string.Empty;
        }

        // 翻页
        protected void TagsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int newPageIndex = e.NewPageIndex;
            if (newPageIndex < 0)
            {
                newPageIndex = 0;
            }
            else if (newPageIndex >= TagsGridView.PageCount)
            {
                newPageIndex = TagsGridView.PageCount - 1;
            }
            TagsGridView.PageIndex = newPageIndex;
            BindTagsGridView();
        }

        protected void btnJumpToPage_Click(object sender, EventArgs e)
        {
            // 通过 FindControl 获取 PagerTemplate 中的控件， 不然找不到名称
            TextBox txtJumpToPage = (TextBox)TagsGridView.BottomPagerRow.FindControl("txtJumpToPage");

            if (txtJumpToPage != null)
            {
                int pageNumber;
                if (int.TryParse(txtJumpToPage.Text.Trim(), out pageNumber))
                {
                    pageNumber = pageNumber - 1;

                    if (pageNumber < 0)
                    {
                        pageNumber = 0;
                    }
                    else if (pageNumber >= TagsGridView.PageCount)
                    {
                        pageNumber = TagsGridView.PageCount - 1;
                    }
                    TagsGridView.PageIndex = pageNumber;
                    BindTagsGridView();
                }
            }
        }

        protected void TagsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = TagsGridView.SelectedRow;
            txtTagID.Text = row.Cells[0].Text;
            txtTagName.Text = row.Cells[1].Text;
        }

        protected void CreateTag_Click(object sender, EventArgs e)
        {
            string tagName = txtTagName.Text.Trim();
            if (!IsTagNameValid(tagName))
            {
                return;
            }
            if (DatabaseInterface.Is_Record_Exists("Tags", "TagName", tagName, connStr))
            {
                Response.Write("<script>alert('该标签已存在！');</script>");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Tags (TagName) VALUES (@TagName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TagName", tagName);
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                conn.Close();
                if (affectedRows > 0)
                {
                    Response.Write("<script>alert('标签创建成功！');</script>");
                    BindTagsGridView();
                }
                else
                {
                    Response.Write("<script>alert('标签创建失败！');</script>");
                }
            }
        }

        protected void UpdateTag_Click(object sender, EventArgs e)
        {
            if (!IsTagNameValid(txtTagName.Text.Trim()))
            {
                return;
            }
            if(!DatabaseInterface.Is_Record_Exists("Tags", "TagID", txtTagID.Text.Trim(), connStr))
            {
                Response.Write("<script>alert('无法修改不存在的标签');</script>");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Tags SET TagName = @TagName WHERE TagID = @TagID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TagName", txtTagName.Text.Trim());
                cmd.Parameters.AddWithValue("@TagID", txtTagID.Text.Trim());
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                conn.Close();
                if (affectedRows > 0)
                {
                    Response.Write("<script>alert('标签修改成功！');</script>");
                    ClearForm();
                    BindTagsGridView();
                }
                else
                {
                    Response.Write("<script>alert('标签修改失败！');</script>");
                }
            }
        }

        protected void DeleteTag_Click(object sender, EventArgs e)
        {
            if(!DatabaseInterface.Is_Record_Exists("Tags", "TagID", txtTagID.Text.Trim(), connStr))
            {
                Response.Write("<script>alert('未找到标签');</script>");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Tags WHERE TagID = @TagID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TagID", txtTagID.Text.Trim());
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                conn.Close();
                if (affectedRows > 0)
                {
                    Response.Write("<script>alert('标签删除成功！');</script>");
                    ClearForm();
                    BindTagsGridView();
                }
                else
                {
                    Response.Write("<script>alert('标签删除失败！');</script>");
                }
            }
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            string filterQuery = "SELECT TagID, TagName FROM Tags WHERE 1=1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(FilterTagName.Text))
            {
                filterQuery += " AND TagName LIKE @FilterTagName";
                parameters.Add(new SqlParameter("@FilterTagName", "%" + FilterTagName.Text + "%"));

            }
            if (!string.IsNullOrEmpty(FilterTagId.Text))
            {
                filterQuery += " AND TagID = @FilterTagId";
                parameters.Add(new SqlParameter("@FilterTagId", FilterTagId.Text));
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(filterQuery, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                TagsGridView.DataSource = dt;
                TagsGridView.DataBind();
            }
        }
    }
}