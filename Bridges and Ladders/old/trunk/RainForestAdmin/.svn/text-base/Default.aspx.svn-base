<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/RainForestAdmin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RainForestAdmin._Default" %>
   


<%@ Register src="UserControls/AddPicturesForComapnyOffices.ascx" tagname="AddPicturesForComapnyOffices" tagprefix="uc2" %>
   

<%@ Register src="UserControls/AddressBar.ascx" tagname="AddressBar" tagprefix="uc3" %>
   

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style2
        {
            text-align: center;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table>
            <tr>
                <td>
                    <h2>
                        Enter Basic Company Details:
                        </h2>
                </td>
            </tr>
            <tr>
                <td>
                    1. Upload company Logo: <asp:FileUpload ID="Fupload_CompanyLogo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    2. Company Registered Name: 
                    <asp:TextBox ID="txt_CompanyName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    3. Company Type [(Public etc) <asp:DropDownList ID="Ddl_CompanyType" 
                        runat="server" DataSourceID="SqlDataSource1" DataTextField="companyType" 
                        DataValueField="companyType"></asp:DropDownList> 
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:rainforest_dbConnectionString %>" 
                        SelectCommand="SELECT DISTINCT [companyType] FROM [TBL_Company_Type]">
                    </asp:SqlDataSource>
                    &nbsp;AND (Foreign MNC, Indian etc)]: <asp:DropDownList ID="Ddl_CompanyOrigin" 
                        runat="server" DataSourceID="SqlDataSource3" DataTextField="origin" 
                        DataValueField="origin"></asp:DropDownList>
                     <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:rainforest_dbConnectionString %>" 
                        SelectCommand="SELECT DISTINCT [origin] FROM [TBL_Company_Origin]">
                    </asp:SqlDataSource>
                     </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    4. Industry (e.g.- Software, IT, Media etc):&nbsp;<asp:DropDownList 
                        ID="ddl_Industry" runat="server" DataSourceID="SqlDataSource2" 
                        DataTextField="IndustryName" DataValueField="IndustryName">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:rainforest_dbConnectionString %>" 
                        SelectCommand="SELECT DISTINCT [IndustryName] FROM [TBL_Industry]">
                    </asp:SqlDataSource>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    5. Number of employees Overall (worldwide):&nbsp;<asp:TextBox 
                        ID="txt_NumberOfEmpOverall" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    6. Number of people in the Indian Operations (if MNC):&nbsp;<asp:TextBox 
                        ID="txt_NumberOfEmpInIndia" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    7. Key people in Indian Operations (Example: Mr X (Country Manager), Mr Y (CFO) 
                    etc):
                    <uc1:AddCompanyHeads ID="AddCompanyHeads1" runat="server" 
                        EnableViewState="True" />
                </td>
            </tr>
            <tr>
                <td>
                    8. Company URL
                    <asp:TextBox ID="txt_CompanyUrl" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    9. Contact Info
                    <asp:TextBox ID="txt_ContactInfo" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
             <tr>
                <td>
                    10. Fortune 1000
                    <asp:DropDownList ID="ddl_Fortune1000" runat="server" >
                        <asp:ListItem Selected="True">No</asp:ListItem>
                        <asp:ListItem>Yes</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <h2>
                        Enter Information that gives a view of the company in India:
                    </h2>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    1. Indian Office Locations:
                    <br />
                    <uc3:AddressBar ID="AddressBar1" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    2. Pictures of Indian Office Locations:<br />
                    <uc2:AddPicturesForComapnyOffices ID="AddPicturesForComapnyOffices1" 
                        runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    3. Videos about the company:
                    <asp:FileUpload ID="fUpload_Videos" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    4. Materials to help candidate prepare for the interview process:
                    <asp:TextBox ID="txt_material" runat="server">
                    </asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    5. Work culture and environment:
                    <asp:TextBox ID="txt_WorkCulture" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    6. An Insider's Scoop about the company, if it exists (good, bad, ugly):
                    <br />
                    Pros:
                    <asp:TextBox ID="txt_Pros" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    Cons:<asp:TextBox ID="txt_Cons" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    7. Provide a sense of day-to-day experience
                    <asp:TextBox ID="txt_dailyexperience" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    8. Recruitment Process
                    <asp:TextBox ID="txt_recruitmentProcess" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
             <tr>
                <td>
                    9. Growth Path / Corporate Ladder
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
                    <asp:TextBox ID="txt_PositionName" runat="server">EnterPositionName</asp:TextBox>
                    <asp:Button ID="btnAddGrowthPath" runat="server" Text="Add" 
                        onclick="btnAddGrowthPath_Click" />
                    </td>
                <td>
                </td>
            </tr>
            
            <tr>
                <td>
                    <h2>
                        Enter Compensation Related Details:
                    </h2>
                </td>
            </tr>
            <tr>
                <td>
                    1. Position and corresponding Compensation:&nbsp;<asp:DropDownList 
                        ID="ddl_PositionCompensation" runat="server">
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
                    <asp:TextBox ID="txt_Compensation" runat="server">Enter Compensation</asp:TextBox>
                    <asp:Button ID="btnAddCompensation" runat="server" Text="Add" 
                        onclick="btnAddCompensation_Click" />
                    </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    3. Other Benefits(Leaves/Sabaticals etc):
                    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
            <td class="style2">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Btn_SaveCompanyInfo" Text="Save" runat="server" Height="25px" 
                    onclick="Btn_SaveCompanyInfo_Click" Width="100px" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_Undo" runat="server" Height="25px" Text="Undo" 
                    Width="100px" onclick="btn_Undo_Click" />
            </td>
            </tr>
        </table>
        </div>

</asp:Content>
