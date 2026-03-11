<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollVerify.aspx.cs" Inherits="AppFunction_HRSalary_PayRollVerify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>薪资发放审核</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script type="text/javascript">

    function doExcuteAction(actionPage) {//执行动作
        window.open(actionPage, 'doAction', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no');

    }
</Script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">

<table class="table" width="100%" height="100%">
    <tr>
        <td class="edit_label" valign="top" align="left" colspan = "3">
            <%--<div id="div1" runat="server" style="float:left; margin:auto">
                <asp:LinkButton ID="aBackFlowDealPage" runat="server" CssClass="a_Left">返回</asp:LinkButton>
            </div>  --%>         
            <div id="divActionArea" runat="server" style="float:left; margin:auto">
            </div>
            <div style="display:none">
                <asp:LinkButton ID="aRefresh" runat="server" CssClass="a_Right" onclick="Refresh_Click"><asp:Label ID="Label_Refresh" runat="server" Text="Label">刷新</asp:Label></asp:LinkButton>
            </div>
        </td>
    </tr>
    <tr width="100%" height="100%">
        <td valign="top" style="width:15%" id="frmTitle">
            <asp:ListBox ID="listPage" runat="server"  Font-Size="Small" Width="100%" Height="600px" BackColor="#CAE1FF" AutoPostBack="true" onselectedindexchanged="listPage_SelectedIndexChanged">
            </asp:ListBox>
        </td>
		<td id="tdSwitchPoint" onclick="switchSysBar()" style="width:5px">
            <img src="../../common/images/fold.gif" name="img1" width=6 id=img1>
		</td>    
		<td valign="top" style="height:100%;width:100%" align="center">
            <%--<table width="100%" style="height:100%" class="table"><tr><td>--%>
			    <iframe id="contentFrame" name="contentFrame" frameborder="0" src="" style="width:100%; height:600px; border: 0px solid #cecece;" runat="server" scrolling="auto" ></iframe>
		    <%--</td></tr></table>--%>
        </td>
    </tr>
</table>
</form>
</body>
</html>
<script language="javascript">

    function switchSysBar() {
        if (document.all("frmTitle").style.display == "") {
            document.all("img1").src = "../../common/images/unfold.gif";
            document.all("frmTitle").style.display = "none";
        }
        else {
            document.all("img1").src = "../../common/images/fold.gif";
            document.all("frmTitle").style.display = "";
        }
    } 

    function SetFrameHeight(obj) {
        var win = obj;
        if (document.getElementById) {
            if (win && !window.opera) {
                if (win.contentDocument && win.contentDocument.body.offsetHeight)
                    win.height = win.contentDocument.body.offsetHeight;
                else if (win.Document && win.Document.body.scrollHeight)
                    win.height = win.Document.body.scrollHeight;
            }
        }
    }
</script>
