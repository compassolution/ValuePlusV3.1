<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTRestVerifyIndex.aspx.cs" Inherits="AppFunction_OverTimeRestVerify_OTRestUserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>加班调休核销首页</title>
<script  src="../../common/js/waitProcess.js"></script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
    <tr>
	    <td width="100%" height="100%" colspan="2">
		    <table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0">
			    <tr style="width:100%">
				  <td valign="top" width="200" height = "100%">
				    <table width="100%">
				        <tr>
				            <td>
				                <asp:TextBox ID="TextBox1" runat="server" Width="80%" MaxLength="40" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
				                <asp:ImageButton runat="server" ImageUrl="../../common/images/search1.png" style="cursor:hand" onclick="TextBox1_TextChanged"></asp:ImageButton>
				            </td>
				        </tr>
				        <tr>
				            <td>
				                <asp:ListBox ID="ListBox1" runat="server"  Font-Size="Small" Width="100%" Height="430px" BackColor="" AutoPostBack="true" onselectedindexchanged="ListBox1_SelectedIndexChanged">
					            </asp:ListBox>
					        </td>
					    </tr>
				    </table>
				  </td>
				  <td align="center" style="height:100%; vertical-align:top; width:auto">
				     <iframe runat="server" id="frmVerify" style="height:430px;width:100%;border: 0px solid #cecece;" frameborder="0" scrolling=auto src="OTRestVerify.aspx" ></iframe>
				  </td>
			    </tr>
		    </table>
	    </td>
    </tr>
    </table>
    </form>
</body>
</html>

