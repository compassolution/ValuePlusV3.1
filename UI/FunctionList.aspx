
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FunctionList.aspx.cs" Inherits="FunctionList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
<link href="common/css/main.css" rel="stylesheet" type="text/css" />
<link href="common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title>用户功能列表页面</title>
</head>
<%--<script src="common/js/waitProcess.js"></script>--%>
<script src="common/js/tableStyle.js" type="text/javascript"></script>
<script src="common/js/MainUtil.js" type="text/javascript"></script>
<script type="text/javascript">
    function showNewOpenWindow(url) {
        var randamNum = (Math.floor((Math.random() * 100) + 1)).toString();//100之内的随机数,可同时打开多个动作页面
        window.open(url, 'FuncitonOpenPage' + randamNum, 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no');
    }

	function forwardSubFunction(menuId,showLocation)
	{
	    if (showLocation == '002') {//弹出窗口打开
	        showNewOpenWindow("FunctionList.aspx?sCode=" + menuId);
	    } else {
	        //	    window.parent.document.getElementById("contentFrame").src = "FunctionList.aspx?sCode="+menuId;
	        //	    window.parent.document.getElementById("topContentFrame").src= "actionPath.aspx?menuId="+menuId;
			window.location.href = "FunctionList.aspx?sCode=" + menuId;
            if (myBrowser() != 'IE') {
                window.parent.ClickFirstLevelMenu(menuId);
            } else {
                window.parent.frames.topContentFrame.location.href = "actionPath.aspx?menuId=" + menuId;
            }
	    }
	}
	
	function forwardPage(url, menuId, showLocation)
	{
	    if (showLocation == '002') {//弹出窗口打开
	        showNewOpenWindow(url);
	    } else {
	        //	    window.parent.document.getElementById("contentFrame").src = url;
	        //	    window.parent.document.getElementById("topContentFrame").src= "actionPath.aspx?menuId="+menuId;
            window.location.href = url;
            if (myBrowser() != 'IE') {
				window.parent.ClickFirstLevelMenu(menuId,"1");
            } else {
                window.parent.frames.topContentFrame.location.href = "actionPath.aspx?menuId=" + menuId;
            }
	    }
	}
</script>
<body>
<!--#include   file= "common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<table id="changecolor" class="table">
 
 
	<tr align="right">
		<td align="right" id="tdTitle" runat="server" class="td_Frame1">
		    <a id="aToIcon" href="" runat="server" class="a_Right"><asp:Label ID="Label3" runat="server" Text="Label">大图标显示</asp:Label></a>
		    <a id="aToList" href="" runat="server" class="a_Right"><asp:Label ID="Label2" runat="server" Text="Label">列表显示</asp:Label></a>
		    <a id="aBack" href="#" runat="server" class="a_Right"><asp:Label ID="Label1" runat="server" Text="Label">点击返回</asp:Label></a>
        </td>
	</tr>
    <div id="divFunctionListArea" runat="server">
    
    </div>

    
</table>

</form>

</body>
</html>
<script type="text/javascript">
	//初始化结果表格
	DefineNoTitleTableCss("changecolor");
</script>
