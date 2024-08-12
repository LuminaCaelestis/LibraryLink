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
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindUserGridView();
            }
        }

        string connStr = DatabaseConfig.ConnectionString;

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
            UserGroup.SelectedValue = row.Cells[4].Text == "Admin" ? "1" : "0";
            DateJoined.Text = row.Cells[5].Text;
        }

        protected void UpdateUser_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(Username.Text, @"^[a-zA-Z]\w{5,29}$"))
            {
                UsernameTip.InnerHtml = "用户名只能包含字母、数字、下划线";
                return;
            }
            if (!Regex.IsMatch(Email.Text, @"^[\w-_]+\@[\w-_]+\.{1}[\w-_]{2,8}$") || Email.Text.Length > 50)
            {
                EmailTip.InnerHtml = "example@domin.com";
                return;
            }

            // 非负decimal(10,2)的正则验证
            if (!Regex.IsMatch(Balance.Text, @"^\d{1,8}(\.\d{1,2})?$"))
            {
                BalanceTip.InnerHtml = "请输入正确的金额";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                if(UserId.Text == string.Empty || !DatabaseInterface.Is_Record_Exists("Users", "UserID", UserId.Text, connStr))
                {
                    Response.Write("<script>alert('用户不存在！');</script>");
                    return;
                }

                string query = "UPDATE Users SET Username=@Username, Email=@Email, Balance=@Balance, PrivilegeID=@UserGroup WHERE UserID=@UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", Username.Text);
                cmd.Parameters.AddWithValue("@Email", Email.Text);
                cmd.Parameters.AddWithValue("@Balance", Convert.ToDecimal(Balance.Text));
                cmd.Parameters.AddWithValue("@UserGroup", Convert.ToInt32(UserGroup.SelectedValue));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(UserId.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                // 更新完成后重新绑定数据
                BindUserGridView();
            }
        }

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

        // 过滤功能，动态构建SQL
        protected void FilterButton_Click(object sender, EventArgs e)
        {
            string filterQuery = "SELECT UserID, Username, Email, Balance, PrivilegeID AS UserGroup, DateJoined FROM Users WHERE 1=1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(FilterUserId.Text))
            {
                filterQuery += " AND UserID = @UserID";
                parameters.Add(new SqlParameter("@UserID", FilterUserId.Text));
            }

            if (!string.IsNullOrEmpty(FilterUsername.Text))
            {
                filterQuery += " AND Username LIKE @Username";
                parameters.Add(new SqlParameter("@Username", "%" + FilterUsername.Text + "%"));
            }

            if (!string.IsNullOrEmpty(FilterEmail.Text))
            {
                filterQuery += " AND Email LIKE @Email";
                parameters.Add(new SqlParameter("@Email", "%" + FilterEmail.Text + "%"));
            }

            if (!string.IsNullOrEmpty(FilterUserGroup.SelectedValue))
            {
                filterQuery += " AND PrivilegeID = @UserGroup";
                parameters.Add(new SqlParameter("@UserGroup", FilterUserGroup.SelectedValue));
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(filterQuery, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                UserGridView.DataSource = dt;
                UserGridView.DataBind();
            }
        }

        // 翻页
        protected void UserGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            UserGridView.PageIndex = e.NewPageIndex;
            BindUserGridView();
        }


    }

}