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
    internal class CGRFinishGoodsTest : System.Web.UI.Page
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
            string query = "TRUNCATE TABLE GR_Finish_Goods;";
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
        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GR FG.csv")]
        public void TestA_GRFinishGoodsCsvReadSucceed(string filePath)
        {
            List<GRFinishGoodsWithConvertionViewModel> CsvDataList;
            List<GRFinishGoodsWithConvertionViewModel> Expected = new List<GRFinishGoodsWithConvertionViewModel>();
            Expected.Add(new GRFinishGoodsWithConvertionViewModel()
            {
                Posting_Date = DateTime.Parse("2023/03/26"),
                Document_Date = DateTime.Parse("2023/03/26"),
                Document_Header_Text = "KIRIM QA PROD 26",
                Material = "124000012",
                Material_Description = "RC 800KG C/B Exp NEX",
                Plant = "2201",
                Storage_Location = "2021",
                Movement_Type = "Z72",
                Material_Document = "4048816493",
                Batch = "P26032023",
                Qty_in_Un_of_Entry = "120",
                Kilos_Convertion = "96000,00000000",
                Unit_of_Entry = "CB",
                Entry_Date = DateTime.Parse("2023/03/27"),
                Time_of_Entry = "15:52:00",
                User_name = "ID0643",
                Base_Unit_of_Measure = "CB",
                Quantity = "120",
                Amount_in_LC = "0",
                Goods_recipient = "",

            });

            Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("grFinishGoods", filePath, ";", ConnectionString);
            if (readCsvResult.Item2.Message != "null")
            {
                Assert.Fail(readCsvResult.Item2.Message);
            }

            CsvDataList = (List<GRFinishGoodsWithConvertionViewModel>)readCsvResult.Item1;

            CollectionAssert.AreEquivalent(Expected, CsvDataList);
        }

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GR FG Fail.csv")]
        public void TestB_GRFinishGoodsCsvReadFail(string filePath)
        {
            List<GRFinishGoodsWithConvertionViewModel> CsvDataList;
            string Expected = "Header with name 'Posting Date'[0] was not found.\r\n";

            try
            {
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("grFinishGoods", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<GRFinishGoodsWithConvertionViewModel>)readCsvResult.Item1;
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

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GR FG.csv")]
        public void TestC_GRFinishGoodsUploadSucceed(string filePath)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "GR_Finish_Goods";
            DatabaseModel databaseModel = new DatabaseModel();

            try
            {
                // basicly this one is LoadCsvData()
                List<GRFinishGoodsWithConvertionViewModel> CsvDataList;
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("grFinishGoods", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<GRFinishGoodsWithConvertionViewModel>)readCsvResult.Item1;
                // until this one

                GRFinishGoodsFunctionModel csvDataProcess = new GRFinishGoodsFunctionModel();
                Tuple<string, ArrayList, Exception> columnNameAndData = csvDataProcess.GRFinishGoodsGenerateColumnAndCsvData(CsvDataList, ConnectionString);
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
