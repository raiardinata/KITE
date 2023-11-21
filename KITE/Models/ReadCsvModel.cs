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
        // Read csv function so it could be tested and reusable
        public Tuple<object, Exception> ReadCsvFunction(string processType, string filePath, string delimiter, string connectionString)
        {
            using (CsvReader csvData = ReadCsvFile(filePath, delimiter))
            {
                Tuple<object, Exception> uomConvertionObject = UomConvertion(csvData, processType, connectionString);
                if (uomConvertionObject.Item2.Message != "null")
                {
                    return new Tuple<object, Exception>(null, uomConvertionObject.Item2);
                }

                csvData.Dispose();
                return Tuple.Create(uomConvertionObject.Item1, new Exception("null"));
            }
        }

        // Read csv file function
        public CsvReader ReadCsvFile(string filePath, string delimiter)
        {
            // Read the content of the CSV file global function
            var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
            };
            return new CsvReader(new StreamReader(filePath), config);
        }

        // File checker function
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

        // Iterate csv object function
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

        // Uom convertion function
        public Tuple<object, Exception> UomConvertion(CsvReader csvDataRead, string type, string connectionString)
        {
            decimal KG;
            object CsvDataList = new object();

            if (type == "mcFrame")
            {
                CsvDataList = csvDataRead.GetRecords<McFrameViewModel>().ToList();
                List<McFrameWithKilosConvertionViewModel> uomConvertionTempList = new List<McFrameWithKilosConvertionViewModel>(); // this will help change McFrameViewModel form into McFrameWithKilosConvertionViewModel form

                foreach (McFrameViewModel csvData in (List<McFrameViewModel>)CsvDataList)
                {
                    KG = 1;
                    try
                    {
                        Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTableIntoDataTable("KilosConvertion", "UoM_To_Kilos_Convertion", $" WHERE BaseUoM = '{csvData.Unit}' ", connectionString);
                        EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable(); // return only one line
                        if (insertResult.Item2.Message == "null")
                        {
                            foreach (DataRow uomConvertionRow in uomConvertionRows) // but still need to iterate it. How to avoid iteration?
                            {
                                KG = uomConvertionRow.Field<decimal>("KilosConvertion");
                            }
                        }
                        if (csvData.Quantity.Contains(','))
                        {
                            return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam konfigurasi csv comma separator menggunakan (','). Silahkan ganti konfigurasi csv comma separator menggunakan ('.')."));
                        }

                        uomConvertionTempList.Add(new McFrameWithKilosConvertionViewModel
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
                            Quantity = Convert.ToString(csvData.Quantity),
                            STD_Qty = Convert.ToString(csvData.Quantity),
                            Kilos_Convertion = (KG != 1) ? Convert.ToString(Convert.ToDecimal(csvData.Quantity) * KG) : Convert.ToString(csvData.Quantity),
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
                            STD_Retur_Cost = (Convert.ToDecimal(csvData.STD_Retur_Cost)).ToString("N4"),
                        });
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam penambahan uomConvertionTempList. Detail : {ex}"));
                    }
                }
                CsvDataList = uomConvertionTempList; // return McFrameWithKilosConvertionViewModel
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            else if (type == "giRawMaterial")
            {
                CsvDataList = csvDataRead.GetRecords<GIRawMaterialViewModel>().ToList();
                List<GIRawMaterialWithConvertionViewModel> uomConvertionTempList = new List<GIRawMaterialWithConvertionViewModel>(); // this will help change McFrameViewModel form into McFrameWithKilosConvertionViewModel form

                foreach (GIRawMaterialViewModel csvData in (List<GIRawMaterialViewModel>)CsvDataList)
                {
                    KG = 1;
                    try
                    {
                        Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTableIntoDataTable("KilosConvertion", "UoM_To_Kilos_Convertion", $" WHERE BaseUoM = '{csvData.Unit_of_Entry}' ", connectionString);
                        EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable(); // return only one line
                        if (insertResult.Item2.Message == "null")
                        {
                            foreach (DataRow uomConvertionRow in uomConvertionRows) // but still need to iterate it. How to avoid iteration?
                            {
                                KG = uomConvertionRow.Field<decimal>("KilosConvertion");
                            }
                        }
                        if (csvData.Qty_in_Un_of_Entry.Contains(','))
                        {
                            return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam konfigurasi csv comma separator menggunakan (','). Silahkan ganti konfigurasi csv comma separator menggunakan ('.')."));
                        }

                        uomConvertionTempList.Add(new GIRawMaterialWithConvertionViewModel
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
                            Qty_in_Un_of_Entry = csvData.Qty_in_Un_of_Entry,
                            Kilos_Convertion = (KG != 1) ? (Convert.ToDecimal(csvData.Qty_in_Un_of_Entry) * KG).ToString("N4") : (Convert.ToDecimal(csvData.Qty_in_Un_of_Entry)).ToString("N4"),
                            Unit_of_Entry = csvData.Unit_of_Entry,
                            Entry_Date = csvData.Entry_Date,
                            Time_of_Entry = csvData.Time_of_Entry,
                            User_name = csvData.User_name,
                            Base_Unit_of_Measure = csvData.Base_Unit_of_Measure,
                            Quantity = csvData.Quantity,
                            Amount_in_LC = csvData.Amount_in_LC,
                            Goods_recipient = csvData.Goods_recipient,
                        });
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam penambahan uomConvertionTempList. Detail : {ex}"));
                    }
                }
                CsvDataList = uomConvertionTempList;
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            else if (type == "grFinishGoods")
            {
                CsvDataList = csvDataRead.GetRecords<GRFinishGoodsViewModel>().ToList();
                List<GRFinishGoodsWithConvertionViewModel> uomConvertionTempList = new List<GRFinishGoodsWithConvertionViewModel>(); // this will help change McFrameViewModel form into McFrameWithKilosConvertionViewModel form

                foreach (GRFinishGoodsViewModel csvData in (List<GRFinishGoodsViewModel>)CsvDataList)
                {
                    KG = 1;
                    try
                    {
                        Tuple<DataTable, Exception> insertResult = new DatabaseModel().SelectTableIntoDataTable("KG", "UOMandMaterial_Convertion", $" WHERE Material = '{csvData.Material}' AND Base_UOM = '{csvData.Unit_of_Entry}' ", connectionString);
                        EnumerableRowCollection<DataRow> uomConvertionRows = insertResult.Item1.AsEnumerable(); // return only one line
                        if (insertResult.Item2.Message == "null")
                        {
                            foreach (DataRow uomConvertionRow in uomConvertionRows) // but still need to iterate it. How to avoid iteration?
                            {
                                KG = uomConvertionRow.Field<decimal>("KG");
                            }
                        }
                        if (csvData.Qty_in_Un_of_Entry.Contains(','))
                        {
                            return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam konfigurasi csv comma separator menggunakan (','). Silahkan ganti konfigurasi csv comma separator menggunakan ('.')."));
                        }

                        uomConvertionTempList.Add(new GRFinishGoodsWithConvertionViewModel
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
                            Qty_in_Un_of_Entry = csvData.Qty_in_Un_of_Entry,
                            Kilos_Convertion = (KG != 1) ? (Convert.ToDecimal(csvData.Qty_in_Un_of_Entry) * KG).ToString("N4") : (Convert.ToDecimal(csvData.Qty_in_Un_of_Entry)).ToString("N4"),
                            Unit_of_Entry = csvData.Unit_of_Entry,
                            Entry_Date = csvData.Entry_Date,
                            Time_of_Entry = csvData.Time_of_Entry,
                            User_name = csvData.User_name,
                            Base_Unit_of_Measure = csvData.Base_Unit_of_Measure,
                            Quantity = csvData.Quantity,
                            Amount_in_LC = csvData.Amount_in_LC,
                            Goods_recipient = csvData.Goods_recipient,
                        });
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam penambahan uomConvertionTempList. Detail : {ex}"));
                    }


                }
                CsvDataList = uomConvertionTempList;
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            else if (type == "exportSales")
            {
                CsvDataList = csvDataRead.GetRecords<ExportSalesViewModel>().ToList();
                List<ExportSalesWithKilosConvertionViewModel> uomConvertionTempList = new List<ExportSalesWithKilosConvertionViewModel>(); // this will help change McFrameViewModel form into ExportSalesWithKilosConvertionViewModel form

                foreach (ExportSalesViewModel csvData in (List<ExportSalesViewModel>)CsvDataList)
                {
                    KG = 1;
                    try
                    {
                        Tuple<DataTable, Exception> insertResultMaterial = new DatabaseModel().SelectTableIntoDataTable("KG", "UOMandMaterial_Convertion", $" WHERE Material = '{csvData.Material}' AND Base_UOM = '{csvData.Unit_of_Entry}' ", connectionString);
                        EnumerableRowCollection<DataRow> materialUomConvertionRows = insertResultMaterial.Item1.AsEnumerable(); // return only one line
                        if (insertResultMaterial.Item2.Message == "null")
                        {
                            foreach (DataRow materialUomConvertionRow in materialUomConvertionRows) // but still need to iterate it. How to avoid iteration?
                            {
                                KG = materialUomConvertionRow.Field<decimal>("KG");
                            }
                        }
                        else
                        {
                            Tuple<DataTable, Exception> insertResultKilos = new DatabaseModel().SelectTableIntoDataTable("KilosConvertion", "UoM_To_Kilos_Convertion", $" WHERE BaseUoM = '{csvData.Unit_of_Entry}' ", connectionString);
                            EnumerableRowCollection<DataRow> kilosUomConvertionRows = insertResultKilos.Item1.AsEnumerable(); // return only one line
                            if (insertResultKilos.Item2.Message == "null")
                            {
                                foreach (DataRow kilosUomConvertionRow in kilosUomConvertionRows) // but still need to iterate it. How to avoid iteration?
                                {
                                    KG = kilosUomConvertionRow.Field<decimal>("KilosConvertion");
                                }
                            }
                        }
                        if (csvData.Qty_in_Un_of_Entry.ToString().Contains(','))
                        {
                            return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam konfigurasi csv comma separator menggunakan (','). Silahkan ganti konfigurasi csv comma separator menggunakan ('.')."));
                        }

                        uomConvertionTempList.Add(new ExportSalesWithKilosConvertionViewModel
                        {
                            Posting_Date = csvData.Posting_Date,
                            Document_Date = csvData.Document_Date,
                            Document_Header_Text = csvData.Document_Header_Text,
                            Purchase_Order = csvData.Purchase_Order,
                            Reference = csvData.Reference,
                            Material = csvData.Material,
                            Material_Description = csvData.Material_Description,
                            Plant = csvData.Plant,
                            Storage_Location = csvData.Storage_Location,
                            Movement_Type = csvData.Movement_Type,
                            Material_Document = csvData.Material_Document,
                            Batch = csvData.Batch,
                            Qty_in_Un_of_Entry = csvData.Qty_in_Un_of_Entry,
                            KilosConvertion = (KG != 1) ? csvData.Qty_in_Un_of_Entry * KG : csvData.Qty_in_Un_of_Entry,
                            Unit_of_Entry = csvData.Unit_of_Entry,
                            Entry_Date = csvData.Entry_Date,
                            Time_of_Entry = csvData.Time_of_Entry,
                            User_Name = csvData.User_Name,
                            Base_Unit_of_Measure = csvData.Base_Unit_of_Measure,
                            Quantity = csvData.Quantity,
                            Debit_Credit_Ind = csvData.Debit_Credit_Ind,
                            Amount_in_LC = csvData.Amount_in_LC,
                            Sales_Order = csvData.Sales_Order,
                            Text = csvData.Text,
                            Customer = csvData.Customer,
                            Vendor = csvData.Vendor,
                            Vendor_Name = csvData.Vendor_Name,
                            Goods_recipient = csvData.Goods_recipient,
                            SO = csvData.SO,
                            SaType = csvData.SaType,
                            Sold_to_party = csvData.Sold_to_party,
                            Name_1 = csvData.Name_1,
                            PO_Number = csvData.PO_Number,
                            NO_PEB = csvData.NO_PEB,
                            Tanggal_PEB = csvData.Tanggal_PEB,
                        });
                    }
                    catch (Exception ex)
                    {
                        return new Tuple<object, Exception>(null, new Exception($"Terdapat kesalahan dalam penambahan uomConvertionTempList. Detail : {ex}"));
                    }


                }
                CsvDataList = uomConvertionTempList;
                return Tuple.Create(CsvDataList, new Exception("null"));
            }
            return new Tuple<object, Exception>(null, new Exception("Type tidak dapat dideteksi dalam pemrosesan konversi UoM."));
        }
    }
}