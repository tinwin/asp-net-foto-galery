<%@  Page Language="C#" MasterPageFile="~/SiteLayout.master" AutoEventWireup="true" CodeBehind="EditPhoto.aspx.cs" Inherits="WebUI.EditPhoto" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
	<div id="editPhotoContainer">
	<div class="left">
		<asp:HiddenField ID="PhotoId" runat="server" Value="0" />
		<div>
			<asp:Label ID="label" 
					   runat="server"
					   AssociatedControlId="PhotoTitle" 
					   Text="Title" />
			<asp:TextBox ID="PhotoTitle" runat="server" />
		</div>
		<div>
			<asp:Label ID="label1"
					   runat="server"
					   AssociatedControlId="PhotoTitle" 
					   Text="Description" />
			<asp:TextBox ID="PhotoDescription" runat="server" TextMode="MultiLine" />
		</div>
		<div>
			<fieldset>
				<legend>File</legend>
					<asp:FileUpload ID="PhotoFile" CssClass="fileSelector" runat="server" />
			</fieldset>
		</div>
		<div>Album</div>
		<div>
		<label><h5>Preview</h5></label>
			
			<img ID="PhotoImage"
					 class="thumbnail"
					 runat="server"
					 src="/GetImage.ashx?size=thumbnail&amp;id=" />
		</div>
	</div>
	<div class="right">
		<div class="tags">
			<asp:CheckBoxList EnableViewState="true"
						  DataTextField="TagTitle"
						  DataValueField="TagId"
						  ID="TagsList"
						  runat="server" />
		</div>
	</div>
	<div class="bottom">
		<div class="submit">
			<asp:Button ID="Button1" runat="server" Text="Save" OnClick="Button1_Click" />
		</div>
	</div>
	</div>
</asp:Content>