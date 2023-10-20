using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace KITE.Models
{
    public class GIRawMaterialFunctionModel : System.Web.UI.Page
    {
        public Tuple<string, ArrayList> GIRawMaterialGenerateColumnAndCsvData(List<GIRawMaterialViewModel> csvList)
        {
            List<string> listColumnName = new List<string>();
            listColumnName.Add("Year_Period");
            listColumnName.Add("Month_Period");

            // Get the type of the object
            Type type = csvList[0].GetType();

            // Get all properties of the type
            PropertyInfo[] properties = type.GetProperties();

            // Loop through the properties and find the one with the desired value
            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(csvList[0]) != null)
                {
                    // Print the name of the property
                    listColumnName.Add(property.Name);
                }
            }
            // populate csv column
            string columns = "";
            foreach (string column in listColumnName)
            {
                columns += column + ",";
            }
            columns = columns.Substring(0, columns.Length - 1);

            // populate csv values
            ArrayList valuesArray = new ArrayList();
            foreach (GIRawMaterialViewModel csvValues in csvList)
            {
                int yearPeriod = csvValues.Posting_Date.Year;
                int monthPeriod = csvValues.Posting_Date.Month;

                List<object> valuesList = new List<object>();
                valuesList.Add(yearPeriod);
                valuesList.Add(monthPeriod);
                valuesList.Add(csvValues.Posting_Date.Date.ToString("yyyy/MM/dd"));
                valuesList.Add(csvValues.Document_Date.Date.ToString("yyyy/MM/dd"));
                valuesList.Add(csvValues.Document_Header_Text);
                valuesList.Add(csvValues.Material);
                valuesList.Add(csvValues.Material_Description);
                valuesList.Add(csvValues.Plant);
                valuesList.Add(csvValues.Storage_Location);
                valuesList.Add(csvValues.Movement_Type);
                valuesList.Add(csvValues.Material_Document);
                valuesList.Add(csvValues.Batch);
                valuesList.Add(Convert.ToDecimal(csvValues.Qty_in_Un_of_Entry));
                valuesList.Add(csvValues.Unit_of_Entry);
                valuesList.Add(csvValues.Entry_Date.Date.ToString("yyyy/MM/dd"));
                valuesList.Add(csvValues.Time_of_Entry);
                valuesList.Add(csvValues.User_name);
                valuesList.Add(csvValues.Base_Unit_of_Measure);
                valuesList.Add(Convert.ToDecimal(csvValues.Quantity));
                valuesList.Add(Convert.ToDecimal(csvValues.Amount_in_LC));
                valuesList.Add(csvValues.Goods_recipient);
                valuesArray.Add(valuesList);
            }
            return Tuple.Create(columns, valuesArray);
        }
        public Tuple<Exception, string, List<GIRawMaterialViewModel>> GIRawMaterialReadCsvFile(FileUpload fileUpload)
        {
            ReadCsvModel readCsv = new ReadCsvModel();
            List<GIRawMaterialViewModel> NullDataList = new List<GIRawMaterialViewModel>();
            NullDataList.Add(new GIRawMaterialViewModel { Material = "null" });

            try
            {
                Tuple<Exception, string> valid = readCsv.FileChecker(fileUpload);
                if (valid.Item1.Message != "null" && valid.Item2 != "valid")
                {
                    return Tuple.Create(valid.Item1, "Not a valid file.", NullDataList);
                }

                List<GIRawMaterialViewModel> CsvDataList;
                //Dictionary<string, GIRawMaterialViewModel> CsvDataDictionary;

                // Save the file to the server
                string filePath = Server.MapPath("~/UploadedFiles/" + fileUpload.FileName);
                fileUpload.SaveAs(filePath);

                using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
                {
                    // Read records using CsvHelper
                    CsvDataList = csvData.GetRecords<GIRawMaterialViewModel>().ToList();

                    // Assign keys based on the Id property
                    //CsvDataDictionary = CsvDataList.ToDictionary(data => data.Name);
                }

                return Tuple.Create(new Exception("null"), filePath, CsvDataList);

            }
            catch (Exception ex)
            {
                string pattern = "(.*?)(Headers:)";
                Match match = Regex.Match(ex.Message, pattern, RegexOptions.Singleline);
                Exception loadCsvException;

                if (match.Success)
                {
                    // Return the substring before "Headers:"
                    loadCsvException = new Exception(match.Groups[1].Value);
                }
                else
                {
                    // Return the original string if "Headers:" is not found
                    loadCsvException = new Exception(ex.Message);
                }
                return Tuple.Create(loadCsvException, "null", NullDataList);
            }
        }
    }

    public class GIRawMaterialViewModel
    {
        [Name("Posting Date")]
        public DateTime Posting_Date { get; set; }

        [Name("Document Date")]
        public DateTime Document_Date { get; set; }

        [Name("Document Header Text")]
        public string Document_Header_Text { get; set; }

        public string Material { get; set; }

        [Name("Material Description")]
        public string Material_Description { get; set; }

        public string Plant { get; set; }

        [Name("Storage Location")]
        public string Storage_Location { get; set; }

        [Name("Movement Type")]
        public string Movement_Type { get; set; }

        [Name("Material Document")]
        public string Material_Document { get; set; }

        public string Batch { get; set; }

        [Name("Qty in Un. of Entry")]
        public string Qty_in_Un_of_Entry { get; set; }

        [Name("Unit of Entry")]
        public string Unit_of_Entry { get; set; }

        [Name("Entry Date")]
        public DateTime Entry_Date { get; set; }

        [Name("Time of Entry")]
        public string Time_of_Entry { get; set; }

        [Name("User name")]
        public string User_name { get; set; }

        [Name("Base Unit of Measure")]
        public string Base_Unit_of_Measure { get; set; }

        public string Quantity { get; set; }

        [Name("Amount in LC")]
        public string Amount_in_LC { get; set; }

        [Name("Goods recipient")]
        public string Goods_recipient { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GIRawMaterialViewModel other = (GIRawMaterialViewModel)obj;
            return
                Posting_Date == other.Posting_Date &&
                Document_Date == other.Document_Date &&
                Document_Header_Text == other.Document_Header_Text &&
                Material == other.Material &&
                Material_Description == other.Material_Description &&
                Plant == other.Plant &&
                Storage_Location == other.Storage_Location &&
                Movement_Type == other.Movement_Type &&
                Material_Document == other.Material_Document &&
                Batch == other.Batch &&
                Qty_in_Un_of_Entry == other.Qty_in_Un_of_Entry &&
                Unit_of_Entry == other.Unit_of_Entry &&
                Entry_Date == other.Entry_Date &&
                Time_of_Entry == other.Time_of_Entry &&
                User_name == other.User_name &&
                Base_Unit_of_Measure == other.Base_Unit_of_Measure &&
                Quantity == other.Quantity &&
                Amount_in_LC == other.Amount_in_LC &&
                Goods_recipient == other.Goods_recipient;
        }

        public override int GetHashCode()
        {
            return (
                Posting_Date,
                Document_Date,
                Document_Header_Text,
                Material,
                Material_Description,
                Plant,
                Storage_Location,
                Movement_Type,
                Material_Document,
                Batch,
                Qty_in_Un_of_Entry,
                Unit_of_Entry,
                Entry_Date,
                Time_of_Entry,
                User_name,
                Base_Unit_of_Measure,
                Quantity,
                Amount_in_LC,
                Goods_recipient
             ).GetHashCode();
        }
    }
}