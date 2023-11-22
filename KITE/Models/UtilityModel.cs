using System;
using System.IO;
using System.IO.Compression;
using System.Web.UI.WebControls;

namespace KITE.Models
{
    public class UtilityModel : System.Web.UI.Page
    {
        public void UploadCsvErrorHandler(Exception ex, GridView[] gridView, Label errorLabel)
        {
            foreach (GridView grid in gridView)
            {
                grid.DataSource = null;
                grid.DataBind();
            }

            errorLabel.Text = "";
            errorLabel.Enabled = true;
            errorLabel.Text = ex.Message + "<br/>";
            Session["FilePath"] = "";
            return;

        }

        public void AddFileToZip(ZipArchive zipArchive, string content, string fileName)
        {
            // Create a new entry in the zip archive
            ZipArchiveEntry entry = zipArchive.CreateEntry(fileName);

            // Write the CSV content to the entry in the zip archive
            using (StreamWriter writer = new StreamWriter(entry.Open()))
            {
                writer.Write(content);
            }
        }
    }
}