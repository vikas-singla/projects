using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Assets.Controls
{
    public partial class CareerProgressionDetailsControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// ID of the career path that should be displayed on the control
        /// </summary>
        private int _displayCareerPathID;
        /// <summary>
        /// ID of the career path that should be displayed on the control
        /// </summary>
        internal int DisplayCareerPathID
        {
            get
            {
                return _displayCareerPathID;
            }
            set
            {
                _displayCareerPathID = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //set the career path to show on the career progression control
            SelectiveCareerProgressionControl.ShowSelectectiveCareerPathID = _displayCareerPathID;
            SelectiveCareerProgressionControl.NotifyLoadCareerDetails += new CareerProgression.LoadCareerDetails(this.LoadCareerDetails);
        }

        /// <summary>
        /// Method to signal what career details to load on the page based on user click
        /// </summary>
        /// <param name="careerPathId_">ID of the career path</param>
        /// <param name="careerId_">ID of the career in the career path</param>
        /// <param name="careerName_">Name of the career</param>
        internal void LoadCareerDetails(int careerPathId_, int careerId_, string careerName_)
        {
            if (careerName_ != null && !careerName_.Equals(string.Empty))
            {
                HeaderCareerNameLbl.Text = careerName_;
            }
        }
    }
}