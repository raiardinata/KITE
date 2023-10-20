using System.Collections.Generic;
using System.Linq;

namespace KITE.Controllers
{
    public class LogController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public List<Models.CatchExeptions> InsertLog(string Action, string Explanation, string Status, string UserAppID, string username)
        {
            object[] param = new[] { Action, Explanation, Status, UserAppID, username };
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec UserApp_InsertUpdateUser @commandname={0} , @userappid={1} , @username={2}, @fulllname={3} , @password={4} , @email={5} , @registeruser={6} , @usertype={7} ", param).ToList<Models.CatchExeptions>();
            return Result;

        }

    }
}