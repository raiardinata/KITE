using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace KITE.Controllers
{
    public class KiteController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
        //
        // GET: /Kite/

        public List<Models.CatchExeptions> InsertQueueA(string Company, string Plant, string Material_LOW, string Material_HIGH, string sloc_low, string sloc_high, string sPeriod_LOW, string sPeriod_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, sloc_low, sloc_high, sPeriod_LOW, sPeriod_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_A @Company={0} , @Plant={1} , @Material_LOW={2}, @sloc_low={4}, @sPeriod_LOW={6} , @sPeriod_HIGH={7}, @User_IP={8}, @action ={9}, @explanation ={10}, @userid ={11}, @username ={12}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueB(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string MoveT_LOW, string MoveT_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, MoveT_LOW, MoveT_HIGH, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Sloc_LOW={4}, @MoveT_LOW={6}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}, @action ={11}, @explanation ={12}, @userid ={13}, @username ={14}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueC(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string MoveT_LOW, string MoveT_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, MoveT_LOW, MoveT_HIGH, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_C @Company={0} , @Plant={1} , @Material_LOW={2}, @Sloc_LOW={4}, @MoveT_LOW={6}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}, @action ={11}, @explanation ={12}, @userid ={13}, @username ={14}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueD(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string MoveT_LOW, string MoveT_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, MoveT_LOW, MoveT_HIGH, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_D @Company={0} , @Plant={1} , @Material_LOW={2}, @Sloc_LOW={4}, @MoveT_LOW={6}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}, @action ={11}, @explanation ={12}, @userid ={13}, @username ={14}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueE(string Company, string SALES, string CUSTGR, string CUSTID, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, SALES, CUSTGR, CUSTID, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_E @Company={0} , @SalesOrganization={1} , @customergroup={2}, @customerid={3}, @sDate_LOW={4} , @sDate_HIGH={5}, @User_IP={6}, @action ={7}, @explanation ={8}, @userid ={9}, @username ={10}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueF(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_F @Company={0} , @Plant={1} , @Material_LOW={2}, @Sloc_LOW={4}, @sDate_LOW={6} , @sDate_HIGH={7}, @User_IP={8}, @action ={9}, @explanation ={10}, @userid ={11}, @username ={12}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueG(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_G @Company={0} , @Plant={1} , @Material_LOW={2}, @Sloc_LOW={4}, @sDate_LOW={6} , @sDate_HIGH={7}, @User_IP={8}, @action ={9}, @explanation ={10}, @userid ={11}, @username ={12}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertQueueH(string Company, string Plant, string Material_LOW, string Material_HIGH, string Sloc_LOW, string Sloc_HIGH, string MoveT_LOW, string MoveT_HIGH, string sDate_LOW, string sDate_HIGH, string User_IP, string action, string explanation, string userid, string username)
        {
            object[] param = new[] { Company, Plant, Material_LOW, Material_HIGH, Sloc_LOW, Sloc_HIGH, MoveT_LOW, MoveT_HIGH, sDate_LOW, sDate_HIGH, User_IP, action, explanation, userid, username };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_H @Company={0} , @Plant={1} , @Material_LOW={2}, @Sloc_LOW={4}, @MoveT_LOW={6}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}, @action ={11}, @explanation ={12}, @userid ={13}, @username ={14}", param).ToList<Models.CatchExeptions>();
            return Result;
        }


        public List<Models.Kite.Kite_SelectMaterial> Select_Material()
        {
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.Kite.Kite_SelectMaterial> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectMaterial>("Exec Select_Material").ToList<Models.Kite.Kite_SelectMaterial>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectMaterialConf> Select_MaterialConf(string menu)
        {
            object[] param = new[] { menu };
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.Kite.Kite_SelectMaterialConf> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectMaterialConf>("Exec Select_Materialbymenu @menu={0}", param).ToList<Models.Kite.Kite_SelectMaterialConf>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectCompany> Select_Company()
        {
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.Kite.Kite_SelectCompany> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectCompany>("Exec Select_Company").ToList<Models.Kite.Kite_SelectCompany>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectCompany> Select_CompanyLabel(string low)
        {
            object[] param = new[] { low };
            List<Models.Kite.Kite_SelectCompany> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectCompany>("Exec Select_Companylabel @low={0}", param).ToList<Models.Kite.Kite_SelectCompany>();
            return Result;
        }



        public List<Models.Kite.Kite_SelectYears> Select_Years(string programid)
        {
            object[] param = new[] { programid };
            List<Models.Kite.Kite_SelectYears> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectYears>("Exec Select_Years @programid={0}", param).ToList<Models.Kite.Kite_SelectYears>();
            return Result;
        }


        public List<Models.Kite.Kite_SelectCustGroup> Select_CustGroup(string programid)
        {
            object[] param = new[] { programid };
            List<Models.Kite.Kite_SelectCustGroup> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectCustGroup>("Exec Select_Custgroup @programid={0}", param).ToList<Models.Kite.Kite_SelectCustGroup>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectCustID> Select_CustID(string programid)
        {
            object[] param = new[] { programid };
            List<Models.Kite.Kite_SelectCustID> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectCustID>("Exec Select_Custid @programid={0}", param).ToList<Models.Kite.Kite_SelectCustID>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectPlant> Select_Plant()
        {
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.Kite.Kite_SelectPlant> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectPlant>("Exec Select_Plant").ToList<Models.Kite.Kite_SelectPlant>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectPlant> Select_Plantlabel(string low)
        {
            object[] param = new[] { low };
            List<Models.Kite.Kite_SelectPlant> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectPlant>("Exec Select_Plantlabel @low={0}", param).ToList<Models.Kite.Kite_SelectPlant>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectSloc> Select_Sloc()
        {
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.Kite.Kite_SelectSloc> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectSloc>("Exec Select_Sloc").ToList<Models.Kite.Kite_SelectSloc>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectSlocConf> Select_Slocconf(string menu)
        {
            object[] param = new[] { menu };
            //object[] param = new[] { filter, StartRow, Row };
            List<Models.Kite.Kite_SelectSlocConf> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectSlocConf>("Exec Select_Slocbymenu @menu={0}", param).ToList<Models.Kite.Kite_SelectSlocConf>();
            return Result;
        }


        public List<Models.Kite.Kite_SelectMove> Select_Move(string ProgramID)
        {
            object[] param = new[] { ProgramID };
            List<Models.Kite.Kite_SelectMove> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectMove>("Exec Select_MovementType @ProgramID={0}", param).ToList<Models.Kite.Kite_SelectMove>();
            return Result;
        }

        public List<Models.Kite.Kite_CheckStatus> Select_CheckStatus(string userid)
        {
            object[] param = new[] { userid };
            List<Models.Kite.Kite_CheckStatus> Result = dc.ExecuteQuery<Models.Kite.Kite_CheckStatus>("Exec CHECK_STATUS @userid={0}", param).ToList<Models.Kite.Kite_CheckStatus>();
            return Result;
        }

        public List<Models.Kite.Kite_CheckStatusPrev> Select_CheckStatusPrev(string userid, string explanation)
        {
            object[] param = new[] { userid, explanation };
            List<Models.Kite.Kite_CheckStatusPrev> Result = dc.ExecuteQuery<Models.Kite.Kite_CheckStatusPrev>("Exec CHECK_STATUS_PREV @userid={0}, @explanation={1}", param).ToList<Models.Kite.Kite_CheckStatusPrev>();
            return Result;
        }

        public List<Models.Kite.Kite_CheckLog> Select_CheckLog()
        {
            //object[] param = new[] { userid };
            List<Models.Kite.Kite_CheckLog> Result = dc.ExecuteQuery<Models.Kite.Kite_CheckLog>("Exec CHECK_log").ToList<Models.Kite.Kite_CheckLog>();
            return Result;
        }

        public List<Models.Kite.Kite_SelectKITEB2> Select_KiteB2(string StartRow, string Row, string USERIP)
        {
            object[] param = new[] { StartRow, Row, USERIP };
            List<Models.Kite.Kite_SelectKITEB2> Result = dc.ExecuteQuery<Models.Kite.Kite_SelectKITEB2>("Exec DATA_REPORT_KITE_B2 @StartRow={0}, @Row={1}, @USERIP={2} ", param).ToList<Models.Kite.Kite_SelectKITEB2>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertMaterialConfiguration(string MENU, string MATERIALID, string CREATEDBY)
        {
            object[] param = new[] { MENU, MATERIALID, CREATEDBY };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec INSERT_MATERIALCONF @MENU={0} , @MATERIALID={1} , @CREATEDBY={2}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> DeleteMaterialConfiguration(string MENU)
        {
            object[] param = new[] { MENU };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec DELETE_MATERIALCONF @MENU={0}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> InsertSlocConfiguration(string MENU, string slocid, string CREATEDBY)
        {
            object[] param = new[] { MENU, slocid, CREATEDBY };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec INSERT_SLOCCONF @MENU={0} , @slocid={1} , @CREATEDBY={2}", param).ToList<Models.CatchExeptions>();
            return Result;
        }

        public List<Models.CatchExeptions> DeleteSlocConfiguration(string MENU)
        {
            object[] param = new[] { MENU };
            //List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec Generate_Report_KITE_B @Company={0} , @Plant={1} , @Material_LOW={2}, @Material_HIGH={3} , @Sloc_LOW={4} , @Sloc_HIGH={5} , @MoveT_LOW={6} , @MoveT_HIGH={7}, @sDate_LOW={8} , @sDate_HIGH={9}, @User_IP={10}", param).ToList<Models.CatchExeptions>();
            List<Models.CatchExeptions> Result = dc.ExecuteQuery<Models.CatchExeptions>("Exec DELETE_SLOCCONF @MENU={0}", param).ToList<Models.CatchExeptions>();
            return Result;
        }
    }
}