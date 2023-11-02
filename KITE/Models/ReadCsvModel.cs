using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace KITE.Models
{
    public class ReadCsvModel : System.Web.UI.Page
    {
        public CsvReader ReadCsvFile(string filePath, string delimiter)
        {
            // Read the content of the CSV file global function
            var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
            };
            return new CsvReader(new StreamReader(filePath), config);
        }

        public Tuple<Exception, string> FileChecker(System.Web.UI.WebControls.FileUpload fileUpload)
        {
            try
            {
                if (fileUpload.HasFile)
                {
                    HttpPostedFile uploadedFile = fileUpload.PostedFile;

                    if (uploadedFile.ContentType == "csv" || Path.GetExtension(uploadedFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        return Tuple.Create(new Exception("null"), "valid");
                    }
                    else
                    {
                        throw new Exception("Your file upload is not csv file, please upload only csv file type.");
                    }
                }
                throw new Exception("No file was selected, please select csv file to proceed.");
            }
            catch (Exception ex)
            {
                return Tuple.Create(ex, "");
            }

        }

        public Tuple<string[], Exception> IterateCsvObject(ArrayList csvValueArrayList)
        {
            List<string> valuesList = new List<string>();
            try
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
                    valuesList.Add(values);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<string[], Exception>(null, new Exception($"Terjadi kesalah dalam pemrosesan IterateCsvObject. Detail : {ex}"));
            }

            return Tuple.Create(valuesList.ToArray(), new Exception("Pemrosesan IterateCsvObject Berhasil."));
        }

        public Tuple<object, Exception> UomConvertion(CsvReader csvDataRead, string type, string connectionString)
        {
            decimal KG = 0;
            object CsvDataList = new object();

            if (type == "mcFrame")
            {
                CsvDataList = csvDataRead.GetRecords<McFrameViewModel>().ToList();
                List<McFrameViewModel> uomConvertionTempList = new List<McFrameViewModel>();

                foreach (McFrameViewModel csvData in (List<McFrameViewModel>)CsvDataList)
                {
                    //try
                    //{
                    //    Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTable("KG", "UOM_Convertion", $" Material = '{csvData.Item_CD}' AND Base_UOM = '{csvData.Unit}' ", connectionString);
                    //    EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable();
                    //    if (insertResult.Item2.Message != "null")
                    //    {
                    //        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam memilih konversi UoM. Detail : {insertResult.Item2}"));
                    //    }
                    //    foreach (DataRow uomConvertionRow in uomConvertionRows)
                    //    {
                    //        KG = uomConvertionRow.Field<decimal>("KG");
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam memilih konversi UoM. Detail : {ex}"));
                    //}

                    uomConvertionTempList.Add(new McFrameViewModel
                    {
                        Calc_No = csvData.Calc_No,
                        Mgmt_dept_CD = csvData.Mgmt_dept_CD,
                        Management_Dept_Name = csvData.Management_Dept_Name,
                        YM = csvData.YM,
                        Lvl = csvData.Lvl,
                        Target_item_CD = csvData.Target_item_CD,
                        Item_CD = csvData.Item_CD,
                        Item_name = csvData.Item_name,
                        Item_type_name = csvData.Item_type_name,
                        Unit = csvData.Unit,
                        Quantity = Convert.ToString(csvData.Quantity),//(csvData.Unit == "") ? Convert.ToString(Convert.ToDecimal(csvData.Quantity) * 1000) : Convert.ToString(csvData.Quantity),
                        STD_Qty = Convert.ToString(csvData.STD_Qty),//(csvData.Unit == "") ? Convert.ToString(Convert.ToDecimal(csvData.Quantity) * 1000) : Convert.ToString(csvData.Quantity),
                        Total = Convert.ToString(Convert.ToDecimal(csvData.Total)),
                        STD_Total = Convert.ToString(Convert.ToDecimal(csvData.STD_Total)),
                        Variable_Cost = Convert.ToString(Convert.ToDecimal(csvData.Variable_Cost)),
                        STD_Variable_Cost = Convert.ToString(Convert.ToDecimal(csvData.STD_Variable_Cost)),
                        Labour_Cost = Convert.ToString(Convert.ToDecimal(csvData.Labour_Cost)),
                        STD_Labour_Cost = Convert.ToString(Convert.ToDecimal(csvData.STD_Labour_Cost)),
                        Depreciation = Convert.ToString(Convert.ToDecimal(csvData.Depreciation)),
                        STD_Depreciation = Convert.ToString(Convert.ToDecimal(csvData.STD_Depreciation)),
                        Repair_Maintenance = Convert.ToString(Convert.ToDecimal(csvData.Repair_Maintenance)),
                        STD_Repair_Maintenance = Convert.ToString(Convert.ToDecimal(csvData.STD_Repair_Maintenance)),
                        Overhead_Cost = Convert.ToString(Convert.ToDecimal(csvData.Overhead_Cost)),
                        STD_Overhead_Cost = Convert.ToString(Convert.ToDecimal(csvData.STD_Overhead_Cost)),
                        Retur_Cost = Convert.ToString(Convert.ToDecimal(csvData.Retur_Cost)),
                        STD_Retur_Cost = Convert.ToString(Convert.ToDecimal(csvData.STD_Retur_Cost)),
                    });
                }
                CsvDataList = uomConvertionTempList;
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            else if (type == "giRawMaterial")
            {
                CsvDataList = csvDataRead.GetRecords<GIRawMaterialViewModel>().ToList();
                List<GIRawMaterialViewModel> uomConvertionTempList = new List<GIRawMaterialViewModel>();

                foreach (GIRawMaterialViewModel csvData in (List<GIRawMaterialViewModel>)CsvDataList)
                {
                    try
                    {
                        Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTable("KG", "UOM_Convertion", $"Material = '{csvData.Material}' AND Base_UOM = '{csvData.Unit_of_Entry}' ", connectionString);
                        EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable();
                        if (insertResult.Item2.Message != "null")
                        {
                            return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam memilih konversi UoM. Detail : {insertResult.Item2}"));
                        }
                        foreach (DataRow uomConvertionRow in uomConvertionRows)
                        {
                            KG = uomConvertionRow.Field<decimal>("KG");
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam memilih konversi UoM. Detail : {ex}"));
                    }

                    uomConvertionTempList.Add(new GIRawMaterialViewModel
                    {
                        Posting_Date = csvData.Posting_Date,
                        Document_Date = csvData.Document_Date,
                        Document_Header_Text = csvData.Document_Header_Text,
                        Material = csvData.Material,
                        Material_Description = csvData.Material_Description,
                        Plant = csvData.Plant,
                        Storage_Location = csvData.Storage_Location,
                        Movement_Type = csvData.Movement_Type,
                        Material_Document = csvData.Material_Document,
                        Batch = csvData.Batch,
                        Qty_in_Un_of_Entry = (KG != 0) ? Convert.ToString(Convert.ToDecimal(csvData.Qty_in_Un_of_Entry) * KG) : csvData.Qty_in_Un_of_Entry,
                        Unit_of_Entry = (KG != 0) ? "KG" : csvData.Unit_of_Entry,
                        Entry_Date = csvData.Entry_Date,
                        Time_of_Entry = csvData.Time_of_Entry,
                        User_name = csvData.User_name,
                        Base_Unit_of_Measure = (KG != 0) ? "KG" : csvData.Unit_of_Entry,
                        Quantity = (KG != 0) ? Convert.ToString(Convert.ToDecimal(csvData.Quantity) * KG) : csvData.Quantity,
                        Amount_in_LC = csvData.Amount_in_LC,
                        Goods_recipient = csvData.Goods_recipient,
                    });
                }
                CsvDataList = uomConvertionTempList;
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            else if (type == "grFinishGoods")
            {
                CsvDataList = csvDataRead.GetRecords<GRFinishGoodsViewModel>().ToList();
                List<GRFinishGoodsViewModel> uomConvertionTempList = new List<GRFinishGoodsViewModel>();

                foreach (GRFinishGoodsViewModel csvData in (List<GRFinishGoodsViewModel>)CsvDataList)
                {
                    try
                    {
                        Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTable("KG", "UOM_Convertion", $"Material = '{csvData.Material}' AND Base_UOM = '{csvData.Unit_of_Entry}' ", connectionString);
                        EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable();
                        if (insertResult.Item2.Message != "null")
                        {
                            return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam memilih konversi UoM. Detail : {insertResult.Item2}"));
                        }
                        foreach (DataRow uomConvertionRow in uomConvertionRows)
                        {
                            KG = uomConvertionRow.Field<decimal>("KG");
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam memilih konversi UoM. Detail : {ex}"));
                    }

                    uomConvertionTempList.Add(new GRFinishGoodsViewModel
                    {
                        Posting_Date = csvData.Posting_Date,
                        Document_Date = csvData.Document_Date,
                        Document_Header_Text = csvData.Document_Header_Text,
                        Material = csvData.Material,
                        Material_Description = csvData.Material_Description,
                        Plant = csvData.Plant,
                        Storage_Location = csvData.Storage_Location,
                        Movement_Type = csvData.Movement_Type,
                        Material_Document = csvData.Material_Document,
                        Batch = csvData.Batch,
                        Qty_in_Un_of_Entry = (KG != 0) ? Convert.ToString(Convert.ToDecimal(csvData.Qty_in_Un_of_Entry) * KG) : csvData.Qty_in_Un_of_Entry,
                        Unit_of_Entry = (KG != 0) ? "KG" : csvData.Unit_of_Entry,
                        Entry_Date = csvData.Entry_Date,
                        Time_of_Entry = csvData.Time_of_Entry,
                        User_name = csvData.User_name,
                        Base_Unit_of_Measure = (KG != 0) ? "KG" : csvData.Unit_of_Entry,
                        Quantity = (KG != 0) ? Convert.ToString(Convert.ToDecimal(csvData.Quantity) * KG) : csvData.Quantity,
                        Amount_in_LC = csvData.Amount_in_LC,
                        Goods_recipient = csvData.Goods_recipient,
                    });
                }
                CsvDataList = uomConvertionTempList;
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            return new Tuple<object, Exception>(null, new Exception("Type tidak dapat dideteksi dalam pemrosesan konversi UoM."));
        }
    }
}