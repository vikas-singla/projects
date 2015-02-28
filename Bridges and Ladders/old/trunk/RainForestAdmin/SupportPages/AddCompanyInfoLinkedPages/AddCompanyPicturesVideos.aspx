<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCompanyPicturesVideos.aspx.cs" Inherits="RainForestAdmin.AddCompanyPictures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Select Location:
<asp:DropDownList ID="ddl_Location" runat="server">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
Upload Picture:
<asp:FileUpload ID="fupload_CompanyPictures" runat="server" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
Upload Video:
<asp:FileUpload ID="fupload_video" runat="server" />
<br />
<br />
<asp:Button ID="btn_SavePictures" runat="server" Height="26px" Text="Save" 
    Width="88px" onclick="btn_SavePictures_Click" />
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
