<%@ Page Title="" Language="C#" MasterPageFile="~/RainForestAdmin.Master" AutoEventWireup="true" CodeBehind="EditGrid.aspx.cs" Inherits="RainForestAdmin.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:GridView ID="gvedit" runat="server" BackColor="LightGoldenrodYellow" 
        BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
        GridLines="Both" SelectedRowStyle-BackColor="YellowGreen" 
           OnRowCancelingEdit="gvedit_RowCancelingEdit" OnRowDataBound="gvedit_RowDataBound" 
           OnRowEditing="gvedit_RowEditing" OnRowUpdating="gvedit_RowUpdating"
            OnRowCommand="gvedit_RowCommand" ShowFooter="True" OnRowDeleting="gvedit_RowDeleting" >
        <AlternatingRowStyle BackColor="PaleGoldenrod" />
        <Columns>
            <asp:CommandField ShowDeleteButton="True"  />
            <%--<asp:TemplateField HeaderText="Edit" ShowHeader="False"> 
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
</asp:TemplateField>--%>
        </Columns>
        <FooterStyle BackColor="Tan" />
        <HeaderStyle BackColor="Tan" Font-Bold="True" />
        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
            HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
        <SortedAscendingCellStyle BackColor="#FAFAE7" />
        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
        <SortedDescendingCellStyle BackColor="#E1DB9C" />
        <SortedDescendingHeaderStyle BackColor="#C2A47B" />

    </asp:GridView>
</asp:Content>

