using CsvHelper;
using KITE.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class McFrameUpload : System.Web.UI.Page
    {
        private List<McFrameViewModel> CsvDataList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && CsvDataGridView.DataSource == null)
            {
                btnUpload.Enabled = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            errorLabel.Text = "";
            McFrameFunctionModel mcFrameFunction = new McFrameFunctionModel();
            Tuple<Exception, string, List<McFrameViewModel>> fileResult = mcFrameFunction.McFrameReadCsvFile(fileUpload);
            if (fileResult.Item1.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mensubmit file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload McFrame.<br/> Detail : " + fileResult.Item1.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
            }
            else if (fileResult.Item2 != null && fileResult.Item3 != null)
            {
                btnUpload.Enabled = true;
                CsvDataList = fileResult.Item3;
                Session["FilePath"] = fileResult.Item2;
                McFrameBindGridView();
            }
        }

        protected void CsvDataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageIndex = CsvDataGridView.PageIndex;
                int pageSize = CsvDataGridView.PageSize;

                // Calculate the sequence number based on the current page and row index
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
            CsvDataGridView.PageSize = int.Parse(CsvPageSizeDropDown.SelectedValue);
            ViewState["Row"] = 0;

            CsvDataGridView.DataSource = CsvDataList;
            CsvDataGridView.DataBind();

            if (CsvDataList.Count() > 0)
            {
                if (ViewState["Row"].ToString().Trim() == "0")
                {
                    ViewState["grandtotal"] = CsvDataList.Count;
                    ViewState["Row"] = 1;
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
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
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
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
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
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
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
                // Load your CSV data here
                ReadCsvModel readCsv = new ReadCsvModel();
                using (CsvReader csvData = readCsv.ReadCsvFile((string)Session["FilePath"], ";"))
                {
                    CsvDataList = csvData.GetRecords<McFrameViewModel>().ToList();
                    csvData.Dispose();
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika memuat file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload McFrame process.<br/> Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
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

                if (forcePushData.Checked)
                {
                    Exception deleteResult = databaseModel.DeleteData(tableName, $" Year_Period = '{yearPeriod}' AND Month_Period = '{monthPeriod}' ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    if (deleteResult.Message != "Data period sebelumnya berhasil dibersihkan.")
                    {
                        utility.UploadCsvErrorHandler(deleteResult, CsvDataGridView, errorLabel);
                    }
                }

                Exception checkPeriodResult = databaseModel.PeriodCheck(tableName, yearPeriod, monthPeriod, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (checkPeriodResult.Message != "Data Period Aman.")
                {
                    loadCsvException = new Exception(checkPeriodResult.Message);
                    utility.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
                }

                Exception insertResult = new ReadCsvModel().IterateCsvObject(tableName, columnNameAndData.Item1, columnNameAndData.Item2, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (insertResult.Message != $"Insert Into Table {tableName} Berhasil.")
                {
                    loadCsvException = new Exception($"Gagal dalam penulisan ke database. Detail : {insertResult.Message}");
                    utility.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
                }

                if (checkPeriodResult.Message == "Data Period Aman." && insertResult.Message == $"Insert Into Table {tableName} Berhasil.")
                {
                    if (File.Exists(Session["FilePath"].ToString()))
                    {
                        File.Delete(Session["FilePath"].ToString());
                    }
                    Session["FilePath"] = "";
                    string script = $"alert('{insertResult.Message}'); window.location.href = '{ResolveUrl("~/Pages/ContentPages/McFrameUpload.aspx")}';";
                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                loadCsvException = new Exception(ex.Message);
                utility.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
            }
        }
    }
}