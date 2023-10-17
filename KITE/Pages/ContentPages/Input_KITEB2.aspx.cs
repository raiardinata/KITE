using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace KITE.Pages.ContentPages
{
    public partial class Input_KITEB2 : System.Web.UI.Page
    {
        Controllers.KiteController KC = new Controllers.KiteController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();

        Controllers.LoginController login = new Controllers.LoginController();

        bool refresh = false;
        static string localip = "";
        //static string localip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            localip = GETIPADDRESS();
            if (!IsPostBack)
            {
                Timer2.Enabled = true;
            }
            //localip = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"]?? )
        }

        protected string GETIPADDRESS()
        {
            //System.Web.HttpContext CONT = System.Web.HttpContext.Current;
            string IPADD = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(IPADD))
            {
                string[] addr = IPADD.Split(',');
                if (addr.Length != 0)
                {
                    return addr[0];
                }

            }
            return Context.Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void CheckUser()
        {
            if (HttpContext.Current.Session["ActiveUser"] == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                LinkButton lb = (LinkButton)Master.FindControl("lnLogout");
                lb.Text = "Logout " + HttpContext.Current.Session["ActiveUser"];

                Label lbl1 = (Label)Master.FindControl("label1");
                lbl1.Text = HttpContext.Current.Session["FullName"].ToString();

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool boleh = true;
            string strMaterialLow = "";
            string strCompany = "";
            string strPlant = "";
            string strMaterialHigh = "";
            string strSlocLow = "";
            string strSlocHigh = "";
            string strMoveLow = "";
            string strMoveHigh = "";
            string strDateLow = "";
            string strDateHigh = "";
            string strUserIp = "";

            DateTime FromDate;
            DateTime ToDate;
            int FromDateint;
            int ToDateint;

            if (txtDateFrom.Text == "" || txtDateTo.Text == "")
            {
                lblErrorPeriod.Text = "Pilih Tanggal";
                lblErrorPeriod.Visible = true;
                boleh = false;
            }

            DateTime.TryParseExact(txtDateFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime.TryParseExact(txtDateTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);
            FromDateint = FromDate.DayOfYear;
            ToDateint = ToDate.DayOfYear;

            lblerrorMat.Visible = false;
            lblErrorSloc.Visible = false;
            lblErrorPeriod.Visible = false;
            lblErrMove.Visible = false;
            if (lbMaterial2.Items.Count == 0)
            {
                lblerrorMat.Visible = true;
                lblerrorMat.Text = "Pilih Material.";
                boleh = false;
            }

            if (ToDateint < FromDateint)
            {
                lblErrorPeriod.Text = "Period Akhir Tidak Boleh Kurang Dari Period Awal";
                lblErrorPeriod.Visible = true;
                boleh = false;
            }

            if (lbMove2.Items.Count == 0)
            {
                lblErrMove.Text = "Pilih Movement Type.";
                lblErrMove.Visible = true;
                boleh = false;
            }

            if (lbsloc2.Items.Count == 0)
            {
                lblErrorSloc.Text = "Pilih Storage Location.";
                lblErrorSloc.Visible = true;
                boleh = false;
            }


            if (boleh)
            {
                for (int i = 0; i < lbMaterial2.Items.Count; i++)
                {

                    strMaterialLow = strMaterialLow + lbMaterial2.Items[i].Text; //+ "|";
                    if (i < lbMaterial2.Items.Count - 1)
                    {
                        strMaterialLow = strMaterialLow + "|";
                    }
                }

                for (int i = 0; i < lbsloc2.Items.Count; i++)
                {

                    strSlocLow = strSlocLow + lbsloc2.Items[i].Text; //+ "|";
                    if (i < lbsloc2.Items.Count - 1)
                    {
                        strSlocLow = strSlocLow + "|";
                    }
                }

                for (int i = 0; i < lbMove2.Items.Count; i++)
                {

                    strMoveLow = strMoveLow + lbMove2.Items[i].Text; //+ "|";
                    if (i < lbMove2.Items.Count - 1)
                    {
                        strMoveLow = strMoveLow + "|";
                    }
                }
                DateTime datef = DateTime.ParseExact(txtDateFrom.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime datet = DateTime.ParseExact(txtDateTo.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                strCompany = ddlCompanyCode.SelectedValue.ToString();
                strPlant = ddlPlant.SelectedValue.ToString();
                strDateLow = datef.ToString("yyyyMMdd"); //.ToString("yyyyMMdd");
                strDateHigh = datet.ToString("yyyyMMdd");
                strUserIp = localip;
                //SubmitInformation();
                SubmitInformation(strCompany, strPlant, strMaterialLow, strMaterialHigh, strSlocLow, strSlocHigh, strMoveLow, strMoveHigh, strDateLow, strDateHigh, strUserIp);

            }

        }

        protected void SubmitInformation(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string MoveT_LOW, string MoveT_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP)
        {

            //KC.InsertQueueB(Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, MoveT_LOW, MoveT_HIGH, sDate_LOW, sDate_HIGH, HttpContext.Current.Session["UserAppID"].ToString(), "Open", "Open Report KITE B", HttpContext.Current.Session["UserAppID"].ToString(), HttpContext.Current.Session["FullName"].ToString());
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Loading')", true);
            Timer1.Enabled = true;
            Timer1.Interval = Convert.ToInt32(Setting.TimerInterval);
            //refresh = true;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            opensearch();

        }

        private void opensearch()
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Insert')", true);
            ListCompany();
            ListPlant();
            ListSloc();
            ListMove();
            //gridSearch(txtSearchCriteria.Text);
            ListMaterial();
            labelCompany();
            labelplant();
        }

        private void ListMaterial()
        {
            lbMaterial.Items.Clear();
            lbMaterial2.Items.Clear();
            var dt = KC.Select_Material();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbMaterial.Items.Add(select.MATERIALID.ToString());
                }
                lbMaterial.Items.Add("ALL Material (121)");
                lbMaterial.Items.Add("ALL Finish Goods (124)");
                lbMaterial.Items.Add("ALL Waste (211)");
            }

        }

        private void ListSloc()
        {
            lbSloc.Items.Clear();
            lbsloc2.Items.Clear();

            var dt = KC.Select_Sloc();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbSloc.Items.Add(select.sloc.ToString());
                }
                lbSloc.Items.Add("ALL");
            }

        }

        private void ListMove()
        {
            lbMove.Items.Clear();
            lbMove2.Items.Clear();

            var dt = KC.Select_Move("REPORT_B");

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbMove.Items.Add(select.low.ToString());
                }
                lbMove.Items.Add("ALL");
            }

        }

        private void ListCompany()
        {
            ddlCompanyCode.Items.Clear();
            var dt = KC.Select_Company();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlCompanyCode.Items.Add(select.low.ToString());
                }
            }

        }

        private void ListPlant()
        {
            ddlPlant.Items.Clear();
            var dt = KC.Select_Plant();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlPlant.Items.Add(select.low.ToString());
                }
            }

        }

        protected void btnSelectmat_Click(object sender, EventArgs e)
        {

            List<int> selecteds = lbMaterial.GetSelectedIndices().ToList();
            string MAT = "";
            bool exists = false;

            for (int i = 0; i < lbMaterial.Items.Count; i++)
            {
                if (lbMaterial.Items[i].Selected)
                {

                    string stritem = lbMaterial.Items[i].Text;

                    if (lbMaterial2.Items.Count > 0)
                    {
                        for (int y = 0; y < lbMaterial2.Items.Count; y++)
                        {
                            if (stritem == lbMaterial2.Items[y].Text)
                            {
                                exists = true;

                            }
                        }
                    }

                    if (!exists)
                    {
                        if (stritem == "ALL Material (121)")
                        {
                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() == "121")
                                {

                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }

                        else if (stritem == "ALL Finish Goods (124)")
                        {

                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() == "124")
                                {
                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }

                        else if (stritem == "ALL Waste (211)")
                        {

                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() == "211")
                                {
                                    //for (int g = 0; g < lbMaterial2.Items.Count; g++)
                                    //{
                                    //    if (MAT !=
                                    //}
                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }
                        else
                        {
                            lbMaterial2.Items.Add(stritem);
                        }
                    }
                }
            }
        }

        protected void btnDiselectmat_Click(object sender, EventArgs e)
        {
            int CON = lbMaterial2.Items.Count;
            for (int i = 0; i < CON; i++)
            {
                if (lbMaterial2.Items[i].Selected)
                {
                    lbMaterial2.Items.Remove(lbMaterial2.Items[i]);
                    CON = CON - 1;
                    i = i - 1;
                }
            }
        }

        protected void btnselectsloc_Click(object sender, EventArgs e)
        {
            bool exists = false;
            List<int> selecteds = lbSloc.GetSelectedIndices().ToList();
            //string SLOC = "";
            for (int i = 0; i < lbSloc.Items.Count; i++)
            {
                if (lbSloc.Items[i].Selected)
                {
                    string stritem = lbSloc.Items[i].Text;
                    if (lbsloc2.Items.Count > 0)
                    {
                        for (int y = 0; y < lbsloc2.Items.Count; y++)
                        {
                            if (stritem == lbsloc2.Items[y].Text)
                            {
                                exists = true;

                            }
                        }
                    }

                    if (!exists)
                    {
                        if (stritem == "ALL")
                        {
                            lbsloc2.Items.Clear();
                            for (int J = 0; J < lbSloc.Items.Count - 1; J++)
                            {
                                lbsloc2.Items.Add(lbSloc.Items[J].Text);
                            }

                        }
                        else
                        {
                            lbsloc2.Items.Add(stritem);
                        }

                    }

                }
            }
        }

        protected void btndiselectsloc_Click(object sender, EventArgs e)
        {
            int con = lbsloc2.Items.Count;
            for (int i = 0; i < con; i++)
            {
                if (lbsloc2.Items[i].Selected)
                {
                    lbsloc2.Items.Remove(lbsloc2.Items[i]);
                    con = con - 1;
                    i = i - 1;
                }
            }
        }

        protected void btnSelectmove_Click(object sender, EventArgs e)
        {
            bool exists = false;
            List<int> selecteds = lbMove.GetSelectedIndices().ToList();

            for (int i = 0; i < lbMove.Items.Count; i++)
            {
                if (lbMove.Items[i].Selected)
                {
                    string stritem = lbMove.Items[i].Text;
                    if (lbMove2.Items.Count > 0)
                    {
                        for (int y = 0; y < lbMove2.Items.Count; y++)
                        {
                            if (stritem == lbMove2.Items[y].Text)
                            {
                                exists = true;

                            }
                        }
                    }

                    if (!exists)
                    {
                        if (stritem == "ALL")
                        {
                            lbMove2.Items.Clear();
                            for (int J = 0; J < lbMove.Items.Count - 1; J++)
                            {
                                lbMove2.Items.Add(lbMove.Items[J].Text);
                            }

                        }
                        else
                        {
                            lbMove2.Items.Add(stritem);
                        }
                    }
                }
            }
        }

        protected void btnDiselectmove_Click(object sender, EventArgs e)
        {
            int con = lbMove2.Items.Count;
            for (int i = 0; i < con; i++)
            {
                if (lbMove2.Items[i].Selected)
                {
                    lbMove2.Items.Remove(lbMove2.Items[i]);
                    con = con - 1;
                    i = i - 1;
                }
            }
        }

        private void show()
        {
            gridSearch("");
        }
        private void gridSearch(string Filter)
        {
            grid.PageSize = int.Parse(ddlPageSize.SelectedValue);


            ViewState["Row"] = 0;
            string StartRow = (grid.PageIndex * grid.PageSize).ToString();
            string Row = ddlPageSize.SelectedValue;
            var dt = KC.Select_KiteB2(StartRow, Row, HttpContext.Current.Session["UserAppID"].ToString());

            grid.DataSource = dt;
            grid.DataBind();


            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    if (ViewState["Row"].ToString().Trim() == "0")
                    {
                        ViewState["grandtotal"] = select.TotalRow;
                        lblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["grandtotal"]);
                        // DivGrid.Visible = true;
                        // lblNoData.Visible = false;
                        ViewState["Row"] = 1;
                        int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / grid.PageSize));
                        lblTotalNumberOfPages.Text = pageCount.ToString();
                        txtGoToPage.Text = (grid.PageIndex + 1).ToString();
                        //  GridViewField.Visible = true;
                    }
                }

            }
            else
            {
                //  GridViewField.Visible = false;
                ViewState["grandtotal"] = 0;
                // DivGrid.Visible = false;
                //lblNoData.Visible = true;
            }
        }

        //private void show()
        //{
        //    string strDateLow = "";
        //    string strDateHigh = "";
        //    DateTime datef = DateTime.ParseExact(txtDateFrom.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //    DateTime datet = DateTime.ParseExact(txtDateTo.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

        //    strDateLow = datef.ToString("MMMM yyyy"); //.ToString("yyyyMMdd");
        //    strDateHigh = datet.ToString("MMMM yyyy");

        //    ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
        //    ReportViewer1.ServerReport.ReportServerUrl = new Uri(Setting.ReportServerUrls); // Report Server URL
        //    ReportViewer1.ServerReport.ReportPath = "/ReportKITE/KITE_B"; // Report Name 
        //    ReportViewer1.ShowParameterPrompts = false;
        //    ReportViewer1.ShowPrintButton = true;

        //    // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
        //    Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];
        //    reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
        //    reportParameterCollection[0].Name = "USERIP";
        //    reportParameterCollection[0].Values.Add(HttpContext.Current.Session["UserAppID"].ToString());
        //    reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
        //    reportParameterCollection[1].Name = "FromDate";
        //    //reportParameterCollection[1].Values.Add(txtDateFrom.Text);
        //    reportParameterCollection[1].Values.Add(strDateLow);

        //    reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter();
        //    if (txtDateTo.Text != "")
        //    {
        //        reportParameterCollection[2].Name = "ToDate";
        //        //reportParameterCollection[2].Values.Add("s/d " + txtDateTo.Text);
        //        reportParameterCollection[2].Values.Add("Sampai       " + strDateHigh);
        //    }
        //    else
        //    {
        //        reportParameterCollection[2].Name = "ToDate";
        //        reportParameterCollection[2].Values.Add("");

        //    }
        //    ReportViewer1.ServerReport.SetParameters(reportParameterCollection);

        //    //ReportViewer1.ServerReport.Refresh();
        //}

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //if (refresh)
            //{
            //show();
            //}
            //if (refresh)
            //{
            CheckKITE_B();
            //}
            //Timer1.

        }

        protected void CheckKITE_B()
        {
            var dt = KC.Select_CheckStatus(HttpContext.Current.Session["UserAppID"].ToString());
            //string status = "";
            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    if (select.status.ToString() == "1")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Loading')", true);
                        show();
                        refresh = false;
                        Timer1.Enabled = false;
                    }
                    else if (select.status.ToString() == "2")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Loading')", true);
                        show();
                        refresh = false;
                        Timer1.Enabled = false;
                    }
                }
            }
            else
            {
            }
        }

        protected void ddlCompanyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanyCode.SelectedIndex != -1)
            {
                labelCompany();
            }
        }

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPlant.SelectedIndex != -1)
            {
                labelplant();
            }
        }

        private void labelCompany()
        {
            var dt = KC.Select_CompanyLabel(ddlCompanyCode.SelectedValue.ToString());

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lblcomcode.Text = select.high.ToString();
                }
            }

        }

        private void labelplant()
        {
            var dt = KC.Select_Plantlabel(ddlPlant.SelectedValue.ToString());

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lblplant.Text = select.high.ToString();
                }
            }

        }

        protected void Timer2_Tick(object sender, EventArgs e)
        {
            opensearch();
            Timer2.Enabled = false;
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.PageSize = int.Parse(ddlPageSize.SelectedValue);
            grid.PageIndex = 0;
            gridSearch("");
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid.PageIndex = e.NewPageIndex;
            gridSearch("");
        }
        private void LoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            grid_PageIndexChanging(this, e);
        }
        protected void txtGoToPage_TextChanged(object sender, EventArgs e)
        {
            int pageNumber;
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / grid.PageSize));
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
            {
                LoadPage(pageNumber - 1);
            }
            else
            {
                LoadPage(0);
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 1)
            {
                LoadPage(pageNumber - 2);
            }
            else
            {
                LoadPage(grid.PageIndex);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int pageNumber;
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / grid.PageSize));
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber < pageCount)
            {
                LoadPage(pageNumber);
            }
            else
            {
                LoadPage(grid.PageIndex);
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
     
    }
}