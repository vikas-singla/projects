<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.LoginControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table cellpadding="0px" cellspacing="0px">
    <tr>
        <td class="ColumnTextBoxLoginWidth">
            <asp:CheckBox ID="CheckBox1" runat="server" Text="Keep me logged in" CssClass="CheckboxRememberLoginStyle" />
        </td>
        <td class="ColumnTextBoxLoginWidth">
            <asp:HyperLink ID="Link_ForgotPassword" runat="server" CssClass="LinkForgotPasswordStyle">Forgot your password?</asp:HyperLink>
        </td>
        <td class="ColumnTextBoxLoginWidth">
        </td>
    </tr>
    <tr>
        <td class="ColumnTextBoxLoginWidth">
            <asp:TextBox ID="TextBox_UserName" runat="server" CssClass="TextBoxSignInFieldStyle"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender_UserName" TargetControlID="TextBox_UserName"
                WatermarkCssClass="watermark_LoginFieldStyle" WatermarkText="Email" runat="server">
            </asp:TextBoxWatermarkExtender>
        </td>
        <td class="ColumnTextBoxLoginWidth">
            <asp:TextBox ID="TextBox_Password" runat="server" CssClass="TextBoxSignInFieldStyle"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender_Password" TargetControlID="TextBox_Password"
                WatermarkCssClass="watermark_LoginFieldStyle" WatermarkText="Password" runat="server">
            </asp:TextBoxWatermarkExtender>
        </td>
        <td class="ColumnTextBoxLoginWidth">
            <asp:Button ID="Btn_SignIn" runat="server" Text="Sign In" CssClass="FormButtonStyle" />
        </td>
    </tr>
</table>
