﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="FG_Tracing_Report_Page.aspx.cs" Inherits="KITE.Pages.ContentPages.FG_Tracing_Report_Page" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="FG_Tracing_Report" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
    <table style="padding-left: 10px; min-width: 1170px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
        <tr>
            <th style ="font-weight: normal; text-align: left; background-color:white; top:9px; left: 5px; position: relative; width: 150px;">
            </th>
        </tr>
        <tr style="border: 1px solid #ccc;">
            <td style="padding: 10px 5px 10px 20px; position: relative;">
                <span style="position: absolute; top: -12px; left: 8px; background-color: white; z-index: 1; font-weight: bold; padding:0px 3px 0px 3px;">FG Tracing Report Page</span>
                
                <table style="padding-left: 10px; min-width: 1170px; vertical-align: top; padding-top: 5px; padding-bottom: 10px; margin-bottom: 10px">
                    <tr>
                        <td style="width: 60px;">
                            <asp:Label runat="server" Text="No. PIB" style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td style="width: 90px; padding-right: 10px;">
                            <asp:TextBox ID="noPIBTxt" runat="server" style="display: inline-block; position: relative; top: 5px;"/>
                        </td>

                        <td style="width: 75px;">
                            <asp:Label runat="server" Text="Invoice No." style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td style="width: 70px; padding-right: 10px;">
                            <asp:TextBox ID="invoiceNoTxt" runat="server" style="display: inline-block; position: relative; top: 5px;"/>
                        </td>

                        <td style="width: 100px;">
                            <asp:Label runat="server" Text="PGI Date From" style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="pgiDateFrom" type="date" runat="server" style="display: inline-block; position: relative; top: 5px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="No. PEB" style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="noPEBTxt" runat="server" style="display: inline-block; position: relative; top: 5px;"/>
                        </td>
                        <td>
                            <asp:Label runat="server" Text="Pembeli" style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="customerTxt" runat="server" style="display: inline-block; position: relative; top: 5px;"/>
                        </td>
                        <td>
                            <asp:Label runat="server" Text="PGI Date Until" style="display: inline-block; position: relative; top: 5px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="pgiDateUntil" type="date" runat="server" style="display: inline-block; position: relative; top: 5px;"/>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Button1" runat="server" Text="Generate Report" class="btn btn-primary" OnClick="GenerateReport" style="display: inline-block; margin-right: 10px;" />
            </td>
        </tr>
    </table>
    <div class="rounded-corners" style="position: relative; width: 102.2%;">
        <rsweb:ReportViewer ID="FG_Tracing_ReportViewer" runat="server" ProcessingMode="Remote" style="width: auto;">
            <%--<ServerReport ReportPath="/KITE_Report/KITE_FG_Tracing" ReportServerUrl="http://127.0.0.1/ReportServer" />--%>
        </rsweb:ReportViewer>
    </div>
</asp:Content>
