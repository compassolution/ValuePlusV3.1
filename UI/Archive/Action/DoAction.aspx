<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DoAction.aspx.cs" Inherits="Archive_Action_DoAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
<title>执行动作</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script src="CompleteAction.js" type="text/javascript"></script>

<%--<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../../common/jquery-calendar/laydate.js"></script>

<%--add by sammen 202404 for 进行公众号的模版消息推送的处理逻辑--%>
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="PushTemplateMsg.js" type="text/javascript"></script>
<script >
    window.focus();
</script>
<script language="javascript">
    function saveCheckedBox(){
        if(window.opener!=null) {
            if(window.opener.document.getElementById("aSaveChecked")!=null){//档案列表页面的保存复选框按钮
                window.opener.document.all["aSaveChecked"].click();
            }
        }
    }
    function showOpenWindow(param) {
        var url = "../../Query/QuerySelect.aspx?" + param;
        //var url = "../../Query/QuerySelect.aspx?sql=" + sql + "&key=" + key + "&element=" + element;
        window.open(url, 'newwindow', 'width=1000,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
    }
</script>

</head>
<body >
<!--#include   file= "../../common/CurPageWaiting.htm"--> 
    <form id="form1" runat="server">
    
    <div style="display:none"><asp:LinkButton ID="btnDoSubmit" runat="server" onclick = "DoExcuteSP_Click">执行存储过程</asp:LinkButton></div>
    
    <div runat="server" id="divParamArea">
    
    </div>
    </form>
  <%--  Jquery引入的位置不能随意修改--%>
<%--<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>--%>
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

