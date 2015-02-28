<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RainForest.master"
    CodeBehind="Default.aspx.cs" Inherits="Rainforest.Pages.Search.Default" %>
<%@ Register Src="~/Assets/Controls/PaginationControl.ascx" TagName="PaginationControl"
    TagPrefix="Rainforest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="CphHTMLHeader">
    <link href="/Styles/UserNavDropDownMenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/SearchCompany.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/PaginationControlStyle.css" rel="stylesheet" type="text/css" /> 
    <script src="/Assets/Javascript/navmenu.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="CphHeader">
    <asp:UpdatePanel ID="UpdatePanel_Header" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Cph_Menu">
    <asp:UpdatePanel ID="UpdatePanel_Menu" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="Cph_Main">
    <asp:UpdatePanel ID="UpdatePanel_Body" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
