using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KITE.Models
{
    public class Kite
    {
        public partial class Kite_SelectMaterial
        {
            public string MATERIALID { get; set; }
            public string Description { get; set; }
        }

        public partial class Kite_SelectMaterialConf
        {
            public string menu { get; set; }
            public string MATERIALID { get; set; }
            //public string Description { get; set; }
        }
        public partial class Kite_SelectCompany
        {
            public string low { get; set; }

            public string high { get; set; }
        }

        public partial class Kite_SelectCustGroup
        {
            public string low { get; set; } 
        }

        public partial class Kite_SelectYears
        {
            public string low { get; set; }
        }

        public partial class Kite_SelectCustID
        {
            public string low { get; set; } 
        }
        public partial class Kite_SelectPlant
        {
            public string low { get; set; }

            public string high { get; set; }

        }

        public partial class Kite_SelectSloc
        {
            public string sloc { get; set; }
        }

        public partial class Kite_SelectSlocConf
        {
            public string menu { get; set; }
            public string slocID { get; set; }
        }

        public partial class Kite_SelectMove
        {
            public string low { get; set; }
        }
        public partial class Kite_CheckStatus
        {
            public int status { get; set; }
        }

        public partial class Kite_CheckStatusPrev
        {
            public int status { get; set; }
        }

        public partial class Kite_CheckLog
        {
            public int no { get; set; }
        }

        public partial class Kite_SelectKITEB2
        {
            public string no { get; set; }
            public string nomor { get; set; }
            public string tanggal { get; set; }
            public string kode_barang { get; set; }
            public string nama_barang { get; set; }
            public string satuan { get; set; }
            public string digunakan { get; set; }
            public string disubkontrakkan { get; set; }
            public string subkontrak { get; set; }
            public string user_ip { get; set; }
            public Int32 TotalRow { get; set; }
        }
        public partial class Kite_SelectKITEB
        {
            //public int no { get; set; }
            public string nomor { get; set; }
            //public string tanggal { get; set; }
            //public string kode_barang { get; set; }
            //public string nama_barang { get; set; }
            ///public string satuan { get; set; }
           // public double digunakan { get; set; }
            //public string disubkontrakkan { get; set; }
            //public string subkontrak { get; set; }
            //public string user_ip { get; set; }
        }
    }
}