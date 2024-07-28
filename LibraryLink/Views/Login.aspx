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

            <!-- 登录区 -->
            <div class="col-md-2 col-sm-5">
                <form id="form1" runat="server">
                    <div class="mb-1">
                        <input type="text" placeholder="用户名" autocomplete="off" class="form-control" style="min-width:300px" />
                    </div>
                    <div class="mb-3">
                        <input type="password" placeholder="密码" autocomplete="off" class="form-control" style="min-width:300px" />
                    </div>
                    <div class="mb-3 d-grid">
                        <asp:Button Text="登录" runat="server" class="btn btn-primary btn-block" style="min-width:300px" />
                    </div>

                    <!-- 下拉选框和注册按钮 -->
                    <div class="d-flex justify-content-between" style="min-width:300px">
                        <div class="flex-grow-1 me-2">
                            <div class="input-group mb-3">
                                <label class="input-group-text" for="inputGroupSelect01" style="width:80px">用户组：</label>
                                <select class="form-select" id="inputGroupSelect01">
                                    <option value="1">用户</option>
                                    <option value="2">管理员</option>
                                </select>
                            </div>
                        </div>
                        <div>
                             <button type="button" class="btn btn-outline-secondary">注册</button>
                        </div>
                    </div>
                    <!-- 结束下拉选框和注册按钮 -->

                </form>
            </div>
            <!-- 结束登录区 -->

        </div>
        <div class ="row"></div>
    </div> <!-- 结束容器 -->

     <script src="../Assets/Lib/js/bootstrap.bundle.js"></script> 

</body>
</html>
