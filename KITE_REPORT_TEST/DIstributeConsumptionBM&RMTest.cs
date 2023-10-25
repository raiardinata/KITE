using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace KITE_REPORT_TEST
{
    internal class DIstributeConsumptionBM_RMTest
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
        [TestCase(
            new string[] {
                " Year_Period,Month_Period,Target_item_CD,Item_CD,Unit,SUM(Quantity) SumQuantity ",
                " UUID,Year_Period,Month_Period,Posting_Date,Material,Plant,Storage_Location,Movement_Type,Batch,Qty_in_Un_of_Entry,Unit_of_Entry,Base_Unit_of_Measure,Quantity,Material_Document "
            },
            new string[] { " McFrame_Cost_Table ", " GI_Raw_Material " },
            new string[] {
                " Item_CD IN (SELECT LOW FROM CommonTable WHERE ProgramID = 'DISTRIBUTED_MATERIAL') AND Target_item_CD LIKE '124%' GROUP BY Year_Period , Month_Period ,Target_item_CD, Item_CD, Unit ",
                " Material = '121001071' ORDER BY Material,Posting_Date,Material_Document ASC " }
        )]
        public async Task PopulateRMperBatchAsync(string[] selectColumn, string[] tableName, string[] condition)
        {
            Tuple<DataTable, Exception> mcFrameDataTable = await new DistributeConsumptionBM_RSFunctionModel().GetDistributedMcFrameData(selectColumn[0], tableName[0], condition[0], ConnectionString);
            if (mcFrameDataTable.Item2.Message != "null" || mcFrameDataTable.Item1.Rows.Count == 0)
            {
                Assert.Fail(mcFrameDataTable.Item2.Message + " || DataTable Row 0");
            }

            Tuple<DataTable, Exception> gIRawMaterialDataTable = await new DistributeConsumptionBM_RSFunctionModel().GetDistributedGIRawMaterialData(selectColumn[1], tableName[1], condition[1], ConnectionString);
            if (gIRawMaterialDataTable.Item2.Message != "null" || gIRawMaterialDataTable.Item1.Rows.Count == 0)
            {
                Assert.Fail(mcFrameDataTable.Item2.Message + " || DataTable Row 0");
            }

            new DistributeConsumptionBM_RSFunctionModel().DistributionCalculationProcess(mcFrameDataTable.Item1, gIRawMaterialDataTable.Item1, ConnectionString);

            Assert.Fail();
        }
    }
}
