
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LicenseWarning.aspx.cs" Inherits="Regist_LicenseWarning" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>License Warning!</title>
    <link href="../common/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container">  
        <div id="divMain" class="row-fluid" style="margin-top:20px">
            <div class="span12">
                <div id="lbErrTip001" runat="server" class="jumbotron text-center text-danger">
                  <h3>
                    <span class="glyphicon glyphicon-info-sign text-danger"></span>
                    <%=strWarningMsg %>
                  </h3>
                  <p></p>
                  <p></p>
                  <p></p>
                  <p>
                    <a id="aToLogin" class="btn btn-danger" href="javascript:toLoginPage();" role="button" style="display:none"><%=strReturnText %></a>
                    <a id="aClose" class="btn btn-danger" href="javascript:closePage();" role="button"><%=strCloseText %></a>
                  </p>
                </div>
                <div id="lbErrTip002" runat="server" class="jumbotron text-center" style="display:none">
                </div>
            </div>
        </div>
    </div>

    <script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../common/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var screenHeight = screen.availHeight;
        function toLoginPage() {
            if (window.parent.parent.parent != null) {
                window.parent.parent.parent.location.href = "../login.aspx";
            } else if (window.parent.parent != null) {
                window.parent.parent.location.href = "../login.aspx";
            } else if (window.parent != null) {
                window.parent.location.href = "../login.aspx";
            } else {
                window.location.href = "../login.aspx";
            }
        }
        function closePage() {
            if (window.parent.parent.parent != null) {
                window.parent.parent.parent.close();
            } else if (window.parent.parent != null) {
                window.parent.parent.close();
            } else if (window.parent != null) {
                window.parent.close();
            } else {
                window.close();
            }
        }

        $(document).ready(function () {
            $("title").text('<%=strTitleText %>');
            if (window.opener == null) {
                $("#aToLogin").show();
                $("#aClose").hide();
            } else {
                $("#aToLogin").hide();
                $("#aClose").show();
            }
        });
    </script>
</body>
</html>
