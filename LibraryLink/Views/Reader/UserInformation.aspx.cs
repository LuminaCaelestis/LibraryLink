using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryLink.Views.Reader
{
    public partial class UserInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 从数据库加载用户信息
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            //// 示例数据加载
            //lblUserID.Text = "1";
            //lblDateJoined.Text = DateTime.Now.AddYears(-1).ToShortDateString();
            //txtUsername.Text = "user123";
            //txtEmail.Text = "user123@example.com";
            //lblBalance.Text = "100.00";
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //if (txtUsername.ReadOnly)
            //{
            //    // 启用编辑模式
            //    txtUsername.ReadOnly = false;
            //    txtEmail.ReadOnly = false;
            //    btnEdit.Text = "确认";
            //}
            //else
            //{
            //    // 保存修改并退出编辑模式
            //    txtUsername.ReadOnly = true;
            //    txtEmail.ReadOnly = true;
            //    btnEdit.Text = "编辑";

            //    // 保存数据到数据库
            //    SaveUserInfo();
            //}
        }

        private void SaveUserInfo()
        {
            // 将修改后的用户名和Email保存到数据库
            //string username = txtUsername.Text;
            //string email = txtEmail.Text;

            // 保存逻辑
        }

        protected void btnRecharge_Click(object sender, EventArgs e)
        {
            // 弹出框或页面输入充值金额
            // 更新账户余额的逻辑
        }

    }
}