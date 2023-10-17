using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace KITE.Pages.ContentPages
{
    public partial class LogActivities : System.Web.UI.Page
    {
        Controllers.KiteController KC = new Controllers.KiteController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();

        Controllers.LoginController login = new Controllers.LoginController();
        Controllers.UserAPPController user = new Controllers.UserAPPController();

        bool refresh = false;
   
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
        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            lblErrorPeriod.Visible = false;
            bool boleh = true;
            DateTime FromDate;
            DateTime ToDate;
            int FromDateint;
            int ToDateint;

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
            if (txtDateFrom.Text == "" || txtDateTo.Text == "")
            {
                lblErrorPeriod.Text = "Isi Periode.";
                lblErrorPeriod.Visible = true;
                boleh = false;
            }
            if (boleh)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Loading')", true);
                Timer1.Enabled = true;
                Timer1.Interval = Convert.ToInt32(Setting.TimerInterval);
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUpBaru('#Insert')", true);
            ListUser();
            ListStatus();
        }
        private void ListUser()
        {
             ddlUser.Items.Clear();
            var dt = user.Select_userall();
            ddlUser.Items.Add("*");
            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlUser.Items.Add(select.FullName.ToString());
                }
            }

        }

        private void ListStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add("*");
            ddlStatus.Items.Add("Success");
            ddlStatus.Items.Add("Failed");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Checklog();          
        }

        protected void Checklog()
        {
            var dt = KC.Select_CheckLog();
            //string status = "";
            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    if (select.no.ToString() != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Loading')", true);
                        show();
                        refresh = false;
                        Timer1.Enabled = false;
                    }
                    else if (select.no.ToString() == "")
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
    

        private void show()
        {
            string useridd = "";
            string status = "";
            if (ddlUser.SelectedValue.ToString() == "*")
            {
                useridd = "";
            }
            else
            {
                useridd = ddlUser.SelectedValue.ToString();
            }

            if (ddlStatus.SelectedValue.ToString() == "*")
            {
                status = "";
            }
            else
            {
                if (ddlStatus.SelectedValue.ToString() == "Success")
                {
                    status = "1";
                }
                else
                {
                    status = "2";
                }
            }
            DateTime datef = DateTime.ParseExact(txtDateFrom.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime datet = DateTime.ParseExact(txtDateTo.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string strDateLow = datef.ToString("yyyy-MM-dd"); //.ToString("yyyyMMdd");
            string strDateHigh = datet.ToString("yyyy-MM-dd");
                 
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(Setting.ReportServerUrls); // Report Server URL
            ReportViewer1.ServerReport.ReportPath = "/ReportKITE/LogActivities"; // Report Name 
            ReportViewer1.ShowParameterPrompts = false;
            ReportViewer1.ShowPrintButton = true;

            // Below code demonstrate the Parameter passing method. User only if you have parameters into the reports.
            Microsoft.Reporting.WebForms.ReportParameter[] reportParameterCollection = new Microsoft.Reporting.WebForms.ReportParameter[4];
            reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[0].Name = "USERIP";
            reportParameterCollection[0].Values.Add(useridd);
            reportParameterCollection[1] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[1].Name = "STATUS";
            reportParameterCollection[1].Values.Add(status);
            reportParameterCollection[2] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[2].Name = "DATELOW";
            reportParameterCollection[2].Values.Add(strDateLow);
            reportParameterCollection[3] = new Microsoft.Reporting.WebForms.ReportParameter();
            reportParameterCollection[3].Name = "DATEHIGH";
            reportParameterCollection[3].Values.Add(strDateHigh);

           
            ReportViewer1.ServerReport.SetParameters(reportParameterCollection);

            //ReportViewer1.ServerReport.Refresh();
        }

        protected void Timer2_Tick(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
            Timer2.Enabled = false;
        }

    }
}