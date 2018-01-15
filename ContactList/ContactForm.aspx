<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactForm.aspx.cs" Inherits="ContactList.ContactForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Form</title>
    <script src="ui_operations.js"> </script>
</head>
<body>
    <form id="contactForm" runat="server">
        <div id="listdiv">
            <h1>Contact List</h1>
            <asp:Repeater ID="repeater" runat="server">
                <HeaderTemplate>
                    <table cellspacing="0" rules="all" border="2">
                    <tr>
                        <th scope="col" style="width: 150px"> First Name </th>
                        <th scope="col" style="width: 150px"> Last Name </th>
                        <th scope="col" style="width: 150px"> Phone Number </th>
                        <th scope="col" style="width: 200px"> E-mail </th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                     <tr>
                        <td> <%# Eval("Firstname") %> </td>
                        <td> <%# Eval("Lastname") %> </td>
                        <td> <%# Eval("Phonenumber") %> </td>
                        <td> <%# Eval("Email") %> </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater> 
        </div>

        <div id="searchdiv">
            Search contact by first and last name : <br />
            <asp:TextBox ID="searchPhrase" runat="server"></asp:TextBox> <br />
            <asp:Button ID="searchButton" runat="server" Text="Search" OnClick="searchButton_Click" />
        </div>

        <div id="contactdetails">
            First Name : <br />
            <asp:TextBox ID="firstName" runat="server"></asp:TextBox> <br />
            Last Name : <br />
            <asp:TextBox ID="lastName" runat="server"></asp:TextBox> <br />
            Phone Number : <br />
            <asp:TextBox ID="phoneNumber" runat="server"></asp:TextBox> <br />
            Email : <br />
            <asp:TextBox ID="email" runat="server"></asp:TextBox> <br />
            <asp:Button ID="saveButton" runat="server" Text="Save" OnClick="saveButton_Click" /> <br />
            <br />
            <asp:Button ID="deleteButton" runat="server" Text="Delete" OnClick="deleteButton_Click" 
                OnClientClick="ConfirmDelete()" /> 
        </div>


    </form>
</body>
</html>
