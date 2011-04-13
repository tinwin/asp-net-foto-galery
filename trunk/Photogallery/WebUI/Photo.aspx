<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="WebUI.Photo" MasterPageFile="~/SiteLayout.master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div>
    <img alt="<%=CurrentPhoto.PhotoTitle %>"
         title="<%=CurrentPhoto.PhotoTitle %>"
     src="/GetImage.ashx?id=<%=CurrentPhoto.PhotoId %>" width="200px" height="200px" />
</div>
<hr />
<div>
    <%=CurrentPhoto.PhotoDescription %>
</div>
</asp:Content>
