using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Rainforest.Assets.Controls
{
    public partial class CompanySalaryControl : System.Web.UI.UserControl
    {
        private const double _headerColumnWidth = 265.0;

        /// <summary>
        /// Width of the column containing the x-axis and salary data
        /// </summary>
        protected virtual double HEADER_COLUMN_WIDTH
        {
            get
            {
                return _headerColumnWidth;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] xAxisLabels = { "$50k", "$100k", "$150k", "$200k", "$250k" };
            showXAxis(xAxisLabels, xAxisPanel);

            addSalaryDataToView("Analyst", 54000.00, 60000, 57000.00, xAxisLabels, SalaryDataTable, "CompanySalaryPosColumn", "CompanySalaryDataColumn");
            addSalaryDataToView("Consultant", 62000.00, 75000.00, 57000.00, xAxisLabels, SalaryDataTable, "CompanySalaryPosColumn", "CompanySalaryDataColumn");
            addSalaryDataToView("Senior Consultant", 70000.00, 100000.00, 57000.00, xAxisLabels, SalaryDataTable, "CompanySalaryPosColumn", "CompanySalaryDataColumn");
            addSalaryDataToView("Manager", 105000.00, 125000.00, 125000.00, xAxisLabels, SalaryDataTable, "CompanySalaryPosColumn", "CompanySalaryDataColumn");
            addSalaryDataToView("Senior Manager", 130000.00, 225000.00, 57000.00, xAxisLabels, SalaryDataTable, "CompanySalaryPosColumn", "CompanySalaryDataColumn");
        }

        protected void addSalaryDataToView(string positionName_, double salaryRangeLow_, double salaryRangeHigh_, double meanSalary_, 
            string[] xAxisLabels_, HtmlTable tableToAddInfoIn_, string labelColumnCSS_, string dataColumnCss_)
        {
            double numSections = xAxisLabels_.Length + 1.0;
            int sectionWidths = (int)(HEADER_COLUMN_WIDTH / ((numSections > 0.0) ? numSections : 1.0));

            HtmlTableRow tableRow = new HtmlTableRow();
            HtmlTableCell positionTableCell = new HtmlTableCell();
            HtmlTableCell salaryTableCell = new HtmlTableCell();

            tableRow.Cells.Add(positionTableCell);
            tableRow.Cells.Add(salaryTableCell);

            //add css class references
            positionTableCell.Attributes["class"] = labelColumnCSS_;
            salaryTableCell.Attributes["class"] = dataColumnCss_;

            positionTableCell.Style.Add("vertical-align", "middle");

            HyperLink positionLink = new HyperLink();
            positionLink.Text = positionName_;

            positionTableCell.Controls.Add(positionLink);

            string salaryBarHTML = string.Empty;

            //add relative panel
            Literal relativeSalaryPanel = new Literal();
            relativeSalaryPanel.Text = "<div class=\"salaryRelativePanel\" style=\"vertical-align:middle\">";

            int salaryBarLowPosition = identifyPosition(salaryRangeLow_, xAxisLabels_, sectionWidths);
            int salaryBarHighPosition = identifyPosition(salaryRangeHigh_, xAxisLabels_, sectionWidths);
            int meanSalaryPosition = identifyPosition(meanSalary_, xAxisLabels_, sectionWidths);

            relativeSalaryPanel.Text += "<div class=" + ((salaryBarLowPosition > 30) ? "\"salaryBarText\"" : "\"salaryBarWhiteText\"") + " style=\"left:" +
                                        ((salaryBarLowPosition > 30) ? (salaryBarLowPosition - 30) : 0) + "px;" +
                                        "width:30px\">" + getSalaryDisplayFormat(salaryRangeLow_) + "</div>";

            relativeSalaryPanel.Text += "<div class=\"salaryBar\" style=\"left:" +
                                        salaryBarLowPosition + "px;" +
                                        "width:" + (salaryBarHighPosition - salaryBarLowPosition) + "px\" " + 
                                        ">";

            relativeSalaryPanel.Text += "<span><strong>Breakdown:</strong><br/><strong>Low:</strong> " + salaryRangeLow_.ToString("c") + " <br/> " +
                                        "<strong>Avg.:</strong> " + meanSalary_.ToString("c") + " <br/> " +
                                        "<strong>High:</strong> " + salaryRangeHigh_.ToString("c") + "</span></div>";

            relativeSalaryPanel.Text += "<div class=" + ((salaryBarHighPosition < (HEADER_COLUMN_WIDTH - 30)) ? "\"salaryBarText\"" : "\"salaryBarWhiteText\"") + " style=\"left:" +
                                        ((salaryBarHighPosition < (HEADER_COLUMN_WIDTH - 30)) ? (salaryBarHighPosition + 1) : (HEADER_COLUMN_WIDTH - 30)) + "px;" +
                                        "width:30px\">" + getSalaryDisplayFormat(salaryRangeHigh_);

            relativeSalaryPanel.Text += "</div>"; 
            relativeSalaryPanel.Text += "</div>";

            Literal blankSpaceLiteral = new Literal();
            blankSpaceLiteral.Text = "&nbsp;";

            salaryTableCell.Controls.Add(relativeSalaryPanel);
            salaryTableCell.Controls.Add(blankSpaceLiteral);

            tableToAddInfoIn_.Rows.Add(tableRow);
        }

        protected string getSalaryDisplayFormat(double salary_)
        {
            string result = string.Empty;

            if (salary_ >= 1000.00)
            {
                salary_ = (salary_ / 1000);
                result = "$" + ((int)salary_) + "k";
            }
            else
            {
                result = "$" + ((int)salary_);
            }

            return result;
        }

        /// <summary>
        /// Identify the position of the salary as a point on the x-Axis
        /// </summary>
        /// <param name="salary_">The salary to identify as a point</param>
        /// <param name="xAxisLabels_">The set of x-axis labels on the UI</param>
        /// <param name="sectionWidths_">The widths of section between salary ranges</param>
        /// <returns>x-Axis point value representing the salary</returns>
        protected int identifyPosition(double salary_, string[] xAxisLabels_, int sectionWidths_)
        {
            int positionVal = 0;
            int xAxisLblIndex = 0; //used to identify where the salary fits in the x-axis range

            try
            {
                //convert x-axis label values to 'double' numbers
                double[] salaryRanges = convertToNumbers(xAxisLabels_);

                //identify range under which the salary value fits
                while (xAxisLblIndex < salaryRanges.Length)
                {
                    if (salary_ > salaryRanges[xAxisLblIndex] && ((xAxisLblIndex + 1) < salaryRanges.Length))
                    {
                        ++xAxisLblIndex;
                    }
                    else
                    {
                        break;
                    }
                }

                //identify salary difference above the section low value (e.g. $57K salary in a 50-100k section would have a difference value of $7k from the section low)
                double salaryToLocateInSection = salary_ - ((xAxisLblIndex > 0) ? salaryRanges[xAxisLblIndex - 1] : 0.0);

                double sectionSalaryHighLowDiff = ((xAxisLblIndex > 0) ? (salaryRanges[xAxisLblIndex] - salaryRanges[xAxisLblIndex - 1]) : salaryRanges[xAxisLblIndex]);

                salaryToLocateInSection = (salaryToLocateInSection > (sectionSalaryHighLowDiff * 2)) ? (sectionSalaryHighLowDiff * 2) : salaryToLocateInSection;

                positionVal = (xAxisLblIndex * sectionWidths_) + (int)(((double)sectionWidths_) *
                              (salaryToLocateInSection / sectionSalaryHighLowDiff));
            }
            catch (Exception e)
            {

            }

            return positionVal;
        }

        /// <summary>
        /// Convert the given x-axis labels to integers
        /// </summary>
        /// <param name="xAxisLabels_">Reference to the x-axis labels to convert</param>
        /// <returns>Converted set of integers, representing x-axis values</returns>
        protected double[] convertToNumbers(string[] xAxisLabels_)
        {
            double[] result = new double[xAxisLabels_.Length];

            for (int i = 0; i < xAxisLabels_.Length; ++i)
            {
                xAxisLabels_[i] = xAxisLabels_[i].Replace(" ", "");
                xAxisLabels_[i] = xAxisLabels_[i].Replace("$", "");
                xAxisLabels_[i] = xAxisLabels_[i].Replace("k", "000");
                xAxisLabels_[i] = xAxisLabels_[i].Replace("m", "000000");
                xAxisLabels_[i] = xAxisLabels_[i].Replace("mm", "000000");

                result[i] = double.Parse(xAxisLabels_[i]);
            }

            return result;
        }

        /// <summary>
        /// Add the controls for the x-axis on the salary UI
        /// </summary>
        /// <param name="xAxisLabels">The labels to use for the axis</param>
        /// <param name="panelToAxisIn_">Reference to the panel in which the axis should be added under</param>
        protected void showXAxis(string[] xAxisLabels_, Panel panelToAxisIn_)
        {
            double numSections = xAxisLabels_.Length + 1.0;
            int sectionWidths = (int)(HEADER_COLUMN_WIDTH / ((numSections > 0.0) ? numSections : 1.0));

            for (int i = 0; i < xAxisLabels_.Length; ++i)
            {
                //add axis ticks
                Panel xAxisTickSection = new Panel();
                xAxisTickSection.CssClass = "xAxisTicks";
                xAxisTickSection.Style.Add("width", sectionWidths + "px");

                int leftTickOffset = (i * sectionWidths);
                xAxisTickSection.Style.Add("left", leftTickOffset + "px");

                panelToAxisIn_.Controls.Add(xAxisTickSection);

                //add axis labels
                Panel xAxisSection = new Panel();
                xAxisSection.CssClass = "xAxis";

                int leftOffset = (sectionWidths / 2) + (i * sectionWidths);
                xAxisSection.Style.Add("left", leftOffset + "px");
                xAxisSection.Style.Add("width", sectionWidths + "px");

                Literal label = new Literal();
                label.Text = xAxisLabels_[i];

                xAxisSection.Controls.Add(label);
                panelToAxisIn_.Controls.Add(xAxisSection);
            }
        }
    }
}