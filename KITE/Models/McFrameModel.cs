using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KITE.Models
{
    public class McFrameFunctionModel : System.Web.UI.Page
    {
        public Tuple<string, ArrayList> McFrameGenerateColumnAndCsvData(List<McFrameViewModel> csvList)
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
            foreach (McFrameViewModel csvValues in csvList)
            {
                int yearPeriod = csvValues.YM.Year;
                int monthPeriod = csvValues.YM.Month;

                List<object> valuesList = new List<object>();
                valuesList.Add(yearPeriod);
                valuesList.Add(monthPeriod);
                valuesList.Add(csvValues.Calc_No);
                valuesList.Add(csvValues.Mgmt_dept_CD);
                valuesList.Add(csvValues.Management_Dept_Name);
                valuesList.Add(csvValues.YM);
                valuesList.Add(Convert.ToInt32(csvValues.Lvl));
                valuesList.Add(csvValues.Target_item_CD);
                valuesList.Add(csvValues.Item_CD);
                valuesList.Add(csvValues.Item_name);
                valuesList.Add(csvValues.Item_type_name);
                valuesList.Add(csvValues.Unit);
                valuesList.Add(Convert.ToDecimal(csvValues.Quantity));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Qty));
                valuesList.Add(Convert.ToDecimal(csvValues.Total));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Total));
                valuesList.Add(Convert.ToDecimal(csvValues.Variable_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Variable_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.Labour_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Labour_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.Depreciation));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Depreciation));
                valuesList.Add(Convert.ToDecimal(csvValues.Repair_Maintenance));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Repair_Maintenance));
                valuesList.Add(Convert.ToDecimal(csvValues.Overhead_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Overhead_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.Retur_Cost));
                valuesList.Add(Convert.ToDecimal(csvValues.STD_Retur_Cost));
                valuesArray.Add(valuesList);
            }
            return Tuple.Create(columns, valuesArray);
        }
        public Tuple<Exception, string, List<McFrameViewModel>> McFrameReadCsvFile(System.Web.UI.WebControls.FileUpload fileUpload)
        {
            ReadCsvModel readCsv = new ReadCsvModel();
            List<McFrameViewModel> NullDataList = new List<McFrameViewModel>();
            NullDataList.Add(new McFrameViewModel { Calc_No = "null" });

            try
            {
                Tuple<Exception, string> valid = readCsv.FileChecker(fileUpload);
                if (valid.Item1.Message != "null" && valid.Item2 != "valid")
                {
                    return Tuple.Create(valid.Item1, "Not a valid file.", NullDataList);
                }

                List<McFrameViewModel> CsvDataList;
                //Dictionary<string, McFrameViewModel> CsvDataDictionary;

                // Save the file to the server
                string filePath = Server.MapPath("~/UploadedFiles/" + fileUpload.FileName);
                fileUpload.SaveAs(filePath);

                using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
                {
                    // Read records using CsvHelper
                    CsvDataList = csvData.GetRecords<McFrameViewModel>().ToList();

                    // Assign keys based on the Id property
                    //CsvDataDictionary = CsvDataList.ToDictionary(data => data.Name);
                    csvData.Dispose();
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
    public class McFrameViewModel
    {
        [Name("Calc. No.")]
        public string Calc_No { get; set; }

        [Name("Mgmt. dept. CD")]
        public string Mgmt_dept_CD { get; set; }

        [Name("Mgmt. dept. name")]
        public string Management_Dept_Name { get; set; }

        [Name("YM")]
        public DateTime YM { get; set; }

        [Name("LVL")]
        public int? Lvl { get; set; }

        [Name("Target item CD")]
        public string Target_item_CD { get; set; }

        [Name("Item CD")]
        public string Item_CD { get; set; }

        [Name("Item name")]
        public string Item_name { get; set; }

        [Name("Item type name")]
        public string Item_type_name { get; set; }

        public string Unit { get; set; }

        [Name("Qty.")]
        public string Quantity { get; set; }

        [Name("[STD]Qty.")]
        public string STD_Qty { get; set; }

        public string Total { get; set; }

        [Name("[STD]Total")]
        public string STD_Total { get; set; }

        [Name("Variable Cost")]
        public string Variable_Cost { get; set; }

        [Name("[STD]Variable Cost")]
        public string STD_Variable_Cost { get; set; }

        [Name("Labour Cost")]
        public string Labour_Cost { get; set; }

        [Name("[STD]Labour Cost")]
        public string STD_Labour_Cost { get; set; }

        public string Depreciation { get; set; }

        [Name("[STD]Depreciation")]
        public string STD_Depreciation { get; set; }

        [Name("Repair Maintenance")]
        public string Repair_Maintenance { get; set; }

        [Name("[STD]Repair Maintenance")]
        public string STD_Repair_Maintenance { get; set; }

        [Name("Overhead Cost")]
        public string Overhead_Cost { get; set; }

        [Name("[STD]Overhead Cost")]
        public string STD_Overhead_Cost { get; set; }

        [Name("Retur Cost")]
        public string Retur_Cost { get; set; }

        [Name("[STD]Retur Cost")]
        public string STD_Retur_Cost { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            McFrameViewModel other = (McFrameViewModel)obj;
            return
                Calc_No == other.Calc_No &&
                Mgmt_dept_CD == other.Mgmt_dept_CD &&
                Management_Dept_Name == other.Management_Dept_Name &&
                YM == other.YM &&
                Lvl == other.Lvl &&
                Target_item_CD == other.Target_item_CD &&
                Item_CD == other.Item_CD &&
                Item_name == other.Item_name &&
                Item_type_name == other.Item_type_name &&
                Unit == other.Unit &&
                Quantity == other.Quantity &&
                STD_Qty == other.STD_Qty &&
                Total == other.Total &&
                STD_Total == other.STD_Total &&
                Variable_Cost == other.Variable_Cost &&
                STD_Variable_Cost == other.STD_Variable_Cost &&
                Labour_Cost == other.Labour_Cost &&
                STD_Labour_Cost == other.STD_Labour_Cost &&
                Depreciation == other.Depreciation &&
                STD_Depreciation == other.STD_Depreciation &&
                Repair_Maintenance == other.Repair_Maintenance &&
                STD_Repair_Maintenance == other.STD_Repair_Maintenance &&
                Overhead_Cost == other.Overhead_Cost &&
                STD_Overhead_Cost == other.STD_Overhead_Cost &&
                Retur_Cost == other.Retur_Cost &&
                STD_Retur_Cost == other.STD_Retur_Cost;
        }

        public override int GetHashCode()
        {
            return (
                Calc_No,
                Mgmt_dept_CD,
                Management_Dept_Name,
                YM,
                Lvl,
                Target_item_CD,
                Item_CD,
                Item_name,
                Item_type_name,
                Unit,
                Quantity,
                STD_Qty,
                Total,
                STD_Total,
                Variable_Cost,
                STD_Variable_Cost,
                Labour_Cost,
                STD_Labour_Cost,
                Depreciation,
                STD_Depreciation,
                Repair_Maintenance,
                STD_Repair_Maintenance,
                Overhead_Cost,
                STD_Overhead_Cost,
                Retur_Cost,
                STD_Retur_Cost
             ).GetHashCode();
        }
    }
}