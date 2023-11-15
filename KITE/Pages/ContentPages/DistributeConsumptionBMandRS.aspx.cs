using KITE.Models;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class DistributeConsumptionBMandRS : System.Web.UI.Page
    {
        private DataTable globalRMperBatchDataTable;
        private DataTable globalFGperBatchDataTable;
        private string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                errorLabel.Text = "";
            }
        }

        public void btnCalculate_Click(object sender, EventArgs e)
        {
            object currentYear = yearPeriodTxt.Text;
            object currentMonth = monthPeriodTxt.Text;
            object[] param = new[] { currentYear, currentMonth };
            GridView[] gridArray = new GridView[] { RMperBatchGridView, FGperBatchGridView };

            if (yearPeriodTxt.Text != "" && monthPeriodTxt.Text != "")
            {
                try
                {
                    Exception returningBalanceGIResult = new DatabaseModel().ExecStoreProcedure("ReturnRMperBatchDistributedValue", param, ConnectionString);
                    if (returningBalanceGIResult != null)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses ReturnRMperBatchDistributedValue. Detail : {returningBalanceGIResult.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }
                    Exception balanceGIResult = new DatabaseModel().ExecStoreProcedure("CreatingBalanceGI", param, ConnectionString);
                    if (balanceGIResult != null)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses CreatingBalanceGI. Detail : {balanceGIResult.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }
                    Exception rmperBatchResult = new DatabaseModel().ExecStoreProcedure("CreatingRMperBatch", param, ConnectionString);
                    if (rmperBatchResult != null)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses CreatingRMperBatch. Detail : {rmperBatchResult.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }
                    Exception fgperBatchResult = new DatabaseModel().ExecStoreProcedure("CreatingFGperBatch", param, ConnectionString);
                    if (fgperBatchResult != null)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses CreatingFGperBatch. Detail : {fgperBatchResult.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }

                    Tuple<DataTable, Exception> RMperBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" * ", "RM_per_Batch", " ORDER BY Finish_Goods ASC, Raw_Material ASC, Batch_Sequence ASC ", ConnectionString);
                    if (RMperBatchDataTable.Item2.Message != "null")
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table RM_per_Batch. Detail : {RMperBatchDataTable.Item2.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }
                    else
                    {
                        globalRMperBatchDataTable = RMperBatchDataTable.Item1;
                        RMperBatchBindGridView();
                    }

                    Tuple<DataTable, Exception> FGperBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" * ", "FG_per_Batch", " ORDER BY Finish_Goods ASC, FG_Batch ASC, Raw_Material ASC, RM_Batch_Sequence ASC ", ConnectionString);
                    if (FGperBatchDataTable.Item2.Message != "null")
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table FG_per_Batch. Detail : {FGperBatchDataTable.Item2.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, gridArray, errorLabel);
                        return;
                    }
                    else
                    {
                        globalFGperBatchDataTable = FGperBatchDataTable.Item1;
                        FGperBatchBindGridView();
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
        private void RMperBatchBindGridView()
        {
            ViewState["RMRow"] = 0;
            RMperBatchGridView.DataSource = globalRMperBatchDataTable;
            RMperBatchGridView.PageSize = int.Parse(RMperBatchPageSizeDropDown.SelectedValue);
            RMperBatchGridView.DataBind();

            if (globalRMperBatchDataTable.Rows.Count > 0)
            {
                if (ViewState["RMRow"].ToString().Trim() == "0")
                {
                    ViewState["RMRow"] = 1;
                    ViewState["RMgrandtotal"] = globalRMperBatchDataTable.Rows.Count;
                    RMlblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["RMgrandtotal"]);

                    int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["RMgrandtotal"]) / RMperBatchGridView.PageSize));
                    RMperBatchlblTotalNumberOfPages.Text = pageCount.ToString();
                    RMperBatchGoToPageTxt.Text = (RMperBatchGridView.PageIndex + 1).ToString();
                }

            }
            else
            {
                ViewState["RMgrandtotal"] = 0;
            }
        }

        protected void RMperBatchGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageSize = RMperBatchGridView.PageSize;
                int pageIndex = RMperBatchGridView.PageIndex;
                int sequenceNumber = pageIndex * pageSize + e.Row.RowIndex + 1;

                Label lblSequence = (Label)e.Row.FindControl("RMperBatchlblSequenceNo");
                lblSequence.Text = sequenceNumber.ToString();
            }
        }

        protected void RMperBatchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            RMperBatchGridView.PageIndex = e.NewPageIndex;
            LoadRMperBatchData();
            RMperBatchBindGridView();
        }

        protected void RMperBatchPageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(RMperBatchPageSizeDropDown.SelectedValue);
            RMperBatchGridView.PageSize = selectedPageSize;

            LoadRMperBatchData();
            RMperBatchBindGridView();
        }

        protected void RMperBatchGoToPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["RMgrandtotal"]) / RMperBatchGridView.PageSize));
                if (int.TryParse(RMperBatchGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
                {
                    RMperBatchLoadPage(pageNumber - 1);
                }
                else
                {
                    RMperBatchLoadPage(0);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mau membuka halaman " + RMperBatchGoToPageTxt.Text.Trim() + ". Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { RMperBatchGridView }, errorLabel);
            }
        }

        protected void RMperBatchbtnPrev_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                if (int.TryParse(RMperBatchGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber > 1)
                {
                    RMperBatchLoadPage(pageNumber - 2);
                }
                else
                {
                    RMperBatchLoadPage(RMperBatchGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol previous page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { RMperBatchGridView }, errorLabel);
            }
        }

        protected void RMperBatchbtnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["RMgrandtotal"]) / RMperBatchGridView.PageSize));
                if (int.TryParse(RMperBatchGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber < pageCount)
                {
                    RMperBatchLoadPage(pageNumber);
                }
                else
                {
                    RMperBatchLoadPage(RMperBatchGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol next page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { RMperBatchGridView }, errorLabel);
            }
        }

        private void RMperBatchLoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            RMperBatchGridView_PageIndexChanging(this, e);
        }

        private void LoadRMperBatchData()
        {
            Tuple<DataTable, Exception> RMperBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" * ", "RM_per_Batch", " ORDER BY Finish_Goods, Raw_Material ASC ", ConnectionString);
            if (RMperBatchDataTable.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table RM_per_Batch. Detail : {RMperBatchDataTable.Item2.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { RMperBatchGridView }, errorLabel);
            }
            else
            {
                globalRMperBatchDataTable = RMperBatchDataTable.Item1;
                RMperBatchBindGridView();
            }
        }

        // FG per Batch Region
        private void FGperBatchBindGridView()
        {
            ViewState["FGRow"] = 0;
            FGperBatchGridView.DataSource = globalFGperBatchDataTable;
            FGperBatchGridView.PageSize = int.Parse(FGperBatchPageSizeDropDown.SelectedValue);
            FGperBatchGridView.DataBind();

            if (globalFGperBatchDataTable.Rows.Count > 0)
            {
                if (ViewState["FGRow"].ToString().Trim() == "0")
                {
                    ViewState["FGRow"] = 1;
                    ViewState["FGgrandtotal"] = globalFGperBatchDataTable.Rows.Count;
                    FGlblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["FGgrandtotal"]);

                    int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGgrandtotal"]) / FGperBatchGridView.PageSize));
                    FGperBatchlblTotalNumberOfPages.Text = pageCount.ToString();
                    FGperBatchGoToPageTxt.Text = (FGperBatchGridView.PageIndex + 1).ToString();
                }
            }
            else
            {
                ViewState["FGgrandtotal"] = 0;
            }
        }

        protected void FGperBatchGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageSize = FGperBatchGridView.PageSize;
                int pageIndex = FGperBatchGridView.PageIndex;
                int sequenceNumber = pageIndex * pageSize + e.Row.RowIndex + 1;

                Label lblSequence = (Label)e.Row.FindControl("FGperBatchlblSequenceNo");
                lblSequence.Text = sequenceNumber.ToString();
            }
        }

        protected void FGperBatchGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FGperBatchGridView.PageIndex = e.NewPageIndex;
            LoadFGperBatchData();
            FGperBatchBindGridView();
        }

        protected void FGperBatchPageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(FGperBatchPageSizeDropDown.SelectedValue);
            FGperBatchGridView.PageSize = selectedPageSize;

            LoadFGperBatchData();
            FGperBatchBindGridView();
        }

        protected void FGperBatchGoToPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGgrandtotal"]) / FGperBatchGridView.PageSize));
                if (int.TryParse(FGperBatchGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
                {
                    FGperBatchLoadPage(pageNumber - 1);
                }
                else
                {
                    FGperBatchLoadPage(0);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mau membuka halaman " + RMperBatchGoToPageTxt.Text.Trim() + ". Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGperBatchGridView }, errorLabel);
            }
        }

        protected void FGperBatchbtnPrev_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                if (int.TryParse(FGperBatchGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber > 1)
                {
                    FGperBatchLoadPage(pageNumber - 2);
                }
                else
                {
                    FGperBatchLoadPage(FGperBatchGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol previous page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGperBatchGridView }, errorLabel);
            }
        }

        protected void FGperBatchbtnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGgrandtotal"]) / FGperBatchGridView.PageSize));
                if (int.TryParse(FGperBatchGoToPageTxt.Text.Trim(), out pageNumber) && pageNumber < pageCount)
                {
                    FGperBatchLoadPage(pageNumber);
                }
                else
                {
                    FGperBatchLoadPage(FGperBatchGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol next page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGperBatchGridView }, errorLabel);
            }
        }

        private void FGperBatchLoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            FGperBatchGridView_PageIndexChanging(this, e);
        }

        private void LoadFGperBatchData()
        {
            Tuple<DataTable, Exception> FGperBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" * ", "FG_per_Batch", " ORDER BY Finish_Goods, Raw_Material ASC ", ConnectionString);
            if (FGperBatchDataTable.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table RM_per_Batch. Detail : {FGperBatchDataTable.Item2.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { FGperBatchGridView }, errorLabel);
            }
            else
            {
                globalFGperBatchDataTable = FGperBatchDataTable.Item1;
                FGperBatchBindGridView();
            }
        }
    }
}