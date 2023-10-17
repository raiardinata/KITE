using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KITE.Models
{
    public partial class Login_User
    {
        public string ActiveUser { get; set; }
        public string username { get; set; }
        public string NIK { get; set; }
        public string Dep { get; set; }
        public string UserType { get; set; }
        public int UserAppID { get; set; }

        public string PSGroup { get; set; }
        public string FirstLogin { get; set; }

        public string FullName { get; set; }
    }

    public partial class Logact
    {
        public string ActiveUser { get; set; }
        public string CompanyID { get; set; }
        public string NIK { get; set; }
        public string Dep { get; set; }
        public string UserType { get; set; }
        public int UserAppID { get; set; }

        public string PSGroup { get; set; }
        public string FirstLogin { get; set; }

        public string FullName { get; set; }
    }
}