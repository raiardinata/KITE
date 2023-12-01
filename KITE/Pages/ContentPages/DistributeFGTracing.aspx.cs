using KITE.Models;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class DistributeFGTracing : System.Web.UI.Page
    {
        private DataTable globalFGTracingDataTable;
        private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                errorLabel.Text = "";
            }

        }
        public void ViewData(object sender, EventArgs e)
        {
            LoadFGTracingData();
        }

        public void btnDownloadToCsv(object sender, EventArgs e)
        {
            LoadFGTracingData();
            string fileName = $"{DateTime.Now:yyyyMMdd}KITE_FGTracing.csv";
            string csvContent = new ReadCsvModel().DataTableToCsv(globalFGTracingDataTable);

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}");
            Response.Write(csvContent);
            Response.End();
        }

        public void btnCalculate_Click(object sender, EventArgs e)
        {
            object currentYear = yearPeriodTxt.Text;
            object currentMonth = monthPeriodTxt.Text;
            object[] param = new[] { currentYear, currentMonth };
            GridView[] gridArray = new GridView[] { FGTracingGridView };

            if (yearPeriodTxt.Text != "" && monthPeriodTxt.Text != "")
            {
                try
                {
                    Exception createFGTracingResult = new DatabaseModel().ExecStoreProcedure("CreatingTracingFG", param, ConnectionString);
                    if (createFGTracingResult != null)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses ReturnFGTracingDistributedValue. Detail : {createFGTracingResult.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }

                    Tuple<DataTable, Exception> FGTracingDataTable = new DatabaseModel().SelectTableIntoDataTable(" * ", "FG_Tracing", $" WHERE Year_Period = '{yearPeriodTxt.Text}' AND Month_Period = '{monthPeriodTxt.Text}' ORDER BY FG_Batch ASC, Finish_Goods ASC, Raw_Material ASC, RM_Batch_Sequence ASC, RM_Batch ASC ", ConnectionString);
                    if (FGTracingDataTable.Item2.Message != "null")
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table FG_Tracing. Detail : {FGTracingDataTable.Item2.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }
                    else
                    {
                        globalFGTracingDataTable = FGTracingDataTable.Item1;
                        FGTracingBindGridView();
                        btnDownloadCsv.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    UtilityModel errorHandler = new UtilityModel();
                    Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses calculate BM & RS.<br>Detail : {ex.Message}");
                    errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                }
            }
            else
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Year Period dan Month Period tidak boleh kosong.");
                errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
            }


        }

        // RM per Batch Region
        private void FGTracingBindGridView()
        {
            ViewState["FGTRow"] = 0;
            FGTracingGridView.DataSource = globalFGTracingDataTable;
            FGTracingGridView.PageSize = int.Parse(FGTracingPageSizeDropDown.SelectedValue);
            FGTracingGridView.DataBind();

            if (globalFGTracingDataTable.Rows.Count > 0)
            {
                if (ViewState["FGTRow"].ToString().Trim() == "0")
                {
                    ViewState["FGTRow"] = 1;
                    ViewState["FGTgrandtotal"] = globalFGTracingDataTable.Rows.Count;
                    FGTlblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["FGTgrandtotal"]);

                    int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGTgrandtotal"]) / FGTracingGridView.PageSize));
                    FGTracinglblTotalNumberOfPages.Text = pageCount.ToString();
                    FGTracingGoToPageTxt.Text = (FGTracingGridView.PageIndex + 1).ToString();
                }

            }
            else
            {
                ViewState["FGTgrandtotal"] = 0;
            }
        }

        protected void FGTracingGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageSize = FGTracingGridView.PageSize;
                int pageIndex = FGTracingGridView.PageIndex;
                int sequenceNumber = pageIndex * pageSize + e.Row.RowIndex + 1;

                Label lblSequence = (Label)e.Row.FindControl("FGTracinglblSequenceNo");
                lblSequence.Text = sequenceNumber.ToString();
            }
        }

        protected void FGTracingGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FGTracingGridView.PageIndex = e.NewPageIndex;
            LoadFGTracingData();
            FGTracingBindGridView();
        }

        protected void FGTracingPageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(FGTracingPageSizeDropDown.SelectedValue);
            FGTracingGridView.PageSize = selectedPageSize;

            LoadFGTracingData();
            FGTracingBindGridView();
        }

        protected void FGTracingGoToPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGTgrandtotal"]) / FGTracingGridView.PageSize));
                if (int.TryParse(FGTracingGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
                {
                    FGTracingLoadPage(pageNumber - 1);
                }
                else
                {
                    FGTracingLoadPage(0);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mau membuka halaman " + FGTracingGoToPageTxt.Text.Trim() + ". Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGTracingGridView }, errorLabel);
            }
        }

        protected void FGTracingbtnPrev_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                if (int.TryParse(FGTracingGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber > 1)
                {
                    FGTracingLoadPage(pageNumber - 2);
                }
                else
                {
                    FGTracingLoadPage(FGTracingGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol previous page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGTracingGridView }, errorLabel);
            }
        }

        protected void FGTracingbtnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGTgrandtotal"]) / FGTracingGridView.PageSize));
                if (int.TryParse(FGTracingGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber < pageCount)
                {
                    FGTracingLoadPage(pageNumber);
                }
                else
                {
                    FGTracingLoadPage(FGTracingGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol next page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGTracingGridView }, errorLabel);
            }
        }

        private void FGTracingLoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            FGTracingGridView_PageIndexChanging(this, e);
        }

        private void LoadFGTracingData()
        {
            Tuple<DataTable, Exception> FGTracingDataTable = new DatabaseModel().SelectTableIntoDataTable(" * ", "FG_Tracing", $" WHERE Year_Period = '{yearPeriodTxt.Text}' AND Month_Period = '{monthPeriodTxt.Text}' ORDER BY FG_Batch ASC, Finish_Goods ASC, Raw_Material ASC, RM_Batch_Sequence ASC, RM_Batch ASC ", ConnectionString);
            if (FGTracingDataTable.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table FG_Tracing. Detail : {FGTracingDataTable.Item2.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGTracingGridView }, errorLabel);
            }
            else
            {
                globalFGTracingDataTable = FGTracingDataTable.Item1;
                FGTracingBindGridView();
            }
        }
    }
}