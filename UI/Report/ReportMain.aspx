
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportMain.aspx.cs" Inherits="Report_ReportMain" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>报表主页</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>

<%--<script src="../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../common/jquery-calendar/laydate.js"></script>

</head>
<script>
    function showOpenWindow(param) {
        var url = "../Query/QuerySelect.aspx?" + param;
        window.open(url, 'newwindow', 'width=1000,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
    }
</script>
<body>
<!--#include   file= "../common/WaitProccess.htm"-->  
<!--#include   file= "../common/CurPageWaiting.htm"-->
    <form id="form1" runat="server" action="ReportMain.aspx">
    <CR:CRYSTALREPORTVIEWER id="CrystalReportViewer1" style="Z-INDEX: 101; LEFT: 6px; POSITION: absolute; TOP: 5px" runat="server" Height="50px" Width="350px" 
        HasPrintButton="True"  
        EnableDatabaseLogonPrompt="False" 
        EnableParameterPrompt="False" 
        PrintMode="PDF" 
        meta:resourcekey="CrystalReportViewer1Resource1">
    </CR:CRYSTALREPORTVIEWER>
        <%--DisplayGroupTree="False"--%> 
    <div runat="server" id="divParamArea">
    
    </div>
    </form>
  <%--  Jquery引入的位置不能随意修改--%>
<script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
</body>
</html>

<script type="text/javascript">
    $(document).ready(function () {
        //更新日期控件
        laydate.skin('molv');//切换皮肤，请查看skins下面皮肤库
        $("input[datetype='date']").each(function () {
            var varId = $(this).attr('id');
            $(this).focus(function () {
                laydate({
                    elem: '#' + varId,
                    format: 'YYYY-MM-DD'
                });
            })
        });
        $("input[datetype='datetime']").each(function () {
            $(this).focus(function () {
                var varId = $(this).attr('id');
                laydate({
                    elem: '#' + varId,
                    format: 'YYYY-MM-DD hh:mm:ss'
                });
            })
        });
    });

</script>
