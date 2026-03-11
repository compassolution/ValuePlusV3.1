<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalculateDealPage.aspx.cs" Inherits="AppFunction_SalaryCalculate_CalculateDealPage" EnableViewState="False" EnableViewStateMac = "False"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>流程表单的操作页面</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script>
    function showOpenWindow(url){
        window.open(url, 'newwindow', 'width=1000,height=800,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }
</script>
    
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">

<table class="table" width="100%" height="100%">
    <tr>
        <td class="edit_label" valign="top" align="left">
           <asp:LinkButton ID="aBackFlowDealPage" runat="server" CssClass="a_Left" OnClick = "BackFlow_Click">返回薪资计算主流程页面</asp:LinkButton>
        </td>
    </tr>
    <tr width="100%" height="100%">
        <td valign="top" colspan="2" width="100%" height="100%">
            <iframe id="frmOpPage" name="frmOpPage" runat="server" width="100%" height="600" frameborder="0" src="" style="border: 1px solid #cecece;" scrolling="auto" ></iframe>
        </td>
    </tr>
</table>
</form>
</body>
</html>
<script language="javascript">    
    function SetFrameHeight(obj) 
    { 
        var win=obj; 
        if (document.getElementById) 
        { 
            if (win && !window.opera) 
            { 
                if (win.contentDocument && win.contentDocument.body.offsetHeight) 
                    win.height = win.contentDocument.body.offsetHeight; 
                else if(win.Document && win.Document.body.scrollHeight) 
                    win.height = win.Document.body.scrollHeight; 
            } 
        } 
    }
</script>
