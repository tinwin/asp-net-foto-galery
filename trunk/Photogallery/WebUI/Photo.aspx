<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="WebUI.Photo" MasterPageFile="~/SiteLayout.master" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<asp:Content ContentPlaceHolderID="Title"  runat="server">Photo</asp:Content>
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

<div class="photo-description">
	<hr />
    <%=CurrentPhoto.PhotoDescription %>
</div>
<div>
	<hr />
	<h3>Comments</h3>
	<asp:Repeater ID="PhotoComments" runat="server">
		<ItemTemplate>
			<div class="comments-item">
				<span class="author"><%# DataBinder.Eval(Container.DataItem, "Author.Username")%></span>
				<span class="date"><%# DataBinder.Eval(Container.DataItem, "AdditionDate")%></span>
				<div class="text">
					<%# DataBinder.Eval(Container.DataItem, "Text")%>
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	
	<cc:PagerV2_8 ID="pager1"
              runat="server"  
              EnableViewState="false"
              OnCommand="pager_Command" />
	<hr />
	<div runat="server" id="AddCommentContainer"> 
		
		<asp:Label AssociatedControlID="TextBoxComment" runat="server"><h3>Add comment</h3></asp:Label>
		<asp:TextBox Width="400px" Height="100px" ID="TextBoxComment" runat="server" TextMode="MultiLine" MaxLength="50" />
		<asp:Button runat="server" Text="Add" ID="ButtonAdd" OnClick="ButtonAdd_Click" />
	</div>
</div>
</asp:Content>
