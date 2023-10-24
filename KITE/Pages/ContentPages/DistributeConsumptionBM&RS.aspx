<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DistributeConsumptionBM&RS.aspx.cs" Inherits="KITE.Pages.ContentPages.DistributeConsumptionBM_RS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <asp:Button ID="btnCalculate" runat="server" Text="Submit" class="btn btn-primary" OnClick="btnCalculate_Click" style="display: inline-block; margin-right: 10px;" />
                </td>
            </tr>
        </table>
        <asp:Label ID="errorLabel" runat="server" Enabled="false" style="color: red; position:relative; width:auto; height:auto;"></asp:Label>
    </div>
</asp:Content>
