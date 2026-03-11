<%@ Page Language="C#" AutoEventWireup="true" CodeFile="actionPath.aspx.cs" Inherits="actionPath" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="common/css/main.css" rel="stylesheet" type="text/css" />
<style type="text/css">
</style>
<script  src="common/js/waitProcess.js"></script>
<title>活动功能路径页面</title><SCRIPT language="javascript">

	function forwardSubFunction(menuId)
	{
	    window.parent.frames.contentFrame.location.href = "FunctionList.aspx?sCode="+menuId;
	    window.location.href= "actionPath.aspx?menuId="+menuId;
	}
	
</SCRIPT>
</head>
<body>
<form id="form1" runat="server">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr style="height:10px" valign=top>
    <td valign="middle" align ="left" background="common/images/welcome/content-bg.gif" width="14">
        <img src="common/images/welcome/ts.gif" width="14" height="14">
    </td>
    <td valign="middle" align ="left" background="common/images/welcome/content-bg.gif">
        <div id="divPath" runat="server" class="titlebt" style="float:left">
        
        </div>
    </td>
  </tr>
</table>
</form>
</body>
</html>
