<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserNavDropDownMenuControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.UserNavDropDownMenuControl" %>
<%@ Register Src="~/Assets/Controls/SearchControl.ascx" TagName="SearchControl" TagPrefix="Rainforest" %>
<div class="div_centertile">
    <table cellpadding="0" cellspacing="0" class="table_menucontent">
        <tr>
            <td>
                <div class="div_lefttile">
                </div>
            </td>
            <td class="column_menucontent">
                <ul id="qm0" class="qmmc">
                    <li><a class="qmparent" href="/">&nbsp;&nbsp;&nbsp;&nbsp;HOME&nbsp;&nbsp;&nbsp;&nbsp;</a>
                    </li>
                    <li><span class="qmdivider qmdividery"></span></li>
                    <li><a class="qmparent" href="/pages/companies/">&nbsp;&nbsp;&nbsp;&nbsp;Companies&nbsp;&nbsp;&nbsp;&nbsp;</a>                        
                    </li>
                    <li><span class="qmdivider qmdividery"></span></li>
                    <li><a class="qmparent" href="javascript:void(0);">&nbsp;&nbsp;&nbsp;&nbsp;Interview Tips&nbsp;&nbsp;&nbsp;&nbsp;</a>                        
                    </li>
                    <li><span class="qmdivider qmdividery"></span></li>
                    <li><a class="qmparent" href="/pages/discussion/">&nbsp;&nbsp;&nbsp;&nbsp;Discussions&nbsp;&nbsp;&nbsp;&nbsp;</a>                        
                    </li>
                    <li><span class="qmdivider qmdividery"></span></li>
                    <li><a class="qmparent" href="/pages/article/">&nbsp;&nbsp;&nbsp;&nbsp;Articles&nbsp;&nbsp;&nbsp;&nbsp;</a>                        
                    </li>
                    <li class="qmclear">&nbsp;</li></ul>
            </td>
            <td class="column_menusearchcontent">
                <Rainforest:SearchControl ID="SearchMenuControl" runat="server" />
            </td>
            <td>
                <div class="div_righttile">
                </div>
            </td>
        </tr>
    </table>
</div>
<!-- Create Menu Settings: (Menu ID, Is Vertical, Show Timer, Hide Timer, On Click ('all' or 'lev2'), Right to Left, Horizontal Subs, Flush Left, Flush Top) -->
<script type="text/javascript">    qm_create(0, false, 0, 250, false, false, false, false, false);</script>
