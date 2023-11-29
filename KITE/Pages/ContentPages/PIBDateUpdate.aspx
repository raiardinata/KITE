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
                    <span style="position: absolute; top: -12px; left: 8px; background-color: white; z-index: 1; font-weight: bold; padding:0px 3px 0px 3px;">Master Batch Page</span>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" style="display: inline-block; margin-right: 10px;" OnClick="SubmitData" />
                    <asp:Label runat="server" ID="rmLbl" Text="Raw Material : "></asp:Label>
                    <asp:TextBox ID="rmTxt" runat="server" style="display: inline-block; position: relative; top: 1px;"/>
                    <asp:Label runat="server" ID="batchLbl" Text="Batch : "></asp:Label>
                    <asp:TextBox ID="batchTxt" runat="server" style="display: inline-block; position: relative; top: 1px;"/>
                </td>
            </tr>
        </table>
        <asp:Label ID="errorLabel" runat="server" Enabled="false" style="color: red; position:relative; width:auto; height:auto;"></asp:Label>

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
                                    <ItemTemplate>
                                        <asp:Label ID="lblSequenceNo" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Raw_Material" SortExpression="Raw_Material" HeaderText="Raw Material" ItemStyle-Width="225px" />
                                <asp:BoundField DataField="RM_Batch" SortExpression="RM_Batch" HeaderText="RM Batch" ItemStyle-Width="225px" />
                                <asp:BoundField DataField="PIB_Date" SortExpression="PIB_Date" HeaderText="PIB Date" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="225px" />
                                <asp:TemplateField Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="UUIDLbl" Text='<%# Eval("UUID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Change PIB Date">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" type="date" ID="TextBoxPIBDate" CssClass="pib-date-textbox" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <HeaderStyle HorizontalAlign="Center" />
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
                                <asp:Button ID ="CloseButton" runat="server" class="btn btn-primary" Text="Close" BorderStyle="Solid" onclick="refreshPage"  />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
