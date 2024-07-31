<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="LibraryLink.Views.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" tyle="text/css" href="../Assets/Lib/css/bootstrap.min.css" />
</head>
<body>
  
<div class="container-fluid mt-3">

  <div class="row bg-dark text-white" style="height:8vh"><h1>Row-1</h1></div>

  <div class="row bg-success" style="height:92vh">
  	<div class="col-1 bg-primary">
    	<h1>Col-1</h1>
    </div>
    <div class="col">
    	<h1>Col-2</h1>
    </div>
  </div>
</div>

</body>
</html>
