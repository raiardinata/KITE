<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="FG_Tracing_Report_Page.aspx.cs" Inherits="KITE.Pages.ContentPages.FG_Tracing_Report_Page" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="FG_Tracing_Report" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
    <asp:Label runat="server" ID="FG_TracingLbl" Text="FG Tracing Report Page"></asp:Label>
    <br />
    <div class="rounded-corners" style="width: 102%;">
        <rsweb:ReportViewer ID="FG_Tracing_ReportViewer" runat="server" ProcessingMode="Remote"  style="width: 1200px;">
          <ServerReport ReportPath="/KITE_Report/KITE_FG_Tracing" ReportServerUrl="http://127.0.0.1/ReportServer" />
        </rsweb:ReportViewer>
    </div>
</asp:Content>
