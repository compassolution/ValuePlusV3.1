<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBBackUp.aspx.cs" Inherits="SystemManager_DBManager_DBBackUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
</head>
<body>

<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="Form1" method="post" runat="server">
<asp:HiddenField ID="hfPleaseSelectFile" runat="server" />
<asp:HiddenField ID="hfFileMaxSize" runat="server" />
    <table width="95%" border="0" cellpadding="0" cellspacing="0" class="table" align="center" style=" height:100%">
        <tr align="left">
		    <td align="left" id="tdTitle" runat="server" class="td_Frame1" colspan="8">
                <asp:Label ID="lbTitle" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr align="center" style="width:80%;height:98%;background-color:#C2DAF1;">
            <td style="width:45%;height:98%"  valign="top" align="right">                
                <asp:LinkButton ID="btnBackUp" runat="server" CssClass="a_Left" Font-Bold="true" OnClick = "btnBackUp_Click">BackUp DataBase</asp:LinkButton>
            </td>
        </tr>
        <tr align="center" style="width:80%">
            <td >
                <iframe runat="server" id="fileListFrame" name="fileListFrame" height="500" width="100%" frameborder="0" src="../../UpDownLoad/DownFileList.aspx?folder=DBBackUp&edit=1" style="border: 0px solid #cecece;" scrolling="auto"></iframe>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>