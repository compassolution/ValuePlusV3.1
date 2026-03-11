<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SPList.aspx.cs" Inherits="Tools_SP_SPList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<title>List of Storage Procedure</title>
</head>
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script type="text/javascript">
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
		<table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td valign="top" width="25%" height = "100%" align="center">
                    <table  width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rdBtnListObject" runat="server" RepeatDirection="Horizontal" 
                                    OnSelectedIndexChanged="rdBtnListObject_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="sp" Selected="True">Procedures</asp:ListItem>
                                    <asp:ListItem Value="fun" >Functions</asp:ListItem>
                                    <asp:ListItem Value="view" >Views</asp:ListItem>
                                </asp:RadioButtonList>    
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Search" Width="50%" runat="server"></asp:TextBox>
                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="a_Center" OnClick="btnFilter_Click">search</asp:LinkButton>
                                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="a_Center" OnClick="btnRefresh_Click">refresh</asp:LinkButton>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">              
				                <asp:ListBox ID="listBoxObjectList" runat="server"  Font-Size="Small" Width="100%" Height="750px" BackColor="#CAE1FF" AutoPostBack="true" onselectedindexchanged="listBoxObjectList_SelectedIndexChanged">
				                </asp:ListBox>
                            </td>
                        </tr>
                    </table>
                    
				</td>
				<td align="center" style="height:100%">
				    <iframe runat="server" id="FrmDetail" style="width:100%;border: 0px solid #CAE1FF;" frameborder="0" scrolling="auto" src="" ></iframe>
				</td>
			</tr>
		</table> 
    </form>
</body>
<script type="text/javascript">
    var screenHeight = screen.availHeight;
    $(document).ready(function () {
        $("#listBoxObjectList").height(screenHeight - 140);
        $("#FrmDetail").height(screenHeight - 140);
    });
</script>
</html>