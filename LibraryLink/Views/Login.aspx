<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LibraryLink.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>LibraryLink-Login</title>
    <link rel = "stylesheet" type="text/css" href ="../Assets/Lib/css/bootstrap.min.css" />
</head>
<body>

    <!-- 容器 -->
    <div class ="container-fluid">

        <div class="row" style="margin-top: 80px;">
            <div class="col-md-3 col-sm-1"></div>
            <div class="col-md-6 col-sm-10 col-12">
                <h1 style="font-size: 150px; font-family: 'Segoe UI'; text-align: center">Library <span></span>Link</h1>
                <br/><br/>
                <p style ="text-align:center; font-size: 30px; font-family:'Segoe UI'">凡我无法创造的, 我便无法理解</p>
            </div>
            <div class="col-md-3 col-sm-1"></div>
        </div>


        <!-- 输入框部分 -->
        <div class="row justify-content-center" style="margin-top: 70px;">
            <div class="col-2 col-sm-2 col-md-4"></div>
            <!-- 登录注册 -->
            <div class="col-8 col-sm-8 col-md-4 col-lg-3" style="max-width:300px">
                <form id="form1" runat="server">
                    <div class="mb-1"> 
                        <input type="text" id="Username_L" placeholder="用户名" autocomplete="off" class="form-control" runat="server"/>
                    </div>
                    <div class="mb-3">
                        <input type="password" id="Password_L" placeholder="密码" autocomplete="off" class="form-control" runat="server"/>
                    </div>
                    <div class="mb-3 d-grid">
                        <button type="submit" runat="server" onserverclick="User_Login_Click" class="btn btn-primary btn-block">登录</button>
                    </div>

                    <!-- 用户组和注册按钮 -->
                    <div class="d-flex justify-content-between">
                        <div class="flex-grow-1 me-2">
                            <a href="/Admin-Login" class="btn btn-outline-secondary">管理员登录</a>
                        </div>
                        <div>
                             <button type="button" class="btn btn-outline-secondary" onserverclick="User_Login_Click">没有账号？</button>
                        </div>
                    </div>
                    <!-- 结束用户组和注册按钮 -->

                </form>
            </div>
            <!-- 结束登录注册 -->
            <div class="col-2 col-sm-2 col-md-4"></div>
        </div>
        <div class ="row"></div>
    </div> <!-- 结束容器 -->

     <script src="../Assets/Lib/js/bootstrap.bundle.js"></script> 

</body>
</html>
