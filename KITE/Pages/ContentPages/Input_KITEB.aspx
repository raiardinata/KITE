<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Input_KITEB.aspx.cs" Inherits="KITE.Pages.ContentPages.Input_KITEB" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

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
    </style>

    <script type="text/javascript">
        $(document).ready(function () {



            $('#dialog-confirm').dialog({
                autoOpen: true,
                resizable: false,
                height: 140,
                modal: true,
                hide: false,
                buttons: {
                    "Delete": function () {
                        $(this).dialog("close");
                    },
                    "Cancel": function () {
                        $(this).dialog("close");
                    }
                }

            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $("#MainContent_txtDateFrom").datepicker({ dateFormat: 'dd-mm-yy', showOtherMonths: true, changeMonth: true, changeYear: true, selectOtherMonths: true });
                $("#MainContent_txtDateTo").datepicker({ dateFormat: 'dd-mm-yy', showOtherMonths: true, changeMonth: true, changeYear: true, selectOtherMonths: true });
                $("#MainContent_txtDateFrom").mask("99-99-9999");
                $("#MainContent_txtDateTo").mask("99-99-9999");
                $("#MainContent_txtTimeFrom").mask("99:99");
                $("#MainContent_txtToTime").mask("99:99");
            }
        });

        function deleteItem(uniqueID, itemID) {

            $("#dialog-confirm").dialog({
                title: 'confirmation',

                resizable: false,
                height: 200,
                width: 350,
                modal: true,
                buttons: {
                    "Delete": function () {
                        __doPostBack(uniqueID, '');
                        $(this).dialog("close");

                    },
                    "Cancel": function () { $(this).dialog("close"); }
                }
            });

            $('#dialog-confirm').dialog('open');
            return false;
        }
        // 
    </script>

    <script>
        function ShowPopUpBaru(Div) {
            $(Div).PopUps({
                appendTo: "form",
                easing: 'easeOutBounce',
                speed: 0,
                transition: 'slideDown'
            });
        }

    </script>

    <script type="text/javascript">
        function confirmation() {
            if (confirm('are you sure you want to delete ?')) {
                return true;
            } else {
                return false;
            }
        }
   </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Enabled="false">

            </asp:Timer>
            <asp:Timer ID="Timer2" runat="server" Interval="100" OnTick="Timer2_Tick" Enabled="false">

            </asp:Timer>
            <div id="inputarea">
                <div id="header1" style="padding-left: 10px; min-width: 750px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #7bfdf6; padding-bottom: 10px; margin-bottom: 10px">
                    <asp:Label ID="lbl1" runat="server" Text="KITE B. Laporan Pemakaian Bahan Baku" Font-Size="X-Large" />
                    <br />
                    <asp:Label ID="lbl2" runat="server" Text="Report > B. Laporan Pemakaian Bahan Baku" />
                </div>

                
                <asp:Label ID="lblerror" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:Button ID="btnSearch" runat="Server" Text="Open Search" CssClass="ui-button" OnClick="btnSearch_Click" />


                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="1200px">
                </rsweb:ReportViewer>


            </div>
           
        </ContentTemplate>

    </asp:UpdatePanel>

    <%--<div id="dialog-confirm">
        <p>do you want to delete this record? </p>
    </div>--%>


    <!-- Region Item ID -->
    <div id="Loading">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
              <ContentTemplate>
                    <%--<a class="b-close">x<a/>--%>
                    <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
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
    <div id="Insert" style="width:55%">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
              <ContentTemplate>
                    <a class="b-close">x<a/>
                    <div class="modal-dialog" style="width:100%">

            <!-- Modal content-->
            <div class="modal-content" style="width:100%">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"></button>
                    <h4 class="modal-title">Search Criteria</h4>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <fieldset class="fielset">
                                <div style="width:100%">
                                     <table style="width:100%">                     
                                            <tr>
                                                <td style="width:17%" >
                                                     <label id ="lblCC">Company Code</label>
                                                    
                                                    <%--<asp:Label ID="Label1" runat="server" Text="Company Code : " Style="text-align: left" />--%>
                                                </td>
                                                <td style="width:10%">
                                                    <asp:DropDownList ID="ddlCompanyCode" AutoPostBack="true" CssClass="norm text string" runat="server" CausesValidation="false" OnSelectedIndexChanged="ddlCompanyCode_SelectedIndexChanged" >
                                                    
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width:40%">
                                                    <asp:Label ID="lblcomcode" runat="server" CssClass="norm text string" ForeColor="Black"></asp:Label>
                                                    
                                                </td>

                                                
                                            </tr>
                                       </table>
                                     <table style="width:100%">
                                            <tr>
                                               
                                                <td style="width:17%" >
                                                     <label for="exampleInputUserName">Plant</label>
                                                    
                                                </td>

                                                <td style="width:10%">
                                                    <asp:DropDownList ID="ddlPlant" AutoPostBack="true" CssClass="norm text string" runat="server" CausesValidation="false" OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                                    
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width:40%">
                                                    <asp:Label ID="lblplant" runat="server" CssClass="norm text string" ForeColor="Black"></asp:Label>
                                                </td>
                                             
                                            </tr>
                                        </table>
                                     <table style="width:100%">
                                            <tr>
                                               
                                                <td style="width:100%">
                                                     <label for="exampleInputUserName" style="width:150px">Material</label>
                                                    
                                                </td>
                                                <td>
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
                                                                                                    <asp:Label ID="lblerrorMat" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>

                                                </td>
                                            </tr>
                                    </table>
                                     <table style="width:100%">                                      
                                        <tr>
                                            <td style="width:100%">
                                                 <label for="exampleInputUserName" style="width:150px">Storage Location</label>
                                                <%--<asp:Label ID="Label4" runat="server" Text="Storage Location : " Style="text-align: left" />--%>
                                            </td>

                                            <td >
                                                <asp:ListBox ID ="lbSloc" runat="server" SelectionMode="Multiple" Width="200" Height="100">
                                                  
                                                </asp:ListBox>
                                            </td>

                                            <td>
                                                <asp:Button ID="btnselectsloc" runat="Server" Text=">>" CssClass="ui-button" OnClick="btnselectsloc_Click"  />
                                                <br />
                                                <br />
                                                <asp:Button ID="btndiselectsloc" runat="Server" Text="<<" CssClass="ui-button" OnClick="btndiselectsloc_Click"  />
                                            </td>
                                            <td>
                                                <asp:ListBox ID ="lbsloc2" runat="server" SelectionMode="Multiple"  Width="200" Height="100">

                                                </asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblErrorSloc" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red" Font-Underline="false"></asp:Label>

                                            </td>
                                        </tr>
                                    </table>
                                     <table style="width:100%">
                                        <tr>
                                            <td style="width:100%">
                                                 <label for="exampleInputUserName" style="width:150px">Movement Type</label>
                                                <%--<asp:Label ID="Label7" runat="server" Text="Movement Type : " Style="text-align: left" />--%>
                                            </td>

                                            <td >
                                                 <asp:ListBox ID ="lbMove" runat="server" SelectionMode="Multiple"  Width="200" Height="100">
                                                   
                                                </asp:ListBox>
                                            </td>

                                            <td>
                                                <asp:Button ID="btnSelectmove" runat="Server" Text=">>" CssClass="ui-button" OnClick="btnSelectmove_Click"  />
                                            <br />
                                                <br />
                                                <asp:Button ID="btnDiselectmove" runat="Server" Text="<<" CssClass="ui-button" OnClick="btnDiselectmove_Click" />
                                            </td>
                                            <td>
                                                <asp:ListBox ID ="lbMove2" runat="server" SelectionMode="Multiple"  Width="200" Height="100">

                                                </asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblErrMove" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                     <table>
                                        <tr>
                                            <td style="width:150px">
                                                 <label for="exampleInputUserName">Date</label>
                                                <%--<asp:Label ID="Label5" runat="server" Text="Date : " Style="text-align: left" />--%>
                                            </td>

                                            <td>
                                                 <asp:TextBox ID="txtDateFrom" runat="server" Width="100" />
                                            </td>

                                            <td>
                                               <label id ="lblid">To</label>
                                            </td>

                                            <td>
                                                 <asp:TextBox ID="txtDateTo" runat="server" Width="100" />
                                            </td>
                                            <td>
                                                                                            <asp:Label ID="lblErrorPeriod" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>

                                            </td>
                                        </tr>
                                    </table>    
                                        
                                           
                                            
                                
                                    
                                   

                                </div>
                                <!-- /.card-body -->
                            </fieldset>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" class="btn btn-primary" ID="Button1" Text="Submit" ValidationGroup="RegisterCheck" CausesValidation="true" OnClick="Button1_Click" />

                    <asp:Button runat="server" class="btn btn-default" ID="Button2" Text="Close" OnClick="Button2_Click" />

                </div>
            </div>

        </div>
                </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

