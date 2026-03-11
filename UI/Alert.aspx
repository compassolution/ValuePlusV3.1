<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Alert.aspx.cs" Inherits="Alert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<link href="common/css/main.css" rel="stylesheet" type="text/css" />
<link href="common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<script language="javascript" type="text/javascript">
    function OpenPendingList() {
        var url = "Archive/Pending/PendingList.aspx";
        window.open(url, 'PendingList', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no'); //最大化打开
    }
	
</script>
</head>
<body>
    <form id="form1" runat="server">
    <table id="changecolor" class="table">
        <tr>
            <td style="width:70%;font-weight:bold; " align="left">
                <img src="common/images/treeIcon/leaf.gif" /><asp:Label ID="lbPendingList" runat="server" Text="Label">待办事项</asp:Label>
                (<asp:Label ID="lbPendingCount" runat="server" Text="Label" ForeColor = "Red">记录条数</asp:Label>)
            </td>
		    <td>
                <asp:LinkButton ID="aToDealAlert" runat="server" CssClass="a_Right" OnClientClick = "OpenPendingList();">点击处理</asp:LinkButton>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
