<%@  Page Language="C#" MasterPageFile="~/SiteLayout.master" AutoEventWireup="true" CodeBehind="AboutUser.aspx.cs" Inherits="WebUI.AboutUser" %>
<%@ Import namespace="System.Linq "  %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
 
 <h3><%=currentUser.Username  %></h3>
 <div class="description"><%=currentUser.Description  %></div>

   <asp:HyperLink runat="server"  NavigateUrl="Albums.aspx?name=<%=currentUser.Username   %>" CssClass="Link"  ><%=currentUser.Username  %> albums</asp:HyperLink>    

  
 
</asp:Content>
