﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Input_KITEE.aspx.cs" Inherits="KITE.Pages.ContentPages.Input_KITEE" %>


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
                <div id="header1" style="padding-left: 10px; min-width: 750px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
                    <asp:Label ID="lbl1" runat="server" Text="KITE E. Laporan Pengeluaran Hasil Produksi" Font-Size="X-Large" />
                    <br />
                    <asp:Label ID="lbl2" runat="server" Text="Report > E. Laporan Pengeluaran Hasil Produksi" />
                </div>

                
                <asp:Label ID="lblerror" runat="server" Text="Label" Visible="false"></asp:Label>
                <asp:Button ID="btnSearch" runat="Server" Text="Open Search" CssClass="ui-button" OnClick="btnSearch_Click" />


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
  
    <div id="Insert" style="width:60%">
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
                                <div>
                                     <table style="width:100%">                     
                                            <tr>
                                                <td style="width:22%" >
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
                                               
                                                <td style="width:22%" >
                                                     <label for="exampleInputUserName">Organization</label>
                                                    
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
                                    <table  style="width:186px">
                                            <tr>
                                            <td style="width:117px">
                                                 <label for="exampleInputUserName" style="width:150px">Date</label>
                                                <%--<asp:Label ID="Label5" runat="server" Text="Date : " Style="text-align: left" />--%>
                                            </td>

                                            <td>
                                                 <asp:TextBox ID="txtDateFrom" runat="server" Width="150" />
                                            </td>

                                            <td>
                                                <label id ="lblid">To</label>
                                            </td>

                                            <td>
                                                 <asp:TextBox ID="txtDateTo" runat="server" Width="150" />
                                            </td>
                                            <td style="width:100px">
                                                                                            <asp:Label ID="lblErrorPeriod" Width="200px" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"   ></asp:Label>

                                            </td>
                                        </tr>
                                    </table>
                                    <table  style="width:186px">
                                            <tr>
                                                <td style="width:100px">
                                                    <label for="exampleInputUserName" style="width:150px">Customer Group</label>
                                                    <%--<asp:Label ID="Label5" runat="server" Text="Customer Group : " Style="text-align: left" />--%>
                                                </td>

                                                <td >
                                                    <asp:DropDownList ID="ddlCustomerID" AutoPostBack="false" CssClass="norm text string" runat="server" CausesValidation="false">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlcustomerGroup" AutoPostBack="false" CssClass="norm text string" runat="server" CausesValidation="false" Visible="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblErrorCustomer" runat="server" Text="Label" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>

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


