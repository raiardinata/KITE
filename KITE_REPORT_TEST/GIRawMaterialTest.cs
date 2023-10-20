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
    public class GIRawMaterialTest : System.Web.UI.Page
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
        [TestCase("20231009-GI Raw Materail.csv", "./KITE_REPORT_TEST/Csv_File_Tester/20231009-GI Raw Materail.csv")]
        public void GIRawMaterialCsvReadSucceed(string fileName, string filePath)
        {
            ReadCsvModel readCsv = new ReadCsvModel();
            List<GIRawMaterialViewModel> CsvDataList;
            List<GIRawMaterialViewModel> Expected = new List<GIRawMaterialViewModel>();
            Expected.Add(new GIRawMaterialViewModel()
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
                Unit_of_Entry = "MT",
                Entry_Date = DateTime.Parse("2023/04/01"),
                Time_of_Entry = "16:22:26",
                User_name = "ID0857",
                Base_Unit_of_Measure = "MT",
                Quantity = "-235,905",
                Amount_in_LC = "0",
                Goods_recipient = "211000065",

            });

            using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
            {
                CsvDataList = csvData.GetRecords<GIRawMaterialViewModel>().ToList();
            }
            CollectionAssert.AreEquivalent(Expected, CsvDataList);
        }

        [TestCase("20231009-GI Raw Materail Fail.csv", "./KITE_REPORT_TEST/Csv_File_Tester/20231009-GI Raw Materail Fail.csv")]
        public void GIRawMaterialCsvReadFail(string fileName, string filePath)
        {
            ReadCsvModel readCsv = new ReadCsvModel();
            List<GIRawMaterialViewModel> CsvDataList;
            string Expected = "Header with name 'Posting Date'[0] was not found.\r\n";

            try
            {
                using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
                {
                    CsvDataList = csvData.GetRecords<GIRawMaterialViewModel>().ToList();
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

        [TestCase("./KITE_REPORT_TEST/Csv_File_Tester/20231009-GI Raw Materail.csv")]
        public void GIRawMaterialUploadSucceed(string filePath)
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
                List<GIRawMaterialViewModel> CsvDataList;
                using (CsvReader csvData = readCsv.ReadCsvFile(filePath, ";"))
                {
                    CsvDataList = csvData.GetRecords<GIRawMaterialViewModel>().ToList();
                }
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

                Exception insertResult = databaseModel.InsertIntoTable(tableName, columnNameAndData.Item1, columnNameAndData.Item2, ConnectionString);
                if (insertResult.Message != $"Insert Into Table {tableName} Berhasil.")
                {
                    Assert.Fail(insertResult.Message);
                }

                if (checkPeriodResult.Message == "Data Period Aman." && insertResult.Message == $"Insert Into Table {tableName} Berhasil.")
                {
                    Assert.AreEqual($"Insert Into Table {tableName} Berhasil.", insertResult.Message);
                }


            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            TearDown();
        }
    }
}