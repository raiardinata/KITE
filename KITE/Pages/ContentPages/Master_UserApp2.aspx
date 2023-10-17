<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPages/Main.Master" CodeBehind="Master_UserApp2.aspx.cs" Inherits="KITE.Pages.ContentPages.Master_UserApp2" %>

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
                hide : false,
                buttons: {
                    "Delete": function () {
                        $(this).dialog("close");
                    },
                    "Cancel": function () {
                        $(this).dialog("close");
                    }
                }
            });


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

            <div id="inputarea">
                <div id="header1" style="padding-left: 10px; min-width: 750px; vertical-align: top; padding-top: 5px; border-bottom: solid 5px #c4ceff; padding-bottom: 10px; margin-bottom: 10px">
                    <asp:Label ID="lbl1" runat="server" Text="Master User App" Font-Size="X-Large" />
                    <br />
                    <asp:Label ID="lbl2" runat="server" Text="Master > Master User App" />
                </div>

                <div id="gridTitle" class="gridTitle" runat="server">
                    <table width="99%">
                        <tr>
                            <td>
                                <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
                                    <asp:DropDownList ID="ddlSearchCriteria" runat="server">

                                        <asp:ListItem Value="UserName">User Name</asp:ListItem>
                                        <asp:ListItem Value="UserType">User Type</asp:ListItem>
                                       <%-- <asp:ListItem Value="Warehouse">Warehouse</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    &nbsp;Containing &nbsp;&nbsp;
                                    <asp:TextBox ID="txtSearchCriteria" Width="110px" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnAdd" runat="server" Text="Filter" CssClass="ui-button" Width="53px" OnClick="btnAdd_Click"></asp:Button>

                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ui-button" Width="53px" OnClick="btnClear_Click" /><br />

                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <asp:LinkButton ID="btnAdd1"
                                    runat="server"
                                    CssClass="btn btn-default btn-sm"
                                    OnClick="btnAdd1_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-plus"></span>
                                </asp:LinkButton>

                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Label ID="lblerror" runat="server" Text="Label" Visible="false"></asp:Label>
                <div id="Div3" runat="server" class="rounded-corners">
                    <asp:Panel ID="Panel2" runat="Server" ScrollBars="Horizontal">
                        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="false" CssClass="GridView" CellPadding="0" CellSpacing="0" AllowSorting="true" OnRowCommand="grid_RowCommand1"
                             OnRowUpdating="grid_RowUpdating" OnRowDataBound="grid_RowDataBound" OnRowEditing="grid_RowEditing" OnRowDeleting="grid_RowDeleting">
                            <HeaderStyle CssClass="ui-widget-header" Font-Size="Small" Wrap="False" VerticalAlign="Middle" />
                            <RowStyle CssClass="NoWarp" />
                            <Columns>

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="lbAction" runat="server"
                                            CommandName="Edit" CommandArgument='<%#Eval("UserAppID")+","+Eval("UserName")+","+Eval("FullName")+","+Eval("UserType")+","+Eval("UserMail") %>'>
                                        Edit
                                        </asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lbAction2" runat="server"
                                            CommandName="Delete" CommandArgument='<%#Eval("UserAppID")%>' OnClientClick="return confirm('Are you sure, you want to Delete?');return false;" OnClick="lbAction2_Click1">
                                        Delete
                                        </asp:LinkButton>
                                        &nbsp;&nbsp;
                                    </ItemTemplate>

                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="User ID" ItemStyle-Width="20%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="UserID" runat="server" Text='Eval("UserAppID")'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Name" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="UserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Full Name" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="FullName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Type" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="UserType" runat="server" Text='<%# Eval("UserType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Mail" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="UserMail" runat="server" Text='<%# Eval("UserMail") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" Checked='<%# Eval("isactive").ToString() == "1" ? true:false %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>

                <div id="dataPager" runat="server" class="pagerstyle">
                    <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records:" CssClass="totalRecords" />
                    <div id="paging" runat="server">
                        <asp:Label ID="lblShowRows" runat="server" Text="Show rows:" />
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                            <asp:ListItem Value="5" />
                            <asp:ListItem Value="10" />
                            <asp:ListItem Value="15" />
                            <asp:ListItem Value="20" />
                            <asp:ListItem Value="25" />
                            <asp:ListItem Value="50" Selected="True" />
                            <asp:ListItem Value="100" />
                        </asp:DropDownList>
                        &nbsp; Page
                        <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" OnTextChanged="GoToPage_TextChanged"
                            CssClass="gotopage" Width="15px" />
                        of
                        <asp:Label ID="lblTotalNumberOfPages" runat="server" />
                        &nbsp;
                        <asp:Button ID="btnPrev" runat="server" Text="<" ToolTip="Previous Page" OnClick="btnPrev_OnClick" />
                        <asp:Button ID="btnNext" runat="server" Text=">" CommandName="Page" ToolTip="Next Page" CommandArgument="Next"
                            OnClick="btnNext_OnClick" />

                    </div>
                </div>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>

    <%--<div id="dialog-confirm">
        <p>do you want to delete this record? </p>
    </div>--%>


    <!-- Region Item ID -->
  
    <div id="Insert">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
              <ContentTemplate>
                    <a class="b-close">x<a/>
                    <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"></button>
                    <h4 class="modal-title">Form Inserts / Update User</h4>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <fieldset class="fielset">

                                <div id="Div1" class="gridTitle" runat="server">
                                    <table width="99%">
                                    </table>
                                </div>

                                <div class="card-body">
                                    <div class="form-group">
                                        <asp:HiddenField ID="txtUserAppID" runat="server" />
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputUserName">User Name</label>
                                        <%--<input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">--%>
                                        <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="Enter User Name"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="InputFullName">Full Name</label>
                                        <%-- <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">--%>
                                        <asp:TextBox ID="txtFullName" runat="server" class="form-control" placeholder="Enter Full Name"></asp:TextBox>

                                    </div>
                                    <div class="form-group">
                                        <label for="InputUserType">User Type</label>
                                        <asp:DropDownList ID="ddlUsertype" runat="server" class="form-control" Width="150px">

                                            <%--<asp:ListItem Value="User">User</asp:ListItem>
                                            <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
                                            <asp:ListItem Value="Logistic">Logistic</asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group" id="pass1" runat="server">
                                        <label for="InputPassword1">Password</label>

                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control" placeholder="Enter Password" ValidationGroup="RegisterCheck"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ValidationGroup="RegisterCheck"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="Minimum password length is 6" ValidationExpression="^([a-zA-Z0-9@#$%^&+=*]{6,30})$"
                                            ValidationGroup="RegisterCheck" />
                                    </div>
                                    <div class="form-group" id="pass2" runat="server">
                                        <label for="InputPassword2">Retype Password</label>

                                        <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" class="form-control" placeholder="Enter Password" ValidationGroup="RegisterCheck"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword2"
                                            ControlToValidate="txtPassword" Type="String" Operator="Equal" ValidationGroup="RegisterCheck"
                                            ErrorMessage="Password should match"></asp:CompareValidator>
                                    </div>
                                    <div class="form-group">
                                        <label for="InputEmail1">Email address</label>

                                        <asp:TextBox ID="txtemail" runat="server" class="form-control" placeholder="Enter email"></asp:TextBox>
                                    </div>

                                </div>
                                <!-- /.card-body -->
                            </fieldset>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" class="btn btn-primary" ID="Button1" Text="Submit" ValidationGroup="RegisterCheck" CausesValidation="true" OnClick="btnSubmit_Click" />

                    <asp:Button runat="server" class="btn btn-default" ID="Button2" Text="Close" OnClick="Button2_Click1" />

                </div>
            </div>

        </div>
                </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--<div id ="Delete">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
             <ContentTemplate>
                    <a class="b-close">x<a/>
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"></button>
                                <h4 class="modal-title">Delete User</h4>
                            </div>
                             <div class="modal-body">
                                 <h4 class="modal-title">Confirm?</h4>
                                <asp:Button runat="server" class="btn btn-primary" ID="Button2" Text="Yes" OnClick="Button2_Click" />

                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>
                            <div class="modal-footer">
                               
                            </div>
                        </div>
                    </div>
             </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>
</asp:Content>
