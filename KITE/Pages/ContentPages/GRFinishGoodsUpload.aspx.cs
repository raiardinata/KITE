﻿using CsvHelper;
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
    public partial class GRFinishGoodsUpload : System.Web.UI.Page
    {
        private List<GRFinishGoodsViewModel> CsvDataList;
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
            GRFinishGoodsFunctionModel grFinishGoodsFunction = new GRFinishGoodsFunctionModel();
            Tuple<Exception, string, List<GRFinishGoodsViewModel>> fileResult = grFinishGoodsFunction.GRFinishGoodsReadCsvFile(fileUpload);
            if (fileResult.Item1.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mensubmit file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload GR Finish Goods.<br/> Detail : " + fileResult.Item1.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
            }
            else if (fileResult.Item2 != null && fileResult.Item3 != null)
            {
                List<GRFinishGoodsViewModel> uomConvertionTempList = new List<GRFinishGoodsViewModel>();
                foreach (GRFinishGoodsViewModel grData in fileResult.Item3)
                {
                    decimal KG = 0;
                    try
                    {
                        Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTable("KG", "UOM_Convertion", $"Material = '{grData.Material}'", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                        EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable();
                        if (insertResult.Item2.Message != "null" || insertResult.Item1.Rows.Count == 0)
                        {
                            UtilityModel errorHandler = new UtilityModel();
                            Exception loadCsvException = new Exception("Terdapat masalah ketika mensubmit file csv. Konversi UoM ke KG gagal.<br/> Detail : " + insertResult.Item2.Message);
                            errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
                        }
                        foreach (DataRow uomConvertionRow in uomConvertionRows)
                        {
                            KG = uomConvertionRow.Field<decimal>("KG");
                        }
                    }
                    catch (Exception ex)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception("Terdapat masalah ketika mensubmit file csv. Konversi UoM ke KG gagal.<br/> Detail : " + ex.Message);
                        errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
                    }

                    uomConvertionTempList.Add(new GRFinishGoodsViewModel
                    {
                        Posting_Date = grData.Posting_Date,
                        Document_Date = grData.Document_Date,
                        Document_Header_Text = grData.Document_Header_Text,
                        Material = grData.Material,
                        Material_Description = grData.Material_Description,
                        Plant = grData.Plant,
                        Storage_Location = grData.Storage_Location,
                        Movement_Type = grData.Movement_Type,
                        Material_Document = grData.Material_Document,
                        Batch = grData.Batch,
                        Qty_in_Un_of_Entry = (KG != 0) ? Convert.ToString(Convert.ToDecimal(grData.Qty_in_Un_of_Entry) * KG) : grData.Qty_in_Un_of_Entry,
                        Unit_of_Entry = (KG != 0) ? "KG" : grData.Unit_of_Entry,
                        Entry_Date = grData.Entry_Date,
                        Time_of_Entry = grData.Time_of_Entry,
                        User_name = grData.User_name,
                        Base_Unit_of_Measure = (KG != 0) ? "KG" : grData.Unit_of_Entry,
                        Quantity = (KG != 0) ? Convert.ToString(Convert.ToDecimal(grData.Quantity) * KG) : grData.Quantity,
                        Amount_in_LC = grData.Amount_in_LC,
                        Goods_recipient = grData.Goods_recipient,
                    });
                }

                btnUpload.Enabled = true;
                CsvDataList = uomConvertionTempList;
                Session["FilePath"] = fileResult.Item2;
                GRFinishGoodsBindGridView();
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
            GRFinishGoodsBindGridView();
        }

        protected void CsvPageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(CsvPageSizeDropDown.SelectedValue);
            CsvDataGridView.PageSize = selectedPageSize;

            LoadCsvData();
            GRFinishGoodsBindGridView();
        }

        private void GRFinishGoodsBindGridView()
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
                    CsvDataList = csvData.GetRecords<GRFinishGoodsViewModel>().ToList();
                    csvData.Dispose();
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika memuat file csv. Mohon untuk cek kembali apakah data file csv cocok dengan format upload GR Finish Goods.<br/> Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "GR_Finish_Goods";
            DatabaseModel databaseModel = new DatabaseModel();
            UtilityModel utility = new UtilityModel();
            Exception loadCsvException;

            try
            {
                LoadCsvData();
                GRFinishGoodsFunctionModel csvDataProcess = new GRFinishGoodsFunctionModel();
                Tuple<string, ArrayList, Exception> columnNameAndData = csvDataProcess.GRFinishGoodsGenerateColumnAndCsvData(CsvDataList, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
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

                Tuple<string[], Exception> iterationResult = new ReadCsvModel().IterateCsvObject(columnNameAndData.Item2);
                if (iterationResult.Item2.Message != "Pemrosesan IterateCsvObject Berhasil.")
                {
                    loadCsvException = new Exception($"Gagal dalam penulisan ke database. Detail : {iterationResult.Item2.Message}");
                    utility.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
                }

                foreach (string iterionValue in iterationResult.Item1)
                {
                    Exception insertResult = new DatabaseModel().InsertIntoTable(tableName, columnNameAndData.Item1, iterionValue, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    if (insertResult.Message != "null")
                    {
                        loadCsvException = new Exception($"Terjadi kesalahan ketika Insert Into Table {tableName}. Detail : {insertResult.Message}");
                        utility.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
                        break;
                    }
                }

                if (File.Exists(Session["FilePath"].ToString()))
                {
                    File.Delete(Session["FilePath"].ToString());
                }
                Session["FilePath"] = "";
                string script = $"alert('Upload CSV berhasil.'); window.location.href = '{ResolveUrl("~/Pages/ContentPages/GRFinishGoodsUpload.aspx")}';";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
            }
            catch (Exception ex)
            {
                loadCsvException = new Exception(ex.Message);
                utility.UploadCsvErrorHandler(loadCsvException, CsvDataGridView, errorLabel);
            }
        }
    }
}