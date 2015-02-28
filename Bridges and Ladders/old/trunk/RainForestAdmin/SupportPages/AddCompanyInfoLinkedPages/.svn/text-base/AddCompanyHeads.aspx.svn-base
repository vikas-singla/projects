<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCompanyHeads.aspx.cs" Inherits="RainForestAdmin.AddCompanyHeads" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" >
    <title>Add Company Heads</title>
  
    
</head>

<body>
     
   
    <form id="CompanyHeadInfo" runat="server" >
    <h2>Enter Company Head Info</h2>
    <table>
    <tr>
    <td>
    Name:
    </td>
    <td style="width:100%;height:25px">
    <asp:TextBox id="txt_headname" runat="server" style="width:50%;height:25px" />
    </td>
    </tr>
    <tr>
    <td>
    Desgination:
    </td>
    <td style="width:100%;height:25px">
    <asp:TextBox id="txt_designation"  runat="server" type="text" style="width:50%;height:25px" />
    </td>
    </tr>
    </table>
    <asp:Button id="Reset"  runat="server" Text="Reset" 
        style="width:20%;height:25px" onclick="Reset_Click" />
    <asp:Button id="Submit" Text="Add" runat="server" onclick="Submit_Click" style="width:20%;height:25px"  />
    
    <asp:GridView ID="GridView1" runat="server" CellPadding="2" 
        ForeColor="Black" GridLines="None" BackColor="LightGoldenrodYellow" 
        BorderColor="Tan" BorderWidth="1px">
        <AlternatingRowStyle BackColor="PaleGoldenrod" />
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
    
    </form>
</body>
</html>
