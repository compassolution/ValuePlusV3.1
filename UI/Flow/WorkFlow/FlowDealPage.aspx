<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowDealPage.aspx.cs" Inherits="Flow_WorkFlow_FlowDealPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>流程处理页面</title>
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

<table class="table">

    <tr>
        <td valign="top" align="center"><span class="left_bt" id="span1" runat="server">流程处理业务待办事项处理页面</span><br>
        </td>
    </tr>
    <tr>
        <td valign="top" colspan="2">
            <table border="0" class="table" width="100%" id="tb1" align="center" style="height:auto">
		      
		      <tr>
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label2" runat="server" Text="待办事项名称"></asp:Label>    
		        </td>
		        <td colspan="3">
                    <asp:Label ID="lbFlowName" runat="server" Text="待办事项名称"></asp:Label>
		        </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center" style="width:16%">
		            <asp:Label ID="Label3" runat="server" Text="上一岗位名称"></asp:Label>
		        </td>
		        <td style="width:34%">
                    <asp:Label ID="lbPrePostName" runat="server" Text="上一岗位名称"></asp:Label>
                </td>
		        <td class="edit_label"align = "center" style="width:16%">
		            <asp:Label ID="Label1" runat="server" Text="上一岗位处理人"></asp:Label>    
		        </td>
		        <td style="width:34%">
                    <asp:Label ID="lbPrePostUserName" runat="server" Text="上一岗位处理人"></asp:Label>
		        </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label4" runat="server" Text="当前岗位名称"></asp:Label>
		        </td>
		        <td>
                    <asp:Label ID="lbCurPostName" runat="server" Text="当前岗位名称"></asp:Label>
                </td>
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label6" runat="server" Text="上一岗位移交意见"></asp:Label>    
		        </td>
		        <td>
                    <asp:Label ID="lbPrePostComment" runat="server" Text="上一岗位移交意见"></asp:Label>
		        </td>
		      </tr>
		      <tr height="18" align="center">
                <TD align ="right" colspan="4" class="td_Frame2" >
                    <asp:LinkButton ID="aAccept" runat="server" CssClass="a_Right" OnClick = "Accept_Click">接收该事项</asp:LinkButton>
                    <asp:LinkButton ID="aBack" runat="server" CssClass="a_Right" OnClick = "Refuse_Click">直接退回</asp:LinkButton>
                    <asp:LinkButton ID="aHisInfo" runat="server" CssClass="a_Right">查看历史流转信息</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" OnClick = "Back_Click">返回待办事项列表</asp:LinkButton>
                </TD>
               </tr>
	        </table>
        </td>
    </tr>
    <p></p>
    <p></p>
    <tr>
        <td valign="top" colspan="2">
            <table border="0" class="table" width="100%" id="Table1" align="center" style="height:auto">
                <tr>
                    <td valign="top" align="left" style="width:100%" colspan="2">
                        <asp:Label ID="Label8" runat="server" Text="在当前岗位您可进行如下操作：" CssClass="left_ts"></asp:Label>
                    </td>
                </tr>
		      <tr>
		        <td class="edit_label"align = "left" style="width:85%">
		            <asp:Label ID="Label5" runat="server" Text="当前可进行的操作"></asp:Label>    
		        </td>
		        <td class="edit_label"align = "left" style="width:15%">
		            <asp:Label ID="Label7" runat="server" Text="操作"></asp:Label>    
		        </td>
		      </tr>
		      <div runat="server" id="divActionArea">
		      </div>
		      <div runat="server" id="divPathArea">
		      </div>
	        </table>
        </td>
    </tr>
</table>
</form>
</body>
</html>
