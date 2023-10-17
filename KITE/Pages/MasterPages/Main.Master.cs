using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
//using System.Web.UI. 
using System.Web.UI.WebControls;

namespace KITE.Pages.MasterPages
{
    public partial class Main : System.Web.UI.MasterPage
    {
        Controllers.MasterController MC = new Controllers.MasterController();
        Controllers.LoginController login = new Controllers.LoginController();
   
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(Session["UserType"] + string.Empty))
            {
                //lblName.Text = ConfigurationManager.AppSettings["ServerName"].ToString();
                foreach (var MenuLink in MC.Menu_Select(Session["UserType"].ToString()))
                {
                    this.FindControl(MenuLink.MenuLink.ToLower()).Visible = true;
                }
            }

        }

        protected void lnLogout_Click(object sender, EventArgs e)
        {
            login.InsertLog("Logout", "Logout Report KITE", "1", HttpContext.Current.Session["UserAppID"].ToString(), HttpContext.Current.Session["FullName"].ToString());
            Response.Redirect("Login");
        }
    }

}