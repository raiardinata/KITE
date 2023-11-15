<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="McFrameUpload.aspx.cs" Inherits="KITE.Pages.ContentPages.McFrameUpload" %>
<asp:Content ID="CsvBodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                    <span style="position: absolute; top: -12px; left: 8px; background-color: white; z-index: 1; font-weight: bold; padding:0px 3px 0px 3px;">McFrane Csv Upload</span>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" OnClick="btnSubmit_Click" style="display: inline-block; margin-right: 10px;" />
                    <asp:FileUpload ID="fileUpload" runat="server" style="display: inline-block; position: relative; top: 1px;" />
                </td>
            </tr>
        </table>
        <asp:Label ID="errorLabel" runat="server" Enabled="false" style="color: red; position:relative; width:auto; height:auto;"></asp:Label>
        <asp:CheckBox ID="forcePushData" runat="server" Text="Overide data sebelumnya" /><br />
        <asp:Label ID="checkDataWarning" runat="server" Text="Pastikan untuk melakukan validasi ulang terhadap data yang di upload." />

        <asp:ScriptManager ID="CsvScriptManager" runat="server">
            <Scripts>
            </Scripts>
        </asp:ScriptManager>
        <asp:UpdatePanel ID="CsvUpdatePanel" runat="server">
            <ContentTemplate>

                <div class="rounded-corners">
                    <asp:Panel ID="CsvPanel" runat="Server" ScrollBars="Horizontal">
                        <asp:GridView class="GridView" ID="CsvDataGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowDataBound="CsvDataGridView_RowDataBound" OnPageIndexChanging="CsvDataGridView_PageIndexChanging">
                            <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                            <RowStyle CssClass="NoWarp" />
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSequenceNo" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Calc_No" HeaderText="Calc. No." SortExpression="Calc_No" />
                                <asp:BoundField DataField="Mgmt_dept_CD" HeaderText="Mgmt. Dept. CD" SortExpression="Mgmt_dept_CD" />
                                <asp:BoundField DataField="Management_Dept_Name" HeaderText="Mgmt. Dept. Name" SortExpression="Management_Dept_Name" />
                                <asp:BoundField DataField="YM" HeaderText="YM" SortExpression="YM" DataFormatString="{0:yyyy/MM/dd}" />
                                <asp:BoundField DataField="Lvl" HeaderText="LVL" SortExpression="Lvl" />
                                <asp:BoundField DataField="Target_item_CD" HeaderText="Target Item CD" SortExpression="Target_item_CD" />
                                <asp:BoundField DataField="Item_CD" HeaderText="Item CD" SortExpression="Item_CD" />
                                <asp:BoundField DataField="Item_name" HeaderText="Item Name" SortExpression="Item_name" />
                                <asp:BoundField DataField="Item_type_name" HeaderText="Item Type Name" SortExpression="Item_type_name" />
                                <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                                <asp:BoundField DataField="Quantity" HeaderText="Qty." SortExpression="Quantity" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STD_Qty" HeaderText="[STD]Qty." SortExpression="STD_Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Kilos_Convertion" HeaderText="Kilos Convertion" SortExpression="Kilos_Convertion" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Variable_Cost" HeaderText="Variable Cost" SortExpression="Variable_Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STD_Variable_Cost" HeaderText="[STD]Variable Cost" SortExpression="STD_Variable_Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Labour_Cost" HeaderText="Labour Cost" SortExpression="Labour_Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STD_Labour_Cost" HeaderText="[STD]Labour Cost" SortExpression="STD_Labour_Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Depreciation" HeaderText="Depreciation" SortExpression="Depreciation" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STD_Depreciation" HeaderText="[STD]Depreciation" SortExpression="STD_Depreciation" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Repair_Maintenance" SortExpression="Repair_Maintenance" HeaderText="[STD]Depreciation" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STD_Repair_Maintenance" SortExpression="STD_Repair_Maintenance" HeaderText="[STD]Repair Maintenance" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Overhead_Cost" SortExpression="Overhead_Cost" HeaderText="Overhead Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STD_Overhead_Cost" SortExpression="STD_Overhead_Cost" HeaderText="[STD]Overhead Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Retur_Cost" SortExpression="Retur_Cost" HeaderText="Retur Cost" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                            <PagerStyle CssClass="custom-pager"/>
                        </asp:GridView>
                    </asp:Panel>
            
                    <div id="dataPager" runat="server" class="pagerstyle">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                        <asp:Label ID="lblShowRows" runat="server" Text="Line of rows:" />
                        <asp:DropDownList ID="CsvPageSizeDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CsvPageSizeDropDown_SelectedIndexChanged">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                            <asp:ListItem Text="250" Value="250" />
                            <asp:ListItem Text="1000" Value="1000" />
                        </asp:DropDownList>
                        &nbsp; Page
                        <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="GoToPage_TextChanged"
                            CssClass="gotopage" Width="30px" />
                        of
                        <asp:Label ID="lblTotalNumberOfPages" runat="server" />
                        &nbsp;
                        <asp:Button ID="btnPrev" runat="server" Text="<" ToolTip="Previous Page" OnClick="btnPrev_OnClick" />
                        <asp:Button ID="btnNext" runat="server" Text=">" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                            OnClick="btnNext_OnClick" />
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Enabled="false" Text="Upload" class="btn btn-primary" style="display: inline-block; margin-right: 0px; float: right;padding-right: 12px;margin-top: 10px;" />
    </div>
</asp:Content>
