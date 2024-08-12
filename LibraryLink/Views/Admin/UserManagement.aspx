<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="LibraryLink.Views.Admin.UserManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SidebarContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <!-- 操作区 -->
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">
                        用户信息
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="UserId" class="form-label">用户ID</label>
                            <asp:TextBox ID="UserId" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="Username" class="form-label">用户名<span id="UsernameTip" class="text-danger" style="font-size: smaller;" runat="server"></span> </label>
                            <asp:TextBox ID="Username" MaxLength="30" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="Email" class="form-label">Email<span id="EmailTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="Email"  MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="Balance" class="form-label">余额<span id="BlanceTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="Balance"  runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="UserGroup" class="form-label">用户组</label>
                            <asp:DropDownList ID="UserGroup" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Reader" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Admin" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label for="DateJoined" class="form-label">注册日期</label>
                            <asp:TextBox ID="DateJoined" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                        </div>
                        <div class="d-grid gap-2">
                            <asp:Button ID="UpdateUser" runat="server" CssClass="btn btn-primary" Text="确认修改" OnClick="UpdateUser_Click" />
                            <asp:Button ID="DeleteUser" runat="server" CssClass="btn btn-danger" Text="删除用户" OnClick="DeleteUser_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 用户列表 -->
        <div class="row">
            <div class="col-12">
                <asp:GridView ID="UserGridView" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="False" OnSelectedIndexChanged="UserGridView_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="用户ID" ReadOnly="True" />
                        <asp:BoundField DataField="Username" HeaderText="用户名" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Balance" HeaderText="余额" />
                        <asp:BoundField DataField="UserGroup" HeaderText="用户组" />
                        <asp:BoundField DataField="DateJoined" HeaderText="注册日期" />
                        <asp:CommandField ShowSelectButton="True" SelectText="选择" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        document.addEventListener(
            "DOMContentLoaded", function () {

                var username = document.getElementById('<%= Username.ClientID %>');
                var email = document.getElementById('<%= Email.ClientID %>');
                var balance = document.getElementById('<%= Balance.ClientID %>');
                var update = document.getElementById('<%= UpdateUser.ClientID %>')

                update.addEventListener('click', function (event) {
                    if (!(ValidEmail() && ValidUsername() && ValidBalance())) {
                        event.preventDefault();
                        return false;
                    }
                });

                username.addEventListener('change ', ValidUsername);
                email.addEventListener('change ', ValidEmail);
                balance.addEventListener('change ', ValidBalance);

                function ValidUsername() {
                    var regex = /^[a-zA-Z]\w{5,29}$/;
                    if (regex.test(username.value)) {
                        document.getElementById('UsernameTip').innerHTML = "";
                        return true;
                    }
                    else {
                        document.getElementById('UsernameTip').innerHTML = "至少6个字符，以字母开头，只含字母、数字、下划线";
                        return false;
                    }
                }

                function ValidEmail() {
                    var regex = /^[\w-_]+@[\w-_]+\.{1}[\w-_]{2,8}$/
                    if (regex.test(email.value)) {
                        document.getElementById('EmailTip').innerHTML = "";
                        return true;
                    }
                    else {
                        document.getElementById('EmailTip').innerHTML = "邮箱格式不正确";
                        return false;
                    }
                }

                function ValidBalance() {
                    var regex = /^\d{1,8}(\.\d{1,2})?$/
                    if (regex.test(blance.value)) {
                        document.getElementById('BlanceTip').innerHTML = ""；
                    }
                    else {
                        document.getElementById('BlanceTip').innerHTML = "金额错误";
                        return false;
                    }
                }
            }
            );
    </script>

</asp:Content>
