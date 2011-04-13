<%@ Page MasterPageFile="~/SiteLayout.master" Language="C#" AutoEventWireup="true" CodeBehind="Photos.aspx.cs" Inherits="WebUI.Photos" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <form id="form1" runat="server">
    
    <cc:PagerV2_8 ID="pager1"
                  runat="server"  
                  EnableViewState="false"
                  OnCommand="pager_Command" />
    
<asp:Repeater ID="PhotoList" runat="server">
    <ItemTemplate>
    <div style="float: left; width:20%">
        <a href="/Photo.aspx?id=<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>">
            <img title="<%# DataBinder.Eval(Container.DataItem, "PhotoTitle")%>"
                 alt="<%# DataBinder.Eval(Container.DataItem, "PhotoTitle")%>"
                 src='<%#"GetImage.ashx?id=" + DataBinder.Eval(Container.DataItem, "PhotoId")%>' 
                 width="60px"
                 height="60px"/>
            <h4><%# DataBinder.Eval(Container.DataItem, "PhotoTitle")%></h4>
        </a>
    </div>
    
    </ItemTemplate>
</asp:Repeater>
<br />

</form>
</asp:Content>