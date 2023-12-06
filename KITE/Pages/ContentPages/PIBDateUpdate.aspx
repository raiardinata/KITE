<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PIBDateUpdate.aspx.cs" Inherits="KITE.Pages.ContentPages.PIBDateUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
            border-radius: 15px;
            color: #000;
            min-width: 400px;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 20vh;
        }
    </style>
    <div>
        <table>
            <tr>
                <td style="padding: 5px; width: 310px;">
                    <span style="position: relative; background-color: white; z-index: 1; font-weight: bold; padding: 0px 3px 0px 3px; top: 10px; left: 3px;">Master Batch</span>
                    <table style="padding-left: 10px; min-width: max-content; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
                        <tr style="border-right: 1px solid #ccc; border-top: 1px solid #ccc; border-left: 1px solid #ccc; height: 37px; vertical-align: bottom;">
                            <td style="width: 100px; text-align: right;">
                                <asp:Label runat="server" ID="yearPeriodLbl" Text="Year Period " Style="padding: 0px 5px 0px 10px;" />
                            </td>
                            <td>
                                <asp:TextBox ID="rmTxt" runat="server" Style="display: inline-block; position: relative; top: 1px; margin-right: 10px;" />
                            </td>
                        </tr>
                        <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 40px;">
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="batchLbl" Text="Batch " Style="padding: 0px 5px 0px 10px;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="batchTxt" runat="server" Style="display: inline-block; position: relative; top: 1px;" />
                            </td>
                        </tr>
                        <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 40px;">
                            <td></td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkEmptyPIBDate" Text="Empty PIB Date" />
                            </td>
                        </tr>
                        <tr style="border-right: 1px solid #ccc; border-left: 1px solid #ccc; height: 50px;">
                            <td colspan="2" style="text-align: right;">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" Style="display: inline-block; margin-right: 10px;" OnClick="SubmitData" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px;">
                    <asp:ScriptManager ID="PIBScriptManager" runat="server" >
                        <Scripts>
                        </Scripts>
                    </asp:ScriptManager>

                    <asp:UpdatePanel ID="PIBUpdatePanel" runat="server">
                        <ContentTemplate>

                            <div class="rounded-corners">
                                <asp:Panel ID="PIBPanel" runat="Server" ScrollBars="Horizontal">
                                    <asp:GridView class="GridView" ID="PIBDataGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnRowCommand="PIBDataGridView_RowCommand" OnRowDataBound="PIBDataGridView_RowDataBound" OnPageIndexChanging="PIBDataGridView_PageIndexChanging">
                                        <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                                        <RowStyle CssClass="NoWarp" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSequenceNo" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" ItemStyle-Width="225px" />
                                            <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" ItemStyle-Width="225px" />
                                            <asp:BoundField DataField="PIB_Date" SortExpression="PIB_Date" HeaderText="PIB Date" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="225px" />
                                            <asp:TemplateField Visible="false">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="UUIDLbl" Text='<%# Eval("UUID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Change PIB Date">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" type="date" ID="TextBoxPIBDate" CssClass="pib-date-textbox" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <HeaderStyle CssClass="text-center" />
                                                <ItemTemplate>
                                                    <asp:Button runat="server" class="btn btn-primary" Text="Update PIB" CommandName="UpdatePIB" CommandArgument='<%# Container.DataItemIndex %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="custom-pager"/>
                                    </asp:GridView>
                                </asp:Panel>

                                <div id="dataPager" runat="server" class="pagerstyle">
                                    <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                                    <asp:Label ID="lblShowRows" runat="server" Text="Line of rows:" />
                                    <asp:DropDownList ID="PIBPageSizeDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PIBPageSizeDropDown_SelectedIndexChanged">
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

                            <asp:Button runat="server" ID="dummyUploadSuccessBtn" Text="Dummy Button" style="display: none;" />

                            <ajaxToolkit:ModalPopupExtender ID="confirmationPopUp" runat="server"
                                TargetControlID="dummyUploadSuccessBtn"
                                PopupControlID="ModalPanel"
                                CancelControlID=""
                                BackgroundCssClass="modalBackground">
                            </ajaxToolkit:ModalPopupExtender>

                            <asp:Panel ID="ModalPanel" runat="server" CssClass="modalPopup">
                                <table>
                                    <tr">
                                        <td>
                                            <asp:Label runat="server" ID="confirmationLable" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="display: flex; justify-content: center; align-items: center;">
                                            <asp:Button ID="CloseButton" runat="server" class="btn btn-primary" Text="Close" BorderStyle="Solid" OnClick="refreshPage" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="errorLabel" runat="server" Enabled="false" Style="color: red; position: relative; width: auto; height: auto;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
