<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyLocation.aspx.cs" Inherits="RainForestAdmin.CompanyLocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>
    Address Line1:<asp:TextBox ID="txt_AddressLine1" runat="server" Height="24px" 
        Width="615px"></asp:TextBox>
</p>
<p>
    Address Line2:<asp:TextBox ID="txt_AddressLine2" runat="server" Height="25px" 
        Width="612px"></asp:TextBox>
</p>
<p>
    Address Line3:<asp:TextBox ID="txt_AddressLine3" runat="server" Height="25px" 
        Width="611px"></asp:TextBox>
</p>
<p>
    Country:<asp:TextBox ID="txtCountry" runat="server" Height="24px" Width="173px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    State:<asp:TextBox ID="txtState" runat="server" Height="24px" Width="173px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    City:<asp:TextBox ID="txtCity" runat="server" Height="24px" Width="173px"></asp:TextBox>
</p>
<p>
    Pincode:<asp:TextBox ID="txtPinCode" runat="server" Height="24px" Width="173px"></asp:TextBox>
</p>
<p>
    <asp:Button ID="btn_SaveAddress" runat="server" Height="23px" 
        Text="Save Address" Width="120px" onclick="btn_SaveAddress_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btn_Reset" runat="server" Height="23px" 
        Text="Reset" Width="120px" onclick="btn_Reset_Click"  />
</p>
    </div>
    <asp:GridView ID="GridView1" runat="server" BackColor="LightGoldenrodYellow" 
        BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
        GridLines="None">
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
