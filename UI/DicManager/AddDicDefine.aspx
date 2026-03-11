<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDicDefine.aspx.cs" Inherits="DicManager_AddDicDefine" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>新增清单定义项</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <div>
        <table border="0" class="warp_table" width="100%" id="tb1" style="display:" >
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2">
		        <asp:Label ID="Label1" runat="server" Text="新增清单定义项"></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" style="width:40%">
		        <asp:Label ID="Label3" runat="server" Text="清单定义编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="20"></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label">
		        <asp:Label ID="Label4" runat="server" Text="清单定义英文名"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="100"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label">
		        <asp:Label ID="Label5" runat="server" Text="清单定义中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="50"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr height="18" align="center">
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
		      <div class="top_table_area">
                  <asp:Button ID="Button1" runat="server" Text="保    存" width="100px" 
                      height="25px" CssClass="btn_2k3" onclick="Button1_Click"/>
                  <asp:Button ID="Button2" runat="server" Text="关    闭" width="100px" height="25px" CssClass="btn_2k3" OnClientClick="javascript:window.parent.close();"/>
		      </div>
            </TD>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
