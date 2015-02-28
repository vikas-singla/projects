<%@ Page Title="" Language="C#" MasterPageFile="~/RainForestAdmin.Master" AutoEventWireup="true" CodeBehind="BasicCompanyInfo.aspx.cs" Inherits="RainForestAdmin.BasicCompanyInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            height: 21px;
            width: 286px;
        }
        .style2
        {
            width: 569px;
        }
        .style3
        {
            height: 21px;
            width: 569px;
        }
        .style4
        {
            height: 30px;
        }
        .style5
        {
            width: 569px;
            height: 30px;
        }
        .style6
        {
            height: 27px;
            width: 286px;
        }
        .style7
        {
            width: 569px;
            height: 27px;
        }
        .style8
        {
            width: 569px;
        }
        .style9
        {
            height: 30px;
            width: 286px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    function AddCompanyHeads()
     {

         window.open('../SupportPages/AddCompanyInfoLinkedPages/AddCompanyHeads.aspx', 'Add Company Heads', 'width=400,height=500')
    }

    function AddCompanyLocations() {
        window.open('../SupportPages/AddCompanyInfoLinkedPages/CompanyLocation.aspx', 'Add Locations', 'width=950,height=500')
    }

    function AddCompanyLocationsPics() {
        window.open('../SupportPages/AddCompanyInfoLinkedPages/AddCompanyPicturesVideos.aspx', 'Add Pics and Videos', 'width=1200,height=300')
    }
</script>



 <div>
 <asp:UpdatePanel ID="up1" runat="server" >
 <ContentTemplate>
        <table>
            
                    <h2>
                        Enter Basic Company Details:
                    </h2>
                
            <tr>
            <td class="style8">
            1. Enter Company Name:
            </td>
            <td class="style2">
            <asp:TextBox runat="server" ID="txt_CompanyName" Height="26px" Width="580px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txt_CompanyName" ErrorMessage="RequiredField" 
                    Font-Bold="True" Font-Italic="True" Font-Underline="True" ForeColor="#FF6600" 
                    SetFocusOnError="True">Company Name cannot be empty</asp:RequiredFieldValidator>
            </td>
            </tr>
            <tr>
            <td class="style8">
            2. Upload Company Logo:
            </td>
            <td class="style2">
            <asp:FileUpload ID="fupload_CompanyLogo" runat="server" Height="26px" 
                    Width="454px" />
            </td>
            </tr>
            <tr>
            <td class="style9">
            3. Company Type(Select):
            </td>
            <td class="style5">
            <asp:DropDownList ID="ddl_CompanyType" runat="server" 
                    DataTextField="companyType" DataValueField="companyTypeId" Height="22px" 
                    Width="219px"></asp:DropDownList>
                
            </td>
            </tr>
            <tr>
            <td class="style6">
            4. Industry(Select):
            </td>
            <td class="style7">
            <asp:DropDownList ID="ddl_Industry" runat="server"  
                    DataTextField="IndustryName" DataValueField="IndustryId" Height="22px" 
                    Width="219px" > </asp:DropDownList>
                
            </td>
            </tr>
             <tr>
            <td class="style9">
            5. Company Origin(Select):
            </td>
            <td class="style5">
            <asp:DropDownList ID="ddl_CompanyOrigin" runat="server" 
                     DataTextField="origin" 
                    DataValueField="CompanyOriginId" Height="21px" Width="218px" > </asp:DropDownList>
            </td>
            </tr>
            <tr>
            <td class="style8">
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
            <td class="style8">
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
            <td class="style8">
            8. Company Heads:
            </td>
            <td class="style2">
                <asp:LinkButton ID="lnkbtn_CompanyHeads" runat="server" 
                    Text="Add Company Heads" OnClientClick="AddCompanyHeads()"
                     />
                <asp:GridView ID="gv_heads" runat="server" BackColor="LightGoldenrodYellow" 
                    BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
                    GridLines="None" Width="562px">
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>
            </td>
            </tr>
            <tr>
            <td class="style8">
            9. Company URL:
            </td>
            <td class="style2">
                <asp:TextBox ID="txt_CompanyUrl" runat="server" Height="26px" Width="564px"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td class="style8">
            10. Company Contact Info:
            </td>
            <td class="style2">
                <asp:TextBox ID="txt_ContactInfo" runat="server" Height="26px" Width="564px"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td class="style8">
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
            <td class="style8">
            12. Indian Office Locations:
            </td>
            <td class="style2">
            <asp:LinkButton ID="lnkbtn_OfficeLocations" runat="server" Text="Add Company Locations" OnClientClick="AddCompanyLocations()"  />
                <asp:GridView ID="gv_Locations" runat="server" BackColor="LightGoldenrodYellow"
                    BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
                    GridLines="None" Width="563px">
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>
            </td>
            </tr>
            <tr>
            <td class="style8">
            13. Pictures/Videos of Office Locations:
            </td>
            <td class="style2">
            <asp:LinkButton ID="lnkbtn_OfficeLocationPics" runat="server" Text="Add Company Locations Pics and Videos" OnClientClick="AddCompanyLocationsPics()" />
                <asp:GridView ID="gv_picsvideos" runat="server" BackColor="LightGoldenrodYellow" 
                    BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
                    GridLines="None" Width="562px">
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>
            </td>
            </tr>
            <tr>
            <td class="style8">
            14. Study Material:
            </td>
            <td class="style2">
            <asp:FileUpload ID="fupload_material" runat="server" Height="26px" Width="433px"  />
            </td>
            </tr>
            <tr>
            <td class="style8">
            15. Work culture and environment:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_wrkculture" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td class="style8">
            16. Material For Interview Process:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_materialInterview" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td class="style8">
            17. Insider Scoop for the company:
            </td>
            <td class="style2">
            Pros:<asp:TextBox ID="txt_Pros" 
                    runat="server" TextMode="MultiLine" Width="535px" />
                <br />
                Cons<asp:TextBox ID="txt_Cons" runat="server" TextMode="MultiLine" 
                    Width="534px" />
            </td>
            <td>
                :</td>
            </tr>
            <tr>
            <td class="style8">
            18. Day To Day Experience:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_d2dExperience" runat="server" TextMode="MultiLine" 
                    Width="564px" />
            </td>
            </tr>
            <tr>
            <td class="style8">
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
            <td class="style3">
                    <asp:DropDownList ID="ddl_PositionGrowthPath" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                    </asp:DropDownList>
                    Position:<asp:TextBox ID="txt_PositionName" runat="server">EnterPositionName</asp:TextBox>
                    Compensation:<asp:TextBox ID="txt_Compensation" runat="server" 
                        ontextchanged="txt_Compensation_TextChanged">1000</asp:TextBox>
                    <asp:Button ID="btnAddGrowthPath" runat="server" Text="Add" 
                        onclick="btnAddGrowthPath_Click" />
                    <br />
                    <asp:RangeValidator ID="RangeValidator2" runat="server" 
                        ControlToValidate="txt_Compensation" 
                        ErrorMessage="Compensation Value should be between 1000 and 100000000" 
                        Font-Bold="True" Font-Italic="True" ForeColor="#FF6666" 
                        MaximumValue="100000000" MinimumValue="1000" Type="Double"></asp:RangeValidator>
                    <asp:GridView ID="GridView1" runat="server" BackColor="LightGoldenrodYellow" 
                        BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
                        GridLines="None" Width="562px">
                        <AlternatingRowStyle BackColor="PaleGoldenrod" />
                        <FooterStyle BackColor="Tan" />
                        <HeaderStyle BackColor="Tan" Font-Bold="True" />
                        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                            HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                        <SortedAscendingCellStyle BackColor="#FAFAE7" />
                        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                        <SortedDescendingCellStyle BackColor="#E1DB9C" />
                        <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                    </asp:GridView>
            </td>
            </tr>
           
            <tr>
            <td class="style8">
            21. Other Benefits:
            </td>
            <td class="style2">
            <asp:TextBox ID="txt_Benefits" runat="server" TextMode="MultiLine" Width="565px" />
            </td>
            </tr>
            <tr>
            <td class="style8">
            <asp:Button ID="btn_Save" runat="server" Text="Save" 
                    Height="35px" Width="244px" onclick="btn_Save_Click" />
             
            </td>
            <td class="style2">
                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" 
                    Height="35px" Width="244px" />   
            </td>
            
            </tr>
            </table>
            </ContentTemplate></asp:UpdatePanel>
            </div>
</asp:Content>
