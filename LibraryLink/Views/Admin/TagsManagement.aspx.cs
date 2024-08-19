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
using LibraryLink.Models.DatabaseModel;
using System.Data.Entity;

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
            ApplyFilters();
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
                    ApplyFilters();
                }
            }
        }

        protected void TagsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = TagsGridView.SelectedRow;
            txtTagID.Text = row.Cells[0].Text;
            txtTagName.Text = row.Cells[1].Text;
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


        protected void CreateTag_Click(object sender, EventArgs e)
        {
            string tagName = txtTagName.Text.Trim();
            if (!IsTagNameValid(tagName))
            {
                return;
            }

            using (var dbContext = new Entities())
            {
                if (dbContext.Tags.Any(t => t.TagName == tagName))
                {
                    Response.Write("<script>alert('该标签已存在！');</script>");
                    return;
                }

                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var newTag = new Tags { TagName = tagName };
                        dbContext.Tags.Add(newTag);
                        dbContext.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('标签创建成功！');</script>");
                        BindTagsGridView();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
                ApplyFilters();
            }
        }

        protected void UpdateTag_Click(object sender, EventArgs e)
        {
            string tagName = txtTagName.Text.Trim();
            int tagId = int.Parse(txtTagID.Text.Trim());

            if (!IsTagNameValid(tagName))
            {
                return;
            }

            using (var dbContext = new Entities())
            {
                var existingTag = dbContext.Tags.FirstOrDefault(t => t.TagID == tagId);
                if (existingTag == null)
                {
                    Response.Write("<script>alert('无法修改不存在的标签');</script>");
                    return;
                }

                using (var trans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        existingTag.TagName = tagName;
                        dbContext.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('标签修改成功！');</script>");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Response.Write("<script>alert('标签修改失败！');</script>");
                    }
                }
                ClearForm();
                ApplyFilters();
            }
        }

        protected void DeleteTag_Click(object sender, EventArgs e)
        {
            int tagId = int.Parse(txtTagID.Text.Trim());

            using (var context = new Entities())
            {
                var tagToDelete = context.Tags
                                         .Include(t => t.Books)  // 加载关联的书籍
                                         .FirstOrDefault(t => t.TagID == tagId);

                if (tagToDelete == null)
                {
                    Response.Write("<script>alert('未找到标签');</script>");
                    return;
                }

                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        tagToDelete.Books.Clear();
                        context.Tags.Remove(tagToDelete);
                        context.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('标签删除成功！');</script>");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Response.Write("<script>alert('标签删除失败！');</script>");
                        return;
                    }
                }
            }
            ClearForm();
            ApplyFilters();
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            // 存储筛选条件
            ViewState["FilterTagId"] = FilterTagId.Text.Trim();
            ViewState["FilterTagName"] = FilterTagName.Text.Trim();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            using (var context = new Entities())
            {
                var query = context.Tags.AsQueryable();

                if (ViewState["FilterTagId"] != null && !string.IsNullOrEmpty(ViewState["FilterTagId"].ToString()))
                {
                    int filterTagId = int.Parse(ViewState["FilterTagId"].ToString());
                    query = query.Where(t => t.TagID == filterTagId);
                }

                if (ViewState["FilterTagName"] != null && !string.IsNullOrEmpty(ViewState["FilterTagName"].ToString()))
                {
                    string filterTagName = ViewState["FilterTagName"].ToString();
                    query = query.Where(t => t.TagName.Contains(filterTagName));
                }

                TagsGridView.DataSource = query.ToList();
                TagsGridView.DataBind();
            }
        }



    }
}