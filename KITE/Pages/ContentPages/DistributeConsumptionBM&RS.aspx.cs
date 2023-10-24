using KITE.Models;
using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace KITE.Pages.ContentPages
{
    public partial class DistributeConsumptionBM_RS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public async Task btnCalculate_Click()
        {
            string[] selectColumn = new string[] {
                " Year_Period,Month_Period,Target_item_CD,Item_CD,Unit,SUM(Quantity) SumQuantity",
                " Year_Period,Month_Period,Posting_Date,Material,Plant,Storage_Location,Movement_Type,Batch,SUM(Qty_in_Un_of_Entry) Sum_QtyInUnOfEntry,Unit_of_Entry,Base_Unit_of_Measure,SUM(Quantity) Sum_Qty,Material_Document "
            };
            string[] tableName = new string[] { " McFrame_Cost_Table ", " GI_Raw_Material " };
            string[] condition = new string[] {
                " Item_CD IN ('121001071','121001072') AND Target_item_CD LIKE '124%' GROUP BY Year_Period , Month_Period ,Target_item_CD, Item_CD, Unit ",
                " Material = '121001071' GROUP BY Year_Period, Month_Period, Material, Posting_Date, Material_Document, Plant, Storage_Location, Movement_Type, Batch, Unit_of_Entry, Base_Unit_of_Measure ORDER BY Material, Posting_Date ASC " };

            // Get McFrame data, make it async
            Tuple<DataTable, Exception> McFrameDataTable = await new DistributeConsumptionBM_RSFunctionModel().GetDistributedMcFrameData(selectColumn[0], tableName[0], condition[0], ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            // Get GI Raw Material data, make it async
            Tuple<DataTable, Exception> GIRawMaterialDataTable = await new DistributeConsumptionBM_RSFunctionModel().GetDistributedGIRawMaterialData(selectColumn[1], tableName[1], condition[1], ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        }
    }
}