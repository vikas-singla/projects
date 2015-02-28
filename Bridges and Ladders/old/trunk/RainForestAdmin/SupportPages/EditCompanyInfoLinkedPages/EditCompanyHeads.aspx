<%@ Page Title="" Language="C#" MasterPageFile="~/RainForestAdmin.Master" AutoEventWireup="true" CodeBehind="EditCompanyHeads.aspx.cs" Inherits="RainForestAdmin.EditCompanyHeads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:GridView ID="gvEditHeads" width="94%" runat="server" 
        AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
           DataKeyNames="CompanyHeadId"
           OnRowCancelingEdit="gvEditHeads_RowCancelingEdit"  
           OnRowEditing="gvEditHeads_RowEditing" OnRowUpdating="gvEditHeads_RowUpdating"
            OnRowCommand="gvEditHeads_RowCommand" ShowFooter="True" 
        OnRowDeleting="gvEditHeads_RowDeleting" > 
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
<asp:BoundField HeaderText="CompanyHeadId" Visible="false" DataField="CompanyHeadId"/>
<asp:TemplateField HeaderText="Head Name" SortExpression="HeadName"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_HeadName" runat="server" Text='<%# Eval("HeadName") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewHeadName" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_HeadName" runat="server" Text='<%# Bind("HeadName") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Designation" SortExpression="Designation"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Designation" runat="server" Text='<%# Eval("Designation") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewDesignation" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Designation" runat="server" Text='<%# Bind("Designation") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 
</Columns>
</asp:GridView>
</asp:Content>
