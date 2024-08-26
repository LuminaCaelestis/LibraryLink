<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Reader/ReaderMaster.Master" AutoEventWireup="true" CodeBehind="BookDetails.aspx.cs" Inherits="LibraryLink.Views.Reader.BookDetails" %>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .clearfix::after {
            content: "";
            display: table;
            clear: both;
        }

        .rating{
            font-size:60px;
            font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            align-self:center;
        }

        .mainInfo {
            height:260px;
           
        }

        .infoBlock {
            height: 100%;
        }

        .inline-block {
            display: inline-block;
            vertical-align: middle;
        }


    </style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container mt-5">
        <!-- Main content -->
        <div class="row mainInfo mb-5">
            <!-- Left side content -->
            <div class="col-md-6">
                <div class="card mb-3 infoBlock">
                    <div class="row g-0">
                        <div class="col-md-4">
                            <asp:Image ID="BookImage" runat="server" CssClass="img-fluid rounded-start" AlternateText="Book Image" />
                        </div>
                        <div class="col-md-8">
                            <div class="card-body">
                                <h5 id="pageBookName" class="card-title">数据结构与算法</h5>
                                <p id="pageAuthorsName" class="card-text">作者: Stellānia Lūminae Caelēstiāna</p>
                                <p id="pageISBN" class="card-text">ISBN: 9788746389234</p>
                                <p id="pagePublisher" class="card-text">机械工业出版社</p>
                                <p id="pagePrice" class="card-text">价格: 69.00 元</p>
                                <button id="BuyBook" type="button" class="btn btn-primary">购买</button>
                                <button id="FavouriteBook" type="button" class="btn btn-secondary">收藏</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Right side content -->
            <div class="col-md-3">
                <div class="card infoBlock">
                    <div class="card-body">
                        <h5 class="card-title">作品标签</h5>
                        <div class="tags-container" style=" overflow-y: auto;">
                            <div id="TagsBox" class="tag-list clearfix" style="display: block;" runat="server">
                                <span class="badge bg-secondary m-1" style="float: left;">计算机科学</span>
                                <span class="badge bg-secondary m-1" style="float: left;">编程</span>
                                <span class="badge bg-secondary m-1" style="float: left;">算法</span>
                                <span class="badge bg-secondary m-1" style="float: left;">平衡二叉树</span>
                                <span class="badge bg-secondary m-1" style="float: left;">排序</span>
                                <span class="badge bg-secondary m-1" style="float: left;">滚动哈希</span>
                                <span class="badge bg-secondary m-1" style="float: left;">分治</span>
                                <span class="badge bg-secondary m-1" style="float: left;">栈</span>
                                <span class="badge bg-secondary m-1" style="float: left;">动态规划</span>
                                <span class="badge bg-secondary m-1" style="float: left;">数据结构</span>
                                <span class="badge bg-secondary m-1" style="float: left;">状态压缩</span>
                                <!-- Add more badges as necessary -->

                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-3 d-flex flex-column">
                    <div class="card flex-grow-1">
                        <div class="card-body">
                            <h5 class="card-title">评分</h5>
                            <div class="row justify-content-center text-center">
                                <p  class="rating">95</p>
                            </div>
                        </div>
                    </div>
                    <div class="card flex-shrink-1">
                        <div class="d-flex">
                            <asp:TextBox CssClass="form-control" placeholder="留下你的评分" runat="server"></asp:TextBox>
                            <asp:Button CssClass="btn btn-primary" Text="提交" runat="server" />
                        </div>
                    </div>

            </div>

        </div> <!-- 第一行结束 -->

            <!-- Bottom section -->
            <div class="row mt-3">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">作品简介</h5>
                        <asp:Label ID="BookDescriptionLabel" runat="server" CssClass="card-text" Text="
                            这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介
                            这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介
                            这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介
                            这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介这是一条很长的简介"
                         ></asp:Label>
                    </div>
                </div>
            </div>
        </div>
</div>









</asp:Content>
