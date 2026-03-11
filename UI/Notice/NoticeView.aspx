<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NoticeView.aspx.cs" Inherits="Notice_NoticeView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公告查看页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" class="warp_table" width="95%" id="tb1" align="center" >
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label_title" runat="server" Text="公告信息"></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center" style="width:15%">
		        <asp:Label ID="Label1" runat="server" Text="公告标题"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="98%" Enabled="false"></asp:TextBox>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label2" runat="server" Text="公告内容"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="98%" TextMode="MultiLine" Rows="27" Enabled="false"></asp:TextBox>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="发布人"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="98%"  Enabled="false"></asp:TextBox>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="发布时间"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox4" runat="server" Width="98%" Enabled="false"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="公告来源"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox5" runat="server" Width="98%" Enabled="false"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr><tr height="18" align="center">
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
		      <div class="top_table_area">
                  <asp:Button ID="Button1" runat="server" Text="关    闭" width="100px" 
                      height="25px" CssClass="btn_2k3" OnClientClick="window.close();"/>
		      </div>
            </TD>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
