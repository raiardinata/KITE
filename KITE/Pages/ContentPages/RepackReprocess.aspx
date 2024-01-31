<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="RepackReprocess.aspx.cs" Inherits="KITE.Pages.ContentPages.RepackReprocess" %>

<asp:Content ID="repackReprocessContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery-3.7.1.min.js"></script>
    <script type="text/javascript">
        function GetRMUsageData() {
            var fgTextBoxID = '<%= fgTextBoxTop.ClientID %>';
            var batchTextBoxID = '<%= batchTextBox.ClientID %>';
            var currentUrl = window.location.href;
            $.ajax({
                type: "POST",
                url: currentUrl + "/GetRepackandReprocessData",
                data: '{fgText: "' + fgTextBoxID + '", batchText: "' + batchTextBoxID + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // Display suggestions in the dropdown
                    showSuggestions(data.d);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    </script>
    <span style="position: relative; background-color: white; z-index: 1; font-weight: bold; padding: 0px 3px 0px 3px; top: 10px; left: 3px;">Repack / Reprocess</span>
    <span style="position: relative; background-color: white; z-index: 1; font-weight: bold; padding: 0px 3px 0px 3px; top: 10px; left: 205px;">Repack / Reprocess To</span>
    <table style="z-index: 2;">
        <tr>
            <td style="vertical-align:top;">
                <table style="padding-left: 10px; min-width: 250px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px   #7bfdf6 ; padding-bottom: 10px; margin-bottom: 5px; margin-right: 10px;">
                    <tr style="border-right: 1px solid #ccc; border-top: 1px solid #ccc; border-left: 1px solid #ccc; height: 37px; vertical-align: bottom;">
                        <td style="width: 100px; text-align: right;">
                            <asp:Label runat="server" ID="fgLableTop" Text="Finish Goods" />
                        </td>
                        <td style="padding: 0px 5px 0px 5px;">
                            <asp:TextBox runat="server" ID="fgTextBoxTop" AutoPostBack="True" OnTextChanged="rmUsageTextBox_TextChanged" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 40px;">
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="batchLable" Text="Batch" />
                        </td>
                        <td style="padding: 0px 5px 0px 5px;">
                            <asp:TextBox runat="server" ID="batchTextBox" AutoPostBack="True" OnTextChanged="rmUsageTextBox_TextChanged" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 25px;">
                        <td colspan="2" style="padding: 0px 5px 0px 5px;">
                            <asp:Label runat="server" ID="qtyGRFGLable" Text="Qty. GR FG :" />
                            <asp:Label runat="server" ID="qtyGRFGResLable" Text="0" />
                            <asp:Label runat="server" ID="dumLable1" Text=" KG" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top; position: relative; top: 25px;">
                <span class="glyphicon glyphicon-arrow-right" style="font-size: 45px; margin-right: 5px; color: darkslategrey;"></span>
            </td>
            <td style="vertical-align: top;">
                <table style="padding-left: 10px; min-width: 250px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px  #7bfdf6; padding-bottom: 10px; margin-bottom: 5px;">
                    <tr style="border-right: 1px solid #ccc; border-top: 1px solid #ccc; border-left: 1px solid #ccc; height: 37px; vertical-align: bottom;">
                        <td style="width: 100px; text-align: right;">
                            <asp:Label runat="server" ID="fgLableBottom" Text="Finish Goods" />
                        </td>
                        <td style="padding: 0px 5px 0px 5px;">
                            <asp:TextBox runat="server" ID="fgTextBoxBottom" AutoPostBack="True" OnTextChanged="repackTextBoxBottom_TextChanged" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 40px;">
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="batchLableBottom" Text="Batch" />
                        </td>
                        <td style="padding: 0px 5px 0px 5px;">
                            <asp:TextBox runat="server" ID="batchTextBoxBottom" AutoPostBack="True" OnTextChanged="repackTextBoxBottom_TextChanged" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 25px;">
                        <td style="text-align: right;">
                            <asp:Label runat="server" ID="qtyLable" Text="Qty." />
                        </td>
                        <td style="padding: 0px 5px 0px 5px;">
                            <asp:TextBox runat="server" ID="qtyTextBox" AutoPostBack="True" OnTextChanged="repackTextBoxBottom_TextChanged" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 25px;">
                        <td colspan="2" style="padding: 0px 5px 0px 25px;">
                            <asp:Label runat="server" ID="sOneQtyLable" Text="121001071 Qty. : " />
                            <asp:Label runat="server" ID="sOneQty" Text=" 0" />
                            <asp:Label runat="server" ID="dumLable2" Text=" KG" />
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 25px;">
                        <td colspan="2" style="padding: 0px 5px 0px 15px;">
                            <asp:Label runat="server" ID="sOneBatchLable" Text="121001071 Batch : " />
                            <asp:Label runat="server" ID="sOneBatch" Text="-" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 25px;">
                        <td colspan="2" style="padding: 0px 5px 0px 25px;">
                            <asp:Label runat="server" ID="sTwoQtyLable" Text="121001072 Qty. : " />
                            <asp:Label runat="server" ID="sTwoQty" Text=" 0" />
                            <asp:Label runat="server" ID="dumLable3" Text=" KG" />
                        </td>
                    </tr>
                    <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 25px;">
                        <td colspan="2" style="padding: 0px 5px 0px 15px;">
                            <asp:Label runat="server" ID="sTwoBatchLable" Text="121001072 Batch : " />
                            <asp:Label runat="server" ID="sTwoBatch" Text="-" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label ID="errorLabel" runat="server" Enabled="false" Style="color: red; position: relative; width: auto; height: auto;"></asp:Label>

    <asp:Label runat="server" ID="rmUsageLable" Text="RM Usage" />
    <asp:ScriptManager ID="rmUsageManager" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
    <asp:UpdatePanel ID="rmUsageUpdatePanel" runat="server">
        <ContentTemplate>

            <div class="rounded-corners">
                <asp:Panel ID="rmUsagePanel" runat="Server" ScrollBars="Horizontal">
                    <asp:GridView class="GridView" ID="rmUsageDataGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowDataBound="rmUsageDataGridView_RowDataBound" OnPageIndexChanging="rmUsageDataGridView_PageIndexChanging">
                        <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                        <RowStyle CssClass="NoWarp" />
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="rmUsagelblSequenceNo" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" />
                            <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" />
                            <asp:BoundField DataField="Total_RM_Qty" SortExpression="Total_RM_Qty" HeaderText="Total RM Qty" />
                            <asp:BoundField DataField="Qty_RM_Batch" SortExpression="Qty_RM_Batch" HeaderText="Qty RM Batch" />
                            <asp:BoundField DataField="Distribution_Qty" SortExpression="Distribution_Qty" HeaderText="Distribution Qty" />
                            <asp:BoundField DataField="Remaining_Qty" SortExpression="Remaining_Qty" HeaderText="Remaining Qty" />
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                        <PagerStyle CssClass="custom-pager"/>
                    </asp:GridView>
                </asp:Panel>

                <div id="dataPager" runat="server" class="pagerstyle">
                    <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                    <asp:Label ID="lblShowRows" runat="server" Text="Line of rows:" />
                    <asp:DropDownList ID="rmUsagePageSizeDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rmUsagePageSizeDropDown_SelectedIndexChanged">
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="50" Value="50" />
                        <asp:ListItem Text="100" Value="100" />
                        <asp:ListItem Text="250" Value="250" />
                        <asp:ListItem Text="1000" Value="1000" />
                    </asp:DropDownList>
                    &nbsp; Page
                    <asp:TextBox ID="rmUsagetxtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="rmUsageGoToPage_TextChanged"
                        CssClass="gotopage" Width="30px" />
                    of
                    <asp:Label ID="rmUsagelblTotalNumberOfPages" runat="server" />
                    &nbsp;
                    <asp:Button ID="btnPrev" runat="server" Text="<" ToolTip="Previous Page" OnClick="rmUsagebtnPrev_OnClick" />
                    <asp:Button ID="btnNext" runat="server" Text=">" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                        OnClick="rmUsagebtnNext_OnClick" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save Repack / Reprocess" class="btn btn-primary" style="display: inline-block; margin-right: 0px; float: right;padding-right: 12px;margin-top: 10px;" />

</asp:Content>
