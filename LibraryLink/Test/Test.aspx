<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="LibraryLink.Views.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Assets/Lib/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Placeholder="新密码"></asp:TextBox>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Placeholder="确认新密码" ></asp:TextBox>
            <asp:Button ID="btnChangePassword" runat="server" Text="重置密码" OnClick="btnChangePassword_Click" />
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="#cc0000"></asp:Label>
        </div>
    </form>

    <script type="text/javascript">
        document.addEventListener(
            "DOMContentLoaded", function () {
                var txtNewPassword = document.getElementById('<%= txtNewPassword.ClientID %>');
                var txtConfirmPassword = document.getElementById('<%= txtConfirmPassword.ClientID %>');
                var lblMessage = document.getElementById('<%= lblMessage.ClientID %>');
                var btnChangePassword = document.getElementById('<%= btnChangePassword.ClientID %>');

            function validatePasswords() {
                if (txtNewPassword.value !== txtConfirmPassword.value) {
                    lblMessage.innerText = "两次输入不一致";
                    return false;
                } else {
                    lblMessage.innerText = "";
                    return true;
                }
            }

            txtNewPassword.addEventListener('input', validatePasswords);
            txtConfirmPassword.addEventListener('input', validatePasswords);

            btnChangePassword.addEventListener('click', function (event) {
                if (!validatePasswords()) {
                    event.preventDefault();
                }
            });
        });
    </script>

    <script src="../../Assets/Lib/js/bootstrap.bundle.min.js"></script>
</body>
</html>
