using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace KITE_REPORT_TEST
{
    internal class ERepackReprocessTest
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

        [Test]
        [TestCase("124", "a", ExpectedResult = true)]
        public bool TestA_GetListofFGSuggestion(string finishgoods, string testType)
        {
            Tuple<DataTable, Exception> fgSuggestion = new DatabaseModel().SelectTableIntoDataTable(" Finish_Goods ", " FG_per_Batch ", $" WHERE Finish_Goods LIKE '%{finishgoods}%' GROUP BY Finish_Goods ", ConnectionString);
            if (fgSuggestion.Item2.Message != "null")
            {
                if (testType == "a")
                {
                    Assert.Fail(fgSuggestion.Item2.Message);
                }
                return false;
            }

            List<FGSuggestionViewModel> supposedList = new List<FGSuggestionViewModel>();
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000034" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000114" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000400" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000012" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000023" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000017" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000033" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000590" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000019" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000026" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000016" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000401" });
            supposedList.Add(new FGSuggestionViewModel { Finish_Goods = "124000021" });

            List<FGSuggestionViewModel> listResult = new ReadCsvModel().ConvertDataTabletoList<FGSuggestionViewModel>(fgSuggestion.Item1);
            CollectionAssert.AreEquivalent(supposedList, listResult);
            return true;
        }

        [TestCase("124999999")]
        public void TestB_GetNoListofFGSuggestion(string finishgoods)
        {
            bool result = TestA_GetListofFGSuggestion(finishgoods, "b");
            if (result)
            {
                Assert.Fail("Test not returning fail.");
            }
            Assert.AreEqual(false, result);
        }
    }
}
