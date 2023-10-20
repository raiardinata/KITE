using CsvHelper;
using System;
using System.IO;
using System.Web;

namespace KITE.Models
{
    public class ReadCsvModel : System.Web.UI.Page
    {
        public CsvReader ReadCsvFile(string filePath, string delimiter)
        {
            // Read the content of the CSV file global function
            var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
            };
            return new CsvReader(new StreamReader(filePath), config);
        }

        public Tuple<Exception, string> FileChecker(System.Web.UI.WebControls.FileUpload fileUpload)
        {
            try
            {
                // Check if a file is selected
                if (fileUpload.HasFile)
                {
                    // Get the uploaded file
                    HttpPostedFile uploadedFile = fileUpload.PostedFile;

                    // Check if the file is a CSV file
                    if (uploadedFile.ContentType == "csv" || Path.GetExtension(uploadedFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        return Tuple.Create(new Exception("null"), "valid");
                    }
                    else
                    {
                        throw new Exception("Your file upload is not csv file, please upload only csv file type.");
                    }
                }
                throw new Exception("No file was selected, please select csv file to proceed.");
            }
            catch (Exception ex)
            {
                return Tuple.Create(ex, "");
            }

        }
    }
}