<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RainForest.master"
    CodeBehind="Default.aspx.cs" Inherits="Rainforest.Pages.InsiderInfo.Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:content id="Content2" runat="server" contentplaceholderid="CphHTMLHeader">
    <link href="/Styles/InsiderInfo.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/UserNavDropDownMenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/PaginationControlStyle.css" rel="stylesheet" type="text/css" />
    <script src="/Assets/Javascript/navmenu.js" type="text/javascript"></script>
    <link href="/Styles/SearchControlStyle.css" rel="stylesheet" type="text/css" />    
</asp:content>
<asp:content id="HeaderContent" runat="server" contentplaceholderid="CphHeader">
    <asp:UpdatePanel ID="UpdatePanel_Header" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>
<asp:content id="Content1" runat="server" contentplaceholderid="Cph_Menu">
    <asp:UpdatePanel ID="UpdatePanel_Menu" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>
<asp:content id="BodyContent" runat="server" contentplaceholderid="Cph_Main">
    <asp:UpdatePanel ID="UpdatePanel_Body" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>
