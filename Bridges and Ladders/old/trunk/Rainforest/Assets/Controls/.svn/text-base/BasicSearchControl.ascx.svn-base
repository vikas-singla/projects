<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasicSearchControl.ascx.cs" Inherits="Rainforest.Assets.Controls.BasicSearchControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table cellpadding="5px" cellspacing="5px" class="tbl_basicSearchMainTable">
    <tr>
        <td>
            <asp:Label ID="Lbl_SearchForJob" runat="server" CssClass="lbl_basicjobsearchstyle">Search:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TextBox_Industry" runat="server" CssClass="textbox_basicJobSearchFieldStyle" ></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender_Keyword" TargetControlID="TextBox_Industry" 
                WatermarkCssClass="watermark_basicJobSearchFieldStyle" WatermarkText="By Industry" runat="server">
            </asp:TextBoxWatermarkExtender>
        </td>
        <td>
            <asp:TextBox ID="TextBox_CompanyName" runat="server" CssClass="textbox_basicJobSearchFieldStyle"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender_CompanyName" TargetControlID="TextBox_CompanyName" 
                WatermarkCssClass="watermark_basicJobSearchFieldStyle" WatermarkText="By Company" runat="server">
            </asp:TextBoxWatermarkExtender>
        </td>
        <td>
            <asp:TextBox ID="TextBox_Location" runat="server" CssClass="textbox_basicJobSearchFieldStyle"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender_Location" TargetControlID="TextBox_Location" 
                WatermarkCssClass="watermark_basicJobSearchFieldStyle" WatermarkText="By Location" runat="server">
            </asp:TextBoxWatermarkExtender>
        </td>
        <td>
            <asp:Button ID="Btn_JoinNow" runat="server" Text="Go" CssClass="btn_basicjobsearchStyle" />
        </td>
    </tr>
</table>