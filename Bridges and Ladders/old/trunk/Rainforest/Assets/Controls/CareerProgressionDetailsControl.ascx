<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CareerProgressionDetailsControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.CareerProgressionDetailsControl" %>
<%@ Register Src="~/Assets/Controls/CareerProgressionControl.ascx" TagName="CareerProgressionControl"
    TagPrefix="Rainforest" %>
<%@ Register Src="~/Assets/Controls/SalaryDetailControl.ascx" TagName="SalaryDetailControl"
    TagPrefix="Rainforest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table cellpadding="0" cellspacing="0" class="PageNavTable">
    <tr>
        <td class="NavColumnStyle">
            <asp:HyperLink ID="link_companyprofilepg" runat="server" Text="Companies" CssClass="link_hierarchyNav"
                NavigateUrl="/pages/companies/"></asp:HyperLink>
            &nbsp;&raquo;&nbsp;
            <asp:HyperLink ID="HyperLink2" runat="server" Text="Deloitte" CssClass="link_hierarchyNav"
                NavigateUrl="/Pages/Companies/Profile/"></asp:HyperLink>
            &nbsp;&raquo;&nbsp;
            <asp:Label ID="lbl_career" runat="server" Text="Deloitte Career" CssClass="lbl_hierarchyPageNavName"></asp:Label>
        </td>
    </tr>
</table>
<table cellpadding="0px" cellspacing="0px" class="PageTitleTable">
    <tr>
        <td class="PageHeader">
            Deloitte Career
        </td>
        <td class="ShareItColumnStyle" align="right">
            <div class="addthis_default_style" style="float:right;">
                <a class="addthis_button_facebook"></a>
                <a class="addthis_button_twitter"></a>
                <a class="addthis_button_email"></a>
                <a class="addthis_button_google"></a>
                <a class="addthis_button_compact"></a>
            </div>
        </td>
    </tr>
</table>
<table cellpadding="0px" cellspacing="0px" class="CPD_ContentTable">
    <tr>
        <td class="CPD_ProgressionControlColumnStyle">
            <Rainforest:CareerProgressionControl ID="SelectiveCareerProgressionControl" runat="server" />
        </td>
        <td class="CPD_ContentColumnStyle">
            <table cellspacing="0px" cellpadding="0px" style="width: 100%; padding-bottom: 2px;">
                <tr>
                    <td class="CPD_PositionHeaderEmptyLeftCol">
                    </td>
                    <td class="CPD_PositionHeaderLeftCol">
                    </td>
                    <td class="CPD_PositionHeaderMiddleCol">
                        <asp:Literal ID="HeaderCareerNameLbl" runat="server">Consultant</asp:Literal>
                    </td>
                    <td class="CPD_PositionHeaderRightCol">
                    </td>
                    <td class="CPD_PositionHeaderEmptyCol">
                    </td>
                </tr>
            </table>
            <table cellpadding="0px" cellspacing="0px" style="width: 100%">
                <tr>
                    <td class="CPD_SectionStyle">
                        <div class="CPD_ContainerHeader">
                            <asp:Label ID="Label15" runat="server" CssClass="CPD_Headerlabelstyle">Overview</asp:Label>
                        </div>
                        <div class="CPD_Containerbody">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                            sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                            nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                            eu venenatis suscipit, quam eros pulvinar mi, id aliquet erat erat et enim. Pellentesque
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="CPD_SectionStyle">
                        <div class="CPD_ContainerHeader">
                            <asp:Label ID="Label4" runat="server" CssClass="CPD_Headerlabelstyle">Salary</asp:Label>
                        </div>
                        <div class="CPD_ContainerBody">
                            <Rainforest:SalaryDetailControl ID="SalaryDetailControlInst" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="CPD_SectionStyle">
                        <div class="CPD_ContainerHeader">
                            <asp:Label ID="Label1" runat="server" CssClass="CPD_Headerlabelstyle">Key Responsibilities</asp:Label>
                        </div>
                        <div class="CPD_Containerbody">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                            sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                            nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="CPD_SectionStyle">
                        <div class="CPD_ContainerHeader">
                            <asp:Label ID="Label2" runat="server" CssClass="CPD_Headerlabelstyle">Typical Qualifications</asp:Label>
                        </div>
                        <div class="CPD_Containerbody">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                            sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                            nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="CPD_SectionStyle">
                        <div class="CPD_ContainerHeader">
                            <asp:Label ID="Label3" runat="server" CssClass="CPD_Headerlabelstyle">Competencies</asp:Label>
                        </div>
                        <div class="CPD_Containerbody">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                            sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                            nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table width="100%" class="RelatedCPD_Content">
    <tr>
        <td class="Related">
            Related
        </td>
    </tr>
</table>
<table cellpadding="0px" cellspacing="0px" width="100%">
    <tr>
        <td class="RelatedCPDSectionLeftStyle" align="center">
            <div class="RelatedContainerHeader">
                <asp:Label ID="Label5" runat="server" CssClass="RelatedHeaderlabelstyle">Related Positions</asp:Label>
            </div>
            <div class="RelatedContainerbody">
                <table cellpadding="0px" cellspacing="0px" width="100%">
                    <tr>
                        <td>
                            <div class="RelatedPositionsCompanyHeader">
                                KPMG &nbsp;<a>(View Profile)</a>
                            </div>
                            <div class="RelatedPositionsListStyle">
                                <ul>
                                    <li><a>Business Analyst</a></li>
                                    <li><a>Analyst</a></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellpadding="0px" cellspacing="0px" width="100%">
                    <tr>
                        <td class="RelatedPositionsCompanyHeader">
                            McKinsey & Company &nbsp;<a>(View Profile)</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="RelatedPositionsListStyle">
                                <ul>
                                    <li><a>Associate</a></li>
                                    <li><a>Business Analyst</a></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
            <div class="RelatedContainerFooter">
                <asp:HyperLink ID="companyprofile_link7" runat="server" Text="View More Related Positions..."
                    CssClass="RelatedMoreLinkStyle"></asp:HyperLink>
            </div>
        </td>
        <td class="RelatedCPDSectionRightStyle" align="center">
            <div class="RelatedContainerHeader">
                <asp:Label ID="Label6" runat="server" CssClass="RelatedHeaderlabelstyle">Interview Tips</asp:Label>
            </div>
            <div class="RelatedContainerbody">
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
            <div class="RelatedContainerFooter">
                <asp:HyperLink ID="HyperLink1" runat="server" Text="View More Interview Tips..."
                    CssClass="RelatedMoreLinkStyle"></asp:HyperLink>
            </div>
        </td>
    </tr>
</table>
