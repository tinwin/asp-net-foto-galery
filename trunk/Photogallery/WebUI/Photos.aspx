<%@ Page MasterPageFile="~/SiteLayout.master" Language="C#" AutoEventWireup="true" CodeBehind="Photos.aspx.cs" Inherits="WebUI.Photos" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
	<div>
		<cc:PagerV2_8 ID="pager1"
              runat="server"  
              EnableViewState="false"
              OnCommand="pager_Command" />
	</div>
    <div id="photos">
		<asp:Repeater ID="PhotoList" runat="server">
			<ItemTemplate>
				<div class="item">
					<div id="PhotoToolbar" runat="server" class="toolbar">
						<a href="/EditPhoto.aspx?id=<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>">
						<img width="10px" height="10px" alt="Edit" title="Edit" src="/images/Edit.gif"/></a>
					</div>
					<a href="/Photo.aspx?id=<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>">
						<img title="<%# DataBinder.Eval(Container.DataItem, "PhotoTitle")%>"
							 alt="<%# DataBinder.Eval(Container.DataItem, "PhotoTitle")%>"
							 src='<%#"GetImage.ashx?size=thumbnail&amp;id=" + DataBinder.Eval(Container.DataItem, "PhotoId")%>' 
							 width="60px"
							 height="60px"/>
						<h4><%# DataBinder.Eval(Container.DataItem, "PhotoTitle")%></h4>
					</a>
				</div>
			</ItemTemplate>
		</asp:Repeater>
    </div>
</asp:Content>