<%@ Page Title="Edit Company Details" Language="C#" MasterPageFile="~/RainForestAdmin.master" AutoEventWireup="true"
    CodeBehind="EditDetails.aspx.cs" Inherits="RainForestAdmin.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   <script type="text/javascript">
       function EditCompanyHeads() {

           window.open('../SupportPages/EditCompanyInfoLinkedPages/EditCompanyHeads.aspx', 'Edit Company Heads', 'width=400,height=200')
       }

       function EditCompanyLocations() {
           window.open('../SupportPages/EditCompanyInfoLinkedPages/EditLocationDetails.aspx', 'Edit Locations', 'width=950,height=500')
       }

       function EditCompanyLocationsPics() {
           window.open('../SupportPages/EditCompanyInfoLinkedPages/EditLocationPics.aspx', 'Edit Pics and Videos', 'width=700,height=300')
       }

       function EditCompensationDetails() {
           window.open('../SupportPages/EditCompanyInfoLinkedPages/EditCompensationDetails.aspx', 'Edit Growth Path and Compensation', 'width=700,height=300')
       }
</script>
 <div>
        <table>
            <tr>
                <td>
                    <h2>
                        Edit Company Details:
                        </h2>
                </td>
            </tr>
            <tr>
            <td>
            Select Company To Edit:
            </td>
            <td>
                <asp:DropDownList ID="ddl_CompanyNames" runat="server" 
                    DataValueField="CompanyId"  
                    DataTextField="CompanyName" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            </tr>
            </table>
            <asp:UpdatePanel ID="up1" runat="server" >
            <ContentTemplate>
            <table>
           <tr>
            <td>
            1. Enter Company Name:
            </td>
            <td class="style2">
            <asp:TextBox runat="server" ID="txt_CompanyName" Height="26px" Width="362px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txt_CompanyName" ErrorMessage="RequiredField" 
                    Font-Bold="True" Font-Italic="True" Font-Underline="True" ForeColor="#FF6600" 
                    SetFocusOnError="True">Company Name cannot be empty</asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
            <td>
            2. Upload Company Logo:
            </td>
            <td class="style2">
            <asp:FileUpload ID="fupload_CompanyLogo" runat="server" Height="26px" 
                    Width="454px" />
            </td>
            </tr>
            <tr>
            <td>
            3. Company Type(Select):
            </td>
            <td class="style2">
            <asp:DropDownList ID="ddl_CompanyType" runat="server" 
                    DataTextField="companyType" DataValueField="companyTypeId"></asp:DropDownList>
                
            </td>
            </tr>
            <tr>
            <td>
            4. Industry(Select):
            </td>
            <td class="style2">
            <asp:DropDownList ID="ddl_Industry" runat="server"  
                    DataTextField="IndustryName" DataValueField="IndustryId" > </asp:DropDownList>
                
            </td>
            </tr>
             <tr>
            <td>
            5. Company Origin(Select):
            </td>
            <td class="style2">
            <asp:DropDownList ID="ddl_CompanyOrigin" runat="server" 
                     DataTextField="origin" 
                    DataValueField="CompanyOriginId" > </asp:DropDownList>
               
            </td>
            </tr>
            <tr>
            <td>
            6. Overall Employees(worldwide):
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_NumberOfEmpOverall" runat="server" text="0"></asp:TextBox>
                <asp:RangeValidator ID="RangeValidator1" runat="server" 
                    ControlToValidate="txt_NumberOfEmpOverall" ErrorMessage="Out of Range" 
                    Font-Bold="True" Font-Italic="True" Font-Underline="True" ForeColor="#FF6666" 
                    MaximumValue="1000000" MinimumValue="1" Type="Integer">Value must be between 1 and 1000000</asp:RangeValidator>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txt_NumberOfEmpOverall" 
                    ErrorMessage="RequiredFieldValidator" Font-Bold="True" Font-Italic="True" 
                    Font-Underline="True" ForeColor="#FF6666">This field is mandatory</asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
            <td>
            7. Overall Employees(India):
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_NumOfEmpIndia" runat="server" text="0"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="txt_NumberOfEmpOverall" ControlToValidate="txt_NumOfEmpIndia" 
                    ErrorMessage="Must be less than overall employees" Font-Bold="True" 
                    Font-Italic="True" Font-Underline="True" ForeColor="#FF6666" 
                    SetFocusOnError="True" Type="Integer" Operator="LessThanEqual">Must be less than overall employees</asp:CompareValidator>
                <br />
            </td>
            </tr>
            <tr>
            <td>
            8. Company Heads:
            </td>
            <td class="style2">
                <asp:LinkButton ID="lnkbtn_CompanyHeads" runat="server" 
                    Text="Edit Company Heads" OnClientClick="EditCompanyHeads()" 
                     />
            </td>
            </tr>
            <tr>
            <td>
            9. Company URL:
            </td>
            <td class="style2">
                <asp:TextBox ID="txt_CompanyUrl" runat="server" Height="26px" Width="564px"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td>
            10. Company Contact Info:
            </td>
            <td class="style2">
                <asp:TextBox ID="txt_ContactInfo" runat="server" Height="26px" Width="564px"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td>
            11. Fortune 1000
            </td>
            <td class="style2">
            <asp:DropDownList ID="ddl_Fortune1000" runat="server" >
            <asp:ListItem Selected="True">No</asp:ListItem>
            <asp:ListItem>Yes</asp:ListItem>
            </asp:DropDownList>
            </td>
            </tr>
            <tr>
            <td>
            12. Indian Office Locations:
            </td>
            <td class="style2">
            <asp:LinkButton ID="lnkbtn_OfficeLocations" runat="server" Text="Edit Company Locations" OnClientClick="EditCompanyLocations()" />
            </td>
            </tr>
            <tr>
            <td>
            13. Pictures/Videos of Office Locations:
            </td>
            <td class="style2">
            <asp:LinkButton ID="lnkbtn_OfficeLocationPics" runat="server" Text="Edit Company Locations Pics and Videos" OnClientClick="EditCompanyLocationsPics()" />
            </td>
            </tr>
            <tr>
            <td>
            14. Study Material:
            </td>
            <td class="style2">
            <asp:FileUpload ID="fupload_material" runat="server" Height="18px" Width="433px"  />
            </td>
            </tr>
            <tr>
            <td>
            15. Work culture and environment:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_wrkculture" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td>
            16. Material For Interview Process:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_materialInterview" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td>
            17. Insider Scoop for the company:
            </td>
            <td class="style2">
            Pros:<asp:TextBox ID="txt_Pros" 
                    runat="server" TextMode="MultiLine" Width="245px" />
                Cons<asp:TextBox ID="txt_Cons" runat="server" TextMode="MultiLine" 
                    Width="261px" />
            </td>
            <td>
                :</td>
            </tr>
            <tr>
            <td>
            18. Day To Day Experience:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_d2dExperience" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td>
            19. Recruitment Process:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_RecruitmentProcess" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td class="style1">
            20. Growth Path / Corporate Ladder
            </td>
            <td class="style2">
            <asp:LinkButton ID="lnkbtn_growthpath" runat="server" Text="Edit Growth Path and Compensation Details" OnClientClick="EditCompensationDetails()" />
            </tr>
           
            <tr>
            <td>
            21. Other Benefits:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_Benefits" runat="server" TextMode="MultiLine" Width="565px" />
            </td>
            </tr>
            <tr>
            <td>
            <asp:Button ID="btn_Update" runat="server" Text="Update" 
                    Height="35px" Width="244px" onclick="btn_Update_Click" />
             
            </td>
            <td class="style2">
                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" 
                    Height="35px" Width="244px" />   
            </td>
            
            </tr>
            </table>
            </ContentTemplate>
            </asp:UpdatePanel>
            </div>
</asp:Content>

