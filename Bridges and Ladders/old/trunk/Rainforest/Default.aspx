<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/RainForest.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Rainforest._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="CphHeader">
    <asp:UpdatePanel ID="UpdatePanel_Header" runat="server">
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
