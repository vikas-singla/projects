<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionThreadDetailControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.DiscussionThreadDetailControl" %>
<%@ Register Src="~/Assets/Controls/PaginationControl.ascx" TagName="PaginationControl"
    TagPrefix="Rainforest" %>
<table cellpadding="0px" cellspacing="0px" class="PageTitleTable">
    <tr>
        <td class="PageHeader">
            Discussions
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
<table cellpadding="0px" cellspacing="10px">
    <tr>
        <td class="DiscusionDetailsUserInfoColumnStyle">
            <div class="DiscusionDetailsUserInfoStyle">
                <table cellpadding="0px" cellspacing="5px">
                    <tr>
                        <td>
                            <img src="/Assets/Images/person_svg.png" width="40px" height="40px" />
                        </td>
                        <td>
                            <asp:HyperLink ID="UserLink1" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">John Doe</asp:HyperLink>
                            <br />
                            <asp:Label ID="Lbl_position1" runat="server">Analyst</asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <td class="DiscusionDetailsContentColumnStyle">
            <div class="DiscusionDetailsContentStyle">
                <div class="DiscussionDetailTopicLabelStyle">
                    <asp:Label ID="Topic1" runat="server">What is the best company to work for in Toronto?</asp:Label>
                </div>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                eu venenatis suscipit, quam eros pulvinar mi, id aliquet erat erat et enim. Pellentesque
                eget dapibus lectus. Ut dignissim facilisis pulvinar. Proin pretium tellus in purus
                ullamcorper placerat tempus dui iaculis. Cras pellentesque mauris eu turpis consequat
                dignissim. Proin lacinia semper lorem iaculis luctus. Cras sed orci massa. Nulla
                malesuada tincidunt elit ornare congue. Aenean ut gravida urna. Nulla sem dolor,
                cursus sit amet imperdiet sed, pulvinar imperdiet quam. Ut nec posuere justo. Sed
                cursus urna ultrices velit auctor dignissim.
            </div>
            <div class="DiscussionDetailsActionsBarStyle">
                <asp:HyperLink ID="Hyperlink2" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/pin.png" class="img_discussionthreadactionbar" />&nbsp;Reply</asp:HyperLink>
                <asp:Label ID="Label9" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                <asp:HyperLink ID="Hyperlink3" runat="server" CssClass="DiscussionThreadActionLinkStyle">
                <img src="/Assets/Images/email.png" class="img_discussionthreadactionbar" />&nbsp;Email</asp:HyperLink>
                <asp:Label ID="Label8" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                <asp:HyperLink ID="actionbutton1" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/cross_shield.png" class="img_discussionthreadactionbar" />&nbsp;Report Abuse</asp:HyperLink>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table cellpadding="0px" cellspacing="0px" class="PageTitleTable">
                <tr>
                    <td class="DiscussionDetailsPageRepliesHeader">
                        Replies
                    </td>
                </tr>
            </table>
            <table cellpadding="5px" cellspacing="0px" class="discussiondetailreplytablestyle">
                <tr>
                    <td class="replycolumn">
                        <img src="/Assets/Images/person_svg.png" width="40px" height="40px" />
                    </td>
                    <td class="contentcolumn">
                        <table cellpadding="5px" cellspacing="0px">
                            <tr>
                                <td class="discussiondetailreplybottomborderstyle">
                                    <asp:HyperLink ID="link1" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">Mary Jane</asp:HyperLink>
                                    <asp:Label ID="lbl1" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">:</asp:Label>
                                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                                    sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                                    nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                                    eu venenatis suscipit, quam eros pulvinar mi, id aliquet erat erat et enim. Pellentesque
                                </td>
                            </tr>
                            <tr>
                                <td class="DiscussionRepliesActionsBarStyle">
                                    <asp:HyperLink ID="Hyperlink1" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/pin.png" class="img_discussionthreadactionbar" />&nbsp;Reply</asp:HyperLink>
                                    <asp:Label ID="Label6" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                                    <asp:HyperLink ID="Hyperlink4" runat="server" CssClass="DiscussionThreadActionLinkStyle">
                <img src="/Assets/Images/email.png" class="img_discussionthreadactionbar" />&nbsp;Email</asp:HyperLink>
                                    <asp:Label ID="Label7" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                                    <asp:HyperLink ID="HyperLink5" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/cross_shield.png" class="img_discussionthreadactionbar" />&nbsp;Report Abuse</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table cellpadding="5px" cellspacing="0px" class="discussiondetailreplytablestyle">
                <tr>
                    <td class="replycolumn">
                        <img src="/Assets/Images/person_svg.png" width="40px" height="40px" />
                    </td>
                    <td class="contentcolumn">
                        <table cellpadding="5px" cellspacing="0px">
                            <tr>
                                <td class="discussiondetailreplybottomborderstyle">
                                    <asp:HyperLink ID="HyperLink6" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">Mary Jane</asp:HyperLink>
                                    <asp:Label ID="Label1" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">:</asp:Label>
                                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                                    sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                                    nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                                    eu venenatis suscipit, quam eros pulvinar mi, id aliquet erat erat et enim. Pellentesque
                                </td>
                            </tr>
                            <tr>
                                <td class="DiscussionRepliesActionsBarStyle">
                                    <asp:HyperLink ID="Hyperlink7" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/pin.png" class="img_discussionthreadactionbar" />&nbsp;Reply</asp:HyperLink>
                                    <asp:Label ID="Label4" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                                    <asp:HyperLink ID="Hyperlink8" runat="server" CssClass="DiscussionThreadActionLinkStyle">
                <img src="/Assets/Images/email.png" class="img_discussionthreadactionbar" />&nbsp;Email</asp:HyperLink>
                                    <asp:Label ID="Label5" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                                    <asp:HyperLink ID="HyperLink9" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/cross_shield.png" class="img_discussionthreadactionbar" />&nbsp;Report Abuse</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table cellpadding="5px" cellspacing="0px" class="discussiondetailreplytablestyle">
                <tr>
                    <td class="replycolumn">
                        <img src="/Assets/Images/person_svg.png" width="40px" height="40px" />
                    </td>
                    <td class="contentcolumn">
                        <table cellpadding="5px" cellspacing="0px">
                            <tr>
                                <td class="discussiondetailreplybottomborderstyle">
                                    <asp:HyperLink ID="HyperLink10" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">Mary Jane</asp:HyperLink>
                                    <asp:Label ID="Label2" runat="server" CssClass="DiscussionDetailUserNameLinkStyle">:</asp:Label>
                                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id diam nec sapien
                                    sagittis porttitor. Morbi at orci a diam malesuada volutpat nec eu eros. In velit
                                    nulla, hendrerit ac dictum sed, pretium sit amet felis. Curabitur convallis, ipsum
                                    eu venenatis suscipit, quam eros pulvinar mi, id aliquet erat erat et enim. Pellentesque
                                </td>
                            </tr>
                            <tr>
                                <td class="DiscussionRepliesActionsBarStyle">
                                    <asp:HyperLink ID="Hyperlink11" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/pin.png" class="img_discussionthreadactionbar" />&nbsp;Reply</asp:HyperLink>
                                    <asp:Label ID="Label3" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                                    <asp:HyperLink ID="Hyperlink12" runat="server" CssClass="DiscussionThreadActionLinkStyle">
                <img src="/Assets/Images/email.png" class="img_discussionthreadactionbar" />&nbsp;Email</asp:HyperLink>
                                    <asp:Label ID="Label10" runat="server" CssClass="lbl_discussionfieldvaluestyle"> | </asp:Label>
                                    <asp:HyperLink ID="HyperLink13" runat="server" CssClass="DiscussionThreadActionLinkStyle"><img src="/Assets/Images/cross_shield.png" class="img_discussionthreadactionbar" />&nbsp;Report Abuse</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<Rainforest:PaginationControl ID="paginationControl1" runat="server" />
