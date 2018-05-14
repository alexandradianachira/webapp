<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newUser.aspx.cs" Inherits="WebApplication.newUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   
         <form id="newUserForm" runat="server"
        method="post">
Surname<br />
<input type="text" name="surname" value="" /><br />
First Name<br />
<input type="text" name="first_name" value="" /><br /><br />
Email<br />
<input type="text" name="email" value="" /><br /><br />
Institution<br />
<input type="text" name="institution" value="" /><br /><br />
        <asp:Button 
  ID="submitButtonNewUserForm" 
  PostBackUrl="~/.aspx"
  runat="server"
  Text="Submit" OnClick="submitNewUser_onClick" />


    <div>

    
    </div>
    </form>
</body>
</html>
