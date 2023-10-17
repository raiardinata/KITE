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
using Microsoft.Reporting.WebForms;

namespace KITE.Pages.ContentPages
{
    public partial class SlocConfiguration : System.Web.UI.Page
    {

        Controllers.KiteController KC = new Controllers.KiteController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();

        Controllers.LoginController login = new Controllers.LoginController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListSloc();
            }
        }

        private void ListSloc()
        {
            lbSloc.Items.Clear();
            //lbSloc2.Items.Clear();

            var dt = KC.Select_Sloc();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbSloc.Items.Add(select.sloc.ToString());
                }
                lbSloc.Items.Add("ALL");
            }

        }

        private void ListSlocConf(string menunya)
        {
            //lbMaterial.Items.Clear();
            lbSloc2.Items.Clear();
            var dt = KC.Select_Slocconf(menunya);

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbSloc2.Items.Add(select.slocID.ToString());
                }
                //lbMaterial.Items.Add("ALL Material (121)");
                //lbMaterial.Items.Add("ALL Finish Goods (124)");
                //lbMaterial.Items.Add("ALL Waste (211)");
            }

        }

        protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSearchCriteria.SelectedValue.ToString() != "--Pilih Menu--")
            {
                Labelmenu.Visible = true;
                if (ddlSearchCriteria.SelectedValue.ToString() == "KITE A")
                {
                    Labelmenu.Text = "Laporan Pemasukan Bahan Baku";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE B")
                {
                    Labelmenu.Text = "Laporan Pemakaian Bahan Baku";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE C")
                {
                    Labelmenu.Text = "Laporan Pemakaian Barang Dalam Proses Dalam Rangka Kegiatan Subkontrak";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE D")
                {
                    Labelmenu.Text = "Laporan Pemasukan Hasil Produksi";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE E")
                {
                    Labelmenu.Text = "Laporan Pengeluaran Hasil Produksi";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE F")
                {
                    Labelmenu.Text = "Laporan Mutasi Bahan Baku";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE G")
                {
                    Labelmenu.Text = "Laporan Mutasi Hasil Produksi";
                }
                else if (ddlSearchCriteria.SelectedValue.ToString() == "KITE H")
                {
                    Labelmenu.Text = "Laporan Penyelesaian Waste/Scrap";
                }
                ListSlocConf(ddlSearchCriteria.SelectedValue.ToString());
            }
            else
            {
                lbSloc2.Items.Clear();
                Labelmenu.Visible = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlSearchCriteria.SelectedValue.ToString() != "--Pilih Menu--")
            {
                INSERTSLOCCONF();
                ddlSearchCriteria.SelectedIndex = 0;
                lbSloc2.Items.Clear();
                //Labelmenu.Visible = false;
                Labelmenu.Text = "Success Saving.";
            }
        }

        protected void INSERTSLOCCONF()
        {
            if (lbSloc2.Items.Count > 0)
            {
                KC.DeleteSlocConfiguration(ddlSearchCriteria.SelectedValue.ToString());
                for (int i = 0; i < lbSloc2.Items.Count; i++)
                {
                    KC.InsertSlocConfiguration(ddlSearchCriteria.SelectedValue.ToString(), lbSloc2.Items[i].Text, HttpContext.Current.Session["UserAppID"].ToString());
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlSearchCriteria.SelectedIndex = -1;
            lbSloc2.Items.Clear();
            Labelmenu.Visible = false;
        }

        protected void btnSelectmat_Click(object sender, EventArgs e)
        {
            List<int> selecteds = lbSloc.GetSelectedIndices().ToList();
            string MAT = "";
            bool exists = false;

            for (int i = 0; i < lbSloc.Items.Count; i++)
            {
                if (lbSloc.Items[i].Selected)
                {
                    string stritem = lbSloc.Items[i].Text;
                    if (lbSloc2.Items.Count > 0)
                    {
                        for (int y = 0; y < lbSloc2.Items.Count; y++)
                        {
                            if (stritem == lbSloc2.Items[y].Text)
                            {
                                exists = true;

                            }
                        }
                    }

                    if (!exists)
                    {
                        if (stritem == "ALL")
                        {
                            lbSloc2.Items.Clear();
                            for (int J = 0; J < lbSloc.Items.Count; J++)
                            {
                                MAT = lbSloc.Items[J].Text;
                                if (MAT!="ALL")
                                {
                                    lbSloc2.Items.Add(MAT);
                                }                                
                            }
                        }

                        else
                        {
                            lbSloc2.Items.Add(stritem);
                        }


                    }
                }
            }
        }

        protected void btnDiselectmat_Click(object sender, EventArgs e)
        {
            int CON = lbSloc2.Items.Count;
            for (int i = 0; i < CON; i++)
            {
                if (lbSloc2.Items[i].Selected)
                {
                    lbSloc2.Items.Remove(lbSloc2.Items[i]);
                    CON = CON - 1;
                    i = i - 1;
                }
            }
        }

        
    }
}