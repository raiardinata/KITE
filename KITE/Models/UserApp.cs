using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KITE.Models
{
    public class UserApp
    {
        public partial class UserApp_Select
        {
            public int UserAppID { get; set; }
            public string ActiveUser { get; set; }

            public string FullName { get; set; }

            public string UserType { get; set; }


        }

        public partial class UserApp_CheckPass
        {
            public int UserAppID { get; set; }
            public string password { get; set; }

        }

        public partial class UserApp_SelectAll
        {

            public int UserAppID { get; set; }
            public string UserName { get; set; }
            public string FullName { get; set; }
            public string plant { get; set; }
            public string warehouse { get; set; }
            public string usertype { get; set; }
            public string usermenu { get; set; }
            public string userprinter { get; set; }
            public string userfolder { get; set; }
            public string usermail { get; set; }

            public int isactive { get; set; }

            public int TotalRow { get; set; }

        }

        public partial class UserType_SelectAll
        {
            public string MENUNAME { get; set; }
        }
    }
}