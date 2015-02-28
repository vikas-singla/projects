<%@ Page Title="" Language="C#" MasterPageFile="~/RainForestAdmin.Master" AutoEventWireup="true" CodeBehind="EditLocationDetails.aspx.cs" Inherits="RainForestAdmin.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:GridView ID="gvEditLoc" width="94%" runat="server" 
        AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
           DataKeyNames="LocationId" 
           OnRowCancelingEdit="gvEditLoc_RowCancelingEdit" OnRowDataBound="gvEditLoc_RowDataBound" 
           OnRowEditing="gvEditLoc_RowEditing" OnRowUpdating="gvEditLoc_RowUpdating"
            OnRowCommand="gvEditLoc_RowCommand" ShowFooter="True" 
        OnRowDeleting="gvEditLoc_RowDeleting" > 
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
<asp:BoundField HeaderText="LocationID" Visible="false" DataField="LocationId"/>
<asp:TemplateField HeaderText="AddLine1" SortExpression="AddLine1"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Addr1" runat="server" Text='<%# Eval("AddressLine1") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewAddr1" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Addr1" runat="server" Text='<%# Bind("AddressLine1") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="AddLine2" SortExpression="AddLine2"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Addr2" runat="server" Text='<%# Eval("AddressLine2") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewAddr2" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Addr2" runat="server" Text='<%# Bind("AddressLine2") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="AddLine3" SortExpression="AddLine3"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Addr3" runat="server" Text='<%# Eval("AddressLine3") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewAddr3" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Addr3" runat="server" Text='<%# Bind("AddressLine3") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="City" SortExpression="City"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_City" runat="server" Text='<%# Eval("City") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewCity" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_City" runat="server" Text='<%# Bind("City") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="State" SortExpression="State"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_State" runat="server" Text='<%# Eval("State") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewState" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_State" runat="server" Text='<%# Bind("State") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Country" SortExpression="Country"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Country" runat="server" Text='<%# Eval("Country") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewCountry" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Country" runat="server" Text='<%# Bind("Country") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="ZipCode" SortExpression="ZipCode"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_PC" runat="server" Text='<%# Eval("PostalCode") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewPC" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_PC" runat="server" Text='<%# Bind("PostalCode") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 
</Columns>
</asp:GridView>
</asp:Content>
