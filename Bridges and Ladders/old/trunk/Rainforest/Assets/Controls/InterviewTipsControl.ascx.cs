using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Rainforest.Assets.Controls
{
    public partial class InterviewTipsControl : System.Web.UI.UserControl
    {
        private const string INTERVIEW_SECTION_NODE = "Section";
        private const string INTERVIEW_SECTION_NODE_TITLE = "Title";
        private const string INTERVIEW_SUBSECTION_NODE = "SubSection";
        private const string INTERVIEW_SUBSECTION_NODE_TITLE = "Title";
        private const string TITLE_ERROR = "<<Error>>";

        private const string VIEW_STATE_INTERVIEW_TIP_DATA = "BL_InterviewTipData";

        /// <summary>
        /// Used to assign IDs to links
        /// </summary>
        private int _linkIDTracker = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            loadInterviewTipsUI();
        }

        /// <summary>
        /// Load the career paths UI based on the XML data
        /// </summary>
        private void loadInterviewTipsUI()
        {
            //open XML document
            XmlDocument xmlDoc = new XmlDocument();

            if (ViewState[VIEW_STATE_INTERVIEW_TIP_DATA] != null)
            {
                xmlDoc.LoadXml((string)ViewState[VIEW_STATE_INTERVIEW_TIP_DATA]);
            }
            else
            {
                xmlDoc.Load(Server.MapPath("/Assets/SampleData/InterviewTipsData.xml"));
            }

            if (xmlDoc.HasChildNodes) // is there content in the XML?
            {
                //identify all interview sections 
                XmlNodeList interviewTips = xmlDoc.GetElementsByTagName(INTERVIEW_SECTION_NODE);

                if (interviewTips != null)
                {
                    //construct the header table containing the process overview in the UI
                    constructHeaderTableUI(interviewTips, 0);

                    //construct the first interview phase subsection
                    if (interviewTips.Count > 0)
                    {
                        Panel interviewSubSectionPanel = constructInterviewTipSection(interviewTips[0]);

                        InterviewTipsContentPanel.Controls.Add(interviewSubSectionPanel);

                        //add the interview tip sections to the viewstate
                        ViewState[VIEW_STATE_INTERVIEW_TIP_DATA] = xmlDoc.InnerXml;
                    }
                }
            }
        }

        /// <summary>
        /// Construct Interview tips section UI (ie. show subsections of the interview phases)
        /// </summary>
        /// <param name="interviewTipSection_">Reference to the XML data</param>
        /// <returns>UI Panel containing the controls to show the interview phase subsections</returns>
        private Panel constructInterviewTipSection(XmlNode interviewTipSection_)
        {
            Panel interviewSectionPanel = new Panel();

            Table interviewSectionTable = new Table();
            interviewSectionPanel.Controls.Add(interviewSectionTable);

            XmlNodeList interviewTipSubsections = interviewTipSection_.ChildNodes;

            //iterate through all the subsections in the interview tip section
            foreach (XmlNode interviewTipSubsection in interviewTipSubsections)
            {
                TableRow interviewSectionRow = new TableRow();
                interviewSectionTable.Rows.Add(interviewSectionRow);

                TableCell subsectionCell = new TableCell();
                subsectionCell.CssClass = "interviewTipSubsectionStyle";

                /**
                 * Create the sub-section HEADER
                 **/

                Panel headerPanel = new Panel();
                headerPanel.CssClass = "containerheader";

                Table headerTable = new Table();
                headerTable.CssClass = "headertablestyle";

                TableRow headerRow = new TableRow();
                headerTable.Rows.Add(headerRow);

                TableCell headerCell = new TableCell();
                headerRow.Cells.Add(headerCell);

                Label headerLbl = new Label();
                headerLbl.CssClass = "headerlabelstyle";
                headerLbl.Text = (interviewTipSubsection.Attributes[INTERVIEW_SUBSECTION_NODE_TITLE] != null) ? (interviewTipSubsection.Attributes[INTERVIEW_SUBSECTION_NODE_TITLE].Value) :
                                    TITLE_ERROR;
                headerCell.Controls.Add(headerLbl);

                //sdd header to header panel
                headerPanel.Controls.Add(headerTable);
                subsectionCell.Controls.Add(headerPanel);

                /**
                 * Create the sub-section BODY
                 **/

                Panel bodyPanel = new Panel();
                bodyPanel.CssClass = "containerbody";

                Literal bodyPanelText = new Literal();
                bodyPanelText.Text = interviewTipSubsection.InnerText;
                bodyPanel.Controls.Add(bodyPanelText);
                subsectionCell.Controls.Add(bodyPanel);

                /**
                 * Create the sub-section FOOTER
                 **/
                Panel footerPanel = new Panel();
                footerPanel.CssClass = "containerfooter";
                //no content for now
                subsectionCell.Controls.Add(footerPanel);

                //add sub-section to the UI
                interviewSectionRow.Cells.Add(subsectionCell);
            }

            return interviewSectionPanel;
        }

        /// <summary>
        /// Construct the header table containing the interview process in the UI
        /// </summary>
        /// <param name="interviewTips_">Reference to the data source of interview tips</param>
        /// <param name="selectedIndex_">Index of phase pre-selected in the UI</param>
        private void constructHeaderTableUI(XmlNodeList interviewTips_, int selectedIndex_)
        {
            //start constructing the UI in a web table format
            int numInterviewSections = interviewTips_.Count;
            Table interviewTipsHeaderSectionTbl = new Table();
            interviewTipsHeaderSectionTbl.CssClass = "tipsHeaderTbl";
            interviewTipsHeaderSectionTbl.CellSpacing = 5;
            interviewTipsHeaderSectionTbl.CellPadding = 0;
            interviewTipsHeaderSectionTbl.Style.Add("max-width", ((numInterviewSections > 4) ? Unit.Percentage(100.0) : Unit.Percentage(25.0 * numInterviewSections)).ToString());

            TableRow headerRow = new TableRow();

            for (int i = 0; i < numInterviewSections; ++i)
            {
                TableCell headerCell = new TableCell();
                headerCell.CssClass = "tipsHeaderSectionCell";

                if (i == selectedIndex_)
                {
                    headerCell.CssClass = "tipsHeaderSectionCellSel";
                } 
                
                LinkButton sectionLinkButton = new LinkButton();

                sectionLinkButton.CommandArgument = i.ToString();
                sectionLinkButton.Command += new CommandEventHandler(sectionLinkButton_Command);

                headerCell.Controls.Add(sectionLinkButton);

                //create a table for cell contents (IMPORTANT: This is done to encaspulate all of the content in a single link)                
                Panel sectionStepImgPanel = new Panel();
                sectionStepImgPanel.CssClass = "tipsHeaderSectionImgCell";

                Image stepImage = new Image();
                stepImage.ImageUrl = "/Assets/Images/Step" + (i + 1) + ".gif";
                stepImage.Width = Unit.Pixel(40);
                stepImage.Style.Add("border", "none");

                sectionStepImgPanel.Controls.Add(stepImage);

                sectionLinkButton.Controls.Add(sectionStepImgPanel);

                Panel sectionContentPanel = new Panel();
                sectionContentPanel.CssClass = "tipsHeaderSectionPanel";
                
                Label interviewStepLabel = new Label();
                interviewStepLabel.Text = "Phase " + (i + 1);
                interviewStepLabel.CssClass = "interviewHeaderStepLbl";

                sectionContentPanel.Controls.Add(interviewStepLabel);

                Literal breakLiteral = new Literal();
                breakLiteral.Text = "<br />";

                sectionContentPanel.Controls.Add(breakLiteral);

                Label interviewSectionTitle = new Label();
                interviewSectionTitle.Text = (interviewTips_[i].Attributes[INTERVIEW_SECTION_NODE_TITLE] == null) ? "" : interviewTips_[i].Attributes[INTERVIEW_SECTION_NODE_TITLE].Value;
                interviewSectionTitle.CssClass = "interviewHeaderTitleLbl";

                sectionContentPanel.Controls.Add(interviewSectionTitle);

                //add the table cell to the content table row
                sectionLinkButton.Controls.Add(sectionContentPanel);

                //add the content cell to the main table
                headerRow.Cells.Add(headerCell);

                //check if we need to add the step seperator
                if (i < (numInterviewSections - 1))
                {
                    TableCell stepSeperatorCell = new TableCell();
                    stepSeperatorCell.VerticalAlign = VerticalAlign.Middle;
                    stepSeperatorCell.Width = Unit.Pixel(13);

                    Image stepSeperatorImg = new Image();
                    stepSeperatorImg.ImageUrl = "/Assets/Images/step_seperator.gif";
                    stepSeperatorImg.Width = Unit.Pixel(13);

                    stepSeperatorCell.Controls.Add(stepSeperatorImg);
                    headerRow.Cells.Add(stepSeperatorCell);
                }
            }

            interviewTipsHeaderSectionTbl.Rows.Add(headerRow);

            Panel tipsHeaderPanel = new Panel();
            tipsHeaderPanel.CssClass = "tipsHeaderPanel";
            tipsHeaderPanel.Controls.Add(interviewTipsHeaderSectionTbl);

            InterviewTipsHeaderPanel.Controls.Add(tipsHeaderPanel);
        }

        void sectionLinkButton_Command(object sender, CommandEventArgs e)
        {
            int interviewPhaseIndex = int.Parse((string)e.CommandArgument);

            if (ViewState[VIEW_STATE_INTERVIEW_TIP_DATA] != null)
            {
                //open XML document
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml((string)ViewState[VIEW_STATE_INTERVIEW_TIP_DATA]);

                if (xmlDoc.HasChildNodes) // is there content in the XML?
                {
                    //identify all interview sections 
                    XmlNodeList interviewTips = xmlDoc.GetElementsByTagName(INTERVIEW_SECTION_NODE);

                    Panel interviewSubSectionPanel = constructInterviewTipSection(interviewTips[interviewPhaseIndex]);

                    //delete all previous controls
                    InterviewTipsHeaderPanel.Controls.Clear();
                    InterviewTipsContentPanel.Controls.Clear();

                    //load new controls
                    constructHeaderTableUI(interviewTips, interviewPhaseIndex);
                    InterviewTipsContentPanel.Controls.Add(interviewSubSectionPanel);
                }
            }
        }
    }
}