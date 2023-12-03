using System;
using System.Web;

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
                label1.Text = HttpContext.Current.Session["FullName"].ToString();

                if (!string.IsNullOrEmpty(Session["UserType"] + string.Empty))
                {
                    //lblName.Text = ConfigurationManager.AppSettings["ServerName"].ToString();
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