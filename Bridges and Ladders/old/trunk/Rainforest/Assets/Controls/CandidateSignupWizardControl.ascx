<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CandidateSignupWizardControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.CandidateSignupWizard" %>

<resource:CssInclude StylesheetUrl="/Styles/CandidateSignupWizardControl.css" runat="server" />
<div>
    <table>
        <tr>
            <td align="right">
                <asp:ImageButton ID="ImgButton_GeneralDetails" runat="server" ImageUrl="~/Assets/Images/kxkb.png"
                    OnClick="ImgButton_GeneralDetails_Click" />
            </td>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:HyperLink ID="Hyp_Step1" runat="server" CssClass="lbl_Step" PostBackUrl="./">Step 1</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Label ID="Lbl_Step1Details" runat="server" Text="Personal Information" CssClass="lbl_StepDetail"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <asp:ImageButton ID="ImgButton_QualificationAndExperience" runat="server" ImageUrl="~/Assets/Images/Experience Icon.png"
                    OnClick="ImgButton_QualificationAndExperience_Click" />
            </td>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:HyperLink ID="Hyp_Step2" runat="server" Text="Step 2" CssClass="lbl_Step" PostBackUrl="./"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Label ID="Lbl_Step2Details" runat="server" Text="Work Preferences" CssClass="lbl_StepDetail"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <asp:ImageButton ID="ImgButton_IntellectualProperty" runat="server" ImageUrl="~/Assets/Images/mydocuments.png"
                    OnClick="ImgButton_IntellectualProperty_Click" />
            </td>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:HyperLink ID="Hyp_Step3" runat="server" Text="Step 3" CssClass="lbl_Step" PostBackUrl="./"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Label ID="Lbl_Step3Details" runat="server" Text="Skills and Expertise" CssClass="lbl_StepDetail"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <asp:ImageButton ID="ImgButton_VideoOrAudioPitch" runat="server" ImageUrl="~/Assets/Images/cam_unmount.png"
                    OnClick="ImgButton_VideoOrAudioPitch_Click" />
            </td>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:HyperLink ID="Hyp_Step4" runat="server" Text="Step 4" CssClass="lbl_Step" PostBackUrl="./"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Label ID="Lbl_Step4Details" runat="server" Text="Beyond Your Resume" CssClass="lbl_StepDetail"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <asp:ImageButton ID="ImgButton_OtherCandidateInfo" runat="server" ImageUrl="~/Assets/Images/Globe2.png"
                    OnClick="ImgButton_OtherCandidateInfo_Click" />
            </td>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:HyperLink ID="Hyp_Step5" runat="server" Text="Step 5" CssClass="lbl_Step" PostBackUrl="./"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Label ID="Lbl_Step5Details" runat="server" Text="Upload Resume" CssClass="lbl_StepDetail"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<br />
