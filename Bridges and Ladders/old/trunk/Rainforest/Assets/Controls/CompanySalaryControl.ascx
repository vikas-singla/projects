<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanySalaryControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.CompanySalaryControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table cellpadding="2px" cellspacing="0px" class="SalaryFilterTbl">
    <tr>
        <td class="SalaryFilterTblRowStyle">
            <asp:Label runat="server" CssClass="SalaryFilterTblLbl">Filter Salaries</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
        <table cellpadding="1px" cellspacing="0px" style="margin:2px 0px 2px 0px">
            <tr>
                <td style="width: 110px">
                    By Location:
                </td>
                <td style="width: 150px">
                    <asp:DropDownList ID="locationDropDown" runat="server" CssClass="SalaryFilterDropDownStyle">
                    </asp:DropDownList>
                </td>
                <td rowspan="2" valign="middle" align="center" style="vertical-align: middle; width:165px;">
                    <asp:Button ID="Btn_SalaryFilter" runat="server" Text="Filter" CssClass="btn_salaryFilterStyle" />
                </td>
            </tr>
            <tr>
                <td style="width: 110px">
                    By Experience:
                </td>
                <td style="width: 150px">
                    <asp:DropDownList ID="experienceDropDown" runat="server" CssClass="SalaryFilterDropDownStyle">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        </td>
    </tr>
</table>
<table cellspacing="0" cellpadding="0" class="MainCompanySalaryTbl" id="SalaryDataTable"
    runat="server">
    <tr>
        <td class="CompanySalaryPosHeaderColumn">
            Position
        </td>
        <td class="CompanySalaryDataHeaderColumn">
            <asp:Panel ID="xAxisPanel" runat="server" CssClass="xAxisPanelStyle" />
        </td>
    </tr>
</table>
