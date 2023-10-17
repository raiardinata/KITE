<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SlocConfiguration.aspx.cs" Inherits="KITE.Pages.ContentPages.SlocConfiguration" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../CSS/Viewer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Script/Viewer.js"></script>
    <link href="../../CSS/GridView.css" rel="stylesheet" />
    <!-- from HR -->
    <script type="text/javascript" src="../../Script/PopUps.js"></script>
    <script type="text/javascript" src="../../Script/PopUp.min.js"></script>
    <script type="text/javascript" src="../../Script/jquery.easing.1.3.js"></script>
    <script type="text/javascript" src="../../Script/jquery.easing.1.3.min.js"></script>
    <script type="text/javascript" src="../../Script/ShowPopUps.js"></script>
    <link href="../../CSS/PopUps.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/InsertAndUpdate.css" rel="stylesheet" type="text/css" />
    <script src="../../Script/jquery-ui.js" type="text/javascript"></script>
    <link href="../../CSS/Main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Script/jquery.maskedinput1.4.1.js"></script>
    <!-- from HR -->

    <style>
        .bg-1 {
            background-color: #ff0000; /* Black Gray */
            color: #fff;
        }

        /*#wrapper {
            margin: 0 auto;
            padding: 0px;
            text-align: center;
            width: auto;
        }*/
        #inputarea {
            border-radius: 25px;
            background-color: white;
        }
        .auto-style3 {
            width: 51px;
        }
        .auto-style4 {
            width: 92px;
        }
        .auto-style5 {
            width: 96px;
        }
        .auto-style6 {
            width: 261px;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div id="inputarea">
                <div id="header1" style="padding-left: 10px; min-width: 750px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
                    <asp:Label ID="lbl1" runat="server" Text="Storage Location Configuration" Font-Size="X-Large" />
                    <br />
                    <asp:Label ID="lbl2" runat="server" Text="Master > Storage Location Configuration" />
                </div>
                <div id="gridTitle" class="gridTitle" runat="server">
                    <table width="99%">
                        <tr>
                            <td class="auto-style4">

                                <asp:Label ID="Label2" runat="server" Text="Menu Report" Font-Bold="True" Font-Size="10pt"></asp:Label>

                               </td>
                            <td class="auto-style6">
                                <asp:Panel ID="searchBox" CssClass="searchBox" runat="server" >
                                    <asp:DropDownList ID="ddlSearchCriteria" runat="server" Height="21px" Width="154px" OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="--Pilih Menu--">--Pilih Menu--</asp:ListItem>
                                        <asp:ListItem Value="KITE A">KITE A</asp:ListItem>
                                        <asp:ListItem Value="KITE B">KITE B</asp:ListItem>
                                        <asp:ListItem Value="KITE C">KITE C</asp:ListItem>
                                        <asp:ListItem Value="KITE D">KITE D</asp:ListItem>
                                        <asp:ListItem Value="KITE E">KITE E</asp:ListItem>
                                        <asp:ListItem Value="KITE F">KITE F</asp:ListItem>
                                        <asp:ListItem Value="KITE G">KITE G</asp:ListItem>
                                        <asp:ListItem Value="KITE H">KITE H</asp:ListItem>
                                       <%-- <asp:ListItem Value="Warehouse">Warehouse</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </asp:Panel>
                                </td>
                            <td>
                                <asp:Label ID="Labelmenu" runat="server" Text="" Font-Bold="True" Font-Size="11pt" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br>
                    <table>
                        <tr>
                            <td class="auto-style4">
                                <asp:Label ID="Label10" runat="server" Text="Storage Location" Font-Bold="True" Font-Size="11pt"></asp:Label>
                            </td>
                            <td>
                                <asp:ListBox ID="lbSloc" runat="server" Height="147px" SelectionMode="Multiple" Width="200px" >

                                </asp:ListBox>
                            </td>
                            <td class="auto-style3">
                                <asp:Button ID="btnSelectmat" runat="Server" CssClass="ui-button" Text="&gt;&gt;" OnClick="btnSelectmat_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnDiselectmat" runat="Server" CssClass="ui-button" Text="&lt;&lt;" OnClick="btnDiselectmat_Click" />
                            </td>
                            <td>
                                <asp:ListBox ID="lbSloc2" runat="server" Height="147px" SelectionMode="Multiple" Width="200">

                                </asp:ListBox>
                            </td>
                        </tr>
                    </table>
                 
                    <br />
                    <table>
                        <tr>
                            <td class="auto-style5">

                                <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="10pt" Visible="False"></asp:Label>

                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Save" Width="120px" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Cancel" Width="120px" OnClick="btnClear_Click" />
                                <br />
                            </td>
                        </tr>
                    </table>
                  </div>
            </div>
       </ContentTemplate>
     </asp:UpdatePanel>

</asp:Content>
