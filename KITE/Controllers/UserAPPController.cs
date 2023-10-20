using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace KITE.Controllers
{
    public class UserAPPController
    {
        //
        // GET: /UserAPP/
        DataClasses1DataContext dc = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
        public IEnumerable<Models.UserApp.UserApp_Select> Login_User(string Username, string Password)
        {
            object[] param = new[] { Username, Password };
            IEnumerable<Models.UserApp.UserApp_Select> Result = dc.ExecuteQuery<Models.UserApp.UserApp_Select>("Exec UserApp_Login_User @Username={0},@Password={1}", param);
            return Result;
        }

        public IEnumerable<Models.UserApp.UserApp_Select> Check_User(string Username)
        {
            object[] param = new[] { Username };
            IEnumerable<Models.UserApp.UserApp_Select> Result = dc.ExecuteQuery<Models.UserApp.UserApp_Select>("Exec UserApp_Check_User @Username={0} ", param);
            return Result;
        }

        public IEnumerable<Models.UserApp.UserApp_CheckPass> Check_Password(string Username, string password)
        {
            object[] param = new[] { Username, password };
            IEnumerable<Models.UserApp.UserApp_CheckPass> Result = dc.ExecuteQuery<Models.UserApp.UserApp_CheckPass>("Exec UserApp_Check_Pass @Username={0}, @password={1} ", param);
            return Result;
        }

        public List<Models.UserApp.UserApp_SelectAll> Show_User(string filter, string StartRow, string Row)
        {
            object[] param = new[] { filter, StartRow, Row };
            List<Models.UserApp.UserApp_SelectAll> Result = dc.ExecuteQuery<Models.UserApp.UserApp_SelectAll>("Exec UserApp_Show_User @Filter={0}, @StartRow={1}, @Row={2} ", param).ToList<Models.UserApp.UserApp_SelectAll>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertUpdateUser(string commandname, string userappid, string username, string fullname, string password, string email, string registeruser, string usertype)
        {
            object[] param = new[] { commandname, userappid, username, fullname, password, email, registeruser, usertype };
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec UserApp_InsertUpdateUser @commandname={0} , @userappid={1} , @username={2}, @fulllname={3} , @password={4} , @email={5} , @registeruser={6} , @usertype={7} ", param).ToList<Models.CatchExeptions>();
            return Result;

        }

        public List<Models.CatchExeptions> UpdatePassword(string username, string password)
        {
            object[] param = new[] { username, password };
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec UserApp_updatepass @username={0}, @password={1}", param).ToList<Models.CatchExeptions>();
            return Result;

        }

        public List<Models.CatchExeptions> DeleteUser(string userappid)
        {
            object[] param = new[] { userappid };
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec UserApp_DeleteUser  @userappid={0} ", param).ToList<Models.CatchExeptions>();
            return Result;

        }

        public List<Models.UserApp.UserApp_Select> Select_userall()
        {
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.UserApp.UserApp_Select> Result = dc.ExecuteQuery<Models.UserApp.UserApp_Select>("Exec userapp_all_user").ToList<Models.UserApp.UserApp_Select>();
            return Result;
        }

        public List<Models.UserApp.UserType_SelectAll> Select_UserType()
        {
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.UserApp.UserType_SelectAll> Result = dc.ExecuteQuery<Models.UserApp.UserType_SelectAll>("Exec SELECT_USERTYPE").ToList<Models.UserApp.UserType_SelectAll>();
            return Result;
        }

    }
}