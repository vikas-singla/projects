using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Pages.Search
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();

            showCompanySearchControls();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showCompanySearchControls()
        {
            Control companySearchControl = LoadControl("/Assets/Controls/SearchCompany.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(companySearchControl);
        }
    }
}