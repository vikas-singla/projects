<%@ Page Title="" Language="C#" MasterPageFile="~/RainForestAdmin.Master" AutoEventWireup="true" CodeBehind="CompanyInfo.aspx.cs" Inherits="RainForestAdmin.AdminPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
           <asp:GridView ID="gv_company" width="100%" runat="server" AutoGenerateSelectButton="true" AutoGenerateColumns="False" 
           DataKeyNames="CompanyId,companyType,origin,IndustryName" SelectedRowStyle-BackColor="YellowGreen" 
           OnRowCancelingEdit="gvcompany_RowCancelingEdit" OnRowDataBound="gvcompany_RowDataBound" 
           OnRowEditing="gvcompany_RowEditing" OnRowUpdating="gvcompany_RowUpdating"
            OnRowCommand="gvcompany_RowCommand" ShowFooter="True" OnRowDeleting="gvcompany_RowDeleting" > 
<Columns>
<asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" /> 
<asp:TemplateField HeaderText="Edit" ShowHeader="False"> 
<EditItemTemplate> 
  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton> 
</FooterTemplate> 
<ItemTemplate> 
  <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
</ItemTemplate> 
</asp:TemplateField>
<asp:BoundField HeaderText="CompanyId" runat="server" DataField="CompanyId" />

<asp:TemplateField HeaderText="Company Name" SortExpression="CompanyName"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_CompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewCompany" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_CompanyName" runat="server" Text='<%# Bind("CompanyName") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Company Logo">
<EditItemTemplate> 
  <asp:TextBox ID="txt_LogoPath" runat="server" Text='<%# Eval("CompanyLogo") %>'></asp:TextBox>
</EditItemTemplate>
<FooterTemplate> 
  <asp:TextBox ID="txt_NewLogoPath" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
   <asp:Image ID="img_CompanyLogo" runat="server" ImageUrl='<%#Bind("CompanyLogo") %>' Width="20px" />
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Company Type" >
<EditItemTemplate> 
  <asp:DropDownList ID="ddl_CompanyType" runat="server" DataTextField="companyType" DataValueField="companyType" > 
    </asp:DropDownList> 
</EditItemTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbCompanyType" runat="server" Text='<%# Eval("companyType") %>' ></asp:Label> 
</ItemTemplate> 
<FooterTemplate> 
  <asp:DropDownList ID="ddl_NewCompanyType" runat="server" DataTextField="companyType" DataValueField="companyType" >
     </asp:DropDownList> 
</FooterTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Company Origin" >
<EditItemTemplate> 
  <asp:DropDownList ID="ddl_CompanyOrigin" runat="server" DataTextField="origin" DataValueField="origin" > 
   </asp:DropDownList> 
</EditItemTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbCompanyOrigin" runat="server" Text='<%# Eval("origin") %>' ></asp:Label> 
</ItemTemplate> 
<FooterTemplate> 
  <asp:DropDownList ID="ddl_NewCompanyOrigin" runat="server" DataTextField="origin" DataValueField="origin" >
     </asp:DropDownList> 
</FooterTemplate> 
</asp:TemplateField>

<asp:TemplateField HeaderText="Industry" >
<EditItemTemplate> 
  <asp:DropDownList ID="ddl_Industry" runat="server" DataTextField="IndustryName" DataValueField="IndustryName"  > 
     </asp:DropDownList> 
</EditItemTemplate> 
<ItemTemplate> 
  <asp:Label ID="lblIndustry" runat="server" Text='<%# Eval("IndustryName") %>' ></asp:Label> 
</ItemTemplate> 
<FooterTemplate> 
  <asp:DropDownList ID="ddl_NewIndustry" runat="server" DataTextField="IndustryName" DataValueField="IndustryName" >
     </asp:DropDownList> 
</FooterTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Overall Employees" SortExpression="OverallEmployees"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_OverallEmployees" runat="server" Text='<%# Eval("overallEmployees") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewOverallEmployees" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_OverallEmployees" runat="server" Text='<%# Bind("overallEmployees") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>  

<asp:TemplateField HeaderText="Overall Employees(India)" SortExpression="OverallEmployeesIndia"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_OverallEmployeesIndia" runat="server" Text='<%# Eval("overallEmployeesIndia") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewOverallEmployeesIndia" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_OverallEmployeesIndia" runat="server" Text='<%# Bind("overallEmployeesIndia") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>

<asp:TemplateField HeaderText="Company Url" SortExpression="CompanyUrl"> 
<EditItemTemplate> 
  <asp:TextBox ID="txt_CompanyUrl" runat="server" Text='<%# Eval("companyurl") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewCompanyUrl" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:HyperLink ID="lbl_CompanyUrl" runat="server" Text='<%# Bind("companyurl") %>'></asp:HyperLink> 
</ItemTemplate> 
</asp:TemplateField> 

<asp:TemplateField HeaderText="Contact Info" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_ContactInfo" runat="server" Text='<%# Eval("contactinfo") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewContactInfo" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_ContactInfo" runat="server" Text='<%# Bind("contactinfo") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>

<asp:TemplateField HeaderText="Fortune1000" SortExpression="Fotune1000"> 
<EditItemTemplate> 
  <asp:DropDownList ID="ddl_Fortune" runat="server" SelectedValue='<%# Eval("Fortune1000") %>'> 
    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
    <asp:ListItem Value="False" Text="No"></asp:ListItem>
  </asp:DropDownList> 
</EditItemTemplate> 
<ItemTemplate> 
  <asp:Label ID="lblFortune" runat="server" Text='<%# Bind("Fortune1000") %>'></asp:Label> 
</ItemTemplate> 
<FooterTemplate> 
  <asp:DropDownList ID="ddl_NewFortune" runat="server" >
    <asp:ListItem Selected="True" Text="True" Value="Y"></asp:ListItem> 
    <asp:ListItem Text="False" Value="N"></asp:ListItem> </asp:DropDownList> 
</FooterTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Workculture" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Workculture" runat="server" Text='<%# Eval("WorkCulture") %>' TextMode="MultiLine"></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewWorkculture" runat="server" TextMode="MultiLine"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Workcultrue" runat="server" Text='<%# Bind("WorkCulture") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Day2DayExperience" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Experience" runat="server" Text='<%# Eval("Experience") %>' TextMode="MultiLine"></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewExperience" runat="server" TextMode="MultiLine"> </asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Experience" runat="server" Text='<%# Bind("Experience") %>'> </asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="RecruitmentProcess" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Recruitment" runat="server" Text='<%# Eval("RecruitmentProcess") %>' TextMode="MultiLine"></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewRecruitment" runat="server" TextMode="MultiLine"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Recruitment" runat="server" Text='<%# Bind("RecruitmentProcess") %>'> </asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Other Benefits" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Benefits" runat="server" Text='<%# Eval("Benefits") %>' TextMode="MultiLine"></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewBenefits" runat="server" TextMode="MultiLine"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Benefits" runat="server" Text='<%# Bind("Benefits") %>'> </asp:Label> 
</ItemTemplate> 
</asp:TemplateField>  
</Columns>
 
</asp:GridView> 
</tr>
</table>
</div>
<div>
 <table>
  <tr>
    <td>
      <h2>
         Enter Basic Location Details:
      </h2>
    </td>
   </tr>
  <tr>
     <asp:GridView ID="gv_location" runat="server" AutoGenerateSelectButton="true" 
        AutoGenerateColumns="False" DataKeyNames="LocationId" OnRowCancelingEdit="gvlocation_RowCancelingEdit" 
        OnRowDataBound="gvlocation_RowDataBound" OnRowEditing="gvlocation_RowEditing" OnRowUpdating="gvlocation_RowUpdating"
        OnRowCommand="gvlocation_RowCommand" ShowFooter="True" OnRowDeleting="gvlocation_RowDeleting" > 
<Columns>
    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" /> 
<asp:TemplateField HeaderText="Edit" ShowHeader="True"> 
<EditItemTemplate> 
  <asp:LinkButton ID="btn_update" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="btn_cancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:LinkButton ID="btn_addnew" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton> 
</FooterTemplate> 
<ItemTemplate> 
  <asp:LinkButton ID="btn_edit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="LocationId" Visible="false"> 
<ItemTemplate> 
  <asp:Label ID="lbl_locationid" runat="server" Text='<%# Eval("LocationId") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="AddressLine1" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_AddressLine1" runat="server" Text='<%# Eval("AddressLine1") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewAddressLine1" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_AddressLine1" runat="server" Text='<%# Bind("AddressLine1") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 
<asp:TemplateField HeaderText="AddressLine2" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_AddressLine2" runat="server" Text='<%# Eval("AddressLine2") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewAddressLine2" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_AddressLine2" runat="server" Text='<%# Bind("AddressLine2") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="AddressLine3" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_AddressLine3" runat="server" Text='<%# Eval("AddressLine3") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewAddressLine3" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_AddressLine3" runat="server" Text='<%# Bind("AddressLine3") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="City" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_City" runat="server" Text='<%# Eval("City") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewCity" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_City" runat="server" Text='<%# Bind("City") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="State" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_State" runat="server" Text='<%# Eval("State") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewState" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_State" runat="server" Text='<%# Bind("State") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Country" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Country" runat="server" Text='<%# Eval("Country") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewCountry" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Country" runat="server" Text='<%# Bind("Country") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>   
<asp:TemplateField HeaderText="Pincode" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Pincode" runat="server" Text='<%# Eval("PostalCode") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewPincode" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Pincode" runat="server" Text='<%# Bind("PostalCode") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
</Columns>
</asp:GridView>
</tr>   
 </table>
  </div> 
<div>
 <table>
  <tr>
    <td>
      <h2>
         Enter Company Head Info Details:
      </h2>
    </td>
   </tr>
  <tr>
     <asp:GridView ID="gv_CompanyHeads" runat="server"  
        AutoGenerateColumns="False" DataKeyNames="LocationId" OnRowCancelingEdit="gvcompanyheads_RowCancelingEdit" 
        OnRowDataBound="gvcompanyheads_RowDataBound" OnRowEditing="gvcompanyheads_RowEditing" OnRowUpdating="gvcompanyheads_RowUpdating"
        OnRowCommand="gvcompanyheads_RowCommand" ShowFooter="True" OnRowDeleting="gvcompanyheads_RowDeleting" > 
<Columns>
    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" /> 
<asp:TemplateField HeaderText="Edit" ShowHeader="True"> 
<EditItemTemplate> 
  <asp:LinkButton ID="btn_update_heads" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="btn_cancel_heads" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:LinkButton ID="btn_addnew_heads" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton> 
</FooterTemplate> 
<ItemTemplate> 
  <asp:LinkButton ID="btn_edit_heads" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="CompanyHeadId" Visible="false"> 
<ItemTemplate> 
  <asp:Label ID="lbl_headid" runat="server" Text='<%# Eval("CompanyHeadId") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="CompanyHeadName" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_HeadName" runat="server" Text='<%# Eval("HeadName") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewHeadName" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_HeadName" runat="server" Text='<%# Bind("HeadName") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField> 
<asp:TemplateField HeaderText="Designation" > 
<EditItemTemplate> 
  <asp:TextBox ID="txt_Designation" runat="server" Text='<%# Eval("Designation") %>'></asp:TextBox> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:TextBox ID="txt_NewDesignation" runat="server"></asp:TextBox>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Label ID="lbl_Designation" runat="server" Text='<%# Bind("Designation") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
</Columns>
</asp:GridView>
</tr>   
 </table>
  </div> 
<div>
 <table>
  <tr>
    <td>
      <h2>
         Enter Company Pictures and Videos Details:
      </h2>
    </td>
   </tr>
  <tr>
     <asp:GridView ID="gv_companypics" runat="server" 
        AutoGenerateColumns="False" DataKeyNames="CompanyId,LocationId" OnRowCancelingEdit="gvcompanypics_RowCancelingEdit" 
        OnRowDataBound="gvcompanypics_RowDataBound" OnRowEditing="gvcompanypics_RowEditing" OnRowUpdating="gvcompanypics_RowUpdating"
        OnRowCommand="gvcompanypics_RowCommand" ShowFooter="True" OnRowDeleting="gvcompanypics_RowDeleting" > 
<Columns>
    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" /> 
<asp:TemplateField HeaderText="Edit" ShowHeader="True"> 
<EditItemTemplate> 
  <asp:LinkButton ID="btn_update_companypics" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="btn_cancel_companypics" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:LinkButton ID="btn_addnew_companypics" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton> 
</FooterTemplate> 
<ItemTemplate> 
  <asp:LinkButton ID="btn_edit_companypics" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="PicId" Visible="false"> 
<ItemTemplate> 
  <asp:Label ID="lbl_picid" runat="server" Text='<%# Eval("CompanyPicId") %>'></asp:Label> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="CompanyPicPath" > 
<EditItemTemplate> 
  <asp:FileUpload ID="txt_PicPath" runat="server" Text='<%# Eval("PicturePath") %>'></asp:FileUpload> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:FileUpload ID="txt_NewPicturePath" runat="server"></asp:FileUpload>
</FooterTemplate> 
<ItemTemplate> 
  <asp:Image ID="lbl_PicturePath" runat="server" ImageUrl='<%# Bind("PicturePath") %>'></asp:Image> 
</ItemTemplate> 
</asp:TemplateField> 
<asp:TemplateField HeaderText="CompanyVideoPath" > 
<EditItemTemplate> 
  <asp:FileUpload ID="txt_VideoPath" runat="server" ></asp:FileUpload> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:FileUpload ID="txt_NewVideoPath" runat="server"></asp:FileUpload>
