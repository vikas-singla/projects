<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyLocations.ascx.cs"
    Inherits="Rainforest.Assets.Controls.CompanyLocations" %>
<%@ Register Src="~/Assets/Controls/CompanyLocationsDetails.ascx" TagName="CompanyLocationsDetails"
    TagPrefix="Rainforest" %>
<table cellpadding="0px" cellspacing="0px" class="CPD_HeaderTable">
    <tr>
        <td class="CPD_NavColumnStyle">
            <asp:HyperLink ID="link_companyprofilepg" runat="server" Text="Company Profiles"
                CssClass="link_cpdhierarchyNav"></asp:HyperLink>
            &nbsp;&raquo;&nbsp;
            <asp:HyperLink ID="HyperLink2" runat="server" Text="Deloitte" CssClass="link_cpdhierarchyNav"
                NavigateUrl="/Pages/Companies/Profile/"></asp:HyperLink>
            &nbsp;&raquo;&nbsp;
            <asp:Label ID="lbl_career" runat="server" Text="Company Location(s)" CssClass="lbl_cpdnamenav"></asp:Label>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td class="CP_SectionStyle">
            <div class="containerheader">
                <table cellpadding="0px" cellspacing="0px" class="headertablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_OfficeLocations" runat="server" CssClass="headerlabelstyle">Office Locations</asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="containerbody">
                <Rainforest:CompanyLocationsDetails ID="locationsControl1" runat="server" />
            </div>
            <div class="containerfooter">
                <asp:HyperLink ID="companyprofile_link6" runat="server" Text="View&nbsp;All&nbsp;&raquo;"
                    NavigateUrl="." CssClass="CPFooterMoreLinkStyle"></asp:HyperLink>
            </div>
        </td>
    </tr>
</table>
