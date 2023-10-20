using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KITE.Models
{
    public class DatabaseModel
    {
        public Exception InsertIntoTable(string tableName, string columnName, ArrayList csvValueArrayList, string connectionString)
        {
            foreach (object csvDataObject in csvValueArrayList)
            {
                string values = "";
                List<object> csvDataList = (List<object>)csvDataObject;
                foreach (object csvData in csvDataList)
                {
                    string modifiedString = $"'{csvData}'";
                    modifiedString = modifiedString.Replace(",", ".");

                    values += modifiedString + ",";
                }
                values = values.Substring(0, values.Length - 1);
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
                        }
                        catch (Exception ex)
                        {
                            return ex;
                        }
                    }
                }
            }

            return new Exception($"Insert Into Table {tableName} Berhasil.");
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
    }
}