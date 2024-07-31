﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="LibraryLink.Views.TestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>LibraryLinkCP</title>
    <link rel="stylesheet" tyle="text/css" href="../Assets/Lib/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">


        <div class="container-fluid">
            <!-- 导航 -->
            <div class="row">
                <nav class="navbar navbar-expand-sm bg-body-tertiary navbar-s " data-bs-theme="dark">
                    <div class="container-fluid">
                        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo03" aria-controls="navbarTogglerDemo03" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>

                        <a class="navbar-brand fs-3" href="#" style="color:#9765e0">Library Link</a>

                        <div class="collapse navbar-collapse" id="navbarTogglerDemo03">
                            <ul class="nav nav-tabs" role="tablist">
                                <li class="nav-item" style="white-space: nowrap;">
                                    <a class="nav-link active fs-5" href="#users" data-bs-toggle="tab">个人信息</a>
                                </li>
                                <li class="nav-item" style="white-space: nowrap;">
                                    <a class="nav-link fs-5 " href="#books" data-bs-toggle="tab">图书馆</a>
                                </li>
                                <li class="nav-item" style="white-space: nowrap;">
                                    <a class="nav-link fs-5" href="#data" data-bs-toggle="tab">收藏夹</a>
                                </li>
                            </ul>
                            <div class="btn btn-light ms-auto" style="white-space: nowrap;">
                                    <a class="nav-link fs-5" style="color:#782a18" href="/User-Login/">退出</a>
                            </div>
                        </div>
                    </div>
                </nav>
            </div>
        </div>


    </form>
    <script src="../Assets/Lib/js/bootstrap.bundle.js"></script> 
</body>
</html>