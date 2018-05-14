<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="loginForm" runat="server"
        method="post">
Email<br />
<input type="text" name="email" value="" /><br />
Password<br />
<input type="text" name="password" value="" /><br /><br />
        <asp:Button 
  ID="submitButton" 
  PostBackUrl="~/.aspx"
  runat="server"
  Text="Submit" OnClick="submit_onClick" />
<asp:Button ID="newUserButton" Text="New User" PostBackUrl="~/newUser.aspx" runat="server" OnClick="newUser_onClick"/>

    <div>
    
    </div>
    </form>
</body>
</html>
