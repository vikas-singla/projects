using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Rainforest.Assets.Controls
{
    public partial class CareerProgression : System.Web.UI.UserControl
    {
        private const string CAREER_PATH_NODE = "Path";
        private const string CAREER_NODE = "Career";
        private const string CAREER_NODE_TITLE_ATTR = "Title";
        private const string CAREER_NODE_LEVEL_ATTR = "Level";
        private const string JOB_OBJECTIVES_NODE = "JobObjectives";
        private const string JOB_RESPONSIBILITIES_NODE = "Responsibilities";
        private const string JOB_REQUIREMENTS_NODE = "JobReqs";
        private const string JOB_COMPETENCIES_NODE = "Competencies";
        /// <summary>
        /// Maximum number of paths shown on the UI for the profile page
        /// </summary>
        private const int MAX_PATHS_SHOWN_ON_COMP_PROFILE = 2;
        /// <summary>
        /// Used to assign IDs to links
        /// </summary>
        private int _linkIDTracker = 0;

        /// <summary>
        /// Should the control only show a particular career path in its UI?
        /// </summary>
        private int _showSelectiveCareerPathId = -1;

        /// <summary>
        /// Should the control only show a particular career path in its UI?
        /// </summary>
        internal int ShowSelectectiveCareerPathID
        {
            get
            {
                return _showSelectiveCareerPathId;
            }
            set
            {
                _showSelectiveCareerPathId = value;
            }
        }

        /// <summary>
        /// Delegate invoked to signal what career details to load on the page based on user click
        /// </summary>
        /// <param name="careerPathId_">ID of the career path</param>
        /// <param name="careerId_">ID of the career in the career path</param>
        /// <param name="careerName_">Name of the career</param>
        internal delegate void LoadCareerDetails(int careerPathId_, int careerId_, string careerName_);
        internal LoadCareerDetails NotifyLoadCareerDetails;

        protected void Page_Load(object sender, EventArgs e)
        {
            loadCareerProgressionUI();
        }

        /// <summary>
        /// Load the career paths UI based on the XML data
        /// </summary>
        private void loadCareerProgressionUI()
        {
            //open XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("/Assets/SampleData/CareerProgression.xml"));

            if (xmlDoc.HasChildNodes) // is there content in the XML?
            {
                //identify all career paths
                XmlNodeList careerPaths = xmlDoc.GetElementsByTagName(CAREER_PATH_NODE);

                if (careerPaths != null)
                {
                    //start constructing the UI in a web table format
                    int numCareerPaths = careerPaths.Count;
                    Table multipleCareerProgressionTbl = new Table();
                    multipleCareerProgressionTbl.CssClass = "Tbl_MultipleCareerProgressionPathsStyle";
                    multipleCareerProgressionTbl.CellSpacing = 0;
                    multipleCareerProgressionTbl.CellPadding = 0;

                    TableRow pathsDisplayRow = new TableRow();
                    multipleCareerProgressionTbl.Rows.Add(pathsDisplayRow);

                    for (int i = 0; i < numCareerPaths; ++i)
                    {
                        TableCell pathsDisplayCell = new TableCell();
                        pathsDisplayRow.Cells.Add(pathsDisplayCell);

                        if (i >= MAX_PATHS_SHOWN_ON_COMP_PROFILE && _showSelectiveCareerPathId == -1)
                        {

                        }
                        else if ((_showSelectiveCareerPathId == -1) || ((i + 1) == _showSelectiveCareerPathId))
                        {
                            Control careerPathControl = processCareerPath(careerPaths[i], i);
                            pathsDisplayCell.Controls.Add(careerPathControl);
                        }
                    }

                    CareerProgressionPanel.Controls.Add(multipleCareerProgressionTbl);
                }
            }
        }

        /// <summary>
        /// Process the XML for the career path
        /// </summary>
        /// <param name="careerPath_">Reference to the XML node containing the root of the career path</param>
        /// <param name="careerPathNum_">Number identifying the career path</param>
        /// <returns>Process and constructed UI for the career path</returns>
        private Control processCareerPath(XmlNode careerPath_, int careerPathNum_)
        {
            Control result = null;

            int numCareers = careerPath_.ChildNodes.Count;
            Control[] careerDisplayControlList = new Control[numCareers]; //array that will contain a sorted list of careers in the career path (bottom up)

            for (int i = 0; i < numCareers; ++i)
            {
                XmlNode career = careerPath_.ChildNodes[i];
                XmlAttribute careerLevelAttr = career.Attributes[CAREER_NODE_LEVEL_ATTR];

                if (careerLevelAttr != null)
                {
                    int careerLevel = -1;

                    //check if we can parse the career level from XML
                    if (int.TryParse(careerLevelAttr.Value, out careerLevel))
                    {
                        //Build the UI for the career
                        Control displayControl = constructCareerUI(career);

                        if (displayControl != null)
                        {
                            careerDisplayControlList[careerLevel] = displayControl; //insert according to career level
                        }
                    }
                }
            }

            result = constructCareerPathUI(careerDisplayControlList, careerPathNum_);

            return result;
        }

        /// <summary>
        /// Construct the full UI for the career path (ie. TREE UI)
        /// </summary>
        /// <param name="careerPathDisplayControls_">The individual controls displaying the careers in the career path</param>
        /// <param name="careerTrackNum_">Number identifying the career path</param>
        /// <returns>Full TREE UI</returns>
        private Control constructCareerPathUI(Control[] careerPathDisplayControls_, int careerTrackNum_)
        {
            Table careerProgressionTbl = new Table();
            careerProgressionTbl.CssClass = "Tbl_CareerProgressionPathStyle";
            careerProgressionTbl.CellSpacing = 0;
            careerProgressionTbl.CellPadding = 0;

            for (int i = careerPathDisplayControls_.Length - 1; i >= 0; --i)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();

                row.Cells.Add(cell);
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.VerticalAlign = VerticalAlign.Middle;

                if (careerPathDisplayControls_[i] != null)
                {
                    cell.Controls.Add(careerPathDisplayControls_[i]);
                    careerProgressionTbl.Rows.Add(row);

                    //Do we need to insert an arrow image in between rows to indicate a path?
                    if (i >= 0)
                    {//yes
                        TableRow dividerRow = new TableRow();
                        TableCell dividerCell = new TableCell();

                        dividerRow.Cells.Add(dividerCell);
                        dividerCell.HorizontalAlign = HorizontalAlign.Center;
                        dividerCell.VerticalAlign = VerticalAlign.Middle;

                        Panel dividerImgPanel = new Panel();
                        dividerImgPanel.CssClass = "CareerPathDividerCellContentStyle";

                        dividerCell.Controls.Add(dividerImgPanel);
                        careerProgressionTbl.Rows.Add(dividerRow);
                    }
                }
            }


            //Construct the bottom row indicating career track number
            TableRow careerTrackRow = new TableRow();
            TableCell careerTrackCell = new TableCell();

            careerTrackRow.Cells.Add(careerTrackCell);
            careerTrackCell.VerticalAlign = VerticalAlign.Middle;
            careerTrackCell.HorizontalAlign = HorizontalAlign.Center;

            careerTrackCell.Controls.Add(getCareerTracksLblPanel(careerTrackNum_));
            careerProgressionTbl.Rows.Add(careerTrackRow);

            return careerProgressionTbl;
        }

        /// <summary>
        /// Get the UI control that shows the "Career Track No. X" in the UI
        /// </summary>
        /// <param name="careerTrackNum_">The career track number to display in the control</param>
        /// <returns>Reference to the UI control</returns>
        private Control getCareerTracksLblPanel(int careerTrackNum_)
        {
            Panel careerTrackPanel = new Panel();
            careerTrackPanel.CssClass = "CareerTrackPanelStyle";

            Literal careerTrackLbl = new Literal();
            careerTrackLbl.Text = "Career Track No. " + (careerTrackNum_ + 1);

            careerTrackPanel.Controls.Add(careerTrackLbl);

            Panel careerTracksEncapsulatingPanel = new Panel();
            careerTracksEncapsulatingPanel.Controls.Add(careerTrackPanel);
            careerTracksEncapsulatingPanel.CssClass = "CareerTrackEncapsulatingPanelStyle";

            return careerTracksEncapsulatingPanel;
        }

        /// <summary>
        /// Build the web control to display the career
        /// </summary>
        /// <param name="career_">The career to display in the UI</param>
        /// <returns>Reference to the UI Control</returns>
        private Control constructCareerUI(XmlNode career_)
        {
            Control result = null;

            XmlAttribute careerTitleAttr = career_.Attributes[CAREER_NODE_TITLE_ATTR];

            if (careerTitleAttr != null)
            {
                Panel linkPanel = new Panel();
                linkPanel.CssClass = "CareerPathLinkPanelStyle";

                Control link = null;
                if (_showSelectiveCareerPathId == -1)
                {
                    link = new HyperLink();
                    ((HyperLink)link).NavigateUrl = "/Pages/Career/";
                    ((HyperLink)link).CssClass = "CarProgBoxEncapLinkStyle";
                }
                else
                { //NO need to navigate to a different page because assumption is that if _showSelectiveCareerPathId != -1, we're visible in the career page
                    link = new LinkButton();
                    ((LinkButton)link).PostBackUrl = "";
                    ((LinkButton)link).Command += new CommandEventHandler(CareerProgression_Command);
                    ((LinkButton)link).CommandArgument = careerTitleAttr.Value;
                    ((LinkButton)link).CssClass = "CarProgBoxEncapLinkStyle";
                    ((LinkButton)link).ID = "link" + _linkIDTracker;
                }

                ++_linkIDTracker;

                Panel careerTopPanel = new Panel();
                careerTopPanel.CssClass = "CarProgBoxTopStyle";

                Panel careerPanel = new Panel();
                careerPanel.CssClass = "CarProgBoxMiddleStyle";

                Label careerNameLabel = new Label();
                careerNameLabel.CssClass = "CarProgBoxLblStyle";
                careerNameLabel.Text = careerTitleAttr.Value;

                careerPanel.Controls.Add(careerNameLabel);

                Panel careerBottomPanel = new Panel();
                careerBottomPanel.CssClass = "CarProgBoxBottomStyle";

                link.Controls.Add(careerTopPanel);
                link.Controls.Add(careerPanel);
                link.Controls.Add(careerBottomPanel);

                linkPanel.Controls.Add(link);

                result = linkPanel;
            }

            return result;
        }

        void CareerProgression_Command(object sender, CommandEventArgs e)
        {
            int careerPathId = 1;//TEMP Value
            int careerId = 1; //TEMP VALUE

            NotifyLoadCareerDetails(careerPathId, careerId, (string)e.CommandArgument);
        }
    }
}