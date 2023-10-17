<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KITE.Pages.ContentPages.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title></title>

   <%-- <link href="../../CSS/bootstrap-3.3.7/bootstrap.min.css" rel="stylesheet" />--%>
   
  <%--  <script src="../../CSS/bootstrap-3.3.7/bootstrap.min.js"></script>--%>

    <link href="../../CSS/bootstrap-3.3.7-dist/css/bootstrap.min.css" rel="stylesheet" />
     <script src="../../CSS/bootstrap-3.3.7/jquery.min.js"></script>
    <script src="../../CSS/bootstrap-3.3.7-dist/js/bootstrap.min.js"></script>
    <!------ Include the above in your HEAD tag ---------->


    <script type="text/javascript" src="../../script/jquery-3.1.1.js"></script>
    <script type="text/javascript" src="../../script/jquery-3.1.1.min.js"></script>



    <link href="../../css/uicss.css" rel="stylesheet" type="text/css" />
    <link href="../../css/theme/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../css/navmenu.css" rel="stylesheet" type="text/css" />


    <%-- <script type="text/javascript" src="../../Script/jquery-1.12.1-ui.js"></script>
    <script type="text/javascript" src="../../Script/jquery-1.12.1-ui.min.js"></script>--%>

    <%--    <script type="text/javascript" src="../../Script/NavMenu.js"></script>--%>
    <script src="../../Scripts/bootstrap.min.js"></script>

    <%-- <link href="login_style.css" type="text/css" rel="stylesheet"/>--%>

    <%-- <link href="../../CSS/materialize/materialize.min.css" rel="stylesheet" />
    <script src="../../CSS/materialize/materialize.min.js"></script>--%>


    <style>
        body {
            text-align: center;
            width: 100%;
            margin: 0 auto;
            padding: 0px;
            /*background-color: DodgerBlue;*/

            /*background-image: url(../../Images/SCM1.jpg);*/
            background-size:100%;
        }

        #wrapper {
            margin: 0 auto;
            padding: 0px;
            text-align: right;
            width: auto;
        }

        #login_div {
            border-radius: 25px;
            background-color: white;
            padding: 20px;
            max-width: 300px;

            margin: 0 auto;
            text-align: center;
            /*margin-left: 345px;
            margin-left: 520px;*/
            margin-top: 70px;
        }

            #login_div button {
                margin-top: 20px;
                height: 40px;
                width: 100%;
            }

            #login_div #register {
                margin-top: 20px;
                float: left;
            }

            #login_div #forgot {
                margin-top: 20px;
                float: right;
            }
    </style>
</head>
<body >



    <div id="wrapper">

        <nav class="navbar navbar-default ">
            <div class="container-fluid">
                <div class="navbar-header">
                    <img src="../../Images/aji_logo.png" style="width:118px; height:85px" />
                </div>   
            </div>
            
        </nav>

         
        <div class="text-center">
            <h2>KITE REPORT PORTAL</h2>
        </div>
        <div id="login_div">
            <%--<img src="../../Images/aji_logo.png" style="width:200px; height:150px" />--%>
            <h4 style="border-bottom: 1px solid #c5c5c5;">
                <i class="glyphicon glyphicon-user"></i>
                Login User
            </h4>
            <form method="post" runat="server">



                <div class="form-group input-group">

                    <span class="input-group-addon">
                        <i class="glyphicon glyphicon-user"></i>
                    </span>
                    <%--<input class="form-control" placeholder="Password" name="password" type="password" value="" required="">--%>

                    <asp:TextBox ID="txtUserName" class="form-control" placeholder="User Name" runat="server"></asp:TextBox>

                </div>

                <div class="form-group input-group">

                    <span class="input-group-addon">
                        <i class="glyphicon glyphicon-lock"></i>
                    </span>
                    <%--<input class="form-control" placeholder="Password" name="password" type="password" value="" required="">--%>
                    <asp:TextBox ID="txtPassword" class="form-control" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                </div>

                <div class="input-field">
                    <%--<button class="btn waves-effect waves-light" type="submit" name="action">Submit</button>--%>
                    <asp:Button class="btn btn-primary" ID="btnSubmit" Text="Login" OnClick="btnSubmit_Click" runat="server" />
                </div>

              

<%--                <p>

                    <a href="#" id="forgot">Forgot password?</a>
                </p>--%>

                <br>
                <br>
            </form>
        </div>


       
        <!-- Footer -->
        <footer class="text-center">

            <br>
            <br>
            <p style="width: 100%; display: block; background-color: red; color: whitesmoke; text-decoration-color: black; text-decoration-style: solid">PT AJINEX INTERNATIONAL @ 2019</p>

        </footer>

    </div>
</body>
</html>
