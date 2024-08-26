<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Reader/UserProfile.master" AutoEventWireup="true" CodeBehind="OrderRecord.aspx.cs" Inherits="LibraryLink.Views.Reader.OrderRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="UserProfileContent" runat="server">

            <div class="container mt-4">
            <h2>订单记录检索</h2>

            <!-- 搜索栏 -->
            <div class="row mb-3">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="输入订单号、ISBN或书名进行搜索"  CssClass="form-control"></asp:TextBox>
            </div>

            <!-- 筛选条件 -->
            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="txtAuthor">作者名</label>
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <label for="txtStartDate">开始日期</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <label for="txtEndDate">结束日期</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </div>

            <!-- 搜索按钮 -->
            <div class="text-end">
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
            </div>

            <!-- GridView 显示订单记录 -->
            <asp:GridView ID="gvOrders" runat="server" CssClass="table table-striped mt-4" AutoGenerateColumns="False" OnRowDataBound="gvOrders_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="PurchaseID" HeaderText="订单号" />
                    <asp:BoundField DataField="PurchaseDate" HeaderText="订单日期" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                    <asp:BoundField DataField="BookName" HeaderText="书名" />
                    <asp:BoundField DataField="PurchaseAmount" HeaderText="价格" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>

</asp:Content>
