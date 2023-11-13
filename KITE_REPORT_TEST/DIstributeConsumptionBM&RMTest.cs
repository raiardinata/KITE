using KITE.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace KITE_REPORT_TEST
{
    internal class DDIstributeConsumptionBM_RMTest
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
                " Year_Period,Month_Period,Lvl,Target_item_CD,Item_CD,Unit,SUM(Quantity) SumQuantity ",
                " UUID,Year_Period,Month_Period,Posting_Date,Material,Plant,Storage_Location,Movement_Type,Batch,Qty_in_Un_of_Entry,Unit_of_Entry,Base_Unit_of_Measure,Quantity,Material_Document "
            },
            new string[] { " McFrame_Cost_Table ", " GI_Raw_Material " },
            new string[] {
                " (Item_CD IN (SELECT LOW FROM CommonTable WHERE ProgramID = 'DISTRIBUTED_MATERIAL') OR Item_CD LIKE '124%') AND Target_item_CD LIKE '124%' GROUP BY Year_Period , Month_Period ,Target_item_CD, Item_CD, Unit,Lvl ",
                " Material = '121001071' ORDER BY Material,Posting_Date,Material_Document ASC " }
        )]
        public void PopulateRMperBatchAsync(string[] selectColumn, string[] tableName, string[] condition)
        {
            // add variable based on csv year and month
            object currentYear = 2023;
            object currentMonth = 3;

            // Initiate BM and RS calculation
            try
            {
                object[] param = new[] { currentYear, currentMonth };
                Exception balanceGIResult = new DatabaseModel().ExecStoreProcedure("CreatingBalanceGI", param, ConnectionString);
                if (balanceGIResult != null)
                {
                    //return new Exception("Terdapat masalah ketika menjalankan proses CreatingBalanceGI.");
                    Assert.Fail("Terdapat masalah ketika menjalankan proses CreatingBalanceGI.");
                }
                Exception rmperBatchResult = new DatabaseModel().ExecStoreProcedure("CreatingRMperBatch", param, ConnectionString);
                if (rmperBatchResult != null)
                {
                    Assert.Fail("Terdapat masalah ketika menjalankan proses CreatingRMperBatch.");
                }
                Exception fgperBatchResult = new DatabaseModel().ExecStoreProcedure("CreatingFGperBatch", param, ConnectionString);
                if (fgperBatchResult != null)
                {
                    Assert.Fail("Terdapat masalah ketika menjalankan proses CreatingFGperBatch.");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // Make RM per Batch

            //Tuple<DataTable, Exception> mcFrameDataTable = await new DistributeConsumptionBM_RSFunctionModel().GetDistributedMcFrameData(selectColumn[0], tableName[0], condition[0], ConnectionString);
            //if (mcFrameDataTable.Item2.Message != "null" || mcFrameDataTable.Item1.Rows.Count == 0)
            //{
            //    Assert.Fail(mcFrameDataTable.Item2.Message + " || DataTable Row 0");
            //}

            //Tuple<DataTable, Exception> gIRawMaterialDataTable = await new DistributeConsumptionBM_RSFunctionModel().GetDistributedGIRawMaterialData(selectColumn[1], tableName[1], condition[1], ConnectionString);
            //if (gIRawMaterialDataTable.Item2.Message != "null" || gIRawMaterialDataTable.Item1.Rows.Count == 0)
            //{
            //    Assert.Fail(mcFrameDataTable.Item2.Message + " || DataTable Row 0");
            //}

            //Tuple<ArrayList, Exception> distrubuteResultJson = new DistributeConsumptionBM_RSFunctionModel().DistributionCalculationProcessInJson(mcFrameDataTable.Item1, gIRawMaterialDataTable.Item1);
            //if (distrubuteResultJson.Item2.Message != "null")
            //{
            //    Assert.Fail(distrubuteResultJson.Item2.Message);
            //}
            //foreach (object distributedResultList in distrubuteResultJson.Item1)
            //{
            //    List<string> rmPerBatchQueryList = (List<string>)distributedResultList;
            //    foreach (string insertQuery in rmPerBatchQueryList)
            //    {
            //        Exception insertResult = new DatabaseModel().InsertIntoTable("RM_per_Batch", "UUID,Year_Period,Month_Period,Target_item_CD,Item_CD,Unit,Sum_Qty,trackingJson", insertQuery, ConnectionString);
            //        if (insertResult.Message != "null")
            //        {
            //            Assert.Fail($"Terjadi kesalahan ketika Insert Into Table {tableName}. Detail : {insertResult.Message}");
            //        }
            //    }
            //}

            //Tuple<ArrayList, Exception> distrubuteResultNormal = new DistributeConsumptionBM_RSFunctionModel().DistributionCalculationProcessInNormal(mcFrameDataTable.Item1, gIRawMaterialDataTable.Item1);
            //if (distrubuteResultNormal.Item2.Message != "null")
            //{
            //    Assert.Fail(distrubuteResultNormal.Item2.Message);
            //}
            //foreach (object distributedResultList in distrubuteResultNormal.Item1)
            //{
            //    List<string> rmPerBatchQueryList = (List<string>)distributedResultList;
            //    foreach (string insertQuery in rmPerBatchQueryList)
            //    {
            //        Exception insertResult = new DatabaseModel().InsertIntoTable("RM_per_Batch_Normal", "UUID,Year_Period,Month_Period,Target_item_CD,Item_CD,Unit,Sum_Qty,giUUID,giYear_Period,giMonth_Period,giPosting_Date,giMaterial,giPlant,giStorage_Location,giMovement_Type,giBatch,giQty_in_Un_of_Entry,giUnit_of_Entry,giBase_Unit_of_Measure,giQuantity,giMaterial_Document", insertQuery, ConnectionString);
            //        if (insertResult.Message != "null")
            //        {
            //            Assert.Fail($"Terjadi kesalahan ketika Insert Into Table {tableName}. Detail : {insertResult.Message}");
            //        }
            //    }
            //}

            Assert.Pass("Distributed Consumption Succeed.");
        }
    }
}
