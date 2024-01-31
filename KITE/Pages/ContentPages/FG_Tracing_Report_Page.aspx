<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="FG_Tracing_Report_Page.aspx.cs" Inherits="KITE.Pages.ContentPages.FG_Tracing_Report_Page" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="FG_Tracing_Report" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
    <table style="padding-left: 10px; min-width: 1170px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #7bfdf6; padding-bottom: 10px; margin-bottom: 10px">
        <tr style="border: 1px solid #ccc;">
            <td style="padding: 10px 5px 10px 20px; position: relative;">
                <span style="position: absolute; top: -12px; left: 8px; background-color: white; z-index: 1; font-weight: bold; padding: 0px 3px 0px 3px;">FG Tracing Report</span>

                <table style="padding-left: 10px; min-width: 1170px; vertical-align: top; padding-top: 5px; padding-bottom: 10px; margin-bottom: 10px">
                    <tr>
                        <td style="width: 60px; text-align: right;">
                            <asp:Label runat="server" Text="No. PIB" Style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td style="width: 90px; padding-right: 10px;">
                            <asp:TextBox ID="noPIBTxt" runat="server" Style="display: inline-block; position: relative; top: 5px;" />
                        </td>

                        <td style="width: 75px; text-align: right;">
                            <asp:Label runat="server" Text="Invoice No." Style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td style="width: 70px; padding-right: 10px;">
                            <asp:TextBox ID="invoiceNoTxt" runat="server" Style="display: inline-block; position: relative; top: 5px;" />
                        </td>

                        <td style="width: 100px; text-align: right;">
                            <asp:Label runat="server" Text="PGI Date From" Style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:TextBox ID="pgiDateFrom" runat="server" Style="display: inline-block; position: relative; top: 5px;" TextMode="Date" />--%>
                            <asp:TextBox ID="pgiDateFrom" type="date" runat="server" Style="display: inline-block; position: relative; top: 5px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label runat="server" Text="No. PEB" Style="display: inline-block; position: relative; top: 5px; text-align: right;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="noPEBTxt" runat="server" Style="display: inline-block; position: relative; top: 5px;" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" Text="Pembeli" Style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="customerTxt" runat="server" Style="display: inline-block; position: relative; top: 5px;" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Label runat="server" Text="PGI Date Until" Style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="pgiDateUntil" type="date" runat="server" Style="display: inline-block; position: relative; top: 5px;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="errorLabel" runat="server" Enabled="false" Style="color: red; position: relative; width: auto; height: auto;"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="generateReportBtn" runat="server" Text="Generate Report" class="btn btn-primary" OnClick="GenerateReport" Style="display: inline-block; margin-right: 10px;" />
            </td>
        </tr>
    </table>
    <div class="rounded-corners" style="position: relative; width: 102.2%;">
        <rsweb:ReportViewer ID="FG_Tracing_ReportViewer" runat="server" ProcessingMode="Remote" Style="width: auto;">
            <%--<ServerReport ReportPath="/KITE_Report/KITE_FG_Tracing" ReportServerUrl="http://127.0.0.1/ReportServer" />--%>
        </rsweb:ReportViewer>
    </div>
</asp:Content>
