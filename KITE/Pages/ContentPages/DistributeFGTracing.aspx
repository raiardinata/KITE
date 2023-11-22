<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DistributeFGTracing.aspx.cs" Inherits="KITE.Pages.ContentPages.DistributeFGTracing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showWarning() {
            alert('Semua data FG Tracing periode yang dipilih akan didelete dan generate ulang.');
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

        <asp:ScriptManager ID="FGTracingScriptManager" runat="server">
            <Scripts>
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdatePanel ID="FGTracingUpdatePanel" runat="server">
            <ContentTemplate>

                <asp:Label runat="server" ID="FGTracingLbl" Text="FG Tracing Data"></asp:Label>
                <div class="rounded-corners">
                    <asp:Panel ID="FGTracingPanel" runat="Server" ScrollBars="Horizontal">
                        <asp:GridView class="GridView" ID="FGTracingGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowDataBound="FGTracingGridView_RowDataBound" OnPageIndexChanging="FGTracingGridView_PageIndexChanging">
                            <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                            <RowStyle CssClass="NoWarp" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="FGTracinglblSequenceNo" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Year_Period" SortExpression="Year_Period" HeaderText="Year Period" />
                                <asp:BoundField DataField="Month_Period" SortExpression="Month_Period" HeaderText="Month Period" />
                                <asp:BoundField DataField="SO" SortExpression="SO" HeaderText="SO" />
                                <asp:BoundField DataField="Finish_Goods" SortExpression="Finish_Goods" HeaderText="Finish Goods"  />
                                <asp:BoundField DataField="FG_Name" SortExpression="FG_Name" HeaderText="FG Name" />
                                <asp:BoundField DataField="FG_Batch" SortExpression="FG_Batch" HeaderText="FG Batch" />
                                <asp:BoundField DataField="Qty_Sales" SortExpression="Qty_Sales" HeaderText="Qty Sales" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UoM" SortExpression="UoM" HeaderText="UoM" />
                                <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" />
                                <asp:BoundField DataField="Material_Description" SortExpression="Material_Description" HeaderText="Material Description" />
                                <asp:BoundField DataField="Total_RM_Qty" SortExpression="Total_RM_Qty" HeaderText="Total RM Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RM_Batch_Sequence" SortExpression="RM_Batch_Sequence" HeaderText="RM Batch Sequence" />
                                <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" />
                                <asp:BoundField DataField="RM_Qty" SortExpression="RM_Qty" HeaderText="RM Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PIB_Date" SortExpression="PIB_Date" HeaderText="PIB Date" DataFormatString="{0:yyyy/MM/dd}" />
                                <asp:BoundField DataField="Customer" SortExpression="Customer" HeaderText="Customer" />
                                <asp:BoundField DataField="Name_1" SortExpression="Name_1" HeaderText="Name_1" />
                                <asp:BoundField DataField="NO_PEB" SortExpression="NO_PEB" HeaderText="NO_PEB" />
                                <asp:BoundField DataField="PEB_Date" SortExpression="PEB_Date" HeaderText="PEB Date" DataFormatString="{0:yyyy/MM/dd}" />
                                <asp:BoundField DataField="PO_Number" SortExpression="PO_Number" HeaderText="PO Number" />
                                <asp:BoundField DataField="Country_Destination" SortExpression="Country_Destination" HeaderText="Country Destination" />
                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" />
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                            <PagerStyle CssClass="custom-pager"/>
                        </asp:GridView>
                    </asp:Panel>

                    <div id="FGTracingdataPager" runat="server" class="pagerstyle">
                        <asp:Label ID="FGTlblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                        <asp:Label ID="FGTlblShowRows" runat="server" Text="Line of rows:" />
                        <asp:DropDownList ID="FGTracingPageSizeDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FGTracingPageSizeDropDown_SelectedIndexChanged">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                            <asp:ListItem Text="250" Value="250" />
                            <asp:ListItem Text="1000" Value="1000" />
                        </asp:DropDownList>
                        &nbsp; Page
                        <asp:TextBox ID="FGTracingGoToPageTxt" runat="server" AutoPostBack="true" OnTextChanged="FGTracingGoToPage_TextChanged"
                            CssClass="gotopage" Width="30px" />
                        of
                        <asp:Label ID="FGTracinglblTotalNumberOfPages" runat="server" />
                        &nbsp;
                        <asp:Button ID="FGTracingbtnPrev" runat="server" Text="<" ToolTip="Previous Page" OnClick="FGTracingbtnPrev_OnClick" />
                        <asp:Button ID="FGTracingbtnNext" runat="server" Text=">" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                            OnClick="FGTracingbtnNext_OnClick" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnDownloadCsv" runat="server" OnClick="btnDownloadToCsv" Enabled="false" Text="Download Csv" class="btn btn-primary" style="display: inline-block; margin-right: 0px; float: right;padding-right: 12px;margin-top: 10px;" />
    </div>
</asp:Content>
