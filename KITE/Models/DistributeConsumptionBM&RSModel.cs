using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace KITE.Models
{
    public class DistributeConsumptionBM_RSFunctionModel
    {
        public async Task<Tuple<DataTable, Exception>> GetDistributedMcFrameData(string selectColumn, string tableName, string condition, string connectionString)
        {
            Tuple<DataTable, Exception> checkMcFrameDataExist = await Task.Run(() =>
            {
                return new DatabaseModel().SelectTableIntoDataTable("*", "McFrame_Cost_Table", "", connectionString);
            });
            if (checkMcFrameDataExist.Item2.Message != "null")
            {
                return new Tuple<DataTable, Exception>(null, checkMcFrameDataExist.Item2);
            }

            Tuple<DataTable, Exception> mcFrameSelectDataTable = await Task.Run(() =>
            {
                return new DatabaseModel().SelectTableIntoDataTable(selectColumn, tableName, condition, connectionString);
            });
            return Tuple.Create(mcFrameSelectDataTable.Item1, mcFrameSelectDataTable.Item2);
        }

        public async Task<Tuple<DataTable, Exception>> GetDistributedGIRawMaterialData(string selectColumn, string tableName, string condition, string connectionString)
        {
            Tuple<DataTable, Exception> checkGiRawMaterialDataExist = await Task.Run(() =>
            {
                return new DatabaseModel().SelectTableIntoDataTable("*", "GI_Raw_Material", "", connectionString);
            });
            if (checkGiRawMaterialDataExist.Item2.Message != "null")
            {
                return new Tuple<DataTable, Exception>(null, checkGiRawMaterialDataExist.Item2);
            }

            Tuple<DataTable, Exception> giRawMaterialSelectDataTable = await Task.Run(() =>
            {
                return new DatabaseModel().SelectTableIntoDataTable(selectColumn, tableName, condition, connectionString);
            });
            return Tuple.Create(giRawMaterialSelectDataTable.Item1, giRawMaterialSelectDataTable.Item2);

        }

        public Tuple<ArrayList, Exception> DistributionCalculationProcessInJson(DataTable mcFrameDataTable, DataTable giRawMaterialDataTable)
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
                        giUUID = giRawMaterialDataRow.Field<string>("UUID"),
                        giYear_Period = giRawMaterialDataRow.Field<string>("Year_Period"),
                        giMonth_Period = giRawMaterialDataRow.Field<string>("Month_Period"),
                        giPosting_Date = giRawMaterialDataRow.Field<DateTime>("Posting_Date"),
                        giMaterial = giRawMaterialDataRow.Field<string>("Material"),
                        giPlant = giRawMaterialDataRow.Field<string>("Plant"),
                        giStorage_Location = giRawMaterialDataRow.Field<string>("Storage_Location"),
                        giMovement_Type = giRawMaterialDataRow.Field<string>("Movement_Type"),
                        giBatch = giRawMaterialDataRow.Field<string>("Batch"),
                        giQty_in_Un_of_Entry = giRawMaterialDataRow.Field<decimal>("Qty_in_Un_of_Entry"),
                        giUnit_of_Entry = giRawMaterialDataRow.Field<string>("Unit_of_Entry"),
                        giBase_Unit_of_Measure = giRawMaterialDataRow.Field<string>("Base_Unit_of_Measure"),
                        giQuantity = giRawMaterialDataRow.Field<decimal>("Quantity"),
                        giMaterial_Document = giRawMaterialDataRow.Field<string>("Material_Document")
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

        public Tuple<ArrayList, Exception> DistributionCalculationProcessInNormal(DataTable mcFrameDataTable, DataTable giRawMaterialDataTable)
        {
            int giRawMaterialIndex = 0;
            decimal leftOver = 0;
            List<string> rmPerBatchQueryList = new List<string>();
            ArrayList rmPerBatchQueryArrayList = new ArrayList();

            EnumerableRowCollection<DataRow> mcFrameDataRows = mcFrameDataTable.AsEnumerable();
            foreach (DataRow mcFrameDataRow in mcFrameDataRows)
            {
                decimal mcFrameSumQty = mcFrameDataRow.Field<decimal>("SumQuantity");
                List<GIRawMaterialJsonViewModel> listGIRawMaterialviewModel = new List<GIRawMaterialJsonViewModel>();

                // populate RMperBatch table
                string sumQuantity = mcFrameDataRow.Field<decimal>("SumQuantity").ToString();

                for (int i = giRawMaterialIndex; i < giRawMaterialDataTable.Rows.Count; i++)
                {
                    Guid uuid = Guid.NewGuid();
                    DataRow giRawMaterialDataRow = giRawMaterialDataTable.Rows[i];
                    mcFrameSumQty += (leftOver != 0) ? leftOver : giRawMaterialDataRow.Field<decimal>("Quantity");
                    string quantity = giRawMaterialDataRow.Field<decimal>("Quantity").ToString();
                    string qty_un = giRawMaterialDataRow.Field<decimal>("Qty_in_Un_of_Entry").ToString();

                    // populate RMperBatch tracking 
                    try
                    {
                        string valuesArray = $"" +
                            $"'{uuid.ToString()}'," +
                            $"'{mcFrameDataRow.Field<string>("Year_Period")}'," +
                            $"'{mcFrameDataRow.Field<string>("Month_Period")}'," +
                            $"'{mcFrameDataRow.Field<string>("Target_item_CD")}'," +
                            $"'{mcFrameDataRow.Field<string>("Item_CD")}'," +
                            $"'{mcFrameDataRow.Field<string>("Unit")}'," +
                            $"'{sumQuantity.Replace(",", ".")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("UUID")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Year_Period")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Month_Period")}'," +
                            $"'{giRawMaterialDataRow.Field<DateTime>("Posting_Date")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Material")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Plant")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Storage_Location")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Movement_Type")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Batch")}'," +
                            $"'{qty_un.Replace(",", ".")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Unit_of_Entry")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Base_Unit_of_Measure")}'," +
                            $"'{quantity.Replace(",", ".")}'," +
                            $"'{giRawMaterialDataRow.Field<string>("Material_Document")}'";
                        rmPerBatchQueryList.Add(valuesArray);
                    }
                    catch (Exception ex)
                    {
                        return Tuple.Create(rmPerBatchQueryArrayList, new Exception($"Terjadi kesalahan saat generate RM per Batch values array query. Detail: {ex}"));
                    }

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

            }
            rmPerBatchQueryArrayList.Add(rmPerBatchQueryList);
            return Tuple.Create(rmPerBatchQueryArrayList, new Exception("null"));
        }

        public Exception ExecCreatingBMandRSProcedure(SqlCommand command, object[] param)
        {
            try
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters to the command
                command.Parameters.Add(new SqlParameter("@Year_Period", SqlDbType.VarChar, 4)).Value = param[0];
                command.Parameters.Add(new SqlParameter("@Month_Period", SqlDbType.VarChar, 2)).Value = param[1];

                // Execute the stored procedure
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }

    public class BalanceGIViewModel
    {
        public string UUID { get; set; }
        public string Year_Period { get; set; }
        public string Month_Period { get; set; }
        public string Material { get; set; }
        public string Batch { get; set; }
        public decimal Qty_GI { get; set; }
        public decimal Distribution_Qty { get; set; }
        public decimal Remaining_Qty { get; set; }
        public DateTime Date_Created { get; set; }
        public DateTime Date_Modified { get; set; }
    }
    public class DistributedRMperBatchViewModel
    {
        public string UUID { get; set; }
        public string Year_Period { get; set; }
        public string Month_Period { get; set; }
        public string Finish_Goods { get; set; }
        public string FG_Qty { get; set; }
        public string Raw_Material { get; set; }
        public int Batch_Sequent { get; set; }
        public string RM_Batch { get; set; }
        public string RM_Qty { get; set; }
        public string Distribution_Qty { get; set; }
        public string Remaining_Qty { get; set; }
    }
    public class GIRawMaterialJsonViewModel
    {
        public string giUUID { get; set; }
        public string giYear_Period { get; set; }
        public string giMonth_Period { get; set; }
        public DateTime giPosting_Date { get; set; }
        public string giMaterial { get; set; }
        public string giPlant { get; set; }
        public string giStorage_Location { get; set; }
        public string giMovement_Type { get; set; }
        public string giBatch { get; set; }
        public decimal giQty_in_Un_of_Entry { get; set; }
        public string giUnit_of_Entry { get; set; }
        public string giBase_Unit_of_Measure { get; set; }
        public decimal giQuantity { get; set; }
        public string giMaterial_Document { get; set; }
    }
}