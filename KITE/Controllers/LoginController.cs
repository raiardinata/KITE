using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KITE.Controllers
{
    public class LoginController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public IEnumerable<Models.Login_User> Login_User(string Username, string Password)
        {
            object[] param = new[] { Username, Password };
            IEnumerable<Models.Login_User> Result = dc.ExecuteQuery<Models.Login_User>("Exec UserApp_Login_User @Username={0},@Password={1}", param);
            return Result;
        }

        public List<Models.InsertAndUpdate> InsertLog(string action, string explanation, string status, string userappid,  string username)
        {
            object[] param = new[] { action, explanation, status, userappid, username };
            //List<Models.InsertAndUpdate> Result = dc.ExecuteQuery<Models.InsertAndUpdate>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.InsertAndUpdate>();
            List<Models.InsertAndUpdate> Result = dc.ExecuteQuery<Models.InsertAndUpdate>("Exec UserApp_Login_UserLOGACT @action={0} , @explanation={1} , @status={2}, @userappid={3}, @username={4}", param).ToList<Models.InsertAndUpdate>();
            return Result;
        }

	}
}