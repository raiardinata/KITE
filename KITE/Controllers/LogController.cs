using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KITE.Controllers
{
    public class LogController 
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public List<Models.InsertAndUpdate> InsertLog(string Action, string Explanation, string Status, string UserAppID, string username)
        {
            object[] param = new[] { Action, Explanation, Status, UserAppID, username };
            List<Models.InsertAndUpdate> Result = dc.ExecuteQuery<Models.InsertAndUpdate>("Exec UserApp_InsertUpdateUser @commandname={0} , @userappid={1} , @username={2}, @fulllname={3} , @password={4} , @email={5} , @registeruser={6} , @usertype={7} ", param).ToList<Models.InsertAndUpdate>();
            return Result;

        }

	}
}