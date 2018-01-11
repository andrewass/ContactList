<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogInForm.aspx.cs" Inherits="ContactList.LogInForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="loginform" runat="server">
        <div>
            Username : <br />
            <asp:TextBox ID="username" runat="server"></asp:TextBox> <br />
            Password : <br />
            <asp:TextBox ID="password" TextMode="Password" runat="server"></asp:TextBox> <br />
            <asp:Button ID="submitLogIn" runat="server" Text="Sign in" OnClick="submitLogIn_Click" />
        </div>
    </form>
</body>
</html>
