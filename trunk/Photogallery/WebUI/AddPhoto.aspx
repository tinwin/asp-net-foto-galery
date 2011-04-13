<%@  Page Language="C#" MasterPageFile="~/SiteLayout.master" AutoEventWireup="true" CodeBehind="AddPhoto.aspx.cs" Inherits="WebUI.AddPhoto" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
        &nbsp;<asp:FileUpload ID="PhotoFile" runat="server" />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    </form>
</asp:Content>