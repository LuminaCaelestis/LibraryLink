<%@ Page Title="标签管理" Language="C#" MasterPageFile="~/Views/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="TagsManagement.aspx.cs" Inherits="LibraryLink.Views.Admin.TagsManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="SidebarContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <div class="container">
        <div class="row mb-4">
            <!-- 操作区 - 角色卡部分 -->
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">
                        标签信息
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="txtTagID" class="form-label">标签ID</label>
                                    <asp:TextBox ID="txtTagID" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label for="txtTagName" class="form-label">标签名<span id="a" class="text-danger" style="font-size: smaller;" runat="server"></span> </label>
                                    <asp:TextBox ID="txtTagName" MaxLength="30" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        
                            <div class="d-grid gap-2" style="margin-top: 40px">
                                <asp:Button ID="CreateTag" runat="server" CssClass="btn btn-success" Text="新建标签" OnClick="CreateTag_Click" />
                                <asp:Button ID="UpdateTag" runat="server" CssClass="btn btn-primary" Text="确认修改" OnClick="UpdateTag_Click" />
                                <asp:Button ID="DeleteTag" runat="server" CssClass="btn btn-danger" Text="删除标签" OnClick="DeleteTag_Click" OnClientClick="return confirm('确定要删除这个标签吗？');"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 筛选区 -->
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">
                        检索标签
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="FilterTagId" class="form-label">标签ID</label>
                            <asp:TextBox ID="FilterTagId" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="FilterTagName" class="form-label">标签名</label>
                            <asp:TextBox ID="FilterTagName" MaxLength="20" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="d-grid gap-2" style="margin-top: 147px">
                            <asp:Button ID="FilterButton" runat="server" CssClass="btn btn-secondary" Text="检索" OnClick="FilterButton_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 用户列表 -->
        <div class="row">
            <div class="col-12">
                <asp:GridView ID="TagsGridView" runat="server" AllowPaging="True" PageSize="6" 
                    CssClass="table table-bordered table-hover"
                    AutoGenerateColumns="False" OnSelectedIndexChanged="TagsGridView_SelectedIndexChanged"
                    OnPageIndexChanging="TagsGridView_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="TagID" HeaderText="标签ID"/>
                        <asp:BoundField DataField="TagName" HeaderText="标签名" />
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
            
                            <span class="m-1">第 <%= TagsGridView.PageIndex + 1 %> 页，共 <%= TagsGridView.PageCount %> 页</span>
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

</asp:Content>
