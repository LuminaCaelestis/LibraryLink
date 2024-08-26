<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="BookSearch.aspx.cs" Inherits="LibraryLink.Views.Admin.BookSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SidebarContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-5">
    <div class="row mb-4">
        <div class ="col-md-12">
            <div class="card">
                <div class ="card-header">
                    书籍检索
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label for="txtBookName" class="form-label">书名 <span id="BookNameTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="txtBookName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label for="txtMaxPrice" class="form-label">最高价格 <span id="MaxPriceTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="txtMaxPrice" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label for="txtMinPrice" class="form-label">最低价格 <span id="MinPriceTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="txtMinPrice" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label for="txtFilterAvailiable" class="form-label">有效性</label>
                            <asp:DropDownList ID="txtFilterAvailiable" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-6">
                           <label for="txtAuthorName" class="form-label">作者 <span id="Span1" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                           <asp:TextBox ID="txtAuthorName" runat="server" CssClass="form-control"></asp:TextBox>
                       </div>

                        <div class="col-md-3">
                            <label for="txtISBN" class="form-label">ISBN <span id="ISBNTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtPublisher" class="form-label">出版社 <span id="PublisherTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                            <asp:TextBox ID="txtPublisher" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                 </div>

                <!-- 按钮 -->
                <asp:Button ID="btnSubmit" runat="server" Text="筛选" CssClass="btn btn-primary btn-block" OnClick="btnSearch_Click" />


            </div>
        </div>
    </div>


        <!-- 列表 -->
    <div class="row">
        <div class="col-12">
            <asp:GridView ID="BookSearchView" runat="server" AllowPaging="True" PageSize="6" 
                CssClass="table table-bordered table-hover" DataKeyNames="BookID" OnRowCommand="BookSearchView_RowCommand"
                AutoGenerateColumns="False" OnPageIndexChanging="BooksSearchView_PageIndexChanging"
                OnRowDeleting="BookSearchView_RowDeleting">

                <Columns>
                    <asp:BoundField DataField="BookName" HeaderText="书名"/>
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                    <asp:BoundField DataField="AuthorName" HeaderText="作者" />
                    <asp:BoundField DataField="PublisherName" HeaderText="出版社" />
                    <asp:BoundField DataField="Price" HeaderText="价格" />
                    <asp:BoundField DataField="Available" HeaderText="有效性" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="text-center">
                                <asp:Button ID="EditButton" Text="编辑" runat="server" PostBackUrl='<%# "/Admin-BookEdit/?BookID=" + Eval("BookID") %>' CssClass="btn btn-primary btn-sm" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="text-center">
                                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete"
                                    CommandArgument='<%# Eval("BookID") %>' Text="切换下架" 
                                    OnClientClick="return confirm('警告！此操作将切换下架状态，确定执行吗？')" 
                                    CssClass="btn btn-danger btn-sm"/>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <PagerTemplate>
                    <div class="d-flex justify-content-center align-items-center">
                        <asp:LinkButton runat="server" CommandName="Page" CommandArgument="First" CssClass="btn btn-outline-primary btn-sm m-1">First</asp:LinkButton>
                        <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Prev" CssClass="btn btn-outline-primary btn-sm m-1">Previous</asp:LinkButton>
            
                        <span class="m-1">第 <%= BookSearchView.PageIndex + 1 %> 页，共 <%= BookSearchView.PageCount %> 页</span>
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
