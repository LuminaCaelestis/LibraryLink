<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LibraryLink.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LibraryLink-Login</title>
    <link rel="stylesheet" type="text/css" href="../Assets/Lib/css/bootstrap.min.css" />
    <style>
        body {
            background-color: #f8f9fa;
        }

        h1 {
            font-size: 150px;
            font-family: 'Segoe UI';
            text-align: center;
        }

        p {
            text-align: center;
            font-size: 35px;
            font-family: 'Segoe UI';
        }

        .form-container {
            max-width: 350px;
            margin-top: 30px;
        }

        .btn-block {
            width: 100%;
        }

        .forgot-password, .register {
            display: inline-block;
            width: 48%;
            text-align: center;
            color: #6c757d;
            text-decoration: none;
        }

            .forgot-password:hover, .register:hover {
                text-decoration: underline;
            }

        .forgot-password {
            text-align: center;
        }

        .register {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- 容器 -->
        <div class="container-fluid">
            <!-- 顶部导航栏 -->
            <nav class="navbar navbar-expand-sm navbar-dark bg-dark">
                <div class="container-fluid d-flex">
                    <a class="navbar-brand" href="#">LibraryLink</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mynavbar">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="mynavbar">
                        <ul class="navbar-nav ms-auto align-items-center">
                            <li class="nav-item">
                                <a class="nav-link" title="本项目的Github页面" href="https://github.com/LuminaCaelestis/LibraryLink.git">Github</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="/Admin-Login/">管理员登录</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <!-- 封面部分 -->
            <div class="row" style="margin-top: 50px; margin-bottom: 30px">
                <div class="col-12">
                    <h1>Library <span>Link</span></h1>
                    <br />
                    <p>凡我无法创造的, 我便无法理解</p>
                </div>
            </div>

            <!-- 输入框部分 -->
            <div class="row justify-content-center">
                <div class="col-12 form-container">
                    <div class="mb-3">
                        <input type="text" id="Username_L" placeholder="用户名" autocomplete="off" class="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <input type="password" id="Password_L" placeholder="密码" autocomplete="off" class="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <button type="submit" runat="server" onserverclick="Reader_Login_Click" class="btn btn-primary btn-block">登录</button>
                    </div>
                    <!-- 辅助按钮 -->
                    <div class="d-flex justify-content-between">
                        <div class="flex-grow-1 me-2">
                            <a href="#" class="btn btn-outline-secondary">忘记密码</a>
                        </div>
                        <div>
                            <a href="/Reader-Register/" class="btn btn-outline-secondary">没有账号?</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>



    <script src="../Assets/Lib/js/bootstrap.bundle.js"></script>

</body>
</html>
