<%@ Page MasterPageFile="~/SiteLayout.master" Language="C#" AutoEventWireup="true" CodeBehind="Photos.aspx.cs" Inherits="WebUI.Photos" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <form id="form1" runat="server">
<asp:Repeater ID="PhotoList" runat="server">
    <ItemTemplate>
        <asp:Image ImageUrl='<%#"GetImage.ashx?id=" + DataBinder.Eval(Container.DataItem, "PhotoId")%>' runat="server" />
    </ItemTemplate>
</asp:Repeater>
</form>
</asp:Content>