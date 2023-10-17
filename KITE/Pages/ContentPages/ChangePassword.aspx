<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="KITE.Pages.ContentPages.ChangePassword" %>

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
        .auto-style22 {
            width: 160px;
            height: 30px;
        }
        .auto-style23 {
            width: 157px;
        }
        .auto-style24 {
            width: 158px;
        }
        .auto-style25 {
            width: 193px;
            height: 30px;
        }
        .auto-style26 {
            height: 30px;
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
                    <asp:Label ID="lbl1" runat="server" Text="Change Password" Font-Size="X-Large" />
                    <br />
                    <asp:Label ID="lbl2" runat="server" Text="Setting > Change Password" />
                </div>
                <div id="gridTitle" class="gridTitle" runat="server">
                    <table>
                        <tr>
                            <td class="auto-style22">

                                <asp:Label ID="Label2" runat="server" Text="UserName" Font-Bold="True" Font-Size="10pt"></asp:Label>

                               </td>
                            <td class="auto-style25">
                               
                                <asp:Label ID="lblUser" runat="server" Text="" Font-Bold="True" Font-Size="10pt"></asp:Label>

                            </td>
                           
                        </tr>
                    </table>
                    <br>
                    <table>
                        <tr>
                            <td class="auto-style23">
                                <asp:Label ID="Label10" runat="server" Text="Old Password" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldPassword" class="form-control" runat="server" placeholder="Password" TextMode="Password" Height="30px" Width="167px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblerrOld" runat="server" Font-Bold="True" Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
                            
                        </tr>
                    </table>
                 
                    <br />
                    <table>
                        <tr>
                            <td class="auto-style24">
                                <asp:Label ID="Label1" runat="server" Text="New Password" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewPassword" class="form-control" runat="server" placeholder="Password" TextMode="Password" Height="30px" Width="167px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblErrNew" runat="server" Font-Bold="True" Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
                            
                        </tr>
                    </table>

                    <br />
                    <table>
                        <tr>
                            <td class="auto-style23">
                                <asp:Label ID="Label3" runat="server" Text="Confirmation Password" Font-Bold="True" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfPassword" class="form-control" runat="server" placeholder="Password" TextMode="Password" Height="30px" Width="167px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblErrConf" runat="server" Font-Bold="True" Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
                            
                        </tr>
                    </table>

                    <br />
                    <table>
                        <tr>
                            <td>

                                <asp:Label ID="Labelerror" runat="server" Font-Bold="True" Font-Size="10pt" Visible="False"></asp:Label>

                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Save" Width="120px" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Cancel" Width="120px" OnClick="btnClear_Click"/>
                                <br />
                            </td>
                        </tr>
                    </table>

                  </div>
            </div>
       </ContentTemplate>
     </asp:UpdatePanel>

</asp:Content>
