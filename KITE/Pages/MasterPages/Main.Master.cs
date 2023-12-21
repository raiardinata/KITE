using System;
using System.Web;
//using System.Web.UI. 

namespace KITE.Pages.MasterPages
{
    public partial class Main : System.Web.UI.MasterPage
    {
        Controllers.MasterController MC = new Controllers.MasterController();
        Controllers.LoginController login = new Controllers.LoginController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Session["UserType"] + string.Empty))
                {
                    label1.Text = HttpContext.Current.Session["FullName"].ToString();
                    foreach (var MenuLink in MC.Menu_Select(Session["UserType"].ToString()))
                    {
                        this.FindControl(MenuLink.MenuLink.ToLower()).Visible = true;
                    }

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