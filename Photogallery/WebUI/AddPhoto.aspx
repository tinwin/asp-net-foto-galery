<%@  Page Language="C#" MasterPageFile="~/SiteLayout.master" AutoEventWireup="true" CodeBehind="AddPhoto.aspx.cs" Inherits="WebUI.AddPhoto" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
        <div>
            <asp:Label id="label" AssociatedControlId="PhotoTitle" Text="Title" runat="server" />
            <asp:TextBox ID="PhotoTitle" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label id="label1" AssociatedControlId="PhotoTitle" Text="Description" runat="server" />
            <asp:TextBox TextMode="MultiLine" ID="PhotoDescription" runat="server"></asp:TextBox>
        </div>
        <div>
         <asp:Label id="label2" AssociatedControlId="PhotoFile" Text="File" runat="server" />
            <asp:FileUpload ID="PhotoFile" runat="server" />
        </div>
        <div>
            <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Save" />
        </div>
        
        
        
    </form>
</asp:Content>