<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="WebUI.Registration" MasterPageFile="~/SiteLayout.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Label ID="Label1" runat="server" Text="Label" Visible="false">user hasn`t been created</asp:Label>
    
    <table>
    
    <tr>
        <td>Login:</td>
        <td><asp:TextBox ID="name" runat="server"></asp:TextBox>
        </td><td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="name" 
                ErrorMessage="User name required"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator4" runat="server" ControlToValidate="name" OnServerValidate="NameCheck" 
                ErrorMessage="This name already exists"></asp:CustomValidator>
            <asp:RegularExpressionValidator runat="server" ID="templateValidator" ControlToValidate="name" ValidationExpression="[a-zA-Z_0-9.-]{2,9}" ErrorMessage="Only two-nine symbols a-zA-Z_0-9.- are acceptable "  ></asp:RegularExpressionValidator>  
        </td>
    </tr>
    <tr>
        <td>Password :</td>
        <td><asp:TextBox ID="password" TextMode="password"  runat="server"></asp:TextBox>
        </td><td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="password"  
                ErrorMessage="password required"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only two-nine symbols a-zA-Z_0-9.- are acceptable" ValidationExpression="[a-zA-Z_0-9.-]{2,9}" ControlToValidate="password" ></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td>ConfirmPasswod:</td> 
        <td><asp:TextBox ID="confirmPassword" TextMode="Password"  runat="server"></asp:TextBox>
        </td><td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="confirmPassword"  
                ErrorMessage="password confirmation required"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="confirmPassword" OnServerValidate="CheckPassword"    
                ErrorMessage="password confirmation does not match password"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td>Email:</td>
        <td><asp:TextBox ID="email" runat="server"></asp:TextBox>
        </td><td>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="email" 
               ValidationExpression="[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+"  
                ErrorMessage="invalid email"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="email" 
                ErrorMessage="your email required"></asp:RequiredFieldValidator>
            <asp:CustomValidator
                    ID="CustomValidator2" runat="server" ErrorMessage="This eemail is already registered" ControlToValidate="email" OnServerValidate="CheckEmail"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td>Description:</td>
        <td><asp:TextBox ID="description" runat="server"></asp:TextBox>
        </td><td>
            <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="description" OnServerValidate="CheckDescription"  
                ErrorMessage="description should be no longer than 800 symbols"></asp:CustomValidator>
        </td>
    </tr>
    
    
    


</table>
    <asp:Button ID="Button1" runat="server" Text="Register" 
        onclick="Button1_Click" />



</asp:Content>
