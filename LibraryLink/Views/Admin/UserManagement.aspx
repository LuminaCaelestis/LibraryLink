<%@ Page Title="用户管理" Language="C#" MasterPageFile="~/Views/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="LibraryLink.Views.Admin.UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row mb-4">
            <!-- 操作区 - 角色卡部分 -->
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">
                        用户信息
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
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
                                    <asp:TextBox ID="Email" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="Balance" class="form-label">余额<span id="BalanceTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                                    <asp:TextBox ID="Balance" runat="server" CssClass="form-control"></asp:TextBox>
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
                            </div>
                        </div>
                        <div class="d-grid gap-2" style="margin-top: 40px">
                            <asp:Button ID="UpdateUser" runat="server" CssClass="btn btn-primary" Text="确认修改" OnClick="UpdateUser_Click" />
                            <asp:Button ID="DeleteUser" runat="server" CssClass="btn btn-danger" Text="删除用户" OnClick="DeleteUser_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- 筛选区 -->
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">
                        筛选用户
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="FilterUserId" class="form-label">用户ID</label>
                            <asp:TextBox ID="FilterUserId" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="FilterUsername" class="form-label">用户名</label>
                            <asp:TextBox ID="FilterUsername" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="FilterEmail" class="form-label">Email</label>
                            <asp:TextBox ID="FilterEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="FilterUserGroup" class="form-label">用户组</label>
                            <asp:DropDownList ID="FilterUserGroup" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                <asp:ListItem Text="Reader" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Admin" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="d-grid gap-2">
                            <asp:Button ID="FilterButton" runat="server" CssClass="btn btn-secondary" Text="筛选" OnClick="FilterButton_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 用户列表 -->
        <div class="row">
            <div class="col-12">
                <asp:GridView ID="UserGridView" runat="server" AllowPaging="True" PageSize="6" 
                    CssClass="table table-bordered table-hover"
                    AutoGenerateColumns="False" OnSelectedIndexChanged="UserGridView_SelectedIndexChanged"
                    OnPageIndexChanging="UserGridView_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="用户ID" ReadOnly="True" />
                        <asp:BoundField DataField="Username" HeaderText="用户名" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Balance" HeaderText="余额" />
                        <asp:BoundField DataField="UserGroup" HeaderText="用户组" />
                        <asp:BoundField DataField="DateJoined" HeaderText="注册日期" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="text-center">
                                    <asp:Button ID="SelectButton" runat="server" Text="选择" CommandName="Select" CssClass="btn btn-primary btn-sm" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <PagerTemplate>
                        <div class="d-flex justify-content-center align-items-center">
                            <asp:LinkButton runat="server" CommandName="Page" CommandArgument="First" CssClass="btn btn-outline-primary btn-sm m-1">First</asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Prev" CssClass="btn btn-outline-primary btn-sm m-1">Previous</asp:LinkButton>
            

                            <span class="m-1">第 <%= UserGridView.PageIndex + 1 %> 页，共 <%= UserGridView.PageCount %> 页</span>
            

                            <span class="m-1">跳转到:</span>
                            <asp:TextBox ID="txtJumpToPage" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="Disabled" Style="width: 100px; display: inline-block;" />
                            <asp:Button ID="btnJumpToPage" runat="server" Text="Go" CssClass="btn btn-primary btn-sm" OnClick="btnJumpToPage_Click" />

                            <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Next" CssClass="btn btn-outline-primary btn-sm m-1">Next</asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Last" CssClass="btn btn-outline-primary btn-sm m-1">Last</asp:LinkButton>
                        </div>
                    </PagerTemplate>

                </asp:GridView>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            var username = document.getElementById('<%= Username.ClientID %>');
            var email = document.getElementById('<%= Email.ClientID %>');
            var balance = document.getElementById('<%= Balance.ClientID %>');
            var update = document.getElementById('<%= UpdateUser.ClientID %>')
            console.log("addEventListener DOMContentLoaded");

            var usernameTip = document.getElementById('<%= UsernameTip.ClientID %>');
            var emailTip = document.getElementById('<%= EmailTip.ClientID %>');
            var balanceTip = document.getElementById('<%= BalanceTip.ClientID %>');

            update.addEventListener('click', function (event) {
                if (!(ValidEmail() && ValidUsername() && ValidBalance())) {
                    event.preventDefault();
                    return false;
                }
            });

            username.addEventListener('change', ValidUsername);
            email.addEventListener('change', ValidEmail);
            balance.addEventListener('change', ValidBalance);

            function ValidUsername() {

                var regex = /^[a-zA-Z]\w{5,29}$/;

                if (regex.test(username.value)) {
                    usernameTip.innerHTML = "";
                    return true;
                }
                else {
                    usernameTip.innerHTML = "至少6个字符，以字母开头，只含字母、数字、下划线";
                    return false;
                }
            }

            function ValidEmail() {
                var regex = /^[\w-_]+@[\w-_]+\.{1}[\w-_]{2,8}$/
                if (regex.test(email.value)) {
                    emailTip.innerHTML = "";
                    return true;
                }
                else {
                    emailTip.innerHTML = "邮箱格式不正确";
                    return false;
                }
            }

            function ValidBalance() {
                var regex = /^\d{1,8}(\.\d{1,2})?$/
                if (regex.test(balance.value)) {
                    balanceTip.innerHTML = "";
                    return true;
                }
                else {
                    balanceTip.innerHTML = "金额错误";
                    return false;
                }
            }
        });
    </script>

</asp:Content>
