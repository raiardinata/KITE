using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace KITE
{
    public class Setting
    {
        public static String ReportServerUrls
        {
            get { return ConfigurationManager.AppSettings["ReportServerUrls"].ToString(); }
        }

        public static string TimerInterval
        {
            get { return ConfigurationManager.AppSettings["TimerInterval"].ToString(); }
        }

       
    }
}