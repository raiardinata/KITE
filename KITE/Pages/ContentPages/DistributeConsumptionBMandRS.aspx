<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DistributeConsumptionBMandRS.aspx.cs" Inherits="KITE.Pages.ContentPages.DistributeConsumptionBMandRS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BMandRSCalculate" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .custom-pager {
            display: none;
        }

        .modalBackground {
            background-color: #000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: #fff;
            border: 1px solid #ccc;
            padding: 10px;
            box-shadow: 0 0 5px #999;
            border-radius:15px;
            color:#000;
            min-width:400px;

            display: flex;
            justify-content: center;
            align-items: center;
            height: 20vh;

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
                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate BM & RS" class="btn btn-primary" style="display: inline-block; margin-right: 10px;" />
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
                                <asp:BoundField DataField="FG_Qty" SortExpression="FG_Qty" HeaderText="FG Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" />
                                <asp:BoundField DataField="Total_RM_Qty" SortExpression="Total_RM_Qty" HeaderText="Total RM Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Batch_Sequence" SortExpression="Batch_Sequence" HeaderText="Batch Sequence" />
                                <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" />
                                <asp:BoundField DataField="RM_Qty" SortExpression="RM_Qty" HeaderText="RM Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Distribution_Qty" SortExpression="Distribution_Qty" HeaderText="Distribution Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Remaining_Qty" SortExpression="Remaining_Qty" HeaderText="Remaining Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
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
                                <asp:BoundField DataField="FG_Qty" SortExpression="FG_Qty" HeaderText="FG Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FG_Batch" SortExpression="FG_Batch" HeaderText="FG Batch" />
                                <asp:BoundField DataField="FG_Batch_Qty" SortExpression="FG_Batch_Qty" HeaderText="FG Batch Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" />
                                <asp:BoundField DataField="Total_RM_Qty" SortExpression="Total_RM_Qty" HeaderText="Total RM Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RM_Batch_Sequence" SortExpression="RM_Batch_Sequence" HeaderText="RM Batch Sequence" />
                                <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" />
                                <asp:BoundField DataField="Qty_RM_Batch" SortExpression="Qty_RM_Batch" HeaderText="Qty RM Batch" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Distribution_Qty" SortExpression="Distribution_Qty" HeaderText="Distribution Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Remaining_Qty" SortExpression="Remaining_Qty" HeaderText="Remaining Qty" DataFormatString="{0:N4}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
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
        <asp:Button ID="btnDownloadCsv" runat="server" OnClick="btnDownloadToCsv" Enabled="false" Text="Download Csv" class="btn btn-primary" style="display: inline-block; margin-right: 0px; float: right;padding-right: 12px;margin-top: 10px;" />
    </div>

    <ajaxToolkit:ModalPopupExtender ID="confirmationPopUp" runat="server"
        TargetControlID="btnCalculate"
        PopupControlID="ModalPanel"
        CancelControlID="CloseButton"
        BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="ModalPanel" runat="server" CssClass="modalPopup">
        <table>
            <tr">
                <td>
                    <p style="padding-right: 10px;">Semua data RM per Batch, FG per Batch dan FG Tracing Calculation periode yang dipilih akan didelete dan generate ulang.</p>
                </td>
            </tr>
            <tr>
                <td style="display: flex; justify-content: center; align-items: center;">
                    <asp:Button ID="confirmButton" OnClick="btnCalculate_Click" class="btn btn-secondary" runat="server" Text="Yes" BorderStyle="Solid" />
                    <asp:Button ID="CloseButton" runat="server" class="btn btn-primary" Text="No" BorderStyle="Solid" />
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
