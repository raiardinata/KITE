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
    public partial class Input_KITEE : System.Web.UI.Page
    {
        Controllers.KiteController KC = new Controllers.KiteController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();

        Controllers.LoginController login = new Controllers.LoginController();

        bool refresh = false;
        static string localip = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            if (!IsPostBack)
            {
                Timer2.Enabled = true;
            }   
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
             bool check = CheckPrev();
             if (check)
             {
                 string strCompany = "";
                 string strSales = "";
                 string strCustGr = "";
                 string strCustID = "";
                 string strDateLow = "";
                 string strDateHigh = "";

                 DateTime FromDate;
                 DateTime ToDate;
                 int FromDateint;
                 int ToDateint;
                 bool boleh = true;
                 if (txtDateFrom.Text == "" || txtDateTo.Text == "")
                 {
                     lblErrorPeriod.Text = "Pilih Tanggal";
                     lblErrorPeriod.Visible = true;
                     boleh = false;
                 }

                

                 //bool boleh = true;
                 lblErrorPeriod.Visible = false;

                 DateTime.TryParseExact(txtDateFrom.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
                 DateTime.TryParseExact(txtDateTo.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);
                 int yearfrom = 0;
                 int yearto = 0;
                 yearfrom = FromDate.Year;
                 yearto = ToDate.Year;

                 if (yearfrom > yearto)
                 {
                     lblErrorPeriod.Text = "Period Akhir Tidak Boleh Kurang Dari Period Awal";
                     lblErrorPeriod.Visible = true;
                     boleh = false;
                 }
                 else if (yearfrom == yearto)
                 {
                     FromDateint = FromDate.DayOfYear;
                     ToDateint = ToDate.DayOfYear;

                     if (ToDateint < FromDateint)
                     {
                         lblErrorPeriod.Text = "Period Akhir Tidak Boleh Kurang Dari Period Awal";
                         lblErrorPeriod.Visible = true;
                         boleh = false;
                     }
                 }

                 if (boleh)
                 {
                     string struserip = HttpContext.Current.Session["UserAppID"].ToString();
                     DateTime datef = DateTime.ParseExact(txtDateFrom.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                     DateTime datet = DateTime.ParseExact(txtDateTo.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                     strCompany = ddlCompanyCode.SelectedValue.ToString();
                     strSales = ddlPlant.SelectedValue.ToString();
                     strCustGr = ddlcustomerGroup.SelectedValue.ToString();
                     strCustID = ddlCustomerID.SelectedValue.ToString();
                     strDateLow = datef.ToString("yyyyMMdd"); //.ToString("yyyyMMdd");
                     strDateHigh = datet.ToString("yyyyMMdd");
                     //SubmitInformation();
                     SubmitInformation(strCompany, strSales, strCustGr, strCustID, strDateLow, strDateHigh, struserip);
                 }

             }
        }

        protected void SubmitInformation(string Company, string sales, string custgr, string custid, string sDate_LOW, string sDate_HIGH, string User_IP)
        {

            KC.InsertQueueE(Company, sales, custgr, custid, sDate_LOW, sDate_HIGH, HttpContext.Current.Session["UserAppID"].ToString(), "Open", "Open Report KITE E", HttpContext.Current.Session["UserAppID"].ToString(), HttpContext.Current.Session["FullName"].ToString());
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Loading')", true);
            LabelWaitingList.Visible = false;
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
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUpBaru('#Insert')", true);
            ListCompany();
            ListOrganization();
            ListCustomerGroup();
            ListCustomerid();
            labelCompany();
            labelplant();
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

        private void ListOrganization()
        {
            ddlPlant.Items.Clear();
            var dt = KC.Select_Company();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlPlant.Items.Add(select.low.ToString());
                }
            }

        }

        private void ListCustomerGroup()
        {
            ddlcustomerGroup.Items.Clear();
            //ddlCustomerID.Items.Clear();
            var dt = KC.Select_CustGroup("REPORT_E");

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlcustomerGroup.Items.Add(select.low.ToString());
                }
            }

        }

        private void ListCustomerid()
        {
            ddlCustomerID.Items.Clear();
            //ddlCustomerID.Items.Clear();
            var dt = KC.Select_CustID("REPORT_E");

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlCustomerID.Items.Add(select.low.ToString());
                }
            }

        }

        private void show()
        {
            string strDateLow = "";
            string strDateHigh = "";
            DateTime datef = DateTime.ParseExact("01-01-2019", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime datet = DateTime.ParseExact("01-01-2019", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            strDateLow = datef.ToString("MMMM yyyy"); //.ToString("yyyyMMdd");
            strDateHigh = datet.ToString("MMMM yyyy");

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(Setting.ReportServerUrls); // Report Server URL
            ReportViewer1.ServerReport.ReportPath = "/ReportKITE/KITE_E"; // Report Name 
            ReportViewer1.ShowParameterPrompts = false;
            ReportViewer1.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[3];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "USERIP";
            reportParameterCollection[0].Values.Add(HttpContext.Current.Session["UserAppID"].ToString());
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[1].Name = "FromDate";
            //reportParameterCollection[1].Values.Add(txtDateFrom.Text);
            reportParameterCollection[1].Values.Add(strDateLow);

            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter();
            if (txtDateTo.Text != "")
            {
                reportParameterCollection[2].Name = "ToDate";
                //reportParameterCollection[2].Values.Add("s/d " + txtDateTo.Text);
                reportParameterCollection[2].Values.Add("Sampai       " + strDateHigh);
            }
            else
            {
                reportParameterCollection[2].Name = "ToDate";
                reportParameterCollection[2].Values.Add("");

            }
            ReportViewer1.ServerReport.SetParameters(reportParameterCollection);


        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            CheckKITE_E();
        }

        protected void CheckKITE_E()
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
                        show();
                        refresh = false;
                        Timer1.Enabled = false;
                    }
                    else if (select.status.ToString() == "2")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Loading')", true);
                        LabelWaitingList.Visible = false;
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

        protected bool CheckPrev()
        {
            bool submit = false;
            var dt = KC.Select_CheckStatusPrev(HttpContext.Current.Session["UserAppID"].ToString(), "Open Report KITE E");
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
            bool check = CheckPrev();
            if (check)
            {
                btnSearch_Click(sender, e);
                Timer2.Enabled = false;
            }
            else
            {
                Timer2.Enabled = false;
            }
        }
    }
}