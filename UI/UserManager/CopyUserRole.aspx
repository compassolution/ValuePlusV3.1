<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CopyUserRole.aspx.cs" Inherits="UserManager_CopyUserRole" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<title>Copy User Role</title>
</head>
<script  src="../common/js/waitProcess.js"></script>
<script>
    function RefreshOpener() {
        alert('Successfully Copy!');
        window.close();
        if (window.opener != null) {
            window.opener.location.href = window.opener.location.href;
        }
    }
    
    function EnterSearchTextBox()
      {
         if(event.keyCode == 13)
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["btnFilter"].click();
         }
    }
</script>

<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
    <tr>
	    <td width="100%" height="100%" colspan="2">
		    <table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0">
			    <tr>
				  <td valign="top" width="40%" height = "100%" align="center">
                      <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                      <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Center" OnClick="btnFilter_Click">search</asp:LinkButton>
                      <asp:LinkButton ID="btnCopy" runat="server" CssClass="a_Center" OnClick="btnCopy_Click">copy</asp:LinkButton>
                      <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Center" OnClientClick="window.close();">close</asp:LinkButton>
				      </asp:ListBox>
				  </td>
				</tr>
			    <tr>
				  <td valign="top" width="40%" height = "100%" align="center">
				      <asp:ListBox ID="ListBox1" runat="server"  Font-Size="Small" Width="90%" Height="500px" BackColor="#CAE1FF" AutoPostBack="false">
				      </asp:ListBox>
				  </td>
				</tr>
		    </table>
	    </td>
    </tr>
    </table>
    </form>
</body>
</html>
