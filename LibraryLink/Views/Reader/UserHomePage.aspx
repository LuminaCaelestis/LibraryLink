<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Reader/ReaderMaster.Master" AutoEventWireup="true" CodeBehind="UserHomePage.aspx.cs" Inherits="LibraryLink.Views.Reader.UserHomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <style>
        .card-body {
            background-color: #f8f9fa;
        }
        .SearchList {

        }

        .book-card-img-container {
            width: 42%;
            height: auto;
            position: relative;
            background-color: #f8f9fa;
        }
        .book-card-info-container {
            width: 58%;
            background-color: #f8f9fa;
        }

        .book-card-img {
            width: 100%;
            height: 100%;
            object-fit: fill;
            background-color: #f8f9fa;
        }

        .card-body {
            background-color: #f8f9fa;
            margin-left: 0px; /* 增加间隙，防止图片和文字重叠 */

        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-5">

    <!-- 搜索框与按钮 -->
    <div class="row justify-content-center mb-4">
        <div class="col-md-9">
            <div class="input-group">
                <input type="text" class="form-control" placeholder="搜索书名、作者或ISBN">
                <button class="btn btn-secondary" type="button">搜索</button>
            </div>
        </div>
    </div>

    <!-- 筛选条件行 -->
    <div class="row justify-content-center mb-4">
        <div class="col-md-3">
            <div class="input-group mb-3">
                <span class="input-group-text">作者: </span>
                <asp:TextBox ID="txtAuthorFilter" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-3">
            <div class="input-group mb-3">
                <span class="input-group-text">出版社: </span>
                <asp:TextBox ID="txtPublisherFilter" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-3">
            <div class="input-group mb-3">
               <span class="input-group-text">标签: </span>
                <asp:TextBox ID="txtTagsFilter" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>

    <!-- 列表 -->
    <div class="container justify-content-center mt-5" style="max-width: 970px;">
        <div id="SearchResultsList" class="row justify-content-left SearchList">
            <asp:Literal ID="Literal1" runat="server">

                    <div class="col-4 px-1">
                        <div class="book-card card my-2 d-flex flex-row" style="height: 12rem;">
                            <!-- 图片部分 -->
                            <div class="book-card-img-container">
                                <a href="BookDetails.aspx/BookID=0">
                                    <img loading="lazy" src="../../Assets/Resource/CoverImages/\s34925368.jpg" class="book-card-img" >
                                </a>
                            </div>
                            <!-- 文字部分 -->
                            <div class="book-card-info-container card-body">
                                <h5 class="card-title">回忆爱玛侬</h5>
                                <p class="card-text">作者：<br />梶尾真治[日],<br /> 测试:Adrian Banner[美国]</p>
                            </div>
                        </div>
                    </div>

            </asp:Literal>
        </div>
    </div>
    <!-- 列表END -->

    <!-- 换页 -->

    <div class="container justify-content-center mt-5" style="max-width: 970px;">
        <!-- 分页控件 -->
        <div class="d-flex justify-content-center mt-4">
            <label id="pageCounting" class="hiding"></label>
            <asp:Button ID="btnPrev" runat="server" Text="上一页" OnClick="ChangePage" CssClass="btn btn-secondary mx-2" />
            <asp:Label ID="lblPageInfo" runat="server" CssClass="align-self-center mx-2">当前第 <%= CurrentPage + 1 %> 页</asp:Label>
            <asp:Button ID="btnNext" runat="server" Text="下一页" OnClick="ChangePage" CssClass="btn btn-secondary mx-2" />
        </div>
    </div>




</div>


</asp:Content>
