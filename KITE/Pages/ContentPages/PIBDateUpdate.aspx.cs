using KITE.Models;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class PIBDateUpdate : System.Web.UI.Page
    {
        private DataTable globalMasterBatchDataTable;

        public void refreshPage(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString(), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tuple<DataTable, Exception> MasterBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" UUID,Raw_Material,RM_Batch,PIB_Date ", "Master_Batch", " ORDER BY RM_Batch ASC, Raw_Material ASC ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (MasterBatchDataTable.Item2.Message != "null")
                {
                    UtilityModel errorHandler = new UtilityModel();
                    Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table FG_per_Batch. Detail : {MasterBatchDataTable.Item2.Message}");
                    errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);
                    return;
                }
                else
                {
                    globalMasterBatchDataTable = MasterBatchDataTable.Item1;
                    MasterBatchBindGridView();
                }
            }
        }

        public void SubmitData(object sender, EventArgs e)
        {

            string condition = $" WHERE Raw_Material LIKE '%{rmTxt.Text}%' AND RM_Batch LIKE '%{batchTxt.Text}%' ";
            if (chkEmptyPIBDate.Checked)
            {
                condition += $" AND PIB_Date IS NULL ";
            }

            condition += " ORDER BY RM_Batch ASC, Raw_Material ASC ";

            Tuple<DataTable, Exception> MasterBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" UUID,Raw_Material,RM_Batch,PIB_Date ", "Master_Batch", condition, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (MasterBatchDataTable.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table Master_Batch. Detail : {MasterBatchDataTable.Item2.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);
            }
            else
            {
                globalMasterBatchDataTable = MasterBatchDataTable.Item1;
                MasterBatchBindGridView();
            }
        }

        public string PIBDateInsertion(string PIBDate, string UUID)
        {
            Exception pibDateException = new DatabaseModel().UpdateTable(" Master_Batch ", $" PIB_Date = '{PIBDate}', Change_By = '{Session["Fullname"]}', Date_Modified = CURRENT_TIMESTAMP ", $" WHERE UUID = '{UUID}' ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (pibDateException.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Update PIB Date Gagal. Detail : {pibDateException.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);

                LoadMasterBatchData();
                MasterBatchBindGridView();
                return pibDateException.Message;
            }
            return "Update PIB Date Berhasil.";
        }

        private void MasterBatchBindGridView()
        {
            ViewState["Row"] = 0;
            PIBDataGridView.DataSource = globalMasterBatchDataTable;
            PIBDataGridView.PageSize = int.Parse(PIBPageSizeDropDown.SelectedValue);
            PIBDataGridView.DataBind();

            if (globalMasterBatchDataTable.Rows.Count > 0)
            {
                if (ViewState["Row"].ToString().Trim() == "0")
                {
                    ViewState["Row"] = 1;
                    ViewState["grandtotal"] = globalMasterBatchDataTable.Rows.Count;
                    lblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["grandtotal"]);

                    int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / PIBDataGridView.PageSize));
                    lblTotalNumberOfPages.Text = pageCount.ToString();
                    txtGoToPage.Text = (PIBDataGridView.PageIndex + 1).ToString();
                }

            }
            else
            {
                ViewState["grandtotal"] = 0;
            }
        }

        protected void PIBDataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int pageSize = PIBDataGridView.PageSize;
                int pageIndex = PIBDataGridView.PageIndex;
                int sequenceNumber = pageIndex * pageSize + e.Row.RowIndex + 1;

                Label lblSequence = (Label)e.Row.FindControl("lblSequenceNo");
                lblSequence.Text = sequenceNumber.ToString();

                TextBox textBoxPIBDate = e.Row.FindControl("TextBoxPIBDate") as TextBox;
                Label UUIDLbl = e.Row.FindControl("UUIDLbl") as Label;

                // Attach client-side event (JavaScript) to trigger after mouse away
                textBoxPIBDate.Attributes["onblur"] = $"updatePIBDateFunc('{textBoxPIBDate.ClientID}', '{UUIDLbl.ClientID}')";
            }
        }

        protected void PIBDataGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdatePIB")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = PIBDataGridView.Rows[rowIndex];

                string PIBDate = ((TextBox)row.FindControl("TextBoxPIBDate")).Text;
                string UUID = ((Label)row.FindControl("UUIDLbl")).Text;
                string[] dateFormats = { "yyyy/MM/dd", "yyyy-MM-dd" };
                DateTime parsedDate;
                bool success = false;

                foreach (string format in dateFormats)
                {
                    success = DateTime.TryParseExact(PIBDate, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);

                    if (success)
                    {
                        break;
                    }
                }

                if (success)
                {
                    PIBDateInsertion(PIBDate, UUID);
                    confirmationPopUp.Show();
                    confirmationLable.Text = "Update PIB Date Berhasil.";
                }
                else
                {
                    Exception loadCsvException = new Exception("Format tanggal harus 'yyyy/mm/dd' atau 'yyyy-mm-dd'. Silahkan memperbaiki format tanggal PIBDate anda sesuai dengan format yang diinginkan. ");
                    confirmationPopUp.Show();
                    confirmationLable.Text = loadCsvException.Message;
                }

            }
        }

        protected void PIBDataGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PIBDataGridView.PageIndex = e.NewPageIndex;
            LoadMasterBatchData();
            MasterBatchBindGridView();
        }

        protected void PIBPageSizeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPageSize = Convert.ToInt32(PIBPageSizeDropDown.SelectedValue);
            PIBDataGridView.PageSize = selectedPageSize;

            LoadMasterBatchData();
            MasterBatchBindGridView();
        }

        protected void GoToPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / PIBDataGridView.PageSize));
                if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
                {
                    MasterBatchLoadPage(pageNumber - 1);
                }
                else
                {
                    MasterBatchLoadPage(0);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika mau membuka halaman " + txtGoToPage.Text.Trim() + ". Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);
            }
        }

        protected void btnPrev_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 1)
                {
                    MasterBatchLoadPage(pageNumber - 2);
                }
                else
                {
                    MasterBatchLoadPage(PIBDataGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol previous page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);
            }
        }

        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            try
            {
                int pageNumber;
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / PIBDataGridView.PageSize));
                if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber < pageCount)
                {
                    MasterBatchLoadPage(pageNumber);
                }
                else
                {
                    MasterBatchLoadPage(PIBDataGridView.PageIndex);
                }
            }
            catch (Exception ex)
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception("Terdapat masalah ketika tombol next page berjalan. Detail : " + ex.Message);
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);
            }
        }

        private void MasterBatchLoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            PIBDataGridView_PageIndexChanging(this, e);
        }

        private void LoadMasterBatchData()
        {
            Tuple<DataTable, Exception> MasterBatchDataTable = new DatabaseModel().SelectTableIntoDataTable(" UUID,Raw_Material,RM_Batch,PIB_Date ", "Master_Batch", " ORDER BY RM_Batch ASC, Raw_Material ASC ", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (MasterBatchDataTable.Item2.Message != "null")
            {
                UtilityModel errorHandler = new UtilityModel();
                Exception loadCsvException = new Exception($"Terdapat masalah ketika menjalankan proses membaca table Master_Batch. Detail : {MasterBatchDataTable.Item2.Message}");
                errorHandler.UploadCsvErrorHandler(loadCsvException, new GridView[] { PIBDataGridView }, errorLabel);
            }
            else
            {
                globalMasterBatchDataTable = MasterBatchDataTable.Item1;
                MasterBatchBindGridView();
            }
        }
    }
}