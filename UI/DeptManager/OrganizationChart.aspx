
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrganizationChart.aspx.cs" Inherits="DeptManager_OrganizationChart" %>

<%@ Register assembly="Com.ValuePlus.OrgChart" namespace="Com.ValuePlus.OrgChart" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Organization Chart</title>

<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script type="text/javascript">

    //获取当前应用域名信息    
    thisTLoc = location.href; 
    tmpHPage = thisTLoc.split( "/" ); 
    thisHPage = tmpHPage[0];
    for(var i=1;i<tmpHPage.length-2;i++){
       thisHPage = thisHPage+"/"+ tmpHPage[i];
    }
    
    	
	function OpenWindowMax(actionPage){
	    window.open(actionPage,'doAction','left=0,top=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-50) +',scrollbars,resizable=yes,toolbar=no,location=no');//最大化打开
	}
	
</script>
</head>
<body>
    <form id="form1" runat="server">
    <object id="WebBrowser" height="0" width="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>  
    <table>
        <tr style="height:1px">
            <div id="divMain" runat="server" class="topBox">
                <input onclick="document.all.WebBrowser.ExecWB(6,1)" type="button" value="Print">     
                <input onclick="document.all.WebBrowser.ExecWB(8,1)" type="button" value="Page Setting">     
                <input onclick="document.all.WebBrowser.ExecWB(7,1)" type="button" value="Print Preview">
                <INPUT type="button" value="New Window" onclick="javascript:OpenWindowMax('OrganizationChart.aspx')">    
                <INPUT type="button" value="Close" onclick="javascript:window.close()">     
            </div>
        </tr>
        <tr style = "height:25px">
        </tr>
        <tr>
            <cc1:OrgChart ID="OrgChart1" runat="server" ChartStyle="Vertical" />
        </tr>
    </table>
    </form>
</body>
</html>
