<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportOrder.aspx.cs" Inherits="AppFunction_BTW_ImportOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script language="javascript">
    function ImportSuccessfully(){
        if(window.opener!=null) {
            if (window.opener.document.getElementById("aRefreshDetail") != null) {//档案明细页面重新加载数据
                window.opener.document.all["aRefreshDetail"].click();
                window.close();
            }
        }
    }
</script>
<body>
    <form id="form1" runat="server">
    
    <div style="display:none"><asp:LinkButton ID="btnDoSubmit" runat="server" onclick = "DoImport_Click">执行导入操作</asp:LinkButton></div>
    
    </form>
</body>
</html>
