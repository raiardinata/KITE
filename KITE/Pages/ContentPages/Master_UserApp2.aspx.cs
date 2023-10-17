using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
//using Microsoft.Reporting.WebForms;
using System.Globalization;
//using PRJ_REPORT_SCM.SQLConnect;
using System.Text;
using System;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace KITE.Pages.ContentPages
{
    
    public partial class Master_UserApp2 : System.Web.UI.Page
    {
        //Controllers.DemandPlanningControllers DPC = new Controllers.DemandPlanningControllers();

        Controllers.UserAPPController UAP = new Controllers.UserAPPController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();
        int grandtotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
            Button1.CausesValidation = true;
            Button1.ValidationGroup = "RegisterCheck";
            if (!IsPostBack) 
            {
                gridSearch(txtSearchCriteria.Text);
                
            }
            
            

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
           // var checkuser = UAP.Check_User(txtUserName.Text);
            //if (ViewState["Insert_Update"].ToString() == "Insert")
            //{
            //    Button1.CausesValidation = true;
            //    Button1.ValidationGroup = "RegisterCheck";
            //}
            SubmitInformation(ViewState["Insert_Update"].ToString(), txtUserAppID.Value, txtUserName.Text, txtFullName.Text, txtPassword.Text, txtemail.Text, HttpContext.Current.Session["ActiveUser"].ToString(), ddlUsertype.SelectedItem.Value.ToString());
        }

        protected void SubmitInformation(string CommandName , string userappid , string username , string fullname , string password , string email , string registeruser , string usertype) 
        {
            password = Kite.EncryptDecrypt.CryptorEngine.Encrypt(password, true);
            UAP.InsertUpdateUser(CommandName, userappid ,username, fullname, password, email , registeruser , usertype);

            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#InsertUpdateUser')", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InsertUpdateUser", "$('#InsertUpdateUser').modal('hide');", true);

             gridSearch("");
        }

        private void gridSearch(string Filter)
        {
            grid.PageSize = int.Parse(ddlPageSize.SelectedValue);

          
            ViewState["Row"] = 0;
            string StartRow = (grid.PageIndex * grid.PageSize).ToString();
            string Row = ddlPageSize.SelectedValue;
            var dt = UAP.Show_User(Filter, StartRow, Row);
              
            grid.DataSource = dt;
            grid.DataBind();


            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    if (ViewState["Row"].ToString().Trim() == "0")
                    {
                        ViewState["grandtotal"] = select.TotalRow;
                        lblTotalRecords.Text = String.Format("Total Records : {0}", ViewState["grandtotal"]);
                       // DivGrid.Visible = true;
                        // lblNoData.Visible = false;
                        ViewState["Row"] = 1;
                        int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / grid.PageSize));
                        lblTotalNumberOfPages.Text = pageCount.ToString();
                        txtGoToPage.Text = (grid.PageIndex + 1).ToString();
                      //  GridViewField.Visible = true;
                    }
                }

            }
            else
            {
              //  GridViewField.Visible = false;
                ViewState["grandtotal"] = 0;
               // DivGrid.Visible = false;
                //lblNoData.Visible = true;
            }
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid.PageIndex = e.NewPageIndex;
            gridSearch(txtSearchCriteria.Text);
        }


        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.PageSize = int.Parse(ddlPageSize.SelectedValue);
            grid.PageIndex = 0;
            gridSearch(txtSearchCriteria.Text);
        }

        protected void GoToPage_TextChanged(object sender, EventArgs e)
        {
            int pageNumber;
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / grid.PageSize));
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= pageCount)
            {
                LoadPage(pageNumber - 1);
            }
            else
            {
                LoadPage(0);
            }
        }

        protected void btnPrev_OnClick(object sender, EventArgs e)
        {
            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 1)
            {
                LoadPage(pageNumber - 2);
            }
            else
            {
                LoadPage(grid.PageIndex);
            }
        }

        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            int pageNumber;
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ViewState["grandtotal"]) / grid.PageSize));
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber < pageCount)
            {
                LoadPage(pageNumber);
            }
            else
            {
                LoadPage(grid.PageIndex);
            }
        }

        private void LoadPage(int pageNumber)
        {
            GridViewPageEventArgs e = new GridViewPageEventArgs(pageNumber);
            grid_PageIndexChanging(this, e);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string filter = ddlSearchCriteria.SelectedValue + " LIKE '%" + txtSearchCriteria.Text + "%'";
            gridSearch(filter);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchCriteria.Text = string.Empty;
            gridSearch( txtSearchCriteria.Text);
        }

        protected void btnDummy_Click(object sender, EventArgs e)
        {
            gridSearch(txtSearchCriteria.Text);
        }

        protected void CheckUser()
        {
            if (HttpContext.Current.Session["ActiveUser"] == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                LinkButton lb = (LinkButton)Master.FindControl("lnLogout");
                lb.Text = "Logout " + HttpContext.Current.Session["ActiveUser"];

                Label lbl1 = (Label)Master.FindControl("label1");
                lbl1.Text = HttpContext.Current.Session["FullName"].ToString();

            }
        }

        protected void btnAdd1_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddNewUserModal", "$('#AddNewUserModal').modal();", true);
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Insert')", true);
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InsertUpdateUser", "$('#InsertUpdateUser').modal();", true);
            setProperty();
            ViewState["Insert_Update"] = "Insert";
            pass1.Visible = true;
            pass2.Visible = true;
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InsertUpdateUser", "$('#InsertUpdateUser').modal();", true);
            
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "InsertUpdateUser", "ShowPopUp('#InsertUpdateUser')", true);
            
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUpBaru('#Insert')", true);
            ListUserType();
        }
        private void ListUserType()
        {
            ddlUsertype.Items.Clear();
            var dt = UAP.Select_UserType();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    ddlUsertype.Items.Add(select.MENUNAME.ToString());
                }
                
            }

        }

        protected void setProperty() 
        {
            txtUserAppID.Value = "";
            txtUserName.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            txtPassword2.Text = "";
            txtemail.Text = "";

            
        }

        

        protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InsertUpdateUser", "$('#InsertUpdateUser').modal();", true);
            txtSearchCriteria.Text = string.Empty;
         
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                //assuming that the required value column is the second column in gridview
              
                int userappid = Int32.Parse( DataBinder.Eval(e.Row.DataItem, "UserAppID").ToString() );

            }
        }

        protected void grid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Button1.CausesValidation = false;
                Button1.ValidationGroup = "";
                ddlUsertype.SelectedIndex = -1;
                ddlUsertype.Items.Clear();
                string[] EditArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUpBaru('#Insert')", true);
                ListUserType();
                //  ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Open", "ShowPopUp('#Insert')", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InsertUpdateUser", "$('#InsertUpdateUser').modal();", true);
                setProperty();
                ViewState["Insert_Update"] = "Update";

                txtUserAppID.Value = EditArgs[0];
                txtUserName.Text = EditArgs[1];
                txtFullName.Text = EditArgs[2];
                ddlUsertype.SelectedValue = EditArgs[3].ToString();

                txtemail.Text = EditArgs[4];

            }
           
        }
        private void onconfirmation()
        {
        
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void lbAction2_Click(object sender, EventArgs e)
        {
            
        }

        protected void lbAction2_Click1(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton) (sender);
            string uappid = btn.CommandArgument;
            UAP.DeleteUser(uappid);

            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#InsertUpdateUser')", true);
            //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
            gridSearch("");
//            SubmitInformation(ViewState["Insert_Update"].ToString(), txtUserAppID.Value, txtUserName.Text, txtFullName.Text, txtPassword.Text, txtemail.Text, HttpContext.Current.Session["ActiveUser"].ToString(), ddlUsertype.SelectedItem.Value.ToString());
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Close", "Close('#Insert')", true);
        }
    }
}