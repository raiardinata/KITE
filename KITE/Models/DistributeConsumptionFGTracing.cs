using System;
using System.Data;
using System.Data.SqlClient;

namespace KITE.Models
{
    public class DistributeConsumptionFGTracingFunctionModel
    {
        public Exception ExecCreatingFGTracingProcedure(SqlCommand command, object[] param)
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

    public class DistributeConsumptionFGTracingViewModel
    {
        public string UUID { get; set; }
        public string Year_Period { get; set; }
        public string Month_Period { get; set; }
        public string SO { get; set; }
        public DateTime PGO_Date { get; set; }
        public string Finish_Goods { get; set; }
        public string FG_Name { get; set; }
        public string FG_Batch { get; set; }
        public decimal Qty_Sales { get; set; }
        public string UoM { get; set; }
        public string Raw_Material { get; set; }
        public string Material_Description { get; set; }
        public decimal Total_RM_Qty { get; set; }
        public string RM_Batch_Sequence { get; set; }
        public string RM_Batch { get; set; }
        public decimal RM_Qty { get; set; }
        public DateTime PIB_Date { get; set; }
        public string Customer { get; set; }
        public string Name_1 { get; set; }
        public string NO_PEB { get; set; }
        public DateTime PEB_Date { get; set; }
        public string PO_Number { get; set; }
        public string Country_Destination { get; set; }
        public string Description { get; set; }
        public string FGperBatch_UUID { get; set; }
    }
}