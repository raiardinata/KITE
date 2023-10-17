<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="KITE.Pages.ContentPages.Home" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../CSS/Main.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Viewer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Script/Viewer.js"></script>

    <!-- from HR -->
    <script type="text/javascript" src="../../Script/PopUps.js"></script>
    <script type="text/javascript" src="../../Script/PopUp.min.js"></script>
    <script type="text/javascript" src="../../Script/jquery.easing.1.3.js"></script>
    <script type="text/javascript" src="../../Script/jquery.easing.1.3.min.js"></script>
    <script type="text/javascript" src="../../Script/ShowPopUps.js"></script>
    <link href="../../CSS/PopUps.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/InsertAndUpdate.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Script/jquery.maskedinput1.4.1.js"></script>
    <!-- from HR -->

    <style>
        .bg-1 {
            background-color: #2f2f2f; /* Black Gray */
            color: #fff;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>




            <div class="text-left">
                <h2>KITE REPORT PORTAL.</h2>
            </div>


        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
