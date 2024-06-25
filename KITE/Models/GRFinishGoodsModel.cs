﻿using CsvHelper.Configuration.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace KITE.Models
{
    public class GRFinishGoodsFunctionModel : System.Web.UI.Page
    {
        public List<GRFinishGoodsWithConvertionViewModel> GRFinishGoodsDatatableToList(DataTable datatable)
        {
            return datatable.AsEnumerable()
            .Select(row => new GRFinishGoodsWithConvertionViewModel
            {
                Posting_Date = row.Field<DateTime>("Posting_Date"),
                Document_Date = row.Field<DateTime>("Document_Date"),
                Document_Header_Text = row.Field<string>("Document_Header_Text"),
                Material = row.Field<string>("Material"),
                Material_Description = row.Field<string>("Material_Description"),
                Plant = row.Field<string>("Plant"),
                Storage_Location = row.Field<string>("Storage_Location"),
                Movement_Type = row.Field<string>("Movement_Type"),
                Material_Document = row.Field<string>("Material_Document"),
                Batch = row.Field<string>("Batch"),
                Qty_in_Un_of_Entry = row.Field<Decimal>("Qty_in_Un_of_Entry"),
                Kilos_Convertion = row.Field<Decimal>("Kilos_Convertion"),
                Unit_of_Entry = row.Field<string>("Unit_of_Entry"),
                Entry_Date = row.Field<DateTime>("Entry_Date"),
                Time_of_Entry = row.Field<TimeSpan>("Time_of_Entry").ToString(),
                User_name = row.Field<string>("User_name"),
                Base_Unit_of_Measure = row.Field<string>("Base_Unit_of_Measure"),
                Quantity = row.Field<Decimal>("Quantity"),
                Amount_in_LC = row.Field<Decimal>("Amount_in_LC"),
                Goods_recipient = row.Field<string>("Goods_recipient"),
            })
            .ToList();
        }

        public Tuple<string, ArrayList, Exception> GRFinishGoodsGenerateColumnAndCsvData(List<GRFinishGoodsWithConvertionViewModel> csvList, string connectionString)
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
            foreach (GRFinishGoodsWithConvertionViewModel csvValues in csvList)
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
                valuesList.Add(Convert.ToDecimal(csvValues.Kilos_Convertion));
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
            return Tuple.Create(columns, valuesArray, new Exception(""));
        }
        public Tuple<Exception, string, List<GRFinishGoodsWithConvertionViewModel>> GRFinishGoodsReadCsvFile(FileUpload fileUpload)
        {
            List<GRFinishGoodsWithConvertionViewModel> CsvDataList;

            try
            {
                Tuple<Exception, string> valid = new ReadCsvModel().FileChecker(fileUpload);
                if (valid.Item1.Message != "null" && valid.Item2 != "valid")
                {
                    return new Tuple<Exception, string, List<GRFinishGoodsWithConvertionViewModel>>(valid.Item1, "Not a valid file.", null);
                }

                string filePath = Server.MapPath("~/UploadedFiles/" + fileUpload.FileName);
                fileUpload.SaveAs(filePath);

                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("grFinishGoods", filePath, ";", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    return new Tuple<Exception, string, List<GRFinishGoodsWithConvertionViewModel>>(readCsvResult.Item2, filePath, null);
                }
                CsvDataList = (List<GRFinishGoodsWithConvertionViewModel>)readCsvResult.Item1;
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
                return new Tuple<Exception, string, List<GRFinishGoodsWithConvertionViewModel>>(loadCsvException, "null", null);
            }
        }
    }

    public class GRFinishGoodsViewModel
    {
        [Name("posting date")]
        public DateTime Posting_Date { get; set; }

        [Name("document date")]
        public DateTime Document_Date { get; set; }

        [Name("document header text")]
        public string Document_Header_Text { get; set; }

        [Name("material")]
        public string Material { get; set; }

        [Name("material description")]
        public string Material_Description { get; set; }

        [Name("plant")]
        public string Plant { get; set; }

        [Name("storage location")]
        public string Storage_Location { get; set; }

        [Name("movement type")]
        public string Movement_Type { get; set; }

        [Name("material document")]
        public string Material_Document { get; set; }

        [Name("batch")]
        public string Batch { get; set; }

        [Name("qty in unit of entry")]
        public decimal Qty_in_Un_of_Entry { get; set; }

        [Name("unit of entry")]
        public string Unit_of_Entry { get; set; }

        [Name("entry date")]
        public DateTime Entry_Date { get; set; }

        [Name("time of entry")]
        public string Time_of_Entry { get; set; }

        [Name("user name")]
        public string User_name { get; set; }

        [Name("base unit of measure")]
        public string Base_Unit_of_Measure { get; set; }

        [Name("quantity")]
        public decimal Quantity { get; set; }

        [Name("amt.in loc.cur.")]
        public decimal Amount_in_LC { get; set; }

        [Name("goods recipient")]
        public string Goods_recipient { get; set; }
    }

    public class GRFinishGoodsWithConvertionViewModel
    {
        [Name("posting date")]
        public DateTime Posting_Date { get; set; }

        [Name("document date")]
        public DateTime Document_Date { get; set; }

        [Name("document header text")]
        public string Document_Header_Text { get; set; }

        [Name("material")]
        public string Material { get; set; }

        [Name("material description")]
        public string Material_Description { get; set; }

        [Name("plant")]
        public string Plant { get; set; }

        [Name("storage location")]
        public string Storage_Location { get; set; }

        [Name("movement type")]
        public string Movement_Type { get; set; }

        [Name("material document")]
        public string Material_Document { get; set; }

        [Name("batch")]
        public string Batch { get; set; }

        [Name("qty in unit of entry")]
        public decimal Qty_in_Un_of_Entry { get; set; }

        public decimal Kilos_Convertion { get; set; }

        [Name("unit of entry")]
        public string Unit_of_Entry { get; set; }

        [Name("entry date")]
        public DateTime Entry_Date { get; set; }

        [Name("time of entry")]
        public string Time_of_Entry { get; set; }

        [Name("user name")]
        public string User_name { get; set; }

        [Name("base unit of measure")]
        public string Base_Unit_of_Measure { get; set; }

        [Name("quantity")]
        public decimal Quantity { get; set; }

        [Name("amt.in loc.cur.")]
        public decimal Amount_in_LC { get; set; }

        [Name("goods recipient")]
        public string Goods_recipient { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GRFinishGoodsWithConvertionViewModel other = (GRFinishGoodsWithConvertionViewModel)obj;
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
                Kilos_Convertion == other.Kilos_Convertion &&
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
                Kilos_Convertion,
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