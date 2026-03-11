
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="UserManager_UserList" enableeventvalidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<title>用户管理界面用户列表</title>
</head>
<script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script  src="../common/js/waitProcess.js" type="text/javascript"></script>
<script>
    function reloadPage(){
        //window.location.href = window.location.href;
        document.getElementById["BtnRefresh"].click();
    }
    
    function EnterSearchTextBox()
      {
         if(event.keyCode == 13)
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["BtnSearch"].click();
         }
    }
</script>

<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
    <table id="tbMain" width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
    <tr>
	    <td width="100%" colspan="2">
		    <table width="100%" cellpadding="0" cellspacing="0" border="0">
    			<tr height="18" align="center">
	              <TD align ="left" colspan="2">
				      <div style="display:none">
                          <asp:DropDownList ID="DropDownList1" runat="server" Width = "400" 
                              onselectedindexchanged="DropDownList1_SelectedIndexChanged" AutoPostBack=true>
                            <asp:ListItem Value="0">------->酒店用户</asp:ListItem>
					        <asp:ListItem Value="1">------->集团用户</asp:ListItem>
                          </asp:DropDownList>
                          <asp:LinkButton ID="BtnRefresh" runat="server" CssClass="a_Center" OnClick="BtnRefresh_Click">重新加载数据</asp:LinkButton>
                          <asp:Label ID="Label1" runat="server" Text="温馨提示" class="left_bt2" style="color:red"></asp:Label>
  				      </div>
	              </TD>
                </tr>
			    <tr>
				  <td valign="top" width="40%" height = "100%" align="center">
                      <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                      <asp:LinkButton ID="BtnSearch" runat="server" CssClass="a_Center" OnClick="BtnSearch_Click">Search</asp:LinkButton>
                      <asp:LinkButton ID="btnAddUser" runat="server" CssClass="a_Center" OnClick="btnAddUser_Click">Add</asp:LinkButton>
				      <asp:ListBox ID="ListBox1" runat="server"  Font-Size="Small" Width="100%" BackColor="#CAE1FF" AutoPostBack="true" onselectedindexchanged="ListBox1_SelectedIndexChanged">
				      </asp:ListBox>
				  </td>
				  <td align="center" style="height:100%">
				     <iframe runat="server" id="FrmDetail" style="height:500px;width:100%;border: 0px solid #CAE1FF;" frameborder="0" scrolling=auto src="" ></iframe>
				  </td>
			    </tr>
		    </table>
	    </td>
    </tr>
    </table>
</form>
</body>
<script type="text/javascript">
    var screenHeight = screen.availHeight;
    $(document).ready(function () {
        $("#tbMain").height(screenHeight - 320);
        $("#ListBox1").height(screenHeight - 320);
        $("#FrmDetail").height(screenHeight - 320);
    });
</script>
</html>
