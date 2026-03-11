<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTools.aspx.cs" Inherits="Tools_DataTools" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>数据工具集</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css"/>
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>

<script>
    function redirectPage(url){
        window.open(url, 'DataTools', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no'); //最大化打开
    }
</script>
</head>
<body>
<form id="form1" runat="server">
<table class="table">
    <tr>
        <td valign="top" colspan="2">
            <table border="0" class="table" width="95%" id="Table1" align="center" style="height:auto">
		      <tr>
		        <td class="edit_label"align = "left" style="width:85%">
		            <asp:Label ID="Label5" runat="server" Text="工具名称"></asp:Label>    
		        </td>
		        <td class="edit_label"align = "left" style="width:15%">
		            <asp:Label ID="Label7" runat="server" Text="操作"></asp:Label>    
		        </td>
		      </tr>
		      <tr>
		        <td>
                    <asp:Label ID="Label9" runat="server" Text="数据查看器"></asp:Label>
                </td>
		        <td>
                    <a href="javascript:redirectPage('DataView.aspx');" class="a_Right">点击进入</a>
		        </td>
		      </tr>
		      <tr>
		        <td>
                    <asp:Label ID="Label1" runat="server" Text="SQL执行窗口"></asp:Label>
                </td>
		        <td>
                    <a href="javascript:redirectPage('ExecuteSql.aspx');" class="a_Right">点击进入</a>
		        </td>
		      </tr>
		      <tr>
		        <td>
                    <asp:Label ID="Label2" runat="server" Text="数据对象"></asp:Label>
                </td>
		        <td>
                    <a href="javascript:redirectPage('SP/SPList.aspx');" class="a_Right">点击进入</a>
		        </td>
		      </tr>
	        </table>
        </td>
    </tr>
</table>
</form>
</body>
</html>