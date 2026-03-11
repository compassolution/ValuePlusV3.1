<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAutoItem.aspx.cs" Inherits="SystemManager_AutoCode_AddAutoItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>新增自动编号项目</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script> 
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
    <div>
        <table border="0" class="warp_table" width="100%" id="tb1" style="display:" >
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2">
		        <asp:Label ID="Label1" runat="server" Text="新增自动编号项"></asp:Label>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" style="width:30%" align=center>
		        <asp:Label ID="Label2" runat="server" Text="自动编号编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="20"></asp:TextBox><font color=red>*</font>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label3" runat="server" Text="自动编号英文名"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="100"></asp:TextBox><font color=red>*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label4" runat="server" Text="自动编号中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="50"></asp:TextBox><font color=red>*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label5" runat="server" Text="自动编号前缀"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox4" runat="server" Width="90%" MaxLength="10"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label6" runat="server" Text="日期类型"></asp:Label>
		    </td>
		    <td>
		        <asp:dropdownlist id="DropDownList1" runat="server">
					<asp:ListItem Selected="True"></asp:ListItem>
					<asp:ListItem Value="YYMM">YYMM</asp:ListItem>
					<asp:ListItem Value="YYYYMM">YYYYMM</asp:ListItem>
					<asp:ListItem Value="YY">YY</asp:ListItem>
					<asp:ListItem Value="YYYY">YYYY</asp:ListItem>
					<asp:ListItem Value="YYMMDD">YYMMDD</asp:ListItem>
					<asp:ListItem Value="YYYYMMDD">YYYYMMDD</asp:ListItem>
				</asp:dropdownlist>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label7" runat="server" Text="自动编号长度"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox5" runat="server" Width="90%" MaxLength="4"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label8" runat="server" Text="下一个编号"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox6" runat="server" Width="90%" MaxLength="4"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align=center>
		        <asp:Label ID="Label9" runat="server" Text="最后日期"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox7" runat="server" Width="90%" MaxLength="8"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr height="18" align="center">
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
		      <div class="top_table_area">
                  <asp:Button ID="Button1" runat="server" Text="保    存" width="100px" 
                      height="25px" CssClass="btn_2k3" onclick="Button1_Click"/>
                  <asp:Button ID="Button2" runat="server" Text="关    闭" width="100px" height="25px" CssClass="btn_2k3" OnClientClick="javascript:window.parent.close();"/>
		      </div>
            </TD>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
