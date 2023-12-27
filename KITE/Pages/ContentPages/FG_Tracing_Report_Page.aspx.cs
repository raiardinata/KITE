using KITE.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;

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
            //if (pgiDateFrom.Text == "")
            //{
            //    UtilityModel errorHandler = new UtilityModel();
            //    Exception loadCsvException = new Exception($"Mohon isi terlebih dahulu PGI Date From dan PGI Date Until.");
            //    errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
            //    return;
            //}
            //if (pgiDateUntil.Text == "")
            //{
            //    UtilityModel errorHandler = new UtilityModel();
            //    Exception loadCsvException = new Exception($"Mohon isi terlebih dahulu PGI Date From dan PGI Date Until.");
            //    errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
            //    return;
            //}

            // Update PIB date
            Exception updateException = new DatabaseModel().UpdateTable(" FG_Tracing ", " PIB_Date = (SELECT PIB_Date FROM Master_Batch b WHERE b.Raw_Material = FG_Tracing.Raw_Material AND b.RM_Batch = FG_Tracing.RM_Batch) ", "", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (updateException.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Update PIB Date Gagal. Detail : {updateException.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
            }

            // Create a list of ReportParameter objects
            List<ReportParameter> parameters = new List<ReportParameter>
            {
                new ReportParameter("RM_Batch", (noPIBTxt.Text == "") ? "%%": noPIBTxt.Text),
                new ReportParameter("NO_PEB", (noPEBTxt.Text == "") ? "%%": noPEBTxt.Text),
                new ReportParameter("PO_Number", (invoiceNoTxt.Text == "") ? "%%": invoiceNoTxt.Text),
                new ReportParameter("Customer", (customerTxt.Text == "") ? "%%": customerTxt.Text),
                new ReportParameter("PGI_Date_From", (pgiDateFrom.Text == "") ? "2019/1/1": pgiDateFrom.Text),
                new ReportParameter("PGI_Date_Until", (pgiDateUntil.Text == "") ? "2200/12/31": pgiDateUntil.Text),
            };

            FG_Tracing_ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            FG_Tracing_ReportViewer.ServerReport.ReportServerUrl = new Uri("http://127.0.0.1/ReportServer");
            FG_Tracing_ReportViewer.ServerReport.ReportPath = "/ReportKITE/KITE_FG_Tracing";
            FG_Tracing_ReportViewer.ServerReport.SetParameters(parameters);
            FG_Tracing_ReportViewer.ServerReport.Refresh();
        }
    }
}