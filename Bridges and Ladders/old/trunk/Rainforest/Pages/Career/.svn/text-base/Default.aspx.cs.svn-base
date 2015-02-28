using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rainforest.Assets.Controls;

namespace Rainforest.Pages.Career
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            showUserNavDropDownMenu();
            showCareerProgressionDetails();
        }

        private void showUserNavDropDownMenu()
        {
            Control userNavDropDownMenuControl = LoadControl("/Assets/Controls/UserNavDropDownMenuControl.ascx");

            UpdatePanel_Menu.ContentTemplateContainer.Controls.Add(userNavDropDownMenuControl);
        }

        private void showCareerProgressionDetails()
        {
            Control careerProgression = LoadControl("/Assets/Controls/CareerProgressionDetailsControl.ascx");

            ((CareerProgressionDetailsControl)careerProgression).DisplayCareerPathID = 1;

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(careerProgression);
        }
    }
}