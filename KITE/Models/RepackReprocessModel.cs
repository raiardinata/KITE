using System;
using System.Data;
using System.Data.SqlClient;

namespace KITE.Models
{
    public class RepackReprocessFunctionModel
    {
        public Exception ExecCreatingRepackReprocessProcedure(SqlCommand command, object[] param)
        {
            try
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters to the command
                SqlParameter FGQty = new SqlParameter("@FGQty", SqlDbType.Decimal);
                FGQty.Precision = 28;
                FGQty.Scale = 4;
                FGQty.Value = decimal.Parse((string)param[4]);

                SqlParameter DsOneQty = new SqlParameter("@DsOneQty", SqlDbType.Decimal);
                DsOneQty.Precision = 28;
                DsOneQty.Scale = 4;
                DsOneQty.Value = decimal.Parse((string)param[5]);

                SqlParameter DsTwoQty = new SqlParameter("@DsTwoQty", SqlDbType.Decimal);
                DsTwoQty.Precision = 28;
                DsTwoQty.Scale = 4;
                DsTwoQty.Value = decimal.Parse((string)param[7]);

                command.Parameters.Add(new SqlParameter("@OldFinishGoods", SqlDbType.VarChar, 20)).Value = param[0];
                command.Parameters.Add(new SqlParameter("@OldFGBatch", SqlDbType.VarChar, 20)).Value = param[1];
                command.Parameters.Add(new SqlParameter("@FinishGoods", SqlDbType.VarChar, 20)).Value = param[2];
                command.Parameters.Add(new SqlParameter("@FGBatch", SqlDbType.VarChar, 20)).Value = param[3];
                command.Parameters.Add(FGQty);
                command.Parameters.Add(DsOneQty);
                command.Parameters.Add(new SqlParameter("@SOneBatch", SqlDbType.VarChar, 20)).Value = param[6];
                command.Parameters.Add(DsTwoQty);
                command.Parameters.Add(new SqlParameter("@STwoBatch", SqlDbType.VarChar, 20)).Value = param[8];

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

    public class FGSuggestionViewModel
    {
        public string Finish_Goods { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FGSuggestionViewModel other = (FGSuggestionViewModel)obj;
            return
                Finish_Goods == other.Finish_Goods;
        }

        public override int GetHashCode()
        {
            return (
                Finish_Goods
             ).GetHashCode();
        }
    }
}