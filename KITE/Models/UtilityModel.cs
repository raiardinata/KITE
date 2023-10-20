using System;
using System.Web.UI.WebControls;

namespace KITE.Models
{
    public class UtilityModel : System.Web.UI.Page
    {
        public void UploadCsvErrorHandler(Exception ex, GridView gridView, Label errorLabel)
        {
            gridView.DataSource = null;
            gridView.DataBind();

            errorLabel.Text = "";
            errorLabel.Enabled = true;
            errorLabel.Text = ex.Message + "<br/>";
            Session["FilePath"] = "";
            return;

        }
    }
}