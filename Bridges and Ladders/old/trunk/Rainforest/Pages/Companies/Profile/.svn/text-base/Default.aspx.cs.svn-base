using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rainforest.Assets.Controls;

namespace Rainforest.Pages.Company
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();
            showCompanyProfileOverview();
            //showCareerDetailsControl();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showCompanyProfileOverview()
        {
            Control companyProfileOverview = LoadControl("/Assets/Controls/CompanyProfileOverviewControl.ascx");

            CompanyProfileOverviewControl profileControl = (CompanyProfileOverviewControl)companyProfileOverview;

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(companyProfileOverview);
        }
    }
}