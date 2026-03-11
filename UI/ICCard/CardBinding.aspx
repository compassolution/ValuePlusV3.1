<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardBinding.aspx.cs" Inherits="ICCard_CardBinding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
    <title>发放员工卡</title>
</head>
<script  src="../common/js/waitProcess.js"></script>
<script>
    function doRefresh(msg) {
        if (window.parent != null) {
            if (window.parent.document.getElementById("btnRefresh") != null) {
                window.parent.document.all["btnRefresh"].click();
            }
        }
        alert(msg);
    }
</script>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
  <tr>
    <td valign="top" style="width:98%">
    <table width="98%" style="height:98%" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr style="height:60px">
        <td valign="top">&nbsp;</td>
      </tr>     
      <tr>
        <td colspan="4" valign="top" style="width:20%;height:100%">
		    <table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0" class="table">    			
			   <tr>
                <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		        <td colspan="2">
                    <span class="left_bt" id="spanTitle" runat="server">员工卡发放</span><br>
                </td>
                <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		      </tr>
		      <tr>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		        <td class="edit_label" align = "center" style="width:30%">
		            <asp:Label ID="lbCardNo" runat="server" Text="">员工卡号</asp:Label><font color=red>*</font>
		        </td>
		        <td>
		            <asp:TextBox ID="txtCardNo" runat="server" Width="90%" MaxLength="40" ReadOnly="false" ForeColor="Red" Font-Bold="true" Font-Size="XX-Large">
                    </asp:TextBox>
                </td>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		      </tr>
		      <tr>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		        <td class="edit_label" align = "center" style="width:30%">
		            <asp:Label ID="Label1" runat="server" Text="">卡面序号</asp:Label><font color=red>*</font>
		        </td>
		        <td>
		            <asp:TextBox ID="txtCardSeq" runat="server" Width="90%" MaxLength="40"></asp:TextBox>
                </td>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		      </tr>
		      <tr>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		        <td class="edit_label" align = "center" style="width:30%">
		            <asp:Label ID="lbDCNO" runat="server" Text="">员工编号></asp:Label><font color=red>*</font>
		        </td>
		        <td>
		            <asp:TextBox ID="txtDCNO" runat="server" Width="90%" MaxLength="40" ReadOnly=true ></asp:TextBox>
                </td>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		      </tr>
		      <tr>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		        <td class="edit_label" align = "center" style="width:30%">
		            <asp:Label ID="lbNAME" runat="server" Text="">员工姓名</asp:Label><font color=red>*</font>
		        </td>
		        <td>
		            <asp:TextBox ID="txtName" runat="server" Width="90%" MaxLength="40" ReadOnly=true ></asp:TextBox>
                </td>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		      </tr>
		      <tr>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		        <td class="edit_label" align = "center" style="width:30%" colspan = "2" >
                      <asp:LinkButton ID="btnBinding" runat="server" CssClass="a_Center" OnClick="btnConfirm_Click">确定发放</asp:LinkButton>

                      <asp:LinkButton ID="btnRecover" runat="server" CssClass="a_Center" OnClick="btnRecover_Click" Visible ="false">回收家属卡</asp:LinkButton>
                </td>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		      </tr>
		    </table>
	    </td>
    </tr>
    </table>
    </td>
  </tr>
</table>
</form>
</body>
</html>
