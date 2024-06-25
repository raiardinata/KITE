using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace KITE_REPORT_TEST
{
    internal class ExportSalesTest : System.Web.UI.Page
    {
        private IConfiguration Configuration;
        private string ConnectionString;
        List<ExportSalesWithKilosConvertionViewModel> CsvDataList;

        [OneTimeSetUp]
        public void SetupConfiguration()
        {
            // Build the configuration provider using the appsettings.json file
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Properties\\launchSettings.json")
                .Build();
            ConnectionString = Configuration["profiles:KITE_REPORT_TEST:environmentVariables:connectionString"];
        }

        [TearDown]
        public void TearDown()
        {
            string query = "TRUNCATE TABLE Export_Sales;";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        [Test]
        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20240604-Sales April 2024.csv")]
        public void TestA_ExportSalesCsvReadSucceed(string filePath)
        {

            List<ExportSalesWithKilosConvertionViewModel> Expected = new List<ExportSalesWithKilosConvertionViewModel>();
            Expected.Add(new ExportSalesWithKilosConvertionViewModel()
            {

                Posting_Date = DateTime.Parse("2024/04/01"),
                Document_Date = DateTime.Parse("2024/04/04"),
                Document_Header_Text = "",
                Purchase_Order = "",
                Reference = "2053708954",
                Material = "124000033",
                Material_Description = "RC 25KG P/B (HALAL Logo) NEX",
                Plant = "2201",
                Storage_Location = "2014",
                Movement_Type = "601",
                Material_Document = "4054516123",
                Batch = "P27022024",
                Qty_in_Un_of_Entry = (decimal)-2,
                Unit_of_Entry = "MT",
                Entry_Date = DateTime.Parse("2024/04/20"),
                Time_of_Entry = "08:56:42",
                User_Name = "ID0485",
                Base_Unit_of_Measure = "BAG",
                Quantity = (decimal)-80,
                Debit_Credit_Ind = "H",
                Amount_in_LC = (decimal)0.00,
                Sales_Order = "",
                Text = "",
                Customer = "1100000001",
                Vendor = "",
                Vendor_Name = "",
                Goods_recipient = "1100000001",
                SO = "853403092",
                SaType = "ZEXO",
                Sold_to_party = "1100000001",
                Name_1 = "AJINOMOTO CO.. INC.",
                PO_Number = "AN-240327",
                NO_PEB = "648553",
                Tanggal_PEB = "2024/03/19",
                Non_KITE = "NON KITE",
                Designated_Country = "Jepang",
                KilosConvertion = (decimal)-2000.0000000000

            });

            Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("exportSales", filePath, ";", ConnectionString);
            if (readCsvResult.Item2.Message != "null")
            {
                Assert.Fail(readCsvResult.Item2.Message);
            }

            CsvDataList = (List<ExportSalesWithKilosConvertionViewModel>)readCsvResult.Item1;

            CollectionAssert.AreEquivalent(Expected, CsvDataList);
        }

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20240604-Sales April Fail 2024.csv")]
        public void TestB_ExportSalesCsvReadFail(string filePath)
        {

            string Expected = "Header with name 'posting date'[0] was not found.\r\n";

            try
            {
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("exportSales", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<ExportSalesWithKilosConvertionViewModel>)readCsvResult.Item1;
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
                CollectionAssert.AreEquivalent(Expected, loadCsvException.Message);
            }
        }

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20240604-Sales April 2024.csv")]
        public void TestC_ExportSalesUploadSucceed(string filePath)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "Export_Sales";
            DatabaseModel databaseModel = new DatabaseModel();

            try
            {
                // basicly this one is LoadCsvData()
                ReadCsvModel readCsv = new ReadCsvModel();

                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("exportSales", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<ExportSalesWithKilosConvertionViewModel>)readCsvResult.Item1;
                // until this one


                ExportSalesFunctionModel csvDataProcess = new ExportSalesFunctionModel();
                Tuple<string, ArrayList> columnNameAndData = csvDataProcess.ExportSalesGenerateColumnAndCsvData(CsvDataList);
                foreach (object csvDataObject in (List<object>)columnNameAndData.Item2[0])
                {
                    if (index == 0) { yearPeriod = (int)csvDataObject; }
                    if (index == 1) { monthPeriod = (int)csvDataObject; break; }
                    index++;
                }

                Exception checkPeriodResult = databaseModel.PeriodCheck(tableName, yearPeriod, monthPeriod, ConnectionString);
                if (checkPeriodResult.Message != "Data Period Aman.")
                {
                    Assert.Fail(checkPeriodResult.Message);
                }

                Tuple<string[], Exception> iterationResult = new ReadCsvModel().IterateCsvObject(columnNameAndData.Item2);
                if (iterationResult.Item2.Message != "Pemrosesan IterateCsvObject Berhasil.")
                {
                    Assert.Fail(iterationResult.Item2.Message);
                }

                foreach (string iterionValue in iterationResult.Item1)
                {
                    Exception insertResult = new DatabaseModel().InsertIntoTable(tableName, columnNameAndData.Item1, iterionValue, ConnectionString);
                    if (insertResult.Message != "null")
                    {
                        Assert.Fail($"Terjadi kesalahan ketika Insert Into Table {tableName}. Detail : {insertResult.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            TearDown();
            Assert.Pass("Upload success");
        }

        [TestCase("2024", "04", "D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20240604-Sales April 2024.csv")]
        public void TestD_ExportSalesViewSucceed(string yearPeriodTxt, string monthPeriodTxt, string filePath)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "Export_Sales";
            DatabaseModel databaseModel = new DatabaseModel();

            try
            {
                // basicly this one is LoadCsvData()
                ReadCsvModel readCsv = new ReadCsvModel();

                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("exportSales", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<ExportSalesWithKilosConvertionViewModel>)readCsvResult.Item1;
                // until this one

                ExportSalesFunctionModel csvDataProcess = new ExportSalesFunctionModel();
                Tuple<string, ArrayList> columnNameAndData = csvDataProcess.ExportSalesGenerateColumnAndCsvData(CsvDataList);
                foreach (object csvDataObject in (List<object>)columnNameAndData.Item2[0])
                {
                    if (index == 0) { yearPeriod = (int)csvDataObject; }
                    if (index == 1) { monthPeriod = (int)csvDataObject; break; }
                    index++;
                }

                Exception checkPeriodResult = databaseModel.PeriodCheck(tableName, yearPeriod, monthPeriod, ConnectionString);
                if (checkPeriodResult.Message != "Data Period Aman.")
                {
                    Assert.Fail(checkPeriodResult.Message);
                }

                Tuple<string[], Exception> iterationResult = new ReadCsvModel().IterateCsvObject(columnNameAndData.Item2);
                if (iterationResult.Item2.Message != "Pemrosesan IterateCsvObject Berhasil.")
                {
                    Assert.Fail(iterationResult.Item2.Message);
                }

                foreach (string iterionValue in iterationResult.Item1)
                {
                    Exception insertResult = new DatabaseModel().InsertIntoTable(tableName, columnNameAndData.Item1, iterionValue, ConnectionString);
                    if (insertResult.Message != "null")
                    {
                        Assert.Fail($"Terjadi kesalahan ketika Insert Into Table {tableName}. Detail : {insertResult.Message}");
                    }
                }

                Tuple<DataTable, Exception> dataTableRes = new UtilityModel().BindGridview("Export_Sales", yearPeriodTxt, monthPeriodTxt, ConnectionString);
                if (dataTableRes.Item2 != null)
                {

                    Assert.Fail(dataTableRes.Item2.Message);
                }
                CsvDataList = new ExportSalesFunctionModel().ExportSalesDatatableToList(dataTableRes.Item1);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            TearDown();
            Assert.Pass("View Success");
        }
    }
}
