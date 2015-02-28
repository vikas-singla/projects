<%@ Page Title="" Language="C#" MasterPageFile="~/RainForestAdmin.Master" AutoEventWireup="true" CodeBehind="EditLocationPics.aspx.cs" Inherits="RainForestAdmin.EditLocationPics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:GridView ID="gvEditPics" width="94%" runat="server" 
        AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
           DataKeyNames="CompanyPicId"
           OnRowCancelingEdit="gvEditPics_RowCancelingEdit"  
           OnRowEditing="gvEditPics_RowEditing" OnRowUpdating="gvEditPics_RowUpdating"
            OnRowCommand="gvEditPics_RowCommand" ShowFooter="True" 
        OnRowDeleting="gvEditPics_RowDeleting" > 
<Columns>
<asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" /> 
<asp:TemplateField HeaderText="Edit" ShowHeader="False"> 
<EditItemTemplate> 
  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton> 
</FooterTemplate> 
<ItemTemplate> 
  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
</ItemTemplate> 
</asp:TemplateField>
<asp:BoundField HeaderText="CompanyPicId" Visible="false" DataField="CompanyPicId"/>
<asp:TemplateField HeaderText="Picture Path" SortExpression="PicPath"> 
<EditItemTemplate> 
  <asp:FileUpload ID="fu_PicPath" runat="server" Text='<%# Eval("PicturePath") %>' ></asp:FileUpload> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:FileUpload ID="fu_NewPicPath" runat="server"></asp:FileUpload>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_PicPath" runat="server" Text='<%# Bind("PicturePath") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Video Path" SortExpression="VideoPath"> 
<EditItemTemplate> 
  <asp:FileUpload ID="fu_VideoPath" runat="server" Text='<%# Eval("VideoPath") %>' ></asp:FileUpload> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:FileUpload ID="fu_NewVideoPath" runat="server"></asp:FileUpload>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_VideoPath" runat="server" Text='<%# Bind("VideoPath") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 
</Columns>
</asp:GridView>
</asp:Content>
