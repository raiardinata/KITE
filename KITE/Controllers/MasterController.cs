using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace KITE.Controllers
{
    public class MasterController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());

        public List<Models.Master.Menu_Select> Menu_Select(string UserType)
        {
            object[] param = new[] { UserType };
            List<Models.Master.Menu_Select> Result = dc.ExecuteQuery<Models.Master.Menu_Select>("Exec Menu_Select @UserType={0}", param).ToList<Models.Master.Menu_Select>();
            return Result;
        }
	}
}