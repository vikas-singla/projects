using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rainforest.Assets.Controls
{
    public partial class CandidateSignupControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddDisclaimer();
        }

        public void AddDisclaimer()
        {
            if (Page.Master is RainForest)
            {
                RainForest masterPage = (RainForest)Page.Master;
                masterPage.AddDisclaimer("*By clicking Join Now, you are indicating that you have read, understoord, and agree to Bridges and Ladders' User Agreement and Privacy Policy");
            }
        }
    }
}