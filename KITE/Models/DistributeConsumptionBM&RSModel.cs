using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace KITE.Models
{
    public class DistributeConsumptionBM_RSFunctionModel
    {

        public async Task<Tuple<DataTable, Exception>> GetDistributedMcFrameData(string selectColumn, string tableName, string condition, string connectionString)
        {
            Tuple<DataTable, Exception> mcFrameSelectDataTable = await Task.Run(() =>
            {
                return new DatabaseModel().SelectTable(selectColumn, tableName, condition, connectionString);
            });
            return Tuple.Create(mcFrameSelectDataTable.Item1, mcFrameSelectDataTable.Item2);
        }

        public async Task<Tuple<DataTable, Exception>> GetDistributedGIRawMaterialData(string selectColumn, string tableName, string condition, string connectionString)
        {
            Tuple<DataTable, Exception> giRawMaterialSelectDataTable = await Task.Run(() =>
            {
                return new DatabaseModel().SelectTable(selectColumn, tableName, condition, connectionString);
            });
            return Tuple.Create(giRawMaterialSelectDataTable.Item1, giRawMaterialSelectDataTable.Item2);

        }

        public Tuple<ArrayList, Exception> DistributionCalculationProcess(DataTable mcFrameDataTable, DataTable giRawMaterialDataTable, string connectionString)
        {
            int giRawMaterialIndex = 0;
            decimal leftOver = 0;
            List<string> rmPerBatchQueryList = new List<string>();
            ArrayList rmPerBatchQueryArrayList = new ArrayList();

            EnumerableRowCollection<DataRow> mcFrameDataRows = mcFrameDataTable.AsEnumerable();
            foreach (DataRow mcFrameDataRow in mcFrameDataRows)
            {
                Guid uuid = Guid.NewGuid();
                decimal mcFrameSumQty = mcFrameDataRow.Field<decimal>("SumQuantity");
                List<GIRawMaterialJsonViewModel> listGIRawMaterialviewModel = new List<GIRawMaterialJsonViewModel>();

                for (int i = giRawMaterialIndex; i < giRawMaterialDataTable.Rows.Count; i++)
                {
                    DataRow giRawMaterialDataRow = giRawMaterialDataTable.Rows[i];
                    mcFrameSumQty += (leftOver != 0) ? leftOver : giRawMaterialDataRow.Field<decimal>("Quantity");

                    // populate RMperBatch tracking json
                    listGIRawMaterialviewModel.Add(new GIRawMaterialJsonViewModel
                    {
                        UUID = giRawMaterialDataRow.Field<string>("UUID"),
                        Year_Period = giRawMaterialDataRow.Field<string>("Year_Period"),
                        Month_Period = giRawMaterialDataRow.Field<string>("Month_Period"),
                        Posting_Date = giRawMaterialDataRow.Field<DateTime>("Posting_Date"),
                        Material = giRawMaterialDataRow.Field<string>("Material"),
                        Plant = giRawMaterialDataRow.Field<string>("Plant"),
                        Storage_Location = giRawMaterialDataRow.Field<string>("Storage_Location"),
                        Movement_Type = giRawMaterialDataRow.Field<string>("Movement_Type"),
                        Batch = giRawMaterialDataRow.Field<string>("Batch"),
                        Qty_in_Un_of_Entry = giRawMaterialDataRow.Field<decimal>("Qty_in_Un_of_Entry"),
                        Unit_of_Entry = giRawMaterialDataRow.Field<string>("Unit_of_Entry"),
                        Base_Unit_of_Measure = giRawMaterialDataRow.Field<string>("Base_Unit_of_Measure"),
                        Quantity = giRawMaterialDataRow.Field<decimal>("Quantity"),
                        Material_Document = giRawMaterialDataRow.Field<string>("Material_Document")
                    });


                    if (mcFrameSumQty >= 0)
                    {
                        leftOver = 0;
                        giRawMaterialIndex++;
                    }
                    else
                    {
                        leftOver = mcFrameSumQty;
                        break;
                    }
                }

                // populate RMperBatch table
                string sumQuantity = mcFrameDataRow.Field<decimal>("SumQuantity").ToString();
                string valuesArray = $"" +
                    $"'{uuid.ToString()}'," +
                    $"'{mcFrameDataRow.Field<string>("Year_Period")}'," +
                    $"'{mcFrameDataRow.Field<string>("Month_Period")}'," +
                    $"'{mcFrameDataRow.Field<string>("Target_item_CD")}'," +
                    $"'{mcFrameDataRow.Field<string>("Item_CD")}'," +
                    $"'{mcFrameDataRow.Field<string>("Unit")}'," +
                    $"'{sumQuantity.Replace(",", ".")}'," +
                    $"'{JsonConvert.SerializeObject(listGIRawMaterialviewModel, Formatting.Indented)}'";
                rmPerBatchQueryList.Add(valuesArray);
            }
            rmPerBatchQueryArrayList.Add(rmPerBatchQueryList);
            return Tuple.Create(rmPerBatchQueryArrayList, new Exception("null"));
        }
    }

    public class DistributedRMperBatchViewModel
    {
        public string UUID { get; set; }
        public string Year_Period { get; set; }
        public string Month_Period { get; set; }
        public string Target_item_CD { get; set; }
        public string Item_CD { get; set; }
        public string Unit { get; set; }
        public decimal Sum_Qty { get; set; }
        public string trackingJson { get; set; }
    }
    public class GIRawMaterialJsonViewModel
    {
        public string UUID { get; set; }
        public string Year_Period { get; set; }
        public string Month_Period { get; set; }
        public DateTime Posting_Date { get; set; }
        public string Material { get; set; }
        public string Plant { get; set; }
        public string Storage_Location { get; set; }
        public string Movement_Type { get; set; }
        public string Batch { get; set; }
        public decimal Qty_in_Un_of_Entry { get; set; }
        public string Unit_of_Entry { get; set; }
        public string Base_Unit_of_Measure { get; set; }
        public decimal Quantity { get; set; }
        public string Material_Document { get; set; }
    }
}