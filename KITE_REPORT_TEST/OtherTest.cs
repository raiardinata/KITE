using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace KITE_REPORT_TEST
{
    internal class FDecimalPointConfigurationTest
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

        //[Ignore("configure pc decimal point into wrong configuration first")]
        [TestCase("D:\\03. Project\\KITE REPORT\\KITE\\KITE_REPORT_TEST\\Csv_File_Tester\\20231009-GR FG Fail.csv")]
        public void TestD_GRFinishGoodsCsvReadDecimalSeparatorFail(string filePath)
        {
            Exception Expected = new Exception("Konfigurasi desimal point belum memakai '.'. Silahkan konfigurasikan desimal point anda ke '.' sesuai dengan WI.");

            Tuple<object, Exception> readCsvResult = new ReadCsvModel().ReadCsvFunction("grFinishGoods", filePath, ";", ConnectionString);
            CollectionAssert.AreEquivalent(Expected.Message, readCsvResult.Item2.Message);
        }
    }
}
