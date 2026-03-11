<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VacationSpList.aspx.cs" Inherits="Vacation_VacationSpList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<title>List of Storage Procedure</title>
</head>
<script  src="../common/js/waitProcess.js"></script>
<script>
    function reloadPage(){
        window.location.href=window.location.href;
    }
    
    function EnterSearchTextBox()
      {
         if(event.keyCode == 13)
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["LinkButton1"].click();
         }
    }
</script>

<body>
    <form id="form1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
    <tr>
	    <td width="100%" colspan="2">
		    <table width="100%"cellpadding="0" cellspacing="0" border="0">
			    <tr>
				  <td valign="top" width="25%" align="center">
                      <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                      <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Center" OnClick="btnFilter_Click">search</asp:LinkButton>
                      <asp:LinkButton ID="LinkButton2" runat="server" CssClass="a_Center" OnClick="btnRefresh_Click">refresh</asp:LinkButton>
				      <asp:ListBox ID="ListBox1" runat="server"  Font-Size="Small" Width="100%" Height="480px" BackColor="#CAE1FF" AutoPostBack="true" onselectedindexchanged="ListBox1_SelectedIndexChanged">
				      </asp:ListBox>
				  </td>
				  <td align="center">
				     <iframe runat="server" id="FrmDetail" style="height:450px;width:100%;border: 0px solid #CAE1FF;" frameborder="0" scrolling=auto src="" ></iframe>
				  </td>
			    </tr>
		    </table>
	    </td>
    </tr>
    </table>
    </form>
</body>
</html>
