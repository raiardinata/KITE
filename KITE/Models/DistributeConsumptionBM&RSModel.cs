using System;
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

        public void DistributionCalculationProcess(DataTable mcFrameDataTable, DataTable giRawMaterialDataTable)
        {
            decimal gi_Sum_Qty = new decimal(0);

            EnumerableRowCollection<DataRow> gIRawMaterialDataRows = giRawMaterialDataTable.AsEnumerable();
            foreach (DataRow gIRawMaterialDataRow in gIRawMaterialDataRows)
            {
                gi_Sum_Qty = gIRawMaterialDataRow.Field<decimal>("Sum_Qty");
                EnumerableRowCollection<DataRow> mcFrameDataRows = from mcFrameDataLinq in mcFrameDataTable.AsEnumerable()
                                                                   where mcFrameDataLinq.Field<string>("Item_CD") == gIRawMaterialDataRow.Field<string>("Material")
                                                                   orderby mcFrameDataLinq.Field<string>("Target_item_CD") ascending
                                                                   select mcFrameDataLinq;
                foreach (DataRow mcFrameDataRow in mcFrameDataRows)
                {
                    gi_Sum_Qty += mcFrameDataRow.Field<decimal>("SumQuantity");
                    Console.WriteLine(gi_Sum_Qty);
                }
            }
        }
    }
}