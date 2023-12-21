using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace KITE
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            if (Session["FullName"] == null)
            {
                Response.Redirect("~\\Pages\\ContentPages\\Login.aspx");
            }
        }

        void Session_End(object sender, EventArgs e)
        {
            Response.Redirect("~\\Pages\\ContentPages\\Login.aspx");
        }
    }
}