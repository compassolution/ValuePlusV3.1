<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SPContent.aspx.cs" Inherits="Tools_SP_SPContent"  ValidateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Content of Storage Procedure</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script  src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>
<style>
    .oText
    {
         border-top-width: 0px;
         border-right-width: 0px;
         border-bottom-width: 0px;
         border-left-width: 0px;
         border-collapse:collapse;
         color: forestgreen;
         font-size: 14px;
         font-weight:600;
         vertical-align: bottom;
         padding: 0px;
    }
</style>
<script language="javascript">

    var varFilterColTRFlag = 0;
    function showFilterColTR() {
        if (varFilterColTRFlag == 0) {
            this.trExcute.style.display = "";
            varFilterColTRFlag = 1;
        }
        else if (varFilterColTRFlag == 1) {
            this.trExcute.style.display = "none";
            varFilterColTRFlag = 0
        }

    }
</script> 
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
        <div class="topBox">
            <table class="warp_table_Template" align="center" width="100%" style="height:50px; padding-right:40px">
                <tr style="height:20px">
                    <td align="left">
                        <asp:Label ID="Label1" runat="server" Text="Data Object Name：" ForeColor="Red" Font-Size="Small"></asp:Label>
                        <asp:Label ID="Label2" runat="server" Text="Data Object Name：" ForeColor="Red" Font-Size="Small"></asp:Label>
                    </td>
                    <td align="right" style="padding-right:20px">
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="a_Right" OnClick="btnRefresh_Click">Refresh</asp:LinkButton>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="a_Right" OnClick="btnSave_Click">Save</asp:LinkButton>
                        <asp:LinkButton ID="btnExcute" runat="server" CssClass="a_Right" OnClientClick="showFilterColTR();">Execute</asp:LinkButton>
                    </td>
                </tr>
                <tr id="trExcute" style="display:none;">
                    <td colspan="2" align="right" style=" padding-right:20px">
                        <asp:TextBox ID="txtExcuteSp" runat="server"  Wrap="true" Width="70%"></asp:TextBox>
                        <asp:LinkButton ID="btnExcuteSp" runat="server" CssClass="a_Right" OnClick="btnExcuteSp_Click">Confirm Execute</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <table id="tbContent" class="warp_table_Template" align="center" width="100%" style="height:100%;margin-top:60px">
            <tr id="trSql" runat="server" style="height:100%">
                <td colspan="2" width="100%" style="height:100%"> 
                    <asp:TextBox ID="txtContent" runat="server" Rows="100" Width = "100%" Height="100%" TextMode="MultiLine" Wrap="true" CssClass="oText"></asp:TextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
<script type="text/javascript">
    var screenHeight = screen.availHeight;
    $(document).ready(function () {
        $("#tbContent").height(screenHeight - 120);
    });
</script>
</html>
