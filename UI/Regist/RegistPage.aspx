<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistPage.aspx.cs" Inherits="Regist_RegistPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>软件注册授权</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <div>
        <table border="0" class="warp_table" width="70%" id="tb1" align="center" style="height:auto">
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label_Title" runat="server" Text="注册授权页面"></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center" style="width:25%">
		        <asp:Label ID="Label1" runat="server" Text="授权客户"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="100"  BackColor="YellowGreen"></asp:TextBox><font color="red">*</font>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label2" runat="server" Text="联系人"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="20"  BackColor="YellowGreen"></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="联系方式"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="50" BackColor="YellowGreen"></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="集团名称"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox4" runat="server" Width="90%" MaxLength="100" BackColor="YellowGreen"></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="注册码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox5" runat="server" Width="90%" MaxLength="1000" TextMode="MultiLine" Rows = "3" BackColor="YellowGreen" ReadOnly="true"></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="授权码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox6" runat="server" Width="90%" MaxLength="1000" TextMode="MultiLine" Rows = "3" BackColor="YellowGreen"></asp:TextBox><font color="red">*</font>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr height="18" align="center">
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
		      <div class="top_table_area">
                  <asp:Button ID="Button1" runat="server" Text="获取授权" width="100px" 
                      height="25px" CssClass="btn_2k3" onclick="Button1_Click"  />
                  <asp:Button ID="Button2" runat="server" Text="重    置" width="100px" height="25px" CssClass="btn_2k3"   />
		      </div>
            </TD>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
