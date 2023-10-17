<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Output_KITEC.aspx.cs" Inherits="KITE.Pages.ContentPages.Output_KITEC" %>

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

     <script type="text/javascript">

         $(document).ready(function () {
             $("#MainContent_txtDateStart").datepicker({ dateFormat: 'dd-mm-yy' });
             $("#MainContent_txtDateEnd").datepicker({ dateFormat: 'dd-mm-yy' });
         });

         function ResetForm() {

             document.getElementById('MainContent_txtTimeVision').value = "";
             document.getElementById('MainContent_txtItemGroup').value = "";
             document.getElementById('MainContent_txtItem').value = "";
             document.getElementById('MainContent_txtLocation').value = "";

         }

         function CheckTimeVision(evt) {

             var keycode = (evt.which) ? evt.which : evt.keyCode;

             if (keycode == 0 || keycode > 31) {
                 return true;
             }
             else {
                 alert("Time vision out of range");
                 return false;
             }
         }

    </script>

    <script type="text/javascript">

        function validatenumerics(key) {
            //getting key code of pressed key
            var keycode = (key.which) ? key.which : key.keyCode;
            //comparing pressed keycodes

            if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                // alert(" You can enter only characters 0 to 9 ");
                return false;
            }
            else return true;


        }

    </script>

    <style>
        #date1 {
            border: 1px solid;
            padding: 5px;
            /* box-shadow: 2px 2px #888888;*/
            height: 28px;
        }

         #inputarea {
            border-radius: 25px;
            background-color: white;
        }

    </style>

    <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="56000" runat="server">
        <Scripts>
        </Scripts>
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="inputarea">
            <!-- First Container -->
            <div class="container-fluid bg-1 text-left">
                <h1 class="margin">KITE C</h1>
                <h2 class="margin">Laporan Pemakaian Barang Dalam Rangka Kegiatan Sub Knotrak</h2>
            <div class="container-fluid bg-1">
                <table border="0" cellpadding="3" cellspacing="3">          
                    <tr>
                        <td>
                             <asp:Button ID="btnExport" runat="Server" Text="Export XLS" CssClass="ui-button" OnClick="btnExport_Click" />
                        </td>

                        <td>
                             <asp:Button ID="btnPrint" runat="Server" Text="Print" CssClass="ui-button" OnClick="btnPrint_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            
            <div id="GridViewField" runat="server">
                        
                            <div id="DivGrid" runat="server" class="rounded-corners">
                                <asp:Panel ID="pnl" runat="Server" ScrollBars="Horizontal">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="GridView" AllowSorting="true" OnSorting="GridView1_Sorting">
                                    <HeaderStyle CssClass="ui-widget-header" />
                                    <RowStyle CssClass="NoWarp" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" SortExpression="No" HeaderStyle-CssClass="HeaderText" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%#Eval("No") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Nomor" SortExpression="Nomor" HeaderStyle-CssClass="HeaderText" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNomor" runat="server" Text='<%#Eval("Nomor") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Tanggal" SortExpression="Tanggal" HeaderStyle-CssClass="HeaderText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTanggal" runat="server" Text='<%#Eval("NIK") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Kode Barang" SortExpression="ToDate" HeaderStyle-CssClass="HeaderText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKOdeBarang" runat="server" Text='<%#Eval("ToDate","{0:dd-MM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Nama Barang" SortExpression="FromTime" HeaderStyle-CssClass="HeaderText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromTime" runat="server" Text='<%#Eval("FromDate","{0:HH:mm:ss}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Satuan" SortExpression="ToTime" HeaderStyle-CssClass="HeaderText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToTime" runat="server" Text='<%#Eval("ToDate","{0:HH:mm:ss}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                   
                                        <asp:TemplateField HeaderText="DisubKontrakan" SortExpression="Remark" HeaderStyle-CssClass="HeaderText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText=" Penerima Subkontrak" SortExpression="Status" HeaderStyle-CssClass="HeaderText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                    </asp:Panel>
                            </div>
                   
                        
                        <div>
                            <div id="dataPager" runat="server" class="pagerstyle">
                                <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                                <div id="paging" runat="server">
                                    <asp:Label ID="lblShowRows" runat="server"  />
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                        <asp:ListItem Value="5" />
                                        <asp:ListItem Value="10" />
                                        <asp:ListItem Value="15" />
                                        <asp:ListItem Value="20" Selected="True"/>
                                        <asp:ListItem Value="25" />
                                        <asp:ListItem Value="50"  />
                                        <asp:ListItem Value="100" />
                                    </asp:DropDownList>
                                    &nbsp; Page
                                    <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="txtGoToPage_TextChanged"
                                        CssClass="gotopage" Width="15px"/>
                                    of
                                    <asp:Label ID="lblTotalNumberOfPages" runat="server" />
                                    &nbsp;
                                    <asp:ImageButton ID="btnPrev" runat="server" OnClick="btnPrev_Click" ImageUrl="~/Images/button_previous.png" CssClass="previous"/>&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="btnNext" runat="server" OnClick="btnNext_Click" ImageUrl="~/Images/button_next.png" CssClass="next"/>
                                </div>
                            </div>
                        </div>
                 </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
