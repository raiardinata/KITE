<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DistributeConsumptionBMandRS.aspx.cs" Inherits="KITE.Pages.ContentPages.DistributeConsumptionBMandRS" %>
<asp:Content ID="BMandRSCalculate" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showWarning() {
            alert('Semua data RM per Batch dan FG per Batch periode yang dipilih akan didelete dan generate ulang.');
        }
    </script>
    <style>
        .custom-pager {
            display: none;
        }
    </style>
    <div>
        <table style="padding-left: 10px; min-width: 1170px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
            <tr>
                <th style ="font-weight: normal; text-align: left; background-color:white; top:9px; left: 5px; position: relative; width: 150px;">
                </th>
            </tr>
            <tr style="border: 1px solid #ccc;">
                <td style="padding: 10px 5px 10px 20px; position: relative;">
                    <span style="position: absolute; top: -12px; left: 8px; background-color: white; z-index: 1; font-weight: bold; padding:0px 3px 0px 3px;">Distribute Consumption BM & RS</span>
                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate BM & RS" class="btn btn-primary" OnClick="btnCalculate_Click" OnClientClick="if (document.getElementById('MainContent_yearPeriodTxt').value != '' && document.getElementById('MainContent_monthPeriodTxt').value != '') {showWarning();}" style="display: inline-block; margin-right: 10px;" />
                    <asp:Label runat="server" ID="yearPeriodLbl" Text="Year Period : "></asp:Label>
                    <asp:TextBox ID="yearPeriodTxt" runat="server" style="display: inline-block; position: relative; top: 1px;"/>
                    <asp:Label runat="server" ID="monthPeriodLbl" Text="Month Period : "></asp:Label>
                    <asp:TextBox ID="monthPeriodTxt" runat="server" style="display: inline-block; position: relative; top: 1px;"/>
                </td>
            </tr>
        </table>
        <asp:Label ID="errorLabel" runat="server" Enabled="false" style="color: red; position:relative; width:auto; height:auto;"></asp:Label>

        <asp:ScriptManager ID="BMandRSScriptManager" runat="server">
            <Scripts>
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdatePanel ID="BMandRSUpdatePanel" runat="server">
            <ContentTemplate>

                <asp:Label runat="server" ID="RMperBatchLbl" Text="RMperBatch Data"></asp:Label>
                <div class="rounded-corners">
                    <asp:Panel ID="RMperBatchPanel" runat="Server" ScrollBars="Horizontal">
                        <asp:GridView class="GridView" ID="RMperBatchGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowDataBound="RMperBatchGridView_RowDataBound" OnPageIndexChanging="RMperBatchGridView_PageIndexChanging">
                            <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                            <RowStyle CssClass="NoWarp" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="RMperBatchlblSequenceNo" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Year_Period" SortExpression="Year_Period" HeaderText="Year Period" />
                                <asp:BoundField DataField="Month_Period" SortExpression="Month_Period" HeaderText="Month Period" />
                                <asp:BoundField DataField="Finish_Goods" SortExpression="Finish_Goods" HeaderText="Finish Goods" />
                                <asp:BoundField DataField="FG_Qty" SortExpression="FG_Qty" HeaderText="FG Qty" />
                                <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" />
                                <asp:BoundField DataField="Batch_Sequent" SortExpression="Batch_Sequent" HeaderText="Batch Sequent" />
                                <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" />
                                <asp:BoundField DataField="RM_Qty" SortExpression="RM_Qty" HeaderText="RM Qty" />
                                <asp:BoundField DataField="Distribution_Qty" SortExpression="Distribution_Qty" HeaderText="Distribution Qty" />
                                <asp:BoundField DataField="Remaining_Qty" SortExpression="Remaining_Qty" HeaderText="Remaining Qty" />
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                            <PagerStyle CssClass="custom-pager"/>
                        </asp:GridView>
                    </asp:Panel>

                    <div id="RMperBatchdataPager" runat="server" class="pagerstyle">
                        <asp:Label ID="RMlblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                        <asp:Label ID="RMlblShowRows" runat="server" Text="Line of rows:" />
                        <asp:DropDownList ID="RMperBatchPageSizeDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RMperBatchPageSizeDropDown_SelectedIndexChanged">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                            <asp:ListItem Text="250" Value="250" />
                            <asp:ListItem Text="1000" Value="1000" />
                        </asp:DropDownList>
                        &nbsp; Page
                        <asp:TextBox ID="RMperBatchGoToPageTxt" runat="server" AutoPostBack="true" OnTextChanged="RMperBatchGoToPage_TextChanged"
                            CssClass="gotopage" Width="30px" />
                        of
                        <asp:Label ID="RMperBatchlblTotalNumberOfPages" runat="server" />
                        &nbsp;
                        <asp:Button ID="RMperBatchbtnPrev" runat="server" Text="<" ToolTip="Previous Page" OnClick="RMperBatchbtnPrev_OnClick" />
                        <asp:Button ID="RMperBatchbtnNext" runat="server" Text=">" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                            OnClick="RMperBatchbtnNext_OnClick" />
                    </div>
                </div>
                <br />


                <asp:Label runat="server" ID="FGperBatchLbl" Text="FGperBatch Data"></asp:Label>
                <div class="rounded-corners">
                    <asp:Panel ID="FGperBatchPanel" runat="Server" ScrollBars="Horizontal">
                        <asp:GridView class="GridView" ID="FGperBatchGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowDataBound="FGperBatchGridView_RowDataBound" OnPageIndexChanging="FGperBatchGridView_PageIndexChanging">
                            <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                            <RowStyle CssClass="NoWarp" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="FGperBatchlblSequenceNo" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Year_Period" SortExpression="Year_Period" HeaderText="Year Period" />
                                <asp:BoundField DataField="Month_Period" SortExpression="Month_Period" HeaderText="Month Period" />
                                <asp:BoundField DataField="Finish_Goods" SortExpression="Finish_Goods" HeaderText="Finish Goods" />
                                <asp:BoundField DataField="FG_Qty" SortExpression="FG_Qty" HeaderText="FG Qty" />
                                <asp:BoundField DataField="FG_Batch" SortExpression="FG_Batch" HeaderText="FG Batch" />
                                <asp:BoundField DataField="FG_Batch_Qty" SortExpression="FG_Batch_Qty" HeaderText="FG Batch Qty" />
                                <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" />
                                <asp:BoundField DataField="RM_Batch_Sequent" SortExpression="RM_Batch_Sequent" HeaderText="RM Batch Sequent" />
                                <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" />
                                <asp:BoundField DataField="Total_RM_Qty" SortExpression="Total_RM_Qty" HeaderText="Total RM Qty" />
                                <asp:BoundField DataField="Qty_RM_Batch_FG" SortExpression="Qty_RM_Batch_FG" HeaderText="Qty RM Batch FG" />
                                <asp:BoundField DataField="Distribution_Qty" SortExpression="Distribution_Qty" HeaderText="Distribution Qty" />
                                <asp:BoundField DataField="Remaining_Qty" SortExpression="Remaining_Qty" HeaderText="Remaining Qty" />
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                            <PagerStyle CssClass="custom-pager"/>
                        </asp:GridView>
                    </asp:Panel>

                    <div id="FGperBatchdataPager" runat="server" class="pagerstyle">
                        <asp:Label ID="FGlblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                        <asp:Label ID="FGlblShowRows" runat="server" Text="Line of rows:" />
                        <asp:DropDownList ID="FGperBatchPageSizeDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FGperBatchPageSizeDropDown_SelectedIndexChanged">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                            <asp:ListItem Text="250" Value="250" />
                            <asp:ListItem Text="1000" Value="1000" />
                        </asp:DropDownList>
                        &nbsp; Page
                        <asp:TextBox ID="FGperBatchGoToPageTxt" runat="server" AutoPostBack="true" OnTextChanged="FGperBatchGoToPage_TextChanged"
                            CssClass="gotopage" Width="30px" />
                        of
                        <asp:Label ID="FGperBatchlblTotalNumberOfPages" runat="server" />
                        &nbsp;
                        <asp:Button ID="FGperBatchbtnPrev" runat="server" Text="<" ToolTip="Previous Page" OnClick="FGperBatchbtnPrev_OnClick" />
                        <asp:Button ID="FGperBatchbtnNext" runat="server" Text=">" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                            OnClick="FGperBatchbtnNext_OnClick" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
