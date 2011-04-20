<%@ Page MasterPageFile="~/SiteLayout.master" Language="C#" AutoEventWireup="true" CodeBehind="EditAlbum.aspx.cs" Inherits="WebUI.EditAlbum" %>
<asp:Content ContentPlaceHolderID="Title"  runat="server">Edit Album</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
	<div id="editPhotoContainer">
	<div class="left">
		<asp:HiddenField ID="AlbumId" runat="server" Value="0" />
		<div>
			<asp:Label ID="label" 
					   runat="server"
					   AssociatedControlId="AlbumTitle" 
					   Text="Title" />
			<asp:TextBox ID="AlbumTitle" BackColor="AliceBlue" BorderStyle="Groove" Height="14" Width ="510" runat="server" />
		</div>
		<div>
			<asp:Label ID="label1"
					   runat="server"
					   AssociatedControlId="AlbumTitle" 
					   Text="Description" />
			<asp:TextBox ID="AlbumDescription" BackColor="AliceBlue" BorderStyle="Groove" Height="45" Width ="510" runat="server" TextMode="MultiLine" />
		</div>
		<asp:Button runat="server" ID="ButtonSave" Text="Save" OnClick="Button1_Click"/>
		<asp:Button runat="server" ID="ButtonDeleteAlbum" Text="Delete" OnClick="ButtonDeleteAlbum_Click"/>
		</div>
		</div>		
</asp:Content>