<div id="step1" runat="server">
    <table style="text-align: left">
        <tr>
            <td>
                <asp:Label ID="Lbl_CurrentProfDetails" runat="server" Text="Contact Details" CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl_MailingAddress" runat="server" Text="Mailing Address*" CssClass="lbl_Labels"
                    Width="100px"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txt_MailingAddress" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
            </td>
            <td>
                <table>
                    <tr>
                        <td width="100px">
                            <asp:Label ID="Lbl_City" runat="server" Text="City*" CssClass="lbl_Labels"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_City" runat="server" Width="207px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_State" runat="server" Text="State*" CssClass="lbl_Labels"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Ddl_State" runat="server" Width="207px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl_MobilePhone" runat="server" Text="Mobile Phone" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txt_MobilePhone" runat="server" TextMode="SingleLine" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Llb_LandLine" runat="server" Text="LandLine Number" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txt_Landlinenumber" runat="server" TextMode="SingleLine" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Lbl_MandatoryMessage" runat="server" Text=" <b>Note:</b> At least one contact number is mandatory"
                    CssClass="lbl_SectionSubLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl_WorkDetails" runat="server" Text="Work Details" CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl" runat="server" Text="Are you currently employed?*" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:RadioButtonList ID="RadioBtnList_YesNo" runat="server" CssClass="lbl_Labels"
                    RepeatColumns="2">
                    <asp:ListItem Selected="True">Yes</asp:ListItem>
                    <asp:ListItem>No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl_Company" runat="server" Text="Company*" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_CompanyName" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
            <td rowspan="2">
                <table>
                    <tr>
                        <td width="100px">
                            <asp:Label ID="Lbl_OtherCompany" runat="server" Text="Other" CssClass="lbl_Labels"></asp:Label>
                            <td>
                                <asp:TextBox ID="Txt_OtherCompany" runat="server" TextMode="SingleLine" Width="200px"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Salary" runat="server" Text="Salary*" CssClass="lbl_Labels"></asp:Label>
                            <td>
                                <asp:DropDownList ID="Ddl_Salary" runat="server" Width="207px">
                                </asp:DropDownList>
                            </td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl_Position" runat="server" Text="Position*" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txt_Position" runat="server" TextMode="SingleLine" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Lbl_EducationDetails" runat="server" Text="Education Details" CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:GridView ID="Gridview_EducationalQuals" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_Delete" runat="server" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Education Institution" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_Institute" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Degree" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_Degree" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_StartDate" runat="server"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="Btn_Calendar_StartDate" ImageUrl="~/Assets/Images/Calendar_scheduleHS.png" />
                                <asp:CalendarExtender ID="StartDate_Calendar" TargetControlID="Txt_StartDate" runat="server"
                                    Format="MMMM d, yyyy" PopupButtonID="Btn_Calendar_StartDate">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Date" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:TextBox ID="Txt_EndDate" runat="server"></asp:TextBox>
                                <asp:ImageButton runat="Server" ID="Btn_Calendar_EndDate" ImageUrl="~/Assets/Images/Calendar_scheduleHS.png" />
                                <asp:CalendarExtender ID="EndDate_Calendar" TargetControlID="Txt_EndDate" runat="server"
                                    Format="MMMM d, yyyy" PopupButtonID="Btn_Calendar_EndDate">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="Btn_AddMoreQualification" runat="server" Text="Add More" CssClass="AddMoreLinkStyle"
                                    OnClick="Btn_AddMoreQualification_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
<div id="step2" runat="server">
    <table>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_GeneralPreferences" runat="server" Text="Company Related Preferences"
                    CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_KindOfCompany" runat="server" Text="What kind of Company do you want to work with?"
                    CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_CompanyKind" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_WhyThatKindOfCompany" runat="server" Text="Can you please help us better understand why you made that selection?"
                    CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="Txt_ReasonForSelectingKindOfCompany" runat="server" TextMode="MultiLine"
                    Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_IndustryPreferences" runat="server" Text="What kind of industries do you want to work in?"
                    CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_FirstPreference" runat="server" Text="First Preference" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_FirstPreference" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_SecondPreference" runat="server" Text="Second Preference" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_SecondPreference" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_ThirdPreference" runat="server" Text="Third Preference" CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_ThirdPreference" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_TravelPreferences" runat="server" Text="Travel Related Preferences"
                    CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_WillingnessToTravel" runat="server" Text="Are you willing to travel on the job?"
                    CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_TravelingPreferences" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_WorkFlexibility" runat="server" Text="Expected Flexibility at Work"
                    CssClass="lbl_SectionLabels"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="Lbl_Flexibility" runat="server" Text="What kind of work flexibility do you expect?"
                    CssClass="lbl_Labels"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="Ddl_WorkFlexibility" runat="server" Width="207px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
<div id="step3" runat="server">
    <table>
        <tr>
            <td>
                <asp:GridView ID="Gridview_TechSkills" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_Delete" runat="server" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Skill(s)" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:DropDownList ID="Ddl_TechSkills" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Number of Years Worked" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:DropDownList ID="Ddl_NoOfYearsWOrked" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Proficiency Level" HeaderStyle-CssClass="TableHeaders">
                            <ItemTemplate>
                                <asp:DropDownList ID="Ddl_ProficiencyLevel" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="Btn_AddMoreTechSkills" runat="server" Text="Add More" CssClass="AddMoreLinkStyle" OnClick="Btn_AddMoreTechSkills_Click"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
