<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LibraryLink.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>LibraryLink-Login</title>
    <link rel = "stylesheet" type="text/css" href ="../Assets/Lib/css/bootstrap.min.css" />
</head>
<body>

    <div class ="container-fluid">
        <div>
            <br /><br /><br /><br />
            <h1 style="font-size: 110px; font-family: 'Segoe UI'; text-align: center">Library <span></span>Link</h1>
            <br /><br /><br />
            <p style ="text-align:center; font-size: 30px">凡我无法创造的, 我便无法理解</p>
            <br /><br /><br />
        </div>
        <div class ="row">
            <div class ="col"></div>
            <div class ="col">
                <form id = "form1" runat = "server">
                    <div>
                        <input type= "text" placeholder = "用户名"
                            autocomplete ="off" class ="form-control" style="min-width:300px"/>
                    </div>
                    <div>
                        <input type= "password" placeholder = "密码"
                            autocomplete ="off" class ="form-control" style="min-width:300px;"/>
                    </div>
                </form>
            </div>
            <div class ="col"></div>
        </div>
        <div class ="row">
        </div>
    </div>

</body>
</html>
