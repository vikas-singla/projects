using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rainforest.Assets.Controls;

namespace Rainforest
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                showSignupPage();
            }

            //showUserLandingPageControl();
            //showCandidateSignupWizard();
        }

        private void showCandidateSignupWizard()
        {
            Control signupWizard = LoadControl("/Assets/Controls/CandidateSignupWizardControl.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(signupWizard);
        }

        /// <summary>
        /// Show the candidate user landing page
        /// </summary>
        private void showUserLandingPageControl()
        {
            Control userLandingPageControl = LoadControl("/Assets/Controls/UserLandingPageControl.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(userLandingPageControl);
        }

        /// <summary>
        /// Show the controls of teh signup page
        /// </summary>
        private void showSignupPage()
        {
            showSignupPageHeaderControls();

            Control candidateSignupControl = LoadControl("/Assets/Controls/CandidateSignupControl.ascx");
            Image dividerImg = new Image();
            dividerImg.ImageUrl = "~/Assets/Images/TopLine_MasterPage.png";
            dividerImg.Width = Unit.Percentage(100.00);
            Control jobSearchControl = LoadControl("/Assets/Controls/BasicSearchControl.ascx");

            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(candidateSignupControl);
            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(dividerImg);
            UpdatePanel_Body.ContentTemplateContainer.Controls.Add(jobSearchControl);
        }

        /// <summary>
        /// Show the drop down menu and login controls on the signup page
        /// </summary>
        private void showSignupPageHeaderControls()
        {
            Table headerTable = new Table();
            TableRow headerTableRow = new TableRow();
            TableCell headerTableLeftCell = new TableCell();
            TableCell headerTableRightCell = new TableCell();

            headerTableLeftCell.HorizontalAlign = HorizontalAlign.Left;
            headerTableRightCell.HorizontalAlign = HorizontalAlign.Right;
            headerTableLeftCell.VerticalAlign = VerticalAlign.Bottom;
            headerTableRightCell.VerticalAlign = VerticalAlign.Bottom;

            //headerTableLeftCell.Style.Add("padding-left", "100px");

            Control menuControl = LoadControl("/Assets/Controls/SignupPageDropDownMenuControl.ascx");
            Control loginControl = LoadControl("/Assets/Controls/LoginControl.ascx");

            headerTableLeftCell.Controls.Add(menuControl);
            headerTableRightCell.Controls.Add(loginControl);

            headerTableRow.Cells.Add(headerTableLeftCell);
            headerTableRow.Cells.Add(headerTableRightCell);

            headerTable.Rows.Add(headerTableRow);
            headerTable.Width = Unit.Percentage(100.00);
            headerTable.CellSpacing = 0;
            headerTable.CellPadding = 0;

            UpdatePanel_Header.ContentTemplateContainer.Controls.Add(headerTable);            
        }
    }
}
