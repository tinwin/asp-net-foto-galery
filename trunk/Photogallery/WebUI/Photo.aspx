<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="WebUI.Photo" MasterPageFile="~/SiteLayout.master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="photoDescription">
	<div class="photoDescription-photo">
		<img alt="<%=CurrentPhoto.PhotoTitle %>"
			 title="<%=CurrentPhoto.PhotoTitle %>"
		 	 src="/GetImage.ashx?id=<%=CurrentPhoto.PhotoId %>" width="200px" height="200px" />
	</div>
     <div class="photoDescription-items">
		<div>Album:</div>
		<div><%=CurrentPhoto.HostAlbum.Title %></div>
		
		<div>Author:</div>
		<div><%=CurrentPhoto.OwningUser.Username %></div>
		
		<div>Added:</div>
		<div><%=CurrentPhoto.AdditionDate %></div>
     </div>
     
</div>

<div>
	<hr />
    <%=CurrentPhoto.PhotoDescription %>
</div>
</asp:Content>
