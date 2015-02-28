using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Pages.CompanyLocation
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();
            showLocations();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showLocations()
        {
            Control locations = LoadControl("/Assets/Controls/CompanyLocations.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(locations);
        }
    }
}