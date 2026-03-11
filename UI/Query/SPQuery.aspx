
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SPQuery.aspx.cs" Inherits="Query_SPQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>存储查询主页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>

<%--<script src="../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../common/jquery-calendar/laydate.js"></script>

</head>
<script>
    function showOpenWindow(param){
        var url="../Query/QuerySelect.aspx?"+param;
        window.open(url, 'newwindow', 'width=1000,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
    }
</script>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <table border="0" class="table" width="60%" id="tb1" align="center" style="height:auto;width:60%">
        <tr><td colspan = "2" align="center"><asp:Label ID="lb_SPTileName" runat="server" Text="查询标题名称" CssClass="left_bt"></asp:Label></td></tr>
        <div runat="server" id="divParamArea">
    
        </div>
    </table>
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
