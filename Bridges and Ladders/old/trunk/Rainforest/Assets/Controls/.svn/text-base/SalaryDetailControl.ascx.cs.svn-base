using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Rainforest.Assets.Controls
{
    public partial class SalaryDetailControl : Rainforest.Assets.Controls.CompanySalaryControl
    {
        private const double _headerColumnWidth = 552.0;

        /// <summary>
        /// Width of the column containing the x-axis and salary data
        /// </summary>
        protected override double HEADER_COLUMN_WIDTH
        {
            get
            {
                return _headerColumnWidth;
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            string[] xAxisLabels = { "$13k", "$26k", "$39k", "$52k", "$65k" };
            showXAxis(xAxisLabels, SalaryDetailsTbl);
            addSalaryDataToView("Base Salary", 54000.00, 60000, 57000.00, xAxisLabels, SalaryDetailsTbl, "SalaryDetailOddLblColumn", "SalaryDetailOddDataColumn");

            addSalaryDataToView("Cash Bonus", 500.00, 1000.00, 2000.00, xAxisLabels, SalaryDetailsTbl, "SalaryDetailLblColumn", "SalaryDetailDataColumn");
            addSalaryDataToView("Profit Sharing", 300.00, 750.00, 1500.00, xAxisLabels, SalaryDetailsTbl, "SalaryDetailOddLblColumn", "SalaryDetailOddDataColumn");
        }

        private void showXAxis(string[] xAxisLabels_, HtmlTable tableToAddAxisIn_)
        {
            HtmlTableRow tableRow = new HtmlTableRow();
            HtmlTableCell blankTableCell = new HtmlTableCell();
            HtmlTableCell xAxisTableCell = new HtmlTableCell();

            tableRow.Cells.Add(blankTableCell);
            tableRow.Cells.Add(xAxisTableCell);

            blankTableCell.Attributes["class"] = "SalaryDetailsAxisLabelColumn";
            xAxisTableCell.Attributes["class"] = "SalaryDetailsAxisColumn";

            Literal blankLiteral = new Literal();
            blankLiteral.Text = "&nbsp;";
            blankTableCell.Controls.Add(blankLiteral);

            //add x-axis to the UI
            Panel xAxisPanel = new Panel();
            xAxisPanel.CssClass = "xAxisPanelStyle";
            base.showXAxis(xAxisLabels_, xAxisPanel);

            xAxisTableCell.Controls.Add(xAxisPanel);

            //add row to table
            tableToAddAxisIn_.Rows.Add(tableRow);
        }
    }
}