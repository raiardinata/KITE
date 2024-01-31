using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KITE.Models
{
    public class ExportSalesFunctionModel : System.Web.UI.Page
    {
        public List<ExportSalesWithKilosConvertionViewModel> ExportSalesDatatableToList(DataTable datatable)
        {
            return datatable.AsEnumerable()
            .Select(row => new ExportSalesWithKilosConvertionViewModel
            {
                Posting_Date = row.Field<DateTime>("Posting_Date"),
                Document_Date = row.Field<DateTime>("Document_Date"),
                Document_Header_Text = row.Field<string>("Document_Header_Text"),
                Purchase_Order = row.Field<string>("Purchase_Order"),
                Reference = row.Field<string>("Reference"),
                Material = row.Field<string>("Material"),
                Material_Description = row.Field<string>("Material_Description"),
                Plant = row.Field<string>("Plant"),
                Storage_Location = row.Field<string>("Storage_Location"),
                Movement_Type = row.Field<string>("Movement_Type"),
                Material_Document = row.Field<string>("Material_Document"),
                Batch = row.Field<string>("Batch"),
                Qty_in_Un_of_Entry = row.Field<Decimal>("Qty_in_Un_of_Entry"),
                Unit_of_Entry = row.Field<string>("Unit_of_Entry"),
                Entry_Date = row.Field<DateTime>("Entry_Date"),
                Time_of_Entry = row.Field<TimeSpan>("Time_of_Entry").ToString(),
                User_Name = row.Field<string>("User_name"),
                Base_Unit_of_Measure = row.Field<string>("Base_Unit_of_Measure"),
                Quantity = row.Field<Decimal>("Quantity"),
                Debit_Credit_Ind = row.Field<string>("Debit_Credit_Ind"),
                Amount_in_LC = row.Field<Decimal>("Amount_in_LC"),
                Sales_Order = row.Field<string>("Sales_Order"),
                Text = row.Field<string>("Text"),
                Customer = row.Field<string>("Customer"),
                Vendor = row.Field<string>("Vendor"),
                Vendor_Name = row.Field<string>("Vendor_Name"),
                Goods_recipient = row.Field<string>("Goods_recipient"),
                SO = row.Field<string>("SO"),
                SaType = row.Field<string>("SaType"),
                Sold_to_party = row.Field<string>("Sold_to_party"),
                Name_1 = row.Field<string>("Name_1"),
                PO_Number = row.Field<string>("PO_Number"),
                NO_PEB = row.Field<string>("NO_PEB"),
                Tanggal_PEB = row.Field<DateTime>("Tanggal_PEB").ToString(),
                Non_KITE = row.Field<string>("Non_KITE"),
                Designated_Country = row.Field<string>("Designated_Country"),
                KilosConvertion = row.Field<decimal>("KilosConvertion"),
            })
            .ToList();
        }

        public Tuple<string, ArrayList> ExportSalesGenerateColumnAndCsvData(List<ExportSalesWithKilosConvertionViewModel> csvList)
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
            foreach (ExportSalesWithKilosConvertionViewModel csvValues in csvList)
            {
                int yearPeriod = csvValues.Posting_Date.Year;
                int monthPeriod = csvValues.Posting_Date.Month;

                List<object> valuesList = new List<object>();
                valuesList.Add(yearPeriod);
                valuesList.Add(monthPeriod);
                valuesList.Add(csvValues.Posting_Date.Date.ToString("yyyy/MM/dd"));
                valuesList.Add(csvValues.Document_Date.Date.ToString("yyyy/MM/dd"));
                valuesList.Add(csvValues.Document_Header_Text);
                valuesList.Add(csvValues.Purchase_Order);
                valuesList.Add(csvValues.Reference);
                valuesList.Add(csvValues.Material);
                valuesList.Add(csvValues.Material_Description);
                valuesList.Add(csvValues.Plant);
                valuesList.Add(csvValues.Storage_Location);
                valuesList.Add(csvValues.Movement_Type);
                valuesList.Add(csvValues.Material_Document);
                valuesList.Add(csvValues.Batch);
                valuesList.Add(csvValues.Qty_in_Un_of_Entry);
                valuesList.Add(csvValues.Unit_of_Entry);
                valuesList.Add(csvValues.Entry_Date.Date.ToString("yyyy/MM/dd"));
                valuesList.Add(csvValues.Time_of_Entry);
                valuesList.Add(csvValues.User_Name);
                valuesList.Add(csvValues.Base_Unit_of_Measure);
                valuesList.Add(csvValues.Quantity);
                valuesList.Add(csvValues.Debit_Credit_Ind);
                valuesList.Add(csvValues.Amount_in_LC);
                valuesList.Add(csvValues.Sales_Order);
                valuesList.Add(csvValues.Text);
                valuesList.Add(csvValues.Customer);
                valuesList.Add(csvValues.Vendor);
                valuesList.Add(csvValues.Vendor_Name);
                valuesList.Add(csvValues.Goods_recipient);
                valuesList.Add(csvValues.SO);
                valuesList.Add(csvValues.SaType);
                valuesList.Add(csvValues.Sold_to_party);
                valuesList.Add(csvValues.Name_1);
                valuesList.Add(csvValues.PO_Number);
                valuesList.Add(csvValues.NO_PEB);
                valuesList.Add(csvValues.Tanggal_PEB);
                valuesList.Add(csvValues.KilosConvertion);
                valuesList.Add(csvValues.Non_KITE);
                valuesList.Add(csvValues.Designated_Country);
                valuesArray.Add(valuesList);
            }
            return Tuple.Create(columns, valuesArray);
        }
        public Tuple<Exception, string, List<ExportSalesWithKilosConvertionViewModel>> ExportSalesReadCsvFile(System.Web.UI.WebControls.FileUpload fileUpload)
        {
            List<ExportSalesWithKilosConvertionViewModel> CsvDataList;
            ReadCsvModel readCsv = new ReadCsvModel();

            try
            {
                Tuple<Exception, string> valid = readCsv.FileChecker(fileUpload);
                if (valid.Item1.Message != "null" && valid.Item2 != "valid")
                {
                    return new Tuple<Exception, string, List<ExportSalesWithKilosConvertionViewModel>>(valid.Item1, "Not a valid file.", null);
                }

                string filePath = Server.MapPath("~/UploadedFiles/" + fileUpload.FileName);
                fileUpload.SaveAs(filePath);

                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("exportSales", filePath, ";", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    return new Tuple<Exception, string, List<ExportSalesWithKilosConvertionViewModel>>(readCsvResult.Item2, filePath, null);
                }

                CsvDataList = (List<ExportSalesWithKilosConvertionViewModel>)readCsvResult.Item1;

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
                return new Tuple<Exception, string, List<ExportSalesWithKilosConvertionViewModel>>(loadCsvException, "null", null);
            }
        }
    }

    public class ExportSalesViewModel
    {
        [Name("Posting Date")]
        public DateTime Posting_Date { get; set; }

        [Name("Document Date")]
        public DateTime Document_Date { get; set; }

        [Name("Document Header Text")]
        public string Document_Header_Text { get; set; }

        [Name("Purchase Order")]
        public string Purchase_Order { get; set; }

        public string Reference { get; set; }

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
        public Decimal Qty_in_Un_of_Entry { get; set; }

        [Name("Unit of Entry")]
        public string Unit_of_Entry { get; set; }

        [Name("Entry Date")]
        public DateTime Entry_Date { get; set; }

        [Name("Time of Entry")]
        public string Time_of_Entry { get; set; }

        [Name("User name")]
        public string User_Name { get; set; }

        [Name("Base Unit of Measure")]
        public string Base_Unit_of_Measure { get; set; }

        public Decimal Quantity { get; set; }

        [Name("Debit/Credit Ind.")]
        public string Debit_Credit_Ind { get; set; }

        [Name("Amount in LC")]
        public Decimal Amount_in_LC { get; set; }

        [Name("Sales Order")]
        public string Sales_Order { get; set; }

        public string Text { get; set; }

        public string Customer { get; set; }

        public string Vendor { get; set; }

        [Name("Vendor Name")]
        public string Vendor_Name { get; set; }

        [Name("Goods recipient")]
        public string Goods_recipient { get; set; }

        [Name("SO")]
        public string SO { get; set; }

        public string SaType { get; set; }

        [Name("Sold to party")]
        public string Sold_to_party { get; set; }

        [Name("Name 1")]
        public string Name_1 { get; set; }

        [Name("PO Number")]
        public string PO_Number { get; set; }

        [Name("NO PEB")]
        public string NO_PEB { get; set; }

        [Name("Tanggal PEB")]
        public string Tanggal_PEB { get; set; }

        [Name("Non KITE")]
        public string Non_KITE { get; set; }

        [Name("Negara Tujuan")]
        public string Designated_Country { get; set; }
    }

    public class ExportSalesWithKilosConvertionViewModel
    {
        [Name("Posting Date")]
        public DateTime Posting_Date { get; set; }

        [Name("Document Date")]
        public DateTime Document_Date { get; set; }

        [Name("Document Header Text")]
        public string Document_Header_Text { get; set; }

        [Name("Purchase Order")]
        public string Purchase_Order { get; set; }

        public string Reference { get; set; }

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
        public Decimal Qty_in_Un_of_Entry { get; set; }

        [Name("Unit of Entry")]
        public string Unit_of_Entry { get; set; }

        [Name("Entry Date")]
        public DateTime Entry_Date { get; set; }

        [Name("Time of Entry")]
        public string Time_of_Entry { get; set; }

        [Name("User Name")]
        public string User_Name { get; set; }

        [Name("Base Unit of Measure")]
        public string Base_Unit_of_Measure { get; set; }

        public Decimal Quantity { get; set; }

        [Name("Debit/Credit Ind.")]
        public string Debit_Credit_Ind { get; set; }

        [Name("Amount in LC")]
        public Decimal Amount_in_LC { get; set; }

        [Name("Sales Order")]
        public string Sales_Order { get; set; }

        public string Text { get; set; }

        public string Customer { get; set; }

        public string Vendor { get; set; }

        [Name("Vendor Name")]
        public string Vendor_Name { get; set; }

        [Name("Goods recipient")]
        public string Goods_recipient { get; set; }

        [Name("SO")]
        public string SO { get; set; }

        public string SaType { get; set; }

        [Name("Sold to party")]
        public string Sold_to_party { get; set; }

        [Name("Name 1")]
        public string Name_1 { get; set; }

        [Name("PO Number")]
        public string PO_Number { get; set; }

        [Name("NO PEB")]
        public string NO_PEB { get; set; }

        [Name("Tanggal PEB")]
        public string Tanggal_PEB { get; set; }

        public decimal KilosConvertion { get; set; }

        [Name("Non KITE")]
        public string Non_KITE { get; set; }

        [Name("Negara Tujuan")]
        public string Designated_Country { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ExportSalesWithKilosConvertionViewModel other = (ExportSalesWithKilosConvertionViewModel)obj;
            return
                Posting_Date == other.Posting_Date &&
                Document_Date == other.Document_Date &&
                Document_Header_Text == other.Document_Header_Text &&
                Purchase_Order == other.Purchase_Order &&
                Reference == other.Reference &&
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
                User_Name == other.User_Name &&
                Base_Unit_of_Measure == other.Base_Unit_of_Measure &&
                Quantity == other.Quantity &&
                Debit_Credit_Ind == other.Debit_Credit_Ind &&
                Amount_in_LC == other.Amount_in_LC &&
                Sales_Order == other.Sales_Order &&
                Text == other.Text &&
                Customer == other.Customer &&
                Vendor == other.Vendor &&
                Vendor_Name == other.Vendor_Name &&
                Goods_recipient == other.Goods_recipient &&
                SO == other.SO &&
                SaType == other.SaType &&
                Sold_to_party == other.Sold_to_party &&
                Name_1 == other.Name_1 &&
                PO_Number == other.PO_Number &&
                NO_PEB == other.NO_PEB &&
                Tanggal_PEB == other.Tanggal_PEB &&
                Non_KITE == other.Non_KITE &&
                Designated_Country == other.Designated_Country;
        }

        public override int GetHashCode()
        {
            return (
                Posting_Date,
                Document_Date,
                Document_Header_Text,
                Purchase_Order,
                Reference,
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
                User_Name,
                Base_Unit_of_Measure,
                Quantity,
                Debit_Credit_Ind,
                Amount_in_LC,
                Sales_Order,
                Text,
                Customer,
                Vendor,
                Vendor_Name,
                Goods_recipient,
                SO,
                SaType,
                Sold_to_party,
                Name_1,
                PO_Number,
                NO_PEB,
                Tanggal_PEB,
                Non_KITE,
                Designated_Country
             ).GetHashCode();
        }
    }
}