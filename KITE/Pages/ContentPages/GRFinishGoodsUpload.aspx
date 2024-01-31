<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="GRFinishGoodsUpload.aspx.cs" Inherits="KITE.Pages.ContentPages.GRFinishGoodsUpload" %>
<asp:Content ID="GRFinishGoodsUploadPage" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .custom-pager {
            display: none;
        }
    </style>
    <div>
        <table>
            <tr>
                <td style="padding: 5px; max-width: 240px;">
                    <span style="position: relative; top: 10px; left: 3px; background-color: white; z-index: 1; font-weight: bold; padding: 0px 3px 0px 3px;">GR Finish Goods Check Data</span>
                    <table style="padding-left: 10px; width: 220px; min-height: 50px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #7bfdf6; padding-bottom: 10px; margin-bottom: 10px">
                        <tr style="border-right: 1px solid #ccc; border-top: 1px solid #ccc; border-left: 1px solid #ccc; height: 37px; vertical-align: bottom;">
                            <td style="width: 110px; text-align: right;">
                                <asp:Label runat="server" ID="yearPeriodLbl" Text="Year Period " Style="padding: 0px 5px 0px 15px;" />
                            </td>
                            <td>
                                <asp:TextBox ID="yearPeriodTxt" runat="server" Width="100px" Style="padding: 0px 5px 0px 15px;" />
                            </td>
                        </tr>
                        <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 37px; vertical-align: bottom;">
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="monthPeriodLbl" Text="Month Period " Style="padding: 0px 5px 0px 15px;" />
                            </td>
                            <td>
                                <asp:TextBox ID="monthPeriodTxt" runat="server" Width="100px" Style="padding: 0px 5px 0px 15px;" />
                            </td>
                        </tr>
                        <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 50px;">
                            <td colspan="2" style="text-align: right;">
                                <asp:Button ID="viewButton" runat="server" Text="View" class="btn btn-primary" OnClick="btnView" Style="display: inline-block; margin-right: 10px;" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top; width: 100%;">
                    <span style="position: relative; top: 10px; left: 3px; background-color: white; z-index: 1; font-weight: bold; padding: 0px 3px 0px 3px;">GR Finish Goods Csv Upload</span>
                    <table style="padding-left: 10px; width: 500px; min-height: 50px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #7bfdf6; padding-bottom: 10px; margin-bottom: 10px">
                        <tr style="border-right: 1px solid #ccc; border-top: 1px solid #ccc; border-left: 1px solid #ccc; height: 37px; vertical-align: bottom;">
                            <td style="padding: 10px 5px 10px 20px; position: relative;">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" OnClick="btnSubmit_Click" Style="display: inline-block; margin-right: 10px;" />
                                <asp:FileUpload ID="fileUpload" runat="server" Style="display: inline-block; position: relative; top: 1px;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="errorLabel" runat="server" Enabled="false" Style="color: red; position: relative; width: auto; height: auto;"></asp:Label>
                    <asp:CheckBox ID="forcePushData" runat="server" Text="Overide data sebelumnya" /><br />
                    <asp:Label ID="checkDataWarning" runat="server" Text="Pastikan untuk melakukan validasi ulang terhadap data yang di upload." />
                </td>
            </tr>
        </table>

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
                                <asp:BoundField DataField="Posting_Date" SortExpression="Posting_Date" HeaderText="Posting Date" />
                                <asp:BoundField DataField="Document_Date" SortExpression="Document_Date" HeaderText="Document Date" />
                                <asp:BoundField DataField="Document_Header_Text" SortExpression="Document_Header_Text" HeaderText="Document Header Text" />
                                <asp:BoundField DataField="Material" SortExpression="Material" HeaderText="Material" />
                                <asp:BoundField DataField="Material_Description" SortExpression="Material_Description" HeaderText="Material Description" />
                                <asp:BoundField DataField="Plant" SortExpression="Plant" HeaderText="Plant" />
                                <asp:BoundField DataField="Storage_Location" SortExpression="Storage_Location" HeaderText="Storage Location" />
                                <asp:BoundField DataField="Movement_Type" SortExpression="Movement_Type" HeaderText="Movement Type" />
                                <asp:BoundField DataField="Material_Document" SortExpression="Material_Document" HeaderText="Material Document" />
                                <asp:BoundField DataField="Batch" SortExpression="Batch" HeaderText="Batch" />
                                <asp:BoundField DataField="Qty_in_Un_of_Entry" SortExpression="Qty_in_Un_of_Entry" HeaderText="Qty in Un. of Entry" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Kilos_Convertion" SortExpression="Kilos_Convertion" HeaderText="Kilos Convertion" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Unit_of_Entry" SortExpression="Unit_of_Entry" HeaderText="Unit of Entry" />
                                <asp:BoundField DataField="Entry_Date" SortExpression="Entry_Date" HeaderText="Entry Date" />
                                <asp:BoundField DataField="Time_of_Entry" SortExpression="Time_of_Entry" HeaderText="Time of Entry" />
                                <asp:BoundField DataField="User_name" SortExpression="User_name" HeaderText="User name" />
                                <asp:BoundField DataField="Base_Unit_of_Measure" SortExpression="Base_Unit_of_Measure" HeaderText="Base Unit of Measure" />
                                <asp:BoundField DataField="Quantity" SortExpression="Quantity" HeaderText="Quantity" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount_in_LC" SortExpression="Amount_in_LC" HeaderText="Amount in LC" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Goods_recipient" SortExpression="Goods_recipient" HeaderText="Goods recipient" />
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
        <asp:Button ID="btnDownloadCsv" runat="server" OnClick="btnDownloadToCsv" Enabled="false" Text="Download Csv" class="btn btn-primary" style="display: inline-block; margin-right: 10px; float: right;padding-right: 12px;margin-top: 10px;" />
    </div>
</asp:Content>
