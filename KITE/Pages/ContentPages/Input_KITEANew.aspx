<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Input_KITEANew.aspx.cs" Inherits="KITE.Pages.ContentPages.Input_KITEANew" %>


<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


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
        .auto-style8 {
            width: 119px;
        }
        .auto-style9 {
            width: 100px;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
             <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Enabled="false">

            </asp:Timer>
            <asp:Timer ID="Timer2" runat="server" Interval="100" OnTick="Timer2_Tick" Enabled="false">

            </asp:Timer>
            <div id="inputarea">
                <div id="header1" style="padding-left: 10px; min-width: 750px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
                    <asp:Label ID="lbl1" runat="server" Text="KITE A. Laporan Pemasukan Bahan Baku" Font-Size="X-Large" />
                    <br />
                    <asp:Label ID="lbl2" runat="server" Text="Report > A. Laporan Pemasukan Bahan Baku" />
                </div>
               <%-- <asp:Label ID="lblerror" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:Button ID="btnSearch" runat="Server" Text="Open Search" CssClass="ui-button" OnClick="btnSearch_Click" />--%>

                 <div id="gridTitle" class="gridTitle" runat="server">
                    
                                    <table>
                                        <tr>
                                                <td class="auto-style8">

                                                     <label id ="lblCC">Company Code</label>
                                                    
                                                    <%--<asp:Label ID="Label1" runat="server" Text="Company Code : " Style="text-align: left" />--%>
                                                </td>
                                                <td class="auto-style9">
                                                    <asp:DropDownList ID="ddlCompanyCode" AutoPostBack="true" CssClass="norm text string" runat="server" CausesValidation="false" OnSelectedIndexChanged="ddlCompanyCode_SelectedIndexChanged" >
                                                    
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblcomcode" runat="server" CssClass="norm text string" ForeColor="Black"></asp:Label>
                                                    
                                                </td>

                                                
                                            </tr>
                                       </table>
                                    <table>
                                            <tr>
                                               
                                                <td  class="auto-style8">
                                                     <label for="exampleInputUserName">Plant</label>
                                                    
                                                </td>

                                                <td  class="auto-style8">
                                                    <asp:DropDownList ID="ddlPlant" AutoPostBack="true" CssClass="norm text string" runat="server" CausesValidation="false" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                                    
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblplant" runat="server" CssClass="norm text string" ForeColor="Black"></asp:Label>
                                                </td>
                                             
                                            </tr>
                                        </table>
                                    <table>
                                            <tr>
                                               
                                                <td class="auto-style6">
                                                     <label for="exampleInputUserName" style="width:117px">Material</label>
                                                    
                                                </td>
                                                <td class="auto-style8">
                                                        <asp:ListBox ID ="lbMaterial" runat="server" SelectionMode="Multiple"  Width="200" Height="100">

                                                        </asp:ListBox>
                                                </td>
                                                <td>
                                               
                                                    <asp:Button ID="btnSelectmat" runat="Server" Text=">>" CssClass="ui-button" OnClick="btnSelectmat_Click"  />
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnDiselectmat" runat="Server" Text="<<" CssClass="ui-button" OnClick="btnDiselectmat_Click"  />

                                                
                                                </td>
                                                <td>
                                                    <asp:ListBox ID ="lbMaterial2" runat="server" SelectionMode="Multiple"  Width="200" Height="100">

                                                    </asp:ListBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblerror2" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>

                                                </td>
                                            </tr>
                                    </table>
                                       
                                    <table>
                                      
                                            <tr>
                                            
                                                <td  class="auto-style8">
                                                <label id ="lblPeriod">Period</label>
                                               
                                                </td>

                                            <td>
                                                
                                                 <asp:DropDownList ID="ddlPeriodFrom" AutoPostBack="false" CssClass="norm text string" runat="server" CausesValidation="false">
                                                     <asp:ListItem>January</asp:ListItem>
                                                     <asp:ListItem>February</asp:ListItem>
                                                     <asp:ListItem>March</asp:ListItem>
                                                     <asp:ListItem>April</asp:ListItem>
                                                     <asp:ListItem>May</asp:ListItem>
                                                     <asp:ListItem>June</asp:ListItem>
                                                     <asp:ListItem>July</asp:ListItem>
                                                     <asp:ListItem>August</asp:ListItem>
                                                     <asp:ListItem>September</asp:ListItem>
                                                     <asp:ListItem>October</asp:ListItem>
                                                     <asp:ListItem>November</asp:ListItem>
                                                     <asp:ListItem>December</asp:ListItem>

                                                 </asp:DropDownList>
                                             </td>
                                             <td>
                                                <asp:DropDownList ID="ddlYearFrom" AutoPostBack="false" CssClass="norm text string" runat="server" CausesValidation="false">
                                                    
                                                </asp:DropDownList>
                                            </td>
                                           
                                            <td>
                                                <label id ="lblid">To</label>
                                            </td>
                                            <td>
                                                 
                                                <asp:DropDownList ID="ddlPeriodTo" AutoPostBack="false" CssClass="norm text string" runat="server" CausesValidation="false" OnSelectedIndexChanged="ddlPeriodTo_SelectedIndexChanged">
                                                     <asp:ListItem>January</asp:ListItem>
                                                     <asp:ListItem>February</asp:ListItem>
                                                     <asp:ListItem>March</asp:ListItem>
                                                     <asp:ListItem>April</asp:ListItem>
                                                     <asp:ListItem>May</asp:ListItem>
                                                     <asp:ListItem>June</asp:ListItem>
                                                     <asp:ListItem>July</asp:ListItem>
                                                     <asp:ListItem>August</asp:ListItem>
                                                     <asp:ListItem>September</asp:ListItem>
                                                     <asp:ListItem>October</asp:ListItem>
                                                     <asp:ListItem>November</asp:ListItem>
                                                     <asp:ListItem>December</asp:ListItem>

                                                 </asp:DropDownList>
                                                </td>
                                                <td>
                                                <asp:DropDownList ID="ddlYearTo" AutoPostBack="false" CssClass="norm text string" runat="server" CausesValidation="false">
                                                    
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                                                            <asp:Label ID="LblErrorPeriod" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>

                                            </td>
                                        </tr>
                                       
                                    
                                    </table>
                                    <br />

                                    <table>
                                        <td class="auto-style6" style="width:118px">

                                        </td>
                                        <td class="auto-style6">
                                                                                    <asp:Button runat="server" class="btn btn-primary" ID="Button1" Text="Submit" ValidationGroup="RegisterCheck" CausesValidation="true" Width="100px" OnClick="Button1_Click"  />

                                                    <asp:Button runat="server" class="btn btn-default" ID="Button2" Text="Close" Width="100px" OnClick="Button2_Click" />

                                        </td>
                                    </table>
                                     
                                                    
                                        
                                
                  </div>

                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="1000px">
                </rsweb:ReportViewer>
            </div>
       </ContentTemplate>
     </asp:UpdatePanel>

     <div id="Loading">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
              <ContentTemplate>
                    <%--<a class="b-close">x<a/>--%>
                    <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content" >
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"></button>
                    <h4 class="modal-title">Loading Report</h4>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <fieldset class="fielset">
                                <div class="card-body">
                                    
                                </div>
                                <!-- /.card-body -->
                            </fieldset>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <asp:Label ID="LabelWaitingList" runat="server" Text="Label" Visible="false"></asp:Label>
               <%-- <div class="modal-footer">
                    <asp:Button runat="server" class="btn btn-primary" ID="Button9" Text="Submit" ValidationGroup="RegisterCheck" CausesValidation="true" OnClick="Button1_Click" />

                    <asp:Button runat="server" class="btn btn-default" ID="Button10" Text="Close" OnClick="Button2_Click" />

                </div>--%>
            </div>

        </div>
                </ContentTemplate>
        </asp:UpdatePanel>
    </div>
  

</asp:Content>
