
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统异常</title>
    <link href="common/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container">  
        <div class="row-fluid" style="margin-top:100px">
            <div class="span12">
                <div id="lbErrTip001" runat="server" class="jumbotron text-center">
                  <h2>
                    <span class="glyphicon glyphicon-remove-sign text-danger"></span>
                    您尚未登录或者登录时间过长而超时
                  </h2>
                  <p></p>
                  <p></p>
                  <p></p>
                  <p>
                    <a class="btn btn-danger" href="javascript:toLoginPage();" role="button">点击重新登录</a>
                  </p>
                </div>
                <div id="lbErrTip002" runat="server" class="jumbotron text-center" style="display:none">
                </div>
            </div>
        </div>
    </div>

    <script src="common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="common/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var screenHeight = screen.availHeight;
        function toLoginPage() {
            if (window.parent.parent.parent != null) {
                window.parent.parent.parent.location.href = "login.aspx";
            } else if (window.parent.parent != null) {
                window.parent.parent.location.href = "login.aspx";
            } else if (window.parent != null) {
                window.parent.location.href = "login.aspx";
            } else {
                window.location.href = "login.aspx";
            }
        }
        //$(document).ready(function () {
        //    $("#aToLogin").click(function () {
        //        "login.aspx";
        //    })
        //});
    </script>
</body>
</html>
