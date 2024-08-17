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
                string query = "SELECT UserID, Username, Email, Balance, PrivilegeID AS UserGroup, DateJoined FROM Users";
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

            using (var context = new LibraryLinkDBContext())
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
            BindUserGridView();
        }

        // 需要重构
        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            if (UserId.Text == string.Empty)
            {
                Response.Write("<script>alert('未选择用户');</script>");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Users WHERE UserID=@UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(UserId.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                BindUserGridView();
                ClearForm();
            }
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
            using (var context = new LibraryLinkDBContext())
            {
                var query = context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(FilterUserId.Text) && Regex.IsMatch(FilterUserId.Text, @"^\d+$"))
                {
                    int userId = int.Parse(FilterUserId.Text);
                    query = query.Where(u => u.UserID == userId);
                }

                if (!string.IsNullOrEmpty(FilterUsername.Text))
                {
                    string username = FilterUsername.Text.Trim();
                    query = query.Where(u => u.Username.Contains(username));
                }

                if (!string.IsNullOrEmpty(FilterEmail.Text))
                {
                    string email = FilterEmail.Text.Trim();
                    query = query.Where(u => u.Email.Contains(email));
                }

                if (!string.IsNullOrEmpty(FilterUserGroup.SelectedValue))
                {
                    int userGroupId = int.Parse(FilterUserGroup.SelectedValue);
                    query = query.Where(u => u.PrivilegeID == userGroupId);
                }

                // 执行查询并绑定到GridView
                var users = query.Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.Email,
                    u.Balance,
                    UserGroup = u.PrivilegeID,  // 按照要求的名字映射
                    u.DateJoined
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
            BindUserGridView();
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
                    BindUserGridView();
                }
            }
        }

    }

}