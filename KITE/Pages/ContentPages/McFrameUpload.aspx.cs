using KITE.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class McFrameUpload : System.Web.UI.Page
    {
        private List<McFrameWithKilosConvertionViewModel> CsvDataList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && CsvDataGridView.DataSource == null)
            {
                btnUpload.Enabled = false;
            }
        }

        protected void btnView(object sender, EventArgs e)
        {
            Tuple<DataTable, Exception> dataTableRes = new UtilityModel().BindGridview("McFrame_Cost_Table", yearPeriodTxt.Text, monthPeriodTxt.Text, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (dataTableRes.Item2 != null)
            {
                new UtilityModel().UploadCsvErrorHandler(dataTableRes.Item2, new GridView[] { }, errorLabel);
                CsvDataGridView.DataSource = null;
                CsvDataGridView.DataBind();
                btnDownloadCsv.Enabled = false;
                return;
            }
            CsvDataList = new McFrameFunctionModel().McFrameDatatableToList(dataTableRes.Item1);
            McFrameBindGridView();
            btnDownloadCsv.Enabled = true;
        }

        public void btnDownloadToCsv(object sender, EventArgs e)
        {
            LoadCsvData();
            DataTable dataTable = new ReadCsvModel().ConvertListToDataTable(CsvDataList);
            string fileName = $"{DateTime.Now:yyyyMMdd}KITE_McFrame.csv";
            string csvContent = new ReadCsvModel().DataTableToCsv(dataTable);

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}");
            Response.Write(csvContent);
            Response.End();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            errorLabel.Text = "";
            McFrameFunctionModel mcFrameFunction = new McFrameFunctionModel();
            Tuple<Exception, string, List<McFrameWithKilosConvertionViewModel>> fileResult = mcFrameFunction.McFrameReadCsvFile(fileUpload);
            if (fileResult.Item1.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mensubmit file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload McFrame.<br/> Detail : " + fileResult.Item1.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
            }
            else if (fileResult.Item2 != null && fileResult.Item3 != null)
            {
                btnUpload.Enabled = true;
                btnDownloadCsv.Enabled = true;
                CsvDataList = fileResult.Item3;
                Session["FilePath"] = fileResult.Item2;
                McFrameBindGridView();
            }
        }

        protected void CsvDataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageSize = CsvDataGridView.PageSize;
                int pageIndex = CsvDataGridView.PageIndex;
                int sequenceNumber = pageIndex * pageSize + e.Row.RowIndex + 1;

                Label lblSequence = (Label)e.Row.FindControl("lblSequenceNo");
                lblSequence.Text = sequenceNumber.ToString();
            }
        }

        protected void CsvDataGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CsvDataGridView.PageIndex = e.NewPageIndex;
            LoadCsvData();
            McFrameBindGridView();
        }

        protected void CsvPageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(CsvPageSizeDropDown.SelectedValue);
            CsvDataGridView.PageSize = selectedPageSize;

            LoadCsvData();
            McFrameBindGridView();
        }

        private void McFrameBindGridView()
        {
            ViewState["Row"] = 0;
            CsvDataGridView.DataSource = CsvDataList;
            CsvDataGridView.PageSize = int.Parse(CsvPageSizeDropDown.SelectedValue);
            CsvDataGridView.DataBind();

            if (CsvDataList.Count() > 0)
            {
                if (ViewState["Row"].ToString().Trim() == "0")
                {
                    ViewState["Row"] = 1;
                    ViewState["grandtotal"] = CsvDataList.Count;
                    lblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["grandtotal"]);

                    int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / CsvDataGridView.PageSize));
                    lblTotalNumberOfPages.Text = pageCount.ToString();
                    txtGoToPage.Text = (CsvDataGridView.PageIndex + 1).ToString();
                }

            }
            else
            {
                ViewState["grandtotal"] = 0;
            }
        }

        protected void GoToPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / CsvDataGridView.PageSize));
                if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
                {
                    LoadPage(pageNumber - 1);
                }
                else
                {
                    LoadPage(0);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mau membuka halaman " + txtGoToPage.Text.Trim() + ". Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
            }
        }

        protected void btnPrev_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 1)
                {
                    LoadPage(pageNumber - 2);
                }
                else
                {
                    LoadPage(CsvDataGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol previous page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
            }
        }

        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / CsvDataGridView.PageSize));
                if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber < pageCount)
                {
                    LoadPage(pageNumber);
                }
                else
                {
                    LoadPage(CsvDataGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol next page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
            }
        }

        private void LoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            CsvDataGridView_PageIndexChanging(this, e);
        }

        private void LoadCsvData()
        {
            try
            {
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("mcFrame", (string)Session["FilePath"], ";", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    UtilityModel errorHandler = new UtilityModel();
                    Exception loadCsvException = new Exception("Terdapat masalah ketika memuat file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload McFrame process.<br/> Detail : " + readCsvResult.Item2.Message);
                    errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                }
                CsvDataList = (List<McFrameWithKilosConvertionViewModel>)readCsvResult.Item1;
                return;
            }
            catch (Exception ex)
            {
                Tuple<DataTable, Exception> dataTableRes = new UtilityModel().BindGridview("McFrame_Cost_Table", yearPeriodTxt.Text, monthPeriodTxt.Text, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (dataTableRes.Item2 == null)
                {
                    CsvDataList = new McFrameFunctionModel().McFrameDatatableToList(dataTableRes.Item1);
                    return;
                }
                else
                {
                    UtilityModel errorHandler = new UtilityModel();
                    Exception loadCsvException = new Exception("Terdapat masalah ketika memuat file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload McFrame process.<br/> Detail : " + ex.Message);
                    errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "McFrame_Cost_Table";
            DatabaseModel databaseModel = new DatabaseModel();
            UtilityModel utility = new UtilityModel();
            Exception loadCsvException;

            try
            {
                LoadCsvData();
                McFrameFunctionModel csvDataProcess = new McFrameFunctionModel();
                Tuple<string, ArrayList> columnNameAndData = csvDataProcess.McFrameGenerateColumnAndCsvData(CsvDataList);
                foreach (object csvDataObject in (List<object>)columnNameAndData.Item2[0])
                {
                    if (index == 0) { yearPeriod = (int)csvDataObject; }
                    if (index == 1) { monthPeriod = (int)csvDataObject; break; }
                    index++;
                }

                Tuple<DataTable, Exception> checkRMperBatch = new DatabaseModel().SelectTableIntoDataTable("UUID", "RM_per_Batch", $" WHERE Year_Period = '{yearPeriod}' AND Month_Period = '{monthPeriod}' ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (checkRMperBatch.Item1.Rows.Count > 0)
                {
                    loadCsvException = new Exception("Periode yang anda pilih sudah memiliki kalkulasi RM per Batch. Upload file tidak dapat dilakukan. Silahkan menghubungi KITE support untuk bantuan lebih lanjut.");
                    utility.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                    return;
                }

                Tuple<DataTable, Exception> checkFGperBatch = new DatabaseModel().SelectTableIntoDataTable("UUID", "FG_per_Batch", $" WHERE Year_Period = '{yearPeriod}' AND Month_Period = '{monthPeriod}' ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (checkFGperBatch.Item1.Rows.Count > 0)
                {
                    loadCsvException = new Exception("Periode yang anda pilih sudah memiliki kalkulasi FG per Batch. Upload file tidak dapat dilakukan. Silahkan menghubungi KITE support untuk bantuan lebih lanjut.");
                    utility.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                    return;
                }

                if (forcePushData.Checked)
                {
                    Exception deleteResult = databaseModel.DeleteData(tableName, $" Year_Period = '{yearPeriod}' AND Month_Period = '{monthPeriod}' ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    if (deleteResult.Message != "Data period sebelumnya berhasil dibersihkan.")
                    {
                        utility.UploadCsvErrorHandler(deleteResult, new GridView[] { CsvDataGridView }, errorLabel);
                    }
                }

                Exception checkPeriodResult = databaseModel.PeriodCheck(tableName, yearPeriod, monthPeriod, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (checkPeriodResult.Message != "Data Period Aman.")
                {
                    loadCsvException = new Exception(checkPeriodResult.Message);
                    utility.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                }

                Tuple<string[], Exception> iterationResult = new ReadCsvModel().IterateCsvObject(columnNameAndData.Item2);
                if (iterationResult.Item2.Message != "Pemrosesan IterateCsvObject Berhasil.")
                {
                    loadCsvException = new Exception($"Gagal dalam penulisan ke database. Detail : {iterationResult.Item2.Message}");
                    utility.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                }

                if (checkPeriodResult.Message == "Data Period Aman." && iterationResult.Item2.Message == "Pemrosesan IterateCsvObject Berhasil.")
                {
                    foreach (string iterionValue in iterationResult.Item1)
                    {
                        Exception insertResult = new DatabaseModel().InsertIntoTable(tableName, columnNameAndData.Item1, iterionValue, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                        if (insertResult.Message != "null")
                        {
                            loadCsvException = new Exception($"Terjadi kesalahan ketika Insert Into Table {tableName}. Detail : {insertResult.Message}");
                            utility.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
                            break;
                        }
                    }

                    if (File.Exists(Session["FilePath"].ToString()))
                    {
                        File.Delete(Session["FilePath"].ToString());
                    }
                    Session["FilePath"] = "";
                    string script = $"alert('Upload CSV berhasil.'); window.location.href = '{ResolveUrl("~/Pages/ContentPages/McFrameUpload.aspx")}';";
                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                loadCsvException = new Exception(ex.Message);
                utility.UploadCsvErrorHandler(loadCsvException, new GridView[] { CsvDataGridView }, errorLabel);
            }
        }
    }
}