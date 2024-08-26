<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Reader/UserProfile.master" AutoEventWireup="true" CodeBehind="UserInformation.aspx.cs" Inherits="LibraryLink.Views.Reader.UserInformation" %>


<asp:Content ID="pageStyle" ContentPlaceHolderID="Styles" runat="server">
    <style>

        .lableFontStyle {
            font-family: 'Segoe UI Symbol';
            font-size: 20px;
            font-weight:600;  
            white-space: nowrap;
        }
        .info {
            font-weight:600;  
        }

    </style>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="UserProfileContent" runat="server">

    <div class="container mt-4">
        <div class="row">
            <div class="col-md-12">

                <div class="card">
                    <div class="card-header">
                        账户信息
                    </div>
                    <div class="card-body">
                        <!-- 第一行：UserID 和 注册日期 -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group d-flex align-items-center">
                                    <span class="lableFontStyle me-4">UserID : </span>
                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" ReadOnly="true" Text="user123"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group d-flex align-items-center">
                                    <span class="lableFontStyle me-2">注册日期 : </span>
                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" ReadOnly="true" Text="2000-10-20"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <!-- 第二行：用户名 和 电子邮件 -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="form-group d-flex align-items-center">
                                    <span class="lableFontStyle me-4">用户名 : </span>
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ReadOnly="true" Text="user123"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group d-flex align-items-center">
                                    <span class="lableFontStyle me-2">电子邮件 : </span>
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" ReadOnly="true" Text="LuminaeCaelestis@example.com"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <!-- 左侧：账户余额 和 充值按钮 -->
                            <div class="col-md-6">
                                <div class="d-flex justify-content-start align-items-center">
                                    <span class="lableFontStyle me-1">账户余额 : </span>
                                    <asp:TextBox ID="BalanceInfo" runat="server" CssClass="form-control" ReadOnly="true" Text="100.00"></asp:TextBox>
                                    <asp:Button ID="Button1" runat="server" Text="充值" CssClass="btn btn-primary ms-auto" />
                                </div>
                            </div>
                            <!-- 右侧：编辑 和 改密按钮 -->
                            <div class="col-md-6 d-flex flex-column align-items-end">
                                <div class="d-flex justify-content-end w-100">
                                    <asp:Button ID="Button2" runat="server" Text="编辑" CssClass="btn btn-primary"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card mt-5">
                    <div class="card-header">密码修改</div>
                    <div class="card-body">
                        <div class="form-group d-flex align-items-center">
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="输入新密码"></asp:TextBox>
                            <asp:TextBox ID="txtNewPasswordConfirm" runat="server" CssClass="form-control" TextMode="Password" placeholder="确认新密码"></asp:TextBox>
                            <asp:Button ID="btnSubmitPassword" runat="server" Text="确认" CssClass="btn btn-primary ms-2" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
