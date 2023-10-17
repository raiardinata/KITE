using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
//using Microsoft.Reporting.WebForms;
using System.Globalization;
//using PRJ_REPORT_SCM.SQLConnect;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace KITE.Pages.ContentPages
{
    public partial class Input_KITEA : System.Web.UI.Page
    {
        Controllers.KiteController KC = new Controllers.KiteController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();

        Controllers.LoginController login = new Controllers.LoginController();

        bool refresh = false;
        static string localip = "";
        static string strYearFROM = "";
        static string stryearTo = "";
        static string strMonthFrom = "";
        static string strMonthTo = "";
        static string strMonthhFrom = "";
        static string strMonthhTo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            localip = GETIPADDRESS();
           
            if (!IsPostBack)
            {
                //bool check = CheckPrev();
                //if (check)
                //{
                    Timer2.Enabled = true;
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Loading')", true);
                //    LabelWaitingList.Text = "Anda sedang membuka report ini sebelumnya. Harap tunggu.";
                //    LabelWaitingList.Visible = true;
                //    Timer1.Interval = Convert.ToInt32(Setting.TimerInterval);
                //    Timer1.Enabled = true;
                //}
            }            
           
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

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool check = CheckPrev();
            if (check)
            {
                string strMaterialLow = "";
                string strCompany = "";
                string strPlant = "";
                string strMaterialHigh = "";
                string strSlocLow = "";
                string strSlocHigh = "";
                string strDateLow = "";
                string strDateHigh = "";
                string strUserIp = "";

                bool boleh = true;
                lblerror2.Visible = false;
                LblErrorPeriod.Visible = false;
                if (lbMaterial2.Items.Count == 0)
                {
                    lblerror2.Visible = true;
                    lblerror2.Text = "Pilih Material.";
                    boleh = false;
                }

                if (Convert.ToInt32(ddlYearFrom.SelectedValue.ToString()) > Convert.ToInt32(ddlYearTo.SelectedValue.ToString()))
                {
                    LblErrorPeriod.Text = "Period Akhir Tidak Boleh Kurang Dari Period Awal";
                    LblErrorPeriod.Visible = true;
                    boleh = false;
                }

                else if (Convert.ToInt32(ddlYearFrom.SelectedValue.ToString()) == Convert.ToInt32(ddlYearTo.SelectedValue.ToString()))
                {
                    if (ddlPeriodTo.SelectedIndex < ddlPeriodFrom.SelectedIndex)
                    {
                        LblErrorPeriod.Text = "Period Akhir Tidak Boleh Kurang Dari Period Awal";
                        LblErrorPeriod.Visible = true;
                        boleh = false;
                    }
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

                    MonthYear();

                    //DateTime datef = DateTime.ParseExact(txtDateFrom.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //DateTime datet = DateTime.ParseExact(txtDateTo.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    strCompany = ddlCompanyCode.SelectedValue.ToString();
                    strPlant = ddlPlant.SelectedValue.ToString();
                    DateTime datee = Convert.ToDateTime(DateTime.Now);
                    strYearFROM = ddlYearFrom.SelectedValue.ToString();//datee.Year.ToString(); //DateTime.ParseExact(DateTime.Now.ToString(), "yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    stryearTo = ddlYearTo.SelectedValue.ToString();
                    strDateLow = strYearFROM + strMonthFrom;//datef.ToString("yyyyMM"); //.ToString("yyyyMMdd");
                    strDateHigh = stryearTo + strMonthTo;
                    strUserIp = localip;
                    //SubmitInformation();
                    SubmitInformation(strCompany, strPlant, strMaterialLow, strMaterialHigh, strSlocLow, strSlocHigh, strDateLow, strDateHigh, strUserIp);
                }
            }
        }

        protected void SubmitInformation(string Company, string Plant, string Material_LOW, string Material_HIGH, string sloc_low, string sloc_high, string sDate_LOW, string sDate_HIGH, string User_IP)
        {
           
            KC.InsertQueueA(Company, Plant, Material_LOW, Material_HIGH, sloc_low, sloc_high, sDate_LOW, sDate_HIGH, HttpContext.Current.Session["UserAppID"].ToString(), "Open", "Open Report KITE A", HttpContext.Current.Session["UserAppID"].ToString(), HttpContext.Current.Session["FullName"].ToString());
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Loading')", true);
            LabelWaitingList.Visible = false;
            //UpdatePanel3.Visible = true;
            //Button1.Attributes.Add("onclick", "document.body.style.cursor='wait';this.disabled = true;");
            Timer1.Interval = Convert.ToInt32(Setting.TimerInterval);
            Timer1.Enabled = true;
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
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUpBaru('#Insert')", true);
            ListCompany();
            ListPlant();
            labelCompany();
            labelplant();
            ListSlocConf("KITE A");
            //ListSloc();
            //ListMove();
            //gridSearch(txtSearchCriteria.Text);
            //ListMaterial();
            ListMaterialConf("KITE A");
            ListYear();
            //show();
            DateTime datee = Convert.ToDateTime(DateTime.Now);
            int intmonth = Convert.ToInt32(datee.Month.ToString());
            ddlPeriodFrom.SelectedIndex = intmonth - 1;
            ddlPeriodTo.SelectedIndex = intmonth - 1;
        }

        private void ListSlocConf(string menunya)
        {
            //lbMaterial.Items.Clear();
            lbSloc.Items.Clear();
            var dt = KC.Select_Slocconf(menunya);

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbSloc.Items.Add(select.slocID.ToString());
                }
                lbSloc.Items.Add("ALL");
                //lbMaterial.Items.Add("ALL Finish Goods (124)");
                //lbMaterial.Items.Add("ALL Waste (211)");
            }

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

        private void ListMaterialConf(string menunya)
        {
            //lbMaterial.Items.Clear();
            lbMaterial.Items.Clear();
            var dt = KC.Select_MaterialConf(menunya);

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbMaterial.Items.Add(select.MATERIALID.ToString());
                }
                lbMaterial.Items.Add("ALL Material.");
                //lbMaterial.Items.Add("ALL Finish Goods (124)");
                //lbMaterial.Items.Add("ALL Waste (211)");
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

        private void ListYear()
        {
            int period = 0;
            DateTime datee = Convert.ToDateTime(DateTime.Now);
            strYearFROM = datee.Year.ToString();
            int intyear = Convert.ToInt32(strYearFROM);
            ddlYearFrom.Items.Clear();
            ddlYearTo.Items.Clear();
            var dt = KC.Select_Years("REPORT_A");

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    period = Convert.ToInt32(select.low.ToString());
                }
            }
            for (int a = 0; a < period; a++)
            {
                ddlYearFrom.Items.Add((intyear - a).ToString());
                ddlYearTo.Items.Add((intyear - a).ToString());

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
                        if (stritem == "ALL Material.")
                        {
                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() != "ALL")
                                {

                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }

                        //else if (stritem == "ALL Finish Goods (124)")
                        //{

                        //    lbMaterial2.Items.Clear();
                        //    for (int J = 0; J < lbMaterial.Items.Count; J++)
                        //    {
                        //        MAT = lbMaterial.Items[J].Text;
                        //        if (MAT.Substring(0, 3).ToString() == "124")
                        //        {
                        //            lbMaterial2.Items.Add(MAT);
                        //        }
                        //    }
                        //}

                        //else if (stritem == "ALL Waste (211)")
                        //{

                        //    lbMaterial2.Items.Clear();
                        //    for (int J = 0; J < lbMaterial.Items.Count; J++)
                        //    {
                        //        MAT = lbMaterial.Items[J].Text;
                        //        if (MAT.Substring(0, 3).ToString() == "211")
                        //        {
                        //            //for (int g = 0; g < lbMaterial2.Items.Count; g++)
                        //            //{
                        //            //    if (MAT !=
                        //            //}
                        //            lbMaterial2.Items.Add(MAT);
                        //        }
                        //    }
                        //}
                        else
                        {
                            //if (lbMaterial2.Items.Count > 0)
                            //{
                            //    for (int J = 0; J < lbMaterial2.Items.Count; J++)
                            //    {
                            //        if (stritem != lbMaterial.Items[J].Text)
                            //        {
                            //            lbMaterial2.Items.Add(stritem);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            lbMaterial2.Items.Add(stritem);
                            //}
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

      
        private void show()
        {
            MonthYear();
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(Setting.ReportServerUrls); // Report Server URL
            ReportViewer1.ServerReport.ReportPath = "/ReportKITE/KITE_A"; // Report Name 
            ReportViewer1.ShowParameterPrompts = false;
            ReportViewer1.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "USERIP";
            reportParameterCollection[0].Values.Add(HttpContext.Current.Session["UserAppID"].ToString());
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[1].Name = "PeriodFrom";
            reportParameterCollection[1].Values.Add(ddlPeriodFrom.SelectedValue.ToString() + " " + ddlYearFrom.SelectedValue.ToString());
            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter();
            if (ddlPeriodTo.SelectedValue.ToString() != "")
            {
                reportParameterCollection[2].Name = "PeriodTo";
                reportParameterCollection[2].Values.Add("Sampai       " + ddlPeriodTo.SelectedValue.ToString() + " " + ddlYearTo.SelectedValue.ToString());
            }
            else
            {
                reportParameterCollection[2].Name = "PeriodTo";
                reportParameterCollection[2].Values.Add("");

            }
            ReportViewer1.ServerReport.SetParameters(reportParameterCollection);
            //ReportViewer1.ServerReport.Refresh();
        }


        public void MonthYear()
        {
            if (ddlPeriodFrom.SelectedValue.ToString() == "January")
            {
                strMonthFrom = "01";
                //strMonthhFrom = ""
            }
            else if (ddlPeriodFrom.SelectedValue.ToString() == "February")
            {
                strMonthFrom = "02";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "March")
            {
                strMonthFrom = "03";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "April")
            {
                strMonthFrom = "04";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "May")
            {
                strMonthFrom = "05";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "June")
            {
                strMonthFrom = "06";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "July")
            {
                strMonthFrom = "07";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "August")
            {
                strMonthFrom = "08";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "September")
            {
                strMonthFrom = "09";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "October")
            {
                strMonthFrom = "10";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "November")
            {
                strMonthFrom = "11";
            }

            else if (ddlPeriodFrom.SelectedValue.ToString() == "December")
            {
                strMonthFrom = "12";
            }

            if (ddlPeriodTo.SelectedValue.ToString() == "January")
            {
                strMonthTo = "01";
            }
            else if (ddlPeriodTo.SelectedValue.ToString() == "February")
            {
                strMonthTo = "02";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "March")
            {
                strMonthTo = "03";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "April")
            {
                strMonthTo = "04";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "May")
            {
                strMonthTo = "05";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "June")
            {
                strMonthTo = "06";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "July")
            {
                strMonthTo = "07";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "August")
            {
                strMonthTo = "08";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "September")
            {
                strMonthTo = "09";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "October")
            {
                strMonthTo = "10";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "November")
            {
                strMonthTo = "11";
            }

            else if (ddlPeriodTo.SelectedValue.ToString() == "December")
            {
                strMonthTo = "12";
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //if (refresh)
            //{
            //show();
            //}
            //if (refresh)
            //{
            CheckKITE_C();
            //}
            //Timer1.

        }

        protected bool CheckPrev()
        {
            bool submit = false;
            var dt = KC.Select_CheckStatusPrev(HttpContext.Current.Session["UserAppID"].ToString(), "Open Report KITE A");
            //string status = "";
            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    if (select.status.ToString() == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Loading')", true);
                        LabelWaitingList.Text = "Anda sedang membuka report ini sebelumnya. Harap tunggu.";
                        LabelWaitingList.Visible = true;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
                        Timer1.Interval = Convert.ToInt32(Setting.TimerInterval);
                        Timer1.Enabled = true;
                    }
                    else if (select.status.ToString() != "0")
                    {
                        submit = true;
                    }

                }
            }
            else
            {
                submit = true;
            }

            return submit;
        }

        protected void CheckKITE_C()
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
                        LabelWaitingList.Visible = false;
                        //UpdatePanel3.Visible = false;
                        show();
                        refresh = false;
                        Timer1.Enabled = false;
                    }
                    else if (select.status.ToString() == "2") 
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Loading')", true);
                        LabelWaitingList.Visible = false;
                        //UpdatePanel3.Visible = false;
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
        protected void ddlPeriodTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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

        protected void Timer2_Tick(object sender, EventArgs e)
        {
            bool check = CheckPrev();
            if (check)
            {
                opensearch();
                Timer2.Enabled = false;
            }
            else
            {
                Timer2.Enabled = false;
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

        
    }
}