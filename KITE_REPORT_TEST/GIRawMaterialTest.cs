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
    internal class BGIRawMaterialTest : System.Web.UI.Page
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

        [TearDown]
        public void TearDown()
        {
            string query = "TRUNCATE TABLE GI_Raw_Material;";
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
        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GI Raw Materail.csv")]
        public void TestA_GIRawMaterialCsvReadSucceed(string filePath)
        {
            List<GIRawMaterialWithConvertionViewModel> CsvDataList;
            List<GIRawMaterialWithConvertionViewModel> Expected = new List<GIRawMaterialWithConvertionViewModel>();
            Expected.Add(new GIRawMaterialWithConvertionViewModel()
            {
                Posting_Date = DateTime.Parse("2023/03/31"),
                Document_Date = DateTime.Parse("2023/03/31"),
                Document_Header_Text = "BM Dill 31.03.2023",
                Material = "121001072",
                Material_Description = "Beet Molasses (F)",
                Plant = "2201",
                Storage_Location = "2018",
                Movement_Type = "Z03",
                Material_Document = "4048920880",
                Batch = "002/078293",
                Qty_in_Un_of_Entry = "-235,905",
                Kilos_Convertion = "-235905,00000000000",
                Unit_of_Entry = "MT",
                Entry_Date = DateTime.Parse("2023/04/01"),
                Time_of_Entry = "16:22:26",
                User_name = "ID0857",
                Base_Unit_of_Measure = "MT",
                Quantity = "-235,905",
                Amount_in_LC = "0,00",
                Goods_recipient = "211000065",

            });

            Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("giRawMaterial", filePath, ";", ConnectionString);
            if (readCsvResult.Item2.Message != "null")
            {
                Assert.Fail(readCsvResult.Item2.Message);
            }

            CsvDataList = (List<GIRawMaterialWithConvertionViewModel>)readCsvResult.Item1;

            CollectionAssert.AreEquivalent(Expected, CsvDataList);
        }

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GI Raw Materail Fail.csv")]
        public void TestB_GIRawMaterialCsvReadFail(string filePath)
        {
            List<GIRawMaterialWithConvertionViewModel> CsvDataList;
            string Expected = "Header with name 'Posting Date'[0] was not found.\r\n";

            try
            {
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("giRawMaterial", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<GIRawMaterialWithConvertionViewModel>)readCsvResult.Item1;
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

        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GI Raw Materail.csv")]
        public void TestC_GIRawMaterialUploadSucceed(string filePath)
        {
            int index = 0;
            int yearPeriod = 0;
            int monthPeriod = 0;
            string tableName = "GI_Raw_Material";
            DatabaseModel databaseModel = new DatabaseModel();

            try
            {
                // basicly this one is LoadCsvData()
                ReadCsvModel readCsv = new ReadCsvModel();
                List<GIRawMaterialWithConvertionViewModel> CsvDataList;
                Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("giRawMaterial", filePath, ";", ConnectionString);
                if (readCsvResult.Item2.Message != "null")
                {
                    Assert.Fail(readCsvResult.Item2.Message);
                }

                CsvDataList = (List<GIRawMaterialWithConvertionViewModel>)readCsvResult.Item1;
                // until this one


                GIRawMaterialFunctionModel csvDataProcess = new GIRawMaterialFunctionModel();
                Tuple<string, ArrayList> columnNameAndData = csvDataProcess.GIRawMaterialGenerateColumnAndCsvData(CsvDataList);
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