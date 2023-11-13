using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace KITE_REPORT_TEST
{
    internal class AMcFrameTest : System.Web.UI.Page
    {
        private IConfiguration Configuration;
        private string ConnectionString;

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

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            string query = "TRUNCATE TABLE McFrame_Cost_Table;";
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
        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\mcFrame 032023.csv")]
        public void TestA_McFrameCsvReadSucceed(string filePath)
        {
            List<McFrameWithKilosConvertionViewModel> CsvDataList;
            List<McFrameWithKilosConvertionViewModel> Expected = new List<McFrameWithKilosConvertionViewModel>();
            Expected.Add(new McFrameWithKilosConvertionViewModel()
            {
                Calc_No = "BL588",
                Mgmt_dept_CD = "2201",
                Management_Dept_Name = "",
                YM = DateTime.Parse("2023/10/01 00:00:00"),
                Lvl = 0,
                Target_item_CD = "121000000",
                Item_CD = "121000000",
                Item_name = "",
                Item_type_name = "",
                Unit = "MT",
                Quantity = "7479,42",
                STD_Qty = "7479,42",
                Kilos_Convertion = "7479420,0000000000",
                Total = "1412743,21",
                STD_Total = "0",
                Variable_Cost = "1412743,21",
                STD_Variable_Cost = "0",
                Labour_Cost = "0",
                STD_Labour_Cost = "0",
                Depreciation = "0",
                STD_Depreciation = "0",
                Repair_Maintenance = "0",
                STD_Repair_Maintenance = "0",
                Overhead_Cost = "0",
                STD_Overhead_Cost = "0",
                Retur_Cost = "0",
                STD_Retur_Cost = "0",
            });

            Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("mcFrame", filePath, ";", ConnectionString);
            if (readCsvResult.Item2.Message != "null")
            {
                Assert.Fail(readCsvResult.Item2.Message);
            }

            CsvDataList = (List<McFrameWithKilosConvertionViewModel>)readCsvResult.Item1;

            CollectionAssert.AreEquivalent(Expected, CsvDataList);
        }

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\mcframe 032023 Fail.csv")]
        public void TestB_McFrameCsvReadFail(string filePath)
        {
            string Expected = "Header with name 'Calc. No.'[0] was not found.\r\n";

            try
            {
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("mcFrame", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }
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

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\mcframe 032023.csv")]
        public void TestC_McFrameUploadSucceed(string filePath)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "McFrame_Cost_Table";
            DatabaseModel databaseModel = new DatabaseModel();

            try
            {
                // basicly this one is LoadCsvData()
                ReadCsvModel readCsv = new ReadCsvModel();
                List<McFrameWithKilosConvertionViewModel> CsvDataList;
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("mcFrame", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }
                CsvDataList = (List<McFrameWithKilosConvertionViewModel>)readCsvResult.Item1;
                // until this one

                McFrameFunctionModel csvDataProcess = new McFrameFunctionModel();
                Tuple<string, ArrayList> columnNameAndData = csvDataProcess.McFrameGenerateColumnAndCsvData(CsvDataList);
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
    }
}