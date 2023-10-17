using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KITE.Pages.ContentPages
{
    public partial class Login : System.Web.UI.Page
    {
        Controllers.LoginController login = new Controllers.LoginController();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["ActiveUser"] = null;
            HttpContext.Current.Session["UserFullName"] = "";
            HttpContext.Current.Session["UserAppID"] = "";
            HttpContext.Current.Session["UserType"] = "";
            //HttpContext.Current.Session.Timeout = 1;
            txtUserName.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            doLogin();
        }

        private void doLogin()
        {
            bool success = false;
            string password = Kite.EncryptDecrypt.CryptorEngine.Encrypt(txtPassword.Text, true);
            foreach (var logins in login.Login_User(txtUserName.Text, password))
            {
                success = true;
                login.InsertLog("Login", "Login Report KITE", "1", logins.UserAppID.ToString(), logins.FullName.ToString());
                HttpContext.Current.Session["UserAppID"] = logins.UserAppID;
                HttpContext.Current.Session["UserName"] = logins.username;
                HttpContext.Current.Session["ActiveUser"] = logins.ActiveUser;
                HttpContext.Current.Session["FullName"] = logins.FullName.ToString();
                HttpContext.Current.Session["UserType"] = logins.UserType.ToString();

                //try
                //{
                //if (HttpContext.Current.Session["Url"] != null)
                //{
                //    string Url = HttpContext.Current.Session["Url"].ToString();
                //    HttpContext.Current.Session["Url"] = null;
                //    Response.Redirect(Url.Split('/')[5].ToString(), true);

                //}
                //else
                //{

                //Response.Redirect("Home", true);

                Response.Redirect("Home", true);

                //}
                //}
                //catch
                //{
                //    Response.Redirect("Home", true);
                //}
            }
            if (!success)
            {
                login.InsertLog("Login", "Login Report KITE", "2", "", txtUserName.Text);                
            }
            //  lbError.Visible = true;

        }
    }
}