<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostTransferPage.aspx.cs" Inherits="Flow_WorkFlow_PostTransferPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>流程岗位移交页面</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script>
    function selectReservedMemo(){
        var d = document.getElementById("<%=ddListMemo.ClientID %>");//根据DropDownList的客户端ID获取该控件
        var typeValue = d.options[d.selectedIndex].text;//获取DropDownList当前选中值
        document.getElementById("txtTrasferIdea").value = typeValue;

    }
    
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
        <td valign="top" align="center"><span class="left_bt" id="spanTitle" runat="server">流程处理业务转交页面</span><br>
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
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label4" runat="server" Text="当前岗位名称"></asp:Label>
		        </td>
		        <td colspan="3">
                    <asp:Label ID="lbCurPostName" runat="server" Text="当前岗位名称"></asp:Label>
                </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center" style="width:16%">
		            <asp:Label ID="Label3" runat="server" Text="下一岗位名称"></asp:Label>
		        </td>
		        <td style="width:34%">
                    <asp:Label ID="lbNextPostName" runat="server" Text="下一岗位名称"></asp:Label>
                </td>
		        <td class="edit_label"align = "center" style="width:16%">
		            <asp:Label ID="Label1" runat="server" Text="下一岗位处理人"></asp:Label>    
		        </td>
		        <td style="width:34%">
		            <asp:DropDownList id="ddListNextPostUserName" runat="server" Width="80%">
				    </asp:DropDownList>
		        </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label6" runat="server" Text="转岗预留意见"></asp:Label>    
		        </td>
		        <td colspan="3">
		            <asp:DropDownList id="ddListMemo" runat="server" Width="90%" onchange="javascript:selectReservedMemo();">
				    </asp:DropDownList>
		        </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label8" runat="server" Text="转岗意见"></asp:Label>    
		        </td>
		        <td colspan="3">
                    <asp:TextBox ID="txtTrasferIdea" runat="server" Width="90%" TextMode="MultiLine" Rows="3" MaxLength="500"></asp:TextBox>
		        </td>
		      </tr>
		      <tr height="18" align="center">
                <TD align ="center" colspan="4" class="td_Frame2" >
                    <asp:LinkButton ID="aTransfer" runat="server" CssClass="a_Right" OnClick="Transfer_Click">流程转交</asp:LinkButton>
                    <asp:LinkButton ID="aHisInfo" runat="server" CssClass="a_Right">查看历史流转信息</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" OnClick="Back_Click">返回流程处理页面</asp:LinkButton>
                </TD>
               </tr>
	        </table>
        </td>
    </tr>
</table>
</form>
</body>
</html>
