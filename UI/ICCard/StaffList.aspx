<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StaffList.aspx.cs" Inherits="ICCard_StaffList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
    <title>员工列表</title>
</head>
<script  src="../common/js/waitProcess.js"></script>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
  <tr>
    <td valign="middle" background="../common/images/welcome/mail_leftbg.gif" style="width:1%">&nbsp;</td>
    <td valign="top" bgcolor="#F7F8F9" style="width:98%">
    <table width="98%" style="height:98%" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr>
        <td valign="top">&nbsp;</td>
      </tr>      
      <tr>
        <td colspan="4" valign="top" style="width:20%;height:100%">
		    <table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0">
    			<tr align="center">
	              <TD align ="left" >
                    <asp:DropDownList ID="ddListDept" runat="server" Width = "400" onselectedindexchanged="ddListDept_SelectedIndexChanged" AutoPostBack="true">

                    </asp:DropDownList>
	              </TD>
                </tr>
    			<tr align="center">
	                <TD align ="left" >
                        <asp:DropDownList ID="ddListCardUserType" runat="server" Width = "400" onselectedindexchanged="ddListCardUserType_SelectedIndexChanged" AutoPostBack="true">

                        </asp:DropDownList>
	                </TD>
                </tr>
			    <tr>
				    <td valign="top" width="20%" height = "100%" align="center">
                        <div style = "display:none">
                            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="a_Center" OnClick="btnRefresh_Click">重新加载数据</asp:LinkButton>
                        </div>

                        <asp:CheckBox ID="cBoxIsFM" runat="server"  AutoPostBack=true OnCheckedChanged = "cBoxIsFM_OnCheckedChanged" />
                        <asp:Label ID="lbFM" runat="server" Text="员工家属" ForeColor="Red" Font-Bold="true"></asp:Label>
                        <asp:TextBox ID="txtStaff" runat="server" Width = "100"></asp:TextBox>
                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="a_Center" OnClick="btnSearch_Click">查找</asp:LinkButton>
				    </td>
			    </tr>
			    <tr>                    
				  <td valign="top" width="20%" height = "100%" align="center">
				      <asp:ListBox ID="lstBoxStaff" runat="server"  Font-Size="Small" Width="100%" Height="400px" BackColor="#CAE1FF" AutoPostBack="true"
                         onselectedindexchanged="lstBoxStaff_SelectedIndexChanged">
				      </asp:ListBox>
				  </td>
		          <td valign="top" style="height:100%">
		              <table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0">
			              <tr valign="top">
                              <td valign="top">
                                <iframe runat="server" id="FrmDetail" style="height:350px;width:100%;border: 0px solid #CAE1FF;" frameborder="0" scrolling=auto src="" ></iframe>
                              </td>
                          </tr>
		              </table>
		          </td>
			    </tr>

		    </table>
	    </td>
    </tr>
    </table>
    </td>
    <td background="../common/images/welcome/mail_rightbg.gif" style="width:1%">&nbsp;</td>
  </tr>
  <tr>
    <td valign="bottom" background="../common/images/welcome/mail_leftbg.gif"><img src="../common/images/welcome/buttom_left2.gif" width="17" height="17" /></td>
    <td background="../common/images/welcome/buttom_bgs.gif"><img src="../common/images/welcome/buttom_bgs.gif" width="17" height="17"></td>
    <td valign="bottom" background="../common/images/welcome/mail_rightbg.gif"><img src="../common/images/welcome/buttom_right2.gif" width="16" height="17" /></td>
  </tr>
</table>
    </form>
</body>
</html>
