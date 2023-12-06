using System;
using System.Data;
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

        public Tuple<DataTable, Exception> BindGridview(string tableName, string yearPeriod, string monthPeriod, string connectionString)
        {
            Tuple<DataTable, Exception> dataTableRes = new DatabaseModel().SelectTableIntoDataTable(" * ", tableName, $" WHERE Year_Period = '{yearPeriod}' AND Month_Period = '{monthPeriod}' ", connectionString);
            if (dataTableRes.Item2.Message != "null")
            {
                return new Tuple<DataTable, Exception>(null, new Exception($"Terdapat masalah ketika menjalankan proses membaca table {tableName}. Detail : {dataTableRes.Item2.Message}"));
            }
            else
            {
                return new Tuple<DataTable, Exception>(dataTableRes.Item1, null);
            }
        }
    }
}