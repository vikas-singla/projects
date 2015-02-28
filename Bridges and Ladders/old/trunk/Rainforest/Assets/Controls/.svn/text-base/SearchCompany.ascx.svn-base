<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchCompany.ascx.cs"
    Inherits="Rainforest.Assets.Controls.SearchCompany" %>
<%@ Register Src="~/Assets/Controls/PaginationControl.ascx" TagName="PaginationControl"
    TagPrefix="Rainforest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table cellpadding="0px" cellspacing="0px" class="ArticleListPageTitleTable">
    <tr>
        <td class="ArticleListPageHeader">
            Companies
        </td>
    </tr>
</table>
<div style="float: left; width: 80%">
    <table width="100%">
        <tr>
            <td class="CompanySearchHeaderWithBackgroundColor" align="center" style="width: 55%;
                height: 40px">
                <asp:Label ID="Lbl_CompanySearch" runat="server" CssClass="headerlabelstyle">Search Company</asp:Label>
                <asp:TextBox ID="Txt_CompanyName" runat="server" Width="150px"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:DropDownList ID="Ddl_Location" runat="server" Width="150px">
                    <asp:ListItem>Alabama</asp:ListItem>
                    <asp:ListItem>Boston</asp:ListItem>
                    <asp:ListItem>New York</asp:ListItem>
                    <asp:ListItem>Santa Ana</asp:ListItem>
                    <asp:ListItem>Seattle</asp:ListItem>
                    <asp:ListItem>Toronto</asp:ListItem>
                    <asp:ListItem>Vancouver</asp:ListItem>
                    <asp:ListItem>Washington DC</asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="LSE" runat="server" TargetControlID="Ddl_Location" PromptCssClass="ListSearchExtenderPrompt"
                    IsSorted="True" PromptText="" />
                &nbsp;&nbsp;
                <asp:Button ID="Btn_SearchCompany" runat="server" Text="Search" />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <br />
            <td class="CompanySearchHeader" align="left" style="width: 55%;">
                Search Results
            </td>
        </tr>
    </table>
    <div>
        <table width="100%">
            <tr>
                <td rowspan="3">
                    <img src="/Assets/Images/CompanyLogos/Deloitte_Logo.png" alt="Deloitte Consulting Logo"
                        width="120px" />
                </td>
                <td align="left">
                    <a href="/pages/companies/profile/" class="CompanyTitle" title="The core of Deloitte's approach is the use of principles of applied behavioral economics
                    to optimize business performance">Deloitte Consulting</a>
                </td>
            </tr>
            <tr>
                <td class="CompanyShortSearchDescription" align="left">
                    The core of Deloitte's approach is the use of principles of applied behavioral economics
                    to optimize business performance
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" class="CompanyCategory">
                    <b>Firm Category:</b> <a>Niche Consulting Firm</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="CompanyListSeparator">
                </td>
            </tr>
            <tr>
                <td rowspan="3">
                    <img src="/Assets/Images/CompanyLogos/Huron_logo_master2.jpg" alt="Huron Consulting Group Logo"
                        width="120px" />
                </td>
                <td align="left">
                    <a href="/pages/companies/profile/" class="CompanyTitle" title="The core of Huron Consulting Group's approach is the use of principles of applied behavioral economics">
                        Huron Consulting Group</a>
                </td>
            </tr>
            <tr>
                <td class="CompanyShortSearchDescription" align="left">
                    The core of Huron Consulting Group's approach is the use of principles of applied
                    behavioral economics to optimize business performance
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" class="CompanyCategory">
                    <b>Firm Category:</b> <a>Niche Consulting Firm</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="CompanyListSeparator">
                </td>
            </tr>
            <tr>
                <td rowspan="3">
                    <img src="/Assets/Images/CompanyLogos/PWC-logo.jpg" alt="PWC Consulting Logo" width="120px" />
                </td>
                <td align="left">
                    <a href="/pages/companies/profile/" class="CompanyTitle" title="The core of PriceWaterhouseCoopers Management Consulting Services' approach is the use of principles of applied behavioral economics">
                        PriceWaterhouseCoopers Management Consulting Services</a>
                </td>
            </tr>
            <tr>
                <td class="CompanyShortSearchDescription" align="left">
                    The core of PriceWaterhouseCoopers Management Consulting Services' approach is the
                    use of principles of applied behavioral economics to optimize business performance
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" class="CompanyCategory">
                    <b>Firm Category:</b> <a>Big Four Consulting Firm</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="CompanyListSeparator">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <Rainforest:PaginationControl ID="paginationControl1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</div>
<div style="float: right; width: 20%">
    <table style="width: 100%; height: 40px;">
        <tr>
            <td class="SearchCompanyByCategory" style="height: 40px;">
                <asp:Label ID="Label1" runat="server" CssClass="headerlabelstyle">Show Me Companies</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:DropDownList ID="DDl_CompanyCategory" runat="server" Width="150px">
                    <asp:ListItem>By Revenue</asp:ListItem>
                    <asp:ListItem>By Size</asp:ListItem>
                    <asp:ListItem>By Reputation</asp:ListItem>
                    <asp:ListItem>By Global Reach</asp:ListItem>
                    <asp:ListItem>By Salary</asp:ListItem>
                </asp:DropDownList>
                <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDl_CompanyCategory"
                    PromptCssClass="ListSearchExtenderPrompt" IsSorted="True" PromptText="" />
            </td>
        </tr>
        <tr class="AlphabeticalListOfCompanies">
            <td align="left">
                <a>McKinsey & Co.</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Bain and Company</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Boston Consulting Group</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Monitor</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Arthur D. Little</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Booz Allen Hamilton</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>AT Kearney</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Oliver Wyman</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Towers Perrin</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Mitchell Madison Group</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Frost and Sullivan</a>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a>Dun and Bradstreet</a>
            </td>
        </tr>
    </table>
</div>
<table width="100%">
    <tr>
        <br />
        <td class="CompanySearchHeader" align="left" style="width: 40%;">
            Popular Consulting Firms
        </td>
    </tr>
</table>
<table style="width: 100%; padding: 5px;">
    <tr>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/Huron_logo_master2.jpg" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/logo_GallupConsulting.gif" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/CornerstoneResearch_Logo.gif" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/Deloitte_Logo.png" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/logo_alixpartners.gif" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <img src="/Assets/Images/CompanyLogos/Keane_Inc.gif" alt="<<<Display title for the compay logo>>>"
                width="125px" />
        </td>
    </tr>
    <tr>
        <td>
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/BearingPoint.gif" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/PWC-logo.jpg" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/AlvarezMarshal.gif" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/ibm_logo.jpg" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <a href="/pages/companies/profile/" class="LogoCompanyLinkStyle">
                <img src="/Assets/Images/CompanyLogos/kpmg_logo.gif" alt="<<<Display title for the compay logo>>>"
                    width="125px" />
            </a>
        </td>
        <td>
            <img src="/Assets/Images/CompanyLogos/ksa_logo.gif" alt="<<<Display title for the compay logo>>>"
                width="125px" />
        </td>
    </tr>
</table>
