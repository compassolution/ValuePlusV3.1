<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CopyrightInfo.aspx.cs" Inherits="Regist_CopyrightInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>版权信息页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
</head>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <div>
        <table border="0" class="warp_table" width="70%" id="tb1" align="center" style="word-break:break-all">
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:gray" colspan="2" align="center">
		        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center" style="width:25%">
		        <asp:Label ID="Label2" runat="server" Text="授权给："></asp:Label>
		    </td>
		    <td class="left_bt2" style="color:gray">
		        <asp:Label ID="Label_ClientName" runat="server" Text=""></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="授权码：" Width="98%"></asp:Label>
		    </td>
		    <td class="left_bt2" style="color:gray">
		        <asp:Label ID="Label_AssignStr" runat="server" Text=""  style="word-break:break-all;word-wrap:break-word;"></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="授权时间："></asp:Label>
		    </td>
		    <td class="left_bt2" style="color:gray">
		        <asp:Label ID="Label_AssignDate" runat="server" Text=""></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
	        <td class="edit_label" align = "center" colspan="2">
		       <font color="gray"><asp:Label ID="Label5" runat="server" Text="温馨提示" ></asp:Label></font>
	        </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr height="18" align="center">
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
                <TD align ="center" colspan="2" class="edit_label">
		          <div id="divButton" style="display:none" runat="server">
                      <asp:Button ID="Button1" runat="server" Text="注册获取授权" width="100px" 
                          height="25px" CssClass="btn_2k3" OnClick="Button1_Click"/>
		          </div>
                </TD>
                <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
