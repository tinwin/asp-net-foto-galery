<%@ Page MasterPageFile="~/SiteLayout.master" Language="C#" AutoEventWireup="true" CodeBehind="Photos.aspx.cs" Inherits="WebUI.Photos" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
	<script type="text/javascript" src="/scripts/photos.js"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div>
		<div class="float-left">
			<cc:PagerV2_8 ID="pager1"
                  runat="server"  
                  EnableViewState="false"
                  OnCommand="pager_Command" />
		</div>
		<div class="float-right">
			<a id="authorModeLink">Author mode</a>
		</div>
    </div>
    <div>
		<asp:Repeater ID="PhotoList" runat="server">
			<ItemTemplate>
				<div style="float: left; width:30%">
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