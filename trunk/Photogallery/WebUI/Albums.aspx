<%@ Page MasterPageFile="~/SiteLayout.master" Language="C#" AutoEventWireup="true" CodeBehind="Albums.aspx.cs" Inherits="WebUI.Albums" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>

<asp:Content ContentPlaceHolderID="Title"  runat="server">Albums</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
    
    <cc:PagerV2_8 ID="pager1"
                  runat="server"  
                  EnableViewState="false"
                  OnCommand="pager_Command" />
    
<asp:Repeater ID="AlbumList" runat="server">
    <ItemTemplate>
    <div class="AlbumListItemContainer" > 
    <div class="AlbumListItem" >
        <a href="/Photos.aspx?album=<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>">
           <%# DataBinder.Eval(Container.DataItem, "Title")%>                      
        </a>        
    </div>
    
    <div  class="AlbumListItemDescription" >
    <%# DataBinder.Eval(Container.DataItem, "Description")%>
    </div>
    <div  class="AlbumListItemCreationDate" >
        <%# DataBinder.Eval(Container.DataItem, "CreationDate")%>
    </div>
    </div>
    
    
    </ItemTemplate>
    
</asp:Repeater>

    <cc:PagerV2_8 ID="pager2"
                  runat="server"  
                  EnableViewState="false"
                  OnCommand="pager_Command" />

</asp:Content>

