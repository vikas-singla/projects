<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddPicturesForComapnyOffices.ascx.cs" Inherits="RainForestAdmin.UserControls.WebUserControl1" %>
Select Location:
<asp:DropDownList ID="ddl_Location" runat="server">
</asp:DropDownList>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
Upload Picture:
<asp:FileUpload ID="fupload_CompanyPictures" runat="server" />
<br />
<br />
<asp:Button ID="btn_SavePictures" runat="server" Height="26px" Text="Save" 
    Width="88px" />
<br />
<br />

