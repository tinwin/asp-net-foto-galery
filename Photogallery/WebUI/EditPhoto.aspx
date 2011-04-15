<%@  Page Language="C#" MasterPageFile="~/SiteLayout.master" AutoEventWireup="true" CodeBehind="EditPhoto.aspx.cs" Inherits="WebUI.EditPhoto" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    
        <div>
        </div>
        <div>
        </div>
        <div>
        </div>
        <div>
            <br />
            <br />
            <br />
            <asp:FormView  ID="FormViewPhoto" runat="server">
                <ItemTemplate>
                    <asp:Label ID="label" runat="server" AssociatedControlId="PhotoTitle" 
                        Text="Title" />
                    <asp:TextBox ID="PhotoTitle" Text='<%# Eval("PhotoTitle") %>' runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="label1" runat="server" AssociatedControlId="PhotoTitle" 
                        Text="Description" />
                    <asp:TextBox ID="PhotoDescription" Text='<%# Eval("PhotoDescription") %>' runat="server" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <asp:Label ID="label2" runat="server" AssociatedControlId="PhotoFile" 
                        Text="File" />
                    <asp:FileUpload ID="PhotoFile" runat="server" />
                    <img class="thumbnail" runat="server" src='<%# "/GetImage.ashx?size=thumbnail&amp;id=" + Eval("PhotoId") %>' />
                    <asp:CheckBoxList  ID="TagsList" runat="server">
                    
                    </asp:CheckBoxList>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Button runat="server" Text="Save" OnClick="Button1_Click" />
                </FooterTemplate>
            </asp:FormView>
            <br />
            <br />
        </div>
        
        
        
    
</asp:Content>