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

namespace LibraryLink.Views.Admin
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindUserGridView();
            }
        }

        private readonly string connStr = DatabaseConfig.ConnectionString;

        private void BindUserGridView()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT UserID, Username, Email, Balance, PrivilegeID AS UserGroup, DateJoined, Freezed FROM Users";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                UserGridView.DataSource = dt;
                UserGridView.DataBind();
            }
        }

        protected void UserGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = UserGridView.SelectedRow;
            UserId.Text = row.Cells[0].Text;
            Username.Text = row.Cells[1].Text;
            Email.Text = row.Cells[2].Text;
            Balance.Text = row.Cells[3].Text;
            UserGroup.SelectedValue = row.Cells[4].Text;
            DateJoined.Text = row.Cells[5].Text;
        }

        protected void UpdateUser_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(Username.Text, @"^[a-zA-Z]\w{5,29}$"))
            {
                UsernameTip.InnerHtml = " 用户名只能包含字母、数字、下划线";
                return;
            }
            else
            {
                UsernameTip.InnerHtml = "";
            }

            if (!Regex.IsMatch(Email.Text, @"^[\w-_]+\@[\w-_]+\.{1}[\w-_]{2,8}$") || Email.Text.Length > 50)
            {
                EmailTip.InnerHtml = " example@domin.com";
                return;
            }
            else
            {
                EmailTip.InnerHtml = "";
            }
            // 非负decimal(10,2)的正则验证
            if (!Regex.IsMatch(Balance.Text, @"^\d{1,8}(\.\d{1,2})?$"))
            {
                BalanceTip.InnerHtml = " 请输入正确的金额";
                return;
            }
            else
            {
                BalanceTip.InnerHtml = "";
            }

            long userId;
            if (string.IsNullOrEmpty(UserId.Text) || !long.TryParse(UserId.Text, out userId))
            {
                Response.Write("<script>alert('无效的用户ID！');</script>");
                return;
            }

            using (var context = new Entities())
            {
                var user = context.Users.FirstOrDefault(u => u.UserID == userId);
                if (user == null)
                {
                    Response.Write("<script>alert('用户不存在！');</script>");
                    return;
                }

                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        user.Username = Username.Text;
                        user.Email = Email.Text;
                        user.Balance = Convert.ToDecimal(Balance.Text);
                        user.PrivilegeID = Convert.ToInt32(UserGroup.SelectedValue);
                        context.SaveChanges();
                        trans.Commit();
                        Response.Write("<script>alert('用户信息更新成功！');</script>");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Response.Write("<script>alert('用户信息更新失败！');</script>");
                    }
                }
            }
            ApplyFilters();
        }

        // 需要重构
        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            if (UserId.Text == string.Empty)
            {
                Response.Write("<script>alert('未选择用户');</script>");
                return;
            }
            using (var context = new Entities())
            {
                var user = context.Users.Find(long.Parse(UserId.Text));
                if (user == null)
                {
                    Response.Write("<script>alert('用户不存在！');</script>");
                    return;
                }
                user.Freezed = !user.Freezed;
                context.SaveChanges();
                Response.Write("<script>alert('账户状态更新成功！');</script>");
            }
            ApplyFilters();
        }

        private void ClearForm()
        {
            UserId.Text = string.Empty;
            Username.Text = string.Empty;
            Email.Text = string.Empty;
            Balance.Text = string.Empty;
            UserGroup.SelectedIndex = -1;
            DateJoined.Text = string.Empty;
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            // 存储筛选条件
            ViewState["FilterUserId"] = FilterUserId.Text;
            ViewState["FilterUsername"] = FilterUsername.Text;
            ViewState["FilterEmail"] = FilterEmail.Text;
            ViewState["FilterFreezed"] = FilterFreezed.SelectedValue;
            ViewState["FilterUserGroup"] = FilterUserGroup.SelectedValue;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            using (var context = new Entities())
            {
                var query = context.Users.AsQueryable();

                if (ViewState["FilterFreezed"] != null && !string.IsNullOrEmpty(ViewState["FilterFreezed"].ToString()))
                {
                    bool parsedStatus = bool.Parse(ViewState["FilterFreezed"].ToString());
                    query = query.Where(u => u.Freezed == parsedStatus);
                }

                if (ViewState["FilterUserGroup"] != null && !string.IsNullOrEmpty(ViewState["FilterUserGroup"].ToString()))
                {
                    int userGroupId = int.Parse(ViewState["FilterUserGroup"].ToString());
                    query = query.Where(u => u.PrivilegeID == userGroupId);
                }

                if (ViewState["FilterUserId"] != null && !string.IsNullOrEmpty(ViewState["FilterUserId"].ToString()) && Regex.IsMatch(ViewState["FilterUserId"].ToString(), @"^\d+$"))
                {
                    int userId = int.Parse(ViewState["FilterUserId"].ToString());
                    query = query.Where(u => u.UserID == userId);
                }

                if (ViewState["FilterUsername"] != null && !string.IsNullOrEmpty(ViewState["FilterUsername"].ToString()))
                {
                    string username = ViewState["FilterUsername"].ToString().Trim();
                    query = query.Where(u => u.Username.Contains(username));
                }

                if (ViewState["FilterEmail"] != null && !string.IsNullOrEmpty(ViewState["FilterEmail"].ToString()))
                {
                    string email = ViewState["FilterEmail"].ToString().Trim();
                    query = query.Where(u => u.Email.Contains(email));
                }

                var users = query.Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.Email,
                    u.Balance,
                    UserGroup = u.PrivilegeID,
                    u.DateJoined,
                    u.Freezed
                }).ToList();

                UserGridView.DataSource = users;
                UserGridView.DataBind();
            }
        }

        // 翻页
        protected void UserGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int newPageIndex = e.NewPageIndex;
            if (newPageIndex < 0)
            {
                newPageIndex = 0;
            }
            else if (newPageIndex >= UserGridView.PageCount)
            {
                newPageIndex = UserGridView.PageCount - 1;
            }
            UserGridView.PageIndex = newPageIndex;
            ApplyFilters();
        }

        protected void btnJumpToPage_Click(object sender, EventArgs e)
        {
            // 通过 FindControl 获取 PagerTemplate 中的控件， 不然找不到名称
            TextBox txtJumpToPage = (TextBox)UserGridView.BottomPagerRow.FindControl("txtJumpToPage");

            if (txtJumpToPage != null)
            {
                int pageNumber;
                if (int.TryParse(txtJumpToPage.Text.Trim(), out pageNumber))
                {
                    // 后端从0数数的
                    pageNumber = pageNumber - 1;

                    if (pageNumber < 0)
                    {
                        pageNumber = 0;
                    }
                    else if (pageNumber >= UserGridView.PageCount)
                    {
                        pageNumber = UserGridView.PageCount - 1;
                    }
                    UserGridView.PageIndex = pageNumber;
                    ApplyFilters();

                }
            }
        }

    }

}