<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLandingPageControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.UserLandingPageControl" %>
<table cellpadding="0px" cellspacing="0px" class="LandingPageContent">
    <tr valign="top">
        <td>
            <div class="LadingPageLeftNavMenuBg">
                <div class="LandingPageLeftNavContent">
                    <asp:Image runat="server" CssClass="ImgPersonClass" ImageUrl="/Assets/Images/person_svg.png" />
                    <asp:Label ID="Lbl_Username" runat="server" CssClass="LandingPageNavMenuUserLabel">User X</asp:Label>
                    <br />
                    <br />
                    <asp:Menu CssClass="menu" EnableViewState="false" IncludeStyleBlock="false" Orientation="Vertical"
                        ID="Menu_LandingPage_Home" runat="server" RenderingMode="List">
                        <Items>
                            <asp:MenuItem Text="Home" ImageUrl="/Assets/Images/Home.png" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                        </Items>
                    </asp:Menu>
                    <br />
                    <asp:Label runat="server" CssClass="LandingPageNavMenuHeaderLabels">ACTIONS:</asp:Label>
                    <asp:Menu CssClass="menu" EnableViewState="false" IncludeStyleBlock="false" Orientation="Vertical"
                        ID="Menu_LandingPage_Actions" runat="server" RenderingMode="List">
                        <Items>
                            <asp:MenuItem ImageUrl="/Assets/Images/chat.png" Text="Messages" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                            <asp:MenuItem ImageUrl="/Assets/Images/tip.png" Text="Interview Tips" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                            <asp:MenuItem ImageUrl="/Assets/Images/notification.png" Text="Notifications"
                                SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png"></asp:MenuItem>
                            <asp:MenuItem ImageUrl="/Assets/Images/link-partners-icon.png" Text="Groups"
                                SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png"></asp:MenuItem>
                            <asp:MenuItem ImageUrl="/Assets/Images/minimenu-info.png" Text="Answers"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                    <br />
                    <asp:Label ID="Label1" runat="server" CssClass="LandingPageNavMenuHeaderLabels">MY PROFILE:</asp:Label>
                    <asp:Menu CssClass="menu" EnableViewState="false" IncludeStyleBlock="false" Orientation="Vertical"
                        ID="Menu1" runat="server" RenderingMode="List">
                        <Items>
                            <asp:MenuItem Text="Personal Information" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                            <asp:MenuItem Text="Work Experience" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                            <asp:MenuItem Text="Skills and Expertise" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                            <asp:MenuItem Text="Achievements" SeparatorImageUrl="/Assets/Images/NavMenu_Seperator.png">
                            </asp:MenuItem>
                            <asp:MenuItem Text="Beyond the Paper"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </div>
            </div>
        </td>
        <td>
            <asp:UpdatePanel ID="UpdatePanel_LandingPg_Content" runat="server">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