</FooterTemplate> 
<ItemTemplate> 
  <asp:HyperLink ID="hyp_VideoPath" runat="server" NavigateUrl='<%# Bind("VideoPath") %>'></asp:HyperLink> 
</ItemTemplate> 
</asp:TemplateField> 
</Columns>
</asp:GridView>
</tr>   
 </table>
  </div> 
<div>
 <table>
  <tr>
    <td>
      <h2>
         Enter Compensation Details:
      </h2>
    </td>
   </tr>
  <tr>
     <asp:GridView ID="gv_Compensation" runat="server" 
        AutoGenerateColumns="False" DataKeyNames="CompanyId,LocationId" OnRowCancelingEdit="gvCompensation_RowCancelingEdit" 
        OnRowDataBound="gvCompensation_RowDataBound" 
          OnRowEditing="gvCompensation_RowEditing" OnRowUpdating="gvCompensation_RowUpdating"
        OnRowCommand="gvCompensation_RowCommand" ShowFooter="True" 
          OnRowDeleting="gvCompensation_RowDeleting" 
           > 
<Columns>
    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" ShowHeader="True" /> 
<asp:TemplateField HeaderText="Edit" ShowHeader="True">
<EditItemTemplate> 
  <asp:LinkButton ID="btn_update_companypics" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
  <asp:LinkButton ID="btn_cancel_companypics" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:LinkButton ID="btn_addnew_companypics" runat="server" CausesValidation="False" CommandName="AddNew" Text="Add New"></asp:LinkButton> 
</FooterTemplate> 
<ItemTemplate> 
  <asp:LinkButton ID="btn_edit_companypics" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Position" Visible="true">  
<EditItemTemplate> 
  <asp:DropDownList ID="ddl_Position" runat="server" SelectedValue='<%# Eval("Position") %>'> 
    <asp:ListItem Value="1" Text="1"></asp:ListItem>
    <asp:ListItem Value="2" Text="2"></asp:ListItem>
    <asp:ListItem Value="3" Text="3"></asp:ListItem>
    <asp:ListItem Value="4" Text="4"></asp:ListItem>
    <asp:ListItem Value="5" Text="5"></asp:ListItem>
    <asp:ListItem Value="6" Text="6"></asp:ListItem>
    <asp:ListItem Value="7" Text="7"></asp:ListItem>
    <asp:ListItem Value="8" Text="8"></asp:ListItem>
    <asp:ListItem Value="9" Text="9"></asp:ListItem>
    <asp:ListItem Value="10" Text="10"></asp:ListItem>
  </asp:DropDownList> 
</EditItemTemplate> 
<ItemTemplate> 
  <asp:DropDownList ID="ddl_Position1" runat="server" SelectedValue='<%# Bind("Position") %>'> 
    </asp:DropDownList> 
</ItemTemplate> 
<FooterTemplate> 
  <asp:DropDownList ID="ddl_Position" runat="server" SelectedValue='<%# Eval("Position") %>'> 
    <asp:ListItem Value="1" Text="1"></asp:ListItem>
    <asp:ListItem Value="2" Text="2"></asp:ListItem>
    <asp:ListItem Value="3" Text="3"></asp:ListItem>
    <asp:ListItem Value="4" Text="4"></asp:ListItem>
    <asp:ListItem Value="5" Text="5"></asp:ListItem>
    <asp:ListItem Value="6" Text="6"></asp:ListItem>
    <asp:ListItem Value="7" Text="7"></asp:ListItem>
    <asp:ListItem Value="8" Text="8"></asp:ListItem>
    <asp:ListItem Value="9" Text="9"></asp:ListItem>
    <asp:ListItem Value="10" Text="10"></asp:ListItem>
  </asp:DropDownList> 
</FooterTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="Compensation" Visible="true"> 
<ItemTemplate> 
  <asp:Label ID="lbl_compensation" runat="server" Text='<%# Bind("Compensation") %>'></asp:Label> 
</ItemTemplate> 
<EditItemTemplate> 
  <asp:FileUpload ID="txt_Compensation" runat="server" Text='<%# Eval("Compensation") %>'></asp:FileUpload> 
</EditItemTemplate> 
<FooterTemplate> 
  <asp:FileUpload ID="txt_NewCompensation" runat="server"></asp:FileUpload>
</FooterTemplate> 
</asp:TemplateField> 
</Columns>
</asp:GridView>
</tr>   
 </table>
  </div>       
</asp:Content>
