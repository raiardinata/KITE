using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;

namespace KITE.Pages.ContentPages
{
    public partial class FG_Tracing_Report_Page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FG_Tracing_ReportViewer.ZoomMode = Microsoft.Reporting.WebForms.ZoomMode.Percent;
                FG_Tracing_ReportViewer.ZoomPercent = 75;
            }
        }

        public void GenerateReport(object sender, EventArgs e)
        {
            // Create a list of ReportParameter objects
            List<ReportParameter> parameters = new List<ReportParameter>
            {
                new ReportParameter("RM_Batch", (noPIBTxt.Text == "") ? "%%": noPIBTxt.Text),
                new ReportParameter("NO_PEB", (noPEBTxt.Text == "") ? "%%": noPEBTxt.Text),
                new ReportParameter("PO_Number", (invoiceNoTxt.Text == "") ? "%%": invoiceNoTxt.Text),
                new ReportParameter("Customer", (customerTxt.Text == "") ? "%%": customerTxt.Text),
                new ReportParameter("PGI_Date_From", (pgiDateFrom.Text == "") ? "2023/1/1": pgiDateFrom.Text),
                new ReportParameter("PGI_Date_Until", (pgiDateUntil.Text == "") ? "2023/12/31": pgiDateUntil.Text),
            };

            FG_Tracing_ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            FG_Tracing_ReportViewer.ServerReport.ReportServerUrl = new Uri("http://127.0.0.1/ReportServer");
            FG_Tracing_ReportViewer.ServerReport.ReportPath = "/KITE_Report/KITE_FG_Tracing";
            FG_Tracing_ReportViewer.ServerReport.SetParameters(parameters);
            FG_Tracing_ReportViewer.ServerReport.Refresh();
        }
    }
}