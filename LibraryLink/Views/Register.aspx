<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LibraryLink.Views.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LibraryLink-Register</title>
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

        .login {
            display: block;
            text-align: center;
            color: #6c757d;
            text-decoration: none;
            margin-top: 20px;
        }

        .login:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="registerList" runat="server">
        <!-- 容器 -->
        <div class="container-fluid">
            <!-- 顶部导航栏 -->
            <nav class="navbar navbar-expand-sm navbar-dark bg-dark">
                <div class="container-fluid d-flex">
                    <a class="navbar-brand" href="javascript:void(0)">LibraryLink</a>
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
            <div class="row">
                <div class="col-12" style="margin-top: 50px;">
                    <h1>Library <span>Link</span></h1>
                    <br />
                    <p>凡我无法创造的, 我便无法理解</p>
                </div>
            </div>

            <!-- 注册表单部分 -->
            <div class="row justify-content-center">
                <div class="col-12 form-container">
                    <div class="mb-3">
                        <label for="Username_R" class="form-label">用户名 <span id="UsernameTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                        <input type="text" id="Username_R" maxlength="30" placeholder="" autocomplete="off" class="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <label for="Email_R" class="form-label">Email <span id="EmailTip" class="text-danger" style="font-size: smaller;" runat="server"></span></label>
                        <input type="email" maxlength="50" id="Email_R" placeholder="example@domain.com" autocomplete="off" class="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <label for="Password_R" class="form-label">密码 <span id="PasswordTip" class="text-danger" style="font-size: smaller;"></span></label>
                        <input type="password" maxlength="30" id="Password_R" placeholder="至少6位字符" autocomplete="off" class="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <label for="ConfirmPassword_R" class="form-label">确认密码 <span id="ConfirmPasswordTip" class="text-danger" style="font-size: smaller;"></span></label>
                        <input type="password" maxlength="30" id="ConfirmPassword_R" placeholder="再次输入密码" autocomplete="off" class="form-control" runat="server" />
                    </div>
                    <div class="mb-3">
                        <!-- class="btn btn-primary btn-block"-->
                        <asp:Button ID="RegisterSubmit" Text="注册" OnClick="Register_Clicked" class="btn btn-primary btn-block" runat="server" ></asp:Button>
                    </div>
                    <!-- 按钮 -->
                    <a href="/Reader-Login/" class="login">已有账号？立即登录</a>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">

    document.addEventListener(
        "DOMContentLoaded", function () {

            var username = document.getElementById('<%= Username_R.ClientID %>');
            var email = document.getElementById('<%= Email_R.ClientID %>');
            var password = document.getElementById('<%= Password_R.ClientID %>');
            var confirmPassword = document.getElementById('<%= ConfirmPassword_R.ClientID %>')
            var registerSubmit = document.getElementById('<%= RegisterSubmit.ClientID %>');

            registerSubmit.addEventListener('click', function (event) {
                if (!(ValidEmail() && ValidPassword() && ValidUsername() && PasswordConfirmCheck())) {
                    event.preventDefault();
                    return false;
                }
            });

            username.addEventListener('input', ValidUsername);
            email.addEventListener('input', ValidEmail);
            confirmPassword.addEventListener('input', PasswordConfirmCheck);
            password.addEventListener('input', ValidPassword);
            password.addEventListener('input', PasswordConfirmCheck);

            function ValidUsername() {
                var regex = /^[a-zA-Z]\w{5,}$/;
                if (regex.test(username.value)) {
                    document.getElementById('UsernameTip').innerHTML = "";
                    return true;
                }
                else {
                    document.getElementById('UsernameTip').innerHTML = "至少6个字符，以字母开头，只含字母、数字、下划线";
                    return false;
                }
            }

            function ValidPassword() {
                var regex = /^\w{6,}$/;
                if (regex.test(password.value)) {
                    document.getElementById('PasswordTip').innerHTML = "";
                    return true;
                }
                else {
                    document.getElementById('PasswordTip').innerHTML = "密码只包含字母、数字、下划线";
                    return false;
                }
            }

            function ValidEmail() {
                var regex = /^[\w-_]+@[\w-_]+\.{1}[\w-_]{2,8}$/
                if (regex.test(email.value)) {
                    document.getElementById('EmailTip').innerHTML = "";
                    return true;
                }
                else {
                    document.getElementById('EmailTip').innerHTML = "邮箱格式不正确";
                    return false;
                }
            }

            function PasswordConfirmCheck() {
                if (password.value == confirmPassword.value) {
                    document.getElementById('ConfirmPasswordTip').innerHTML = "";
                    return true;
                } else {
                    document.getElementById('ConfirmPasswordTip').innerHTML = "密码输入不一致";
                    return false;
                }
            }
        }
        );
    </script>

    <script src="../Assets/Lib/js/bootstrap.bundle.js"></script>
</body>
</html>

