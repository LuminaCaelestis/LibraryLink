<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="Users_CP.aspx.cs" Inherits="LibraryLink.Views.Admin.Users_CP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid" style="background:#f0f4f5; padding-top: 20px;">

        <div class="row justify-content-center">
            <h3 class="text-center" style="font-family:'Segoe UI'; color:#535dad; margin-bottom: 30px;">用户管理</h3>
        </div>

        <div class="row justify-content-center" style="height:calc(100vh - 152px); padding: 0 15px;">
            <!-- 查询显示区 -->
            <div class="col-md-8" style="background:#ffffff; padding: 15px; margin-right: 20px;">
                <p>结果</p>
            </div>

            <!-- 查询输入面板 -->
            <div class="col-md-3" style="background:#ffffff; padding: 15px;">
                <p>查询</p>
            </div>
        </div>

    </div>

</asp:Content>
