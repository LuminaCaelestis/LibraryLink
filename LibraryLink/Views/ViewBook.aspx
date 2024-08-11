<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewBook.aspx.cs" Inherits="LibraryLink.Views.ViewBook" %>

<!DOCTYPE html>

<!-- 临时测试 -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>在线阅读</title>
    <style>
        iframe {
            width: 100%;
            height: 100vh;
            border: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <iframe id="pdfViewer" runat="server"></iframe>
        </div>
    </form>
</body>
</html>
