<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserDetail.aspx.cs" Inherits="UserManager_UserDetail"  EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>用户信息明细页</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script language="javascript" type="text/javascript">
	function setUserId(){
	    document.getElementById('TextBox1').value = document.getElementById('TextBox2').value;
	}
	function RefreshUserList() {
	    if (window.parent != null) {
	        if (window.parent.document.getElementById("BtnRefresh") != null) {//用户列表页面的重新加载按钮
	            window.parent.document.getElementById("BtnRefresh").click();
	        }
	    }

	}
	function UserListPageAddUser() {
	    if (window.parent != null) {
	        if (window.parent.document.getElementById("btnAddUser") != null) {//用户列表页面的新增用户按钮
	            window.parent.document.getElementById("btnAddUser").click();
	        }
	    }

	}
</script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfIsRequiredMatchStaffNo" runat ="server" />
    <div>
		<div style="display:none">
            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="a_Center" OnClick="btnRefresh_Click">重新加载数据</asp:LinkButton>
  		</div>
        <table border="0" class="warp_table" width="95%" id="tb1" align="center" style="height:auto">
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label1" runat="server" Text="系统用户明细信息"></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="用户编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="40" ReadOnly=true></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="登录名"></asp:Label>    <font color="red">*</font>
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="30"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="登录密码"></asp:Label><font color="red">*</font>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="16"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label2" runat="server" Text="所在部门"></asp:Label><font color="red" id="fontRed_Dept" style="display:none">*</font>
		    </td>
		    <td>
                <asp:DropDownList ID="ddListDept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddListDept_SelectedIndexChanged">
                </asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label9" runat="server" Text="所在部门英文名"></asp:Label> 
		    </td>
		    <td><asp:TextBox ID="TextBox7" runat="server" Width="90%" MaxLength="40"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label10" runat="server" Text="所在部门中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox8" runat="server" Width="90%" MaxLength="40"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="对应员工编号"></asp:Label><font color="red" id="fontRed_StaffNo" style="display:none">*</font>
		    </td>
		    <td>
		        <%--<asp:TextBox ID="TextBox4" runat="server" Width="90%" MaxLength="20"></asp:TextBox>--%>
                <asp:DropDownList ID="ddListDCNO" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddListDCNO_SelectedIndexChanged">
                </asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label7" runat="server" Text="用户英文名"></asp:Label><font color="red">*</font>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox5" runat="server" Width="90%" MaxLength="50"></asp:TextBox>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label8" runat="server" Text="用户中文名"></asp:Label><font color="red">*</font>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox6" runat="server" Width="90%" MaxLength="50"></asp:TextBox>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label11" runat="server" Text="职位英文名" ></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox9" runat="server" Width="90%" MaxLength="40"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label12" runat="server" Text="职位中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox10" runat="server" Width="90%" MaxLength="40"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label13" runat="server" Text="是否重新分配"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList1" runat="server" Width="200px">
					<asp:ListItem Value="0">否</asp:ListItem>
					<asp:ListItem Value="1">是</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label14" runat="server" Text="是否集团用户"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList2" runat="server" Width="200px" Enabled="false">
					<asp:ListItem Value="0">否</asp:ListItem>
					<asp:ListItem Value="1">是</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
<%--		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label16" runat="server" Text="用户类型"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="ddListUserType" runat="server" Width="200px">
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>--%>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label15" runat="server" Text="是否停用"></asp:Label><font color="red">*</font>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList3" runat="server" Width="200px">
					<asp:ListItem Value="0">否</asp:ListItem>
					<asp:ListItem Value="1">是</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  
		  <tr height="30" align="center">
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
                  <asp:LinkButton ID="BtnSave" runat="server" CssClass="a_Center" OnClick="BtnSave_Click" >修    改</asp:LinkButton>
                  <asp:LinkButton ID="BtnDelete" runat="server" CssClass="a_Center" OnClick="BtnDelete_Click" >Delete</asp:LinkButton>
                  <asp:LinkButton ID="BtnAdd" runat="server" CssClass="a_Center" OnClick="BtnAdd_Click" >新    增</asp:LinkButton>
            </TD>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
    <script language="javascript">
        if (document.getElementById("hfIsRequiredMatchStaffNo").value == '1') {
            document.getElementById("fontRed_Dept").style.display = "";
            document.getElementById("fontRed_StaffNo").style.display = "";
        }
    </script>
</body>
</html>

