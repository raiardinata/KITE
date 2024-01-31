using KITE.Models;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class RepackReprocess : System.Web.UI.Page
    {
        Tuple<DataTable, Exception> repackDTandExc;
        bool valid = false;
        decimal DsOneQty, DsTwoQty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void rmUsageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (fgTextBoxTop.Text != "" && batchTextBox.Text != "")
            {
                RMUsageData();
            }
        }

        public void RMUsageData()
        {
            repackDTandExc = new DatabaseModel().SelectTableIntoDataTable(" FG_Batch_Qty, Raw_Material, RM_Batch, Total_RM_Qty, Qty_RM_Batch, Distribution_Qty, Remaining_Qty ", " FG_per_Batch ", $" WHERE Finish_Goods = '{fgTextBoxTop.Text}' AND FG_Batch = '{batchTextBox.Text}' AND Remaining_Qty > 1 ORDER BY Raw_Material ASC, RM_Batch ASC ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (repackDTandExc.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah dalam pemilihan data repack / reprocess. Detail " + repackDTandExc.Item2.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
                qtyGRFGResLable.Text = "0";
                return;
            }
            foreach (DataRow row in repackDTandExc.Item1.Rows)
            {
                decimal FGQty = (decimal)row["FG_Batch_Qty"];
                qtyGRFGResLable.Text = FGQty.ToString("N0");
                errorLabel.Text = "";
            }
            RMUsageBindGridView();
        }


        private void RMUsageBindGridView()
        {
            ViewState["FGRow"] = 0;
            rmUsageDataGridView.DataSource = repackDTandExc.Item1;
            rmUsageDataGridView.PageSize = int.Parse(rmUsagePageSizeDropDown.SelectedValue);
            rmUsageDataGridView.DataBind();

            if (repackDTandExc.Item1.Rows.Count > 0)
            {
                if (ViewState["FGRow"].ToString().Trim() == "0")
                {
                    ViewState["FGRow"] = 1;
                    ViewState["FGgrandtotal"] = repackDTandExc.Item1.Rows.Count;
                    lblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["FGgrandtotal"]);

                    int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGgrandtotal"]) / rmUsageDataGridView.PageSize));
                    rmUsagelblTotalNumberOfPages.Text = pageCount.ToString();
                    rmUsagetxtGoToPage.Text = (rmUsageDataGridView.PageIndex + 1).ToString();
                }
            }
            else
            {
                ViewState["FGgrandtotal"] = 0;
            }
        }

        protected void rmUsageDataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageSize = rmUsageDataGridView.PageSize;
                int pageIndex = rmUsageDataGridView.PageIndex;
                int sequenceNumber = pageIndex * pageSize + e.Row.RowIndex + 1;

                Label lblSequence = (Label)e.Row.FindControl("rmUsagelblSequenceNo");
                lblSequence.Text = sequenceNumber.ToString();
            }
        }

        protected void rmUsageDataGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            rmUsageDataGridView.PageIndex = e.NewPageIndex;
            LoadrmUsageData();
            RMUsageBindGridView();
        }

        protected void rmUsagePageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(rmUsagePageSizeDropDown.SelectedValue);
            rmUsageDataGridView.PageSize = selectedPageSize;

            LoadrmUsageData();
            RMUsageBindGridView();
        }

        protected void rmUsageGoToPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGgrandtotal"]) / rmUsageDataGridView.PageSize));
                if (int.TryParse(rmUsagetxtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
                {
                    rmUsageLoadPage(pageNumber - 1);
                }
                else
                {
                    rmUsageLoadPage(0);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mau membuka halaman " + rmUsagetxtGoToPage.Text.Trim() + ". Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
            }
        }

        protected void rmUsagebtnPrev_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                if (int.TryParse(rmUsagetxtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 1)
                {
                    rmUsageLoadPage(pageNumber - 2);
                }
                else
                {
                    rmUsageLoadPage(rmUsageDataGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol previous page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
            }
        }

        protected void rmUsagebtnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["FGgrandtotal"]) / rmUsageDataGridView.PageSize));
                if (int.TryParse(rmUsagetxtGoToPage.Text.Trim(), out pageNumber) && pageNumber < pageCount)
                {
                    rmUsageLoadPage(pageNumber);
                }
                else
                {
                    rmUsageLoadPage(rmUsageDataGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol next page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
            }
        }

        private void rmUsageLoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            rmUsageDataGridView_PageIndexChanging(this, e);
        }

        private void LoadrmUsageData()
        {
            repackDTandExc = new DatabaseModel().SelectTableIntoDataTable(" Raw_Material, RM_Batch, Total_RM_Qty, Qty_RM_Batch, Distribution_Qty, Remaining_Qty ", " FG_per_Batch ", $" WHERE Finish_Goods = '{fgTextBoxTop.Text}' AND FG_Batch = '{batchTextBox.Text}' ORDER BY RM_Batch ASC, Raw_Material ASC ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (repackDTandExc.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table RM_per_Batch. Detail : {repackDTandExc.Item2.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
            }
            else
            {
                RMUsageBindGridView();
            }
        }

        protected void repackTextBoxBottom_TextChanged(object sender, EventArgs e)
        {
            if (fgTextBoxBottom.Text != "" && batchTextBoxBottom.Text != "" && qtyTextBox.Text != "")
            {
                GetRepackandReprocessData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sOneBatch.Text == "-" && sTwoBatch.Text == "-")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah dalam kalkulasi repack / reprocess. Mohon untuk cek kembali kalkulasi pada tab repack / reprocess.");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
                return;
            }
            else
            {
                object[] param = new[] { fgTextBoxTop.Text, batchTextBox.Text, fgTextBoxBottom.Text, batchTextBoxBottom.Text, qtyTextBox.Text, sOneQty.Text, sOneBatch.Text, sTwoQty.Text, sTwoBatch.Text };
                try
                {
                    Exception CreatingRepackReprocessResult = new DatabaseModel().ExecStoreProcedure("CreatingRepackReprocess", param, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                    if (CreatingRepackReprocessResult != null)
                    {
                        UtilityModel errorHandler = new UtilityModel();
                        Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses CreatingRepackReprocess. Detail : {CreatingRepackReprocessResult.Message}");
                        errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
                        return;
                    }
                    string script = $"alert('Repack / Reprocess berhasil.'); window.location.href = '{ResolveUrl("~/Pages/ContentPages/RepackReprocess.aspx")}';";
                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                }
                catch (Exception ex)
                {
                    UtilityModel errorHandler = new UtilityModel();
                    Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses CreatingRepackReprocess. Detail : {ex}.");
                    errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
                    return;
                }
            }
        }

        public void GetRepackandReprocessData()
        {
            // for 71
            repackDTandExc = new DatabaseModel().SelectTableIntoDataTable(" FG_Batch_Qty, Raw_Material, RM_Batch, Total_RM_Qty, Qty_RM_Batch, Distribution_Qty, Remaining_Qty ", " FG_per_Batch ", $" WHERE Finish_Goods = '{fgTextBoxTop.Text}' AND FG_Batch = '{batchTextBox.Text}' AND Raw_Material = '121001071' AND Remaining_Qty >= 1 ORDER BY RM_Batch ASC ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (repackDTandExc.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah dalam kalkulasi repack / reprocess. Detail " + repackDTandExc.Item2.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
                qtyGRFGResLable.Text = "0";
                return;
            }
            foreach (DataRow row in repackDTandExc.Item1.Rows)
            {
                decimal remQty = (decimal)row["Remaining_Qty"] + 1;
                DsOneQty = (decimal.Parse(qtyTextBox.Text) / (decimal)row["FG_Batch_Qty"]) * (decimal)row["Total_RM_Qty"];
                sOneQty.Text = DsOneQty.ToString("N4");
                sOneBatch.Text = (string)row["RM_Batch"];
                if (remQty >= DsOneQty)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah dalam kalkulasi repack / reprocess, remaining qty. 121001071 batch {sOneBatch.Text} kurang dari qty. yang di repack.");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
            }

            valid = false;

            // for 72
            repackDTandExc = new DatabaseModel().SelectTableIntoDataTable(" FG_Batch_Qty, Raw_Material, RM_Batch, Total_RM_Qty, Qty_RM_Batch, Distribution_Qty, Remaining_Qty ", " FG_per_Batch ", $" WHERE Finish_Goods = '{fgTextBoxTop.Text}' AND FG_Batch = '{batchTextBox.Text}' AND Raw_Material = '121001072' AND Remaining_Qty >= 1 ORDER BY RM_Batch ASC ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (repackDTandExc.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah dalam kalkulasi repack / reprocess. Detail " + repackDTandExc.Item2.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { rmUsageDataGridView }, errorLabel);
                qtyGRFGResLable.Text = "0";
                return;
            }
            foreach (DataRow row in repackDTandExc.Item1.Rows)
            {
                decimal remQty = (decimal)row["Remaining_Qty"] + 1;
                DsTwoQty = (decimal.Parse(qtyTextBox.Text) / (decimal)row["FG_Batch_Qty"]) * (decimal)row["Total_RM_Qty"];
                sTwoQty.Text = DsTwoQty.ToString("N4");
                sTwoBatch.Text = (string)row["RM_Batch"];
                if (remQty >= DsTwoQty)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah dalam kalkulasi repack / reprocess, remaining qty. 121001071 batch {sTwoBatch.Text} kurang dari qty. yang di repack.");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { }, errorLabel);
            }
        }
    }
}