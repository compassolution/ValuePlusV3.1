<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditFormDefine.aspx.cs" Inherits="Flow_FormManage_EditFormDefine" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>表单定义明细页</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" />
<script  src="../../common/js/waitProcess.js"></script>
</head>
<body>
    
<!--#include   file= "../../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <div>
        <table border="0" class="table" width="95%" id="tb1" align="center" style="height:auto">
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label1" runat="server" Text="表单定义明细信息"></asp:Label>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="表单定义编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="40"></asp:TextBox><font color=red>*</font>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="表单定义英文名"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="200"></asp:TextBox><font color=red>*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="表单定义中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="200"></asp:TextBox><font color=red>*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="表单英文描述"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox4" runat="server" Width="90%" MaxLength="500" TextMode="MultiLine" Rows = "3"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label7" runat="server" Text="表单中文描述"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox5" runat="server" Width="90%" MaxLength="500" TextMode="MultiLine" Rows = "3"></asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label8" runat="server" Text="保存后执行插件"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox6" runat="server" Width="90%" MaxLength="500"></asp:TextBox><BR>
		        <asp:Label ID="Label2" runat="server" Text="需加前缀（CLASS：类；SP：存储过程；URL：页面）" CssClass="left_txt"></asp:Label>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label9" runat="server" Text="是否进行版本控制"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList1" runat="server" Width="200px">
					<asp:ListItem Value="0">否</asp:ListItem>
					<asp:ListItem Value="1">是</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label10" runat="server" Text="是否停用"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList2" runat="server" Width="200px">
					<asp:ListItem Value="0">否</asp:ListItem>
					<asp:ListItem Value="1">是</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  
		  <tr height="18" align="center">
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
                  <asp:Button ID="Button1" runat="server" Text="保    存" width="100px" 
                      height="25px" CssClass="btn_2k3" OnClick="Button1_Click"/>
                  <asp:Button ID="Button2" runat="server" Text="返    回" width="100px" height="25px" CssClass="btn_2k3" OnClick="Button2_Click" />

            </TD>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
