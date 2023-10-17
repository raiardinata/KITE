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
using System;
using Microsoft.Reporting.WebForms;
using System.Globalization;


namespace KITE.Pages.ContentPages
{
    public partial class ChangePassword : System.Web.UI.Page
    {

        Controllers.UserAPPController UAP = new Controllers.UserAPPController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUser.Text = HttpContext.Current.Session["UserName"].ToString();
            if (!IsPostBack)
            {
                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfPassword.Text = "";
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblErrConf.Visible = false;
            lblErrNew.Visible = false;
            lblerrOld.Visible = false;
            lblErrConf.Text = "";
            lblErrNew.Text = "";
            lblerrOld.Text = "";
            bool boleh = false;
            bool nulltext = true;
            if (txtOldPassword.Text == "")
            {
                lblerrOld.Visible = true;
                lblerrOld.Text = "*)";
                nulltext = false;
            }

            if (txtNewPassword.Text == "")
            {
                lblErrNew.Visible = true;
                lblErrNew.Text = "*)";
                nulltext = false;
            }

            if (txtConfPassword.Text == "")
            {
                lblErrConf.Visible = true;
                lblErrConf.Text = "*)";
                nulltext = false;
            }

            if (nulltext)
            {
                if (txtNewPassword.Text == txtConfPassword.Text)
                {
                    boleh = checkold(lblUser.Text, txtOldPassword.Text);
                    if (boleh)
                    {
                        updatepass(lblUser.Text, txtNewPassword.Text);
                        txtOldPassword.Text = "";
                        txtNewPassword.Text = "";
                        txtConfPassword.Text = "";
                        lblerrOld.Visible = false;
                        lblErrNew.Visible = false;
                        lblErrConf.Visible = false;
                        Response.Redirect("Home", true);
                    }
                    else
                    {
                        lblerrOld.Visible = true;
                        lblerrOld.Text = "Old Password Not Matched.";
                    }

                }
                else
                {
                    lblErrConf.Visible = true;
                    lblErrConf.Text = "Not Matched.";
                    boleh = false;

                    lblErrNew.Visible = true;
                    lblErrNew.Text = "Not Matched.";
                    boleh = false;
                }
            }

        }

        protected bool checkold(string userappid, string password)
        {
            bool match = false;

            password = Kite.EncryptDecrypt.CryptorEngine.Encrypt(password, true);

            var dt = UAP.Check_Password(userappid, password);

            if (dt.Count() > 0)
            {
                match = true;
            }
            return match;
        }

        protected void updatepass(string userappid, string password)
        {
            password = Kite.EncryptDecrypt.CryptorEngine.Encrypt(password, true);

            UAP.UpdatePassword(userappid, password);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOldPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfPassword.Text = "";
        }
    }
}