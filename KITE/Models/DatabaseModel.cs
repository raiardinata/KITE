using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace KITE.Models
{
    public class DatabaseModel : DbContext
    {

        public Exception InsertIntoTable(string tableName, string columnName, string values, string connectionString)
        {
            string query = $"INSERT INTO {tableName}({columnName}) VALUES({values})";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                    }
                    catch (Exception ex)
                    {
                        return ex;
                    }
                }
            }
            return new Exception("null");
        }
        public Exception UpdateTable(string tableName, string values, string dynamicCondition, string connectionString)
        {
            string query = $"UPDATE {tableName} SET {values} ";
            if (dynamicCondition != "")
            {
                query += $" {dynamicCondition} ";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                    }
                    catch (Exception ex)
                    {
                        return ex;
                    }
                }
            }
            return new Exception("null");
        }

        public Exception PeriodCheck(string tableName, int yearPeriod, int monthPeriod, string connectionString)
        {
            string query;
            query = $"SELECT Year_Period, Month_Period FROM {tableName} WHERE Year_Period = '{yearPeriod}' AND Month_Period = '{monthPeriod}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return new Exception("Year Period dan Month Period data yang diupload sudah ada di dalam database.");
                            }
                        }
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Exception loadException = new Exception("Gagal dalam pengecekan Year Period dan Month Period. Detail : " + ex.Message);
                        return loadException;
                    }
                }
            }
            return new Exception("Data Period Aman.");
        }

        public Exception DeleteData(string tableName, string condition, string connectionString)
        {
            string query;
            query = $"DELETE FROM {tableName} WHERE {condition};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                    }
                    catch (Exception ex)
                    {
                        return new Exception("Data period sebelumnya gagal dibersihkan. Detail : " + ex.Message);
                    }
                }
            }
            return new Exception("Data period sebelumnya berhasil dibersihkan.");
        }

        public Tuple<DataTable, Exception> SelectTableIntoDataTable(string dynamicSelectColumn, string dynamicTableName, string dynamicCondition, string connectionString)
        {
            DataTable dataTable = new DataTable();
            string query = $"SELECT {dynamicSelectColumn} FROM {dynamicTableName} ";
            if (dynamicCondition != "")
            {
                query += $" {dynamicCondition} ";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        connection.Close();
                        command.Dispose();

                        if (dataTable.Rows.Count == 0)
                        {
                            return Tuple.Create(dataTable, new Exception($"SelectTable {dynamicTableName} query tidak menghasilkan data. Row is empty."));
                        }

                        return Tuple.Create(dataTable, new Exception("null"));
                    }
                    catch (Exception ex)
                    {
                        return Tuple.Create(dataTable, new Exception($"Terdapat masalah ketika memproses select table. Detail : {ex.Message}"));
                    }
                }
            }
        }

        // Initiate execute a store procedure function
        public Exception ExecStoreProcedure(string storeProcedureName, object[] param, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storeProcedureName, connection))
                {
                    // dynamic store procedure param selection
                    Exception switchResult = SwitchStoreProcedure(storeProcedureName, param, command);
                    command.Dispose();
                    connection.Close();
                    return switchResult;
                }
            }
        }

        // Switch store procedure parameter to their respective parameter
        public Exception SwitchStoreProcedure(string storeProcedureName, object[] param, SqlCommand command)
        {
            Exception switchStoreProcedureResult = new Exception();
            switch (storeProcedureName)
            {
                case "CreatingBalanceGI":
                    switchStoreProcedureResult = new DistributeConsumptionBM_RSFunctionModel().ExecCreatingBMandRSProcedure(command, param);
                    break;
                case "CreatingRMperBatch":
                    switchStoreProcedureResult = new DistributeConsumptionBM_RSFunctionModel().ExecCreatingBMandRSProcedure(command, param);
                    break;
                case "CreatingFGperBatch":
                    switchStoreProcedureResult = new DistributeConsumptionBM_RSFunctionModel().ExecCreatingBMandRSProcedure(command, param);
                    break;
                case "ReturnRMperBatchDistributedValue":
                    switchStoreProcedureResult = new DistributeConsumptionBM_RSFunctionModel().ExecCreatingBMandRSProcedure(command, param);
                    break;
                case "CreatingTracingFG":
                    switchStoreProcedureResult = new DistributeConsumptionFGTracingFunctionModel().ExecCreatingFGTracingProcedure(command, param);
                    break;
                case "Master_Batch_Process":
                    switchStoreProcedureResult = new GIRawMaterialFunctionModel().ExecMaster_Batch_ProcessProcedure(command, param);
                    break;
                case "CreatingRepackReprocess":
                    switchStoreProcedureResult = new RepackReprocessFunctionModel().ExecCreatingRepackReprocessProcedure(command, param);
                    break;
                case "CreatingTracingRepackReprocess":
                    switchStoreProcedureResult = new DistributeConsumptionFGTracingFunctionModel().ExecCreatingFGTracingProcedure(command, param);
                    break;
            }

            return switchStoreProcedureResult;
        }
    }
}