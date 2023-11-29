using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KITE.Models
{
    public class PIBDateInsertFunctionModel
    {

    }

    public class MasterBatchViewModel
    {
        public string Raw_Material { get; set; }
        public string RM_Batch { get; set; }
        public string PIB_Date { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            MasterBatchViewModel other = (MasterBatchViewModel)obj;
            return
                Raw_Material == other.Raw_Material &&
                RM_Batch == other.RM_Batch &&
                PIB_Date == other.PIB_Date;
        }

        public override int GetHashCode()
        {
            return (
                Raw_Material,
                RM_Batch,
                PIB_Date
             ).GetHashCode();
        }
    }
}