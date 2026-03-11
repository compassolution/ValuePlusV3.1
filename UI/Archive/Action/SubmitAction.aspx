<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubmitAction.aspx.cs" Inherits="Archive_Action_SubmitAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>输入参数后执行动作</title>

<script src="CompleteAction.js" type="text/javascript"></script>

<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 

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
</script>
</head>
<body style=" display:none">
<!--#include   file= "../../common/CurPageWaiting.htm"--> 
    <form id="form1" runat="server">
    <div style="display:none"><asp:LinkButton ID="btnDoSubmit" runat="server" onclick = "DoExcuteSP_Click">执行存储过程</asp:LinkButton></div>
    <div>
    
    </div>
    </form>
</body>
</html>
