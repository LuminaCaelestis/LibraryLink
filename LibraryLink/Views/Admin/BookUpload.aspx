<%@ Page Title="书籍上传" Language="C#" AutoEventWireup="true" CodeBehind="BookUpload.aspx.cs" Inherits="LibraryLink.Views.Admin.BookUpload" MasterPageFile="~/Views/Admin/AdminMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row mb-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        书籍上传
                    </div>
                    <div class="card-body">
   
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="txtBookName" class="form-label">书籍名称</label>
                                <asp:TextBox ID="txtBookName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtISBN" class="form-label">ISBN</label>
                                <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtPrice" class="form-label">价格</label>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-3">
                                <label for="txtAuthor" class="form-label">作者</label>
                                <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtAuthorNationality" class="form-label">作者国籍</label>
                                <asp:TextBox ID="txtAuthorNationality" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="txtPublisher" class="form-label">出版社</label>
                                <asp:TextBox ID="txtPublisher" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="calPublicationDate" class="form-label">出版日期</label>
                                <asp:TextBox ID="calPublicationDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="fuCoverImage" class="form-label">封面图片</label>
                                <asp:FileUpload ID="fuCoverImage" runat="server" CssClass="form-control"></asp:FileUpload>
                            </div>
                            <div class="col-md-6">
                                <label for="fuBookFile" class="form-label">书籍文件 (PDF)</label>
                                <asp:FileUpload ID="fuBookFile" runat="server" CssClass="form-control"></asp:FileUpload>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-12">
                                <label for="txtTags" class="form-label">标签</label>
                                <<asp:TextBox ID="txtTags" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-12">
                                <label for="txtDescription" class="form-label">书籍描述</label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="9"></asp:TextBox>
                            </div>
                        </div>

                        <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="btn btn-primary btn-block" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

