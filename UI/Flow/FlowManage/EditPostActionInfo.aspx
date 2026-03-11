<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditPostActionInfo.aspx.cs" Inherits="Flow_FlowManage_EditPostActionInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>岗位活动明细信息</title>
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
		        <asp:Label ID="Label1" runat="server" Text="岗位活动明细信息"></asp:Label>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center" style="width:35%">
		        <asp:Label ID="Label11" runat="server" Text="流程定义编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtFlowId" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center" style="width:35%">
		        <asp:Label ID="Label2" runat="server" Text="流程岗位编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtPostCode" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="岗位活动编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="40"></asp:TextBox><font color=red>*</font>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="岗位活动英文名"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="100"></asp:TextBox><font color=red>*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="岗位活动中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="100"></asp:TextBox><font color=red>*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="活动内容"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox4" runat="server" Width="90%" MaxLength="1000" TextMode="MultiLine" Rows=4></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label7" runat="server" Text="活动前插件类名"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox5" runat="server" Width="90%" MaxLength="200" ></asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label8" runat="server" Text="活动后插件类名"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox6" runat="server" Width="90%" MaxLength="200" ></asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label9" runat="server" Text="顺序号"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox7" runat="server" Width="90%" MaxLength="9" ></asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label10" runat="server" Text="业务状态"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox8" runat="server" Width="90%" MaxLength="10" >0</asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  
		  <tr height="18" align="center">
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2" class="td_Frame2" >
                <asp:LinkButton ID="Button1" runat="server" CssClass="a_Left" onclick="Button1_Click">保    存</asp:LinkButton>
                <asp:LinkButton ID="Button2" runat="server" CssClass="a_Left" onclick="Button2_Click">返    回</asp:LinkButton>

            </TD>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
