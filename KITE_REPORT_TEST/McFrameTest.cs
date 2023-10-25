using CsvHelper;
using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace KITE_REPORT_TEST
{
    internal class McFrameTest : System.Web.UI.Page
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
            string query = "TRUNCATE TABLE GI_Raw_Material; TRUNCATE TABLE McFrame_Cost_Table;";
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
        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\mcframe 032023.csv")]
        public void McFrameCsvRead(string filePath)
        {
            ReadCsvModel readCsv = new ReadCsvModel();
            List<McFrameViewModel> CsvDataList;
            List<McFrameViewModel> Expected = new List<McFrameViewModel>();
            Expected.Add(new McFrameViewModel()
            {
                Calc_No = "BL572",
                Mgmt_dept_CD = "2201",
                Management_Dept_Name = "",
                YM = DateTime.Parse("2023/03/01 00:00:00"),
                Lvl = 0,
                Target_item_CD = "121000004",
                Item_CD = "121000004",
                Item_name = "",
                Item_type_name = "",
                Unit = "",
                Quantity = "1010300",
                STD_Qty = "1010300",
                Total = "932286,83",
                STD_Total = "0",
                Variable_Cost = "932286,83",
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

            using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
            {
                CsvDataList = csvData.GetRecords<McFrameViewModel>().ToList();
            }
            CollectionAssert.AreEquivalent(Expected, CsvDataList);
        }

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\mcframe 032023 Fail.csv")]
        public void McFrameCsvReadFail(string filePath)
        {
            ReadCsvModel readCsv = new ReadCsvModel();
            List<McFrameViewModel> CsvDataList;
            string Expected = "Header with name 'Calc. No.'[0] was not found.\r\n";

            try
            {
                using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
                {
                    CsvDataList = csvData.GetRecords<McFrameViewModel>().ToList();
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
        public void McFrameUploadSucceed(string filePath)
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
                List<McFrameViewModel> CsvDataList;
                using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
                {
                    CsvDataList = csvData.GetRecords<McFrameViewModel>().ToList();
                }
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

                Exception insertResult = new ReadCsvModel().IterateCsvObject(tableName, columnNameAndData.Item1, columnNameAndData.Item2, ConnectionString);
                if (insertResult.Message != $"Insert Into Table {tableName} Berhasil.")
                {
                    Assert.Fail(insertResult.Message);
                }

                Assert.AreEqual($"Insert Into Table {tableName} Berhasil.", insertResult.Message);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            TearDown();
        }
    }
}