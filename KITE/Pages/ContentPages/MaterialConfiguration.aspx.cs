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
    public partial class MaterialConfiguration : System.Web.UI.Page
    {
        Controllers.KiteController KC = new Controllers.KiteController();

        SQLConnect.SQLConnect sqlconnect = new SQLConnect.SQLConnect();

        Controllers.LoginController login = new Controllers.LoginController();

        protected void Page_Load(object sender, EventArgs e)
        {
            //CheckUser();
            //localip = GETIPADDRESS();

            if (!IsPostBack)
            {
                ListMaterial();
            
            }
        }

        private void ListMaterial()
        {
            lbMaterial.Items.Clear();
            //lbMaterial2.Items.Clear();
            var dt = KC.Select_Material();

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbMaterial.Items.Add(select.MATERIALID.ToString());
                }
                lbMaterial.Items.Add("ALL Material (121)");
                lbMaterial.Items.Add("ALL Finish Goods (124)");
                lbMaterial.Items.Add("ALL Waste (211)");
            }

        }

        private void ListMaterialConf(string menunya)
        {
            //lbMaterial.Items.Clear();
            lbMaterial2.Items.Clear();
            var dt = KC.Select_MaterialConf(menunya);

            if (dt.Count() > 0)
            {
                foreach (var select in dt)
                {
                    lbMaterial2.Items.Add(select.MATERIALID.ToString());
                }
                //lbMaterial.Items.Add("ALL Material (121)");
                //lbMaterial.Items.Add("ALL Finish Goods (124)");
                //lbMaterial.Items.Add("ALL Waste (211)");
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlSearchCriteria.SelectedValue.ToString() != "--Pilih Menu--")
            {
                INSERTMATERIALCONF();
                ddlSearchCriteria.SelectedIndex = 0;
                lbMaterial2.Items.Clear();
                //Labelmenu.Visible = false;
                Labelmenu.Text = "Success Saving.";
            }
        }

        protected void INSERTMATERIALCONF()
        {
            if (lbMaterial2.Items.Count > 0)
            {
               KC.DeleteMaterialConfiguration(ddlSearchCriteria.SelectedValue.ToString());
               for (int i = 0; i < lbMaterial2.Items.Count; i++)
               {
                   KC.InsertMaterialConfiguration(ddlSearchCriteria.SelectedValue.ToString(), lbMaterial2.Items[i].Text, HttpContext.Current.Session["UserAppID"].ToString());
               }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlSearchCriteria.SelectedIndex = -1;
            lbMaterial2.Items.Clear();
            Labelmenu.Visible = false;
        }

        protected void btnSelectmat_Click(object sender, EventArgs e)
        {
            List<int> selecteds = lbMaterial.GetSelectedIndices().ToList();
            string MAT = "";
            bool exists = false;

            for (int i = 0; i < lbMaterial.Items.Count; i++)
            {
                if (lbMaterial.Items[i].Selected)
                {
                    string stritem = lbMaterial.Items[i].Text;
                    if (lbMaterial2.Items.Count > 0)
                    {
                        for (int y = 0; y < lbMaterial2.Items.Count; y++)
                        {
                            if (stritem == lbMaterial2.Items[y].Text)
                            {
                                exists = true;

                            }
                        }
                    }

                    if (!exists)
                    {
                        if (stritem == "ALL Material (121)")
                        {
                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() == "121")
                                {

                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }

                        else if (stritem == "ALL Finish Goods (124)")
                        {

                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() == "124")
                                {
                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }

                        else if (stritem == "ALL Waste (211)")
                        {

                            lbMaterial2.Items.Clear();
                            for (int J = 0; J < lbMaterial.Items.Count; J++)
                            {
                                MAT = lbMaterial.Items[J].Text;
                                if (MAT.Substring(0, 3).ToString() == "211")
                                {
                                    //for (int g = 0; g < lbMaterial2.Items.Count; g++)
                                    //{
                                    //    if (MAT !=
                                    //}
                                    lbMaterial2.Items.Add(MAT);
                                }
                            }
                        }
                        else
                        {
                            //if (lbMaterial2.Items.Count > 0)
                            //{
                            //    for (int J = 0; J < lbMaterial2.Items.Count; J++)
                            //    {
                            //        if (stritem != lbMaterial.Items[J].Text)
                            //        {
                            //            lbMaterial2.Items.Add(stritem);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            lbMaterial2.Items.Add(stritem);
                            //}
                        }


                    }
                }
            }
        }

        protected void btnDiselectmat_Click(object sender, EventArgs e)
        {
            int CON = lbMaterial2.Items.Count;
            for (int i = 0; i < CON; i++)
            {
                if (lbMaterial2.Items[i].Selected)
                {
                    lbMaterial2.Items.Remove(lbMaterial2.Items[i]);
                    CON = CON - 1;
                    i = i - 1;
                }
            }
        }

        protected void ddlSearchCriteria_SelectedIndexChanged1(object sender, EventArgs e)
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
                ListMaterialConf(ddlSearchCriteria.SelectedValue.ToString());
            }
            else
            {
                lbMaterial2.Items.Clear();
                Labelmenu.Visible = false;
            }
        }
    }
}