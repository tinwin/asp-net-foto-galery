<%@  Page Language="C#" MasterPageFile="~/SiteLayout.master" AutoEventWireup="true" CodeBehind="AdminRoom.aspx.cs" Inherits="WebUI.Admin.AdminRoom " %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label ID="userExistsLabel" runat="server" Text="Label" CssClass="validator"  Visible="false" > entered name has been already registered  </asp:Label>
    <asp:Label ID="emailExistsLabel" runat="server" CssClass="validator" Visible="false"  Text="Label">entered email has been already registered </asp:Label> 
   
 
    
    <asp:GridView ID="GridView1" runat="server" OnRowDataBound="AdminRoomBind"   AutoGenerateColumns="False" 
        DataSourceID="ObjectDataSource1" AllowPaging="false" OnRowUpdating="GridViewRowUpdating" >
        <Columns>
            

        
            
           <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text=<%#Eval("Username") %>></asp:Label>
                    <asp:HiddenField ID="UserId1"  Value=<%#Bind("UserId")%>   runat="server" />
                </ItemTemplate>
                <EditItemTemplate >
                    <asp:HiddenField ID="UserId2" Value=<%#Bind("UserId") %>  runat="server"> </asp:HiddenField>
                    <asp:TextBox ID="Username" Text=<%#Bind("Username") %>  runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="UsernameValidator" ControlToValidate="Username" runat="server" ValidationExpression="[a-zA-Z_0-9.-]{2,9}" ErrorMessage="Only two-nine symbols a-zA-Z_0-9.- are acceptable "  ></asp:RegularExpressionValidator>  
                </EditItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text=<%#Eval("UserMail") %>></asp:Label>
                </ItemTemplate>
                <EditItemTemplate >
                    <asp:TextBox ID="UserMail" Text=<%#Bind("UserMail") %>  runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="EmailValidator" ControlToValidate="UserMail" runat="server" ValidationExpression="[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+"  ErrorMessage="invalid email"></asp:RegularExpressionValidator> 
                </EditItemTemplate>
           </asp:TemplateField>
          
           
             <asp:TemplateField  HeaderText="Role">
                <EditItemTemplate >
                    <asp:DropDownList ID="DropDownList1" runat="server" DataSource=<%#repository.GalleryRoles%> SelectedValue=<%#Bind("UserRole") %> >
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label ID="Label1" runat="server" Text=<%#Bind("UserRole") %>></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
  
        </Columns>
    </asp:GridView>
    

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
         DeleteMethod="DeleteUser" 
        SelectMethod="GetAllUsers" 
        TypeName="WebUI.Admin.AdminRoom+AdminRoomProvider" 
        UpdateMethod="UpdateUser">
        <DeleteParameters>
           <asp:Parameter DbType="Guid" Name="UserId" />
        </DeleteParameters> 
        <UpdateParameters>
            <asp:Parameter DbType="Guid" Name="UserId" />
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="UserMail" Type="String" />
            <asp:Parameter Name="UserRole" Type="String" />
            
        </UpdateParameters>
    </asp:ObjectDataSource>
    
 
</asp:Content>

