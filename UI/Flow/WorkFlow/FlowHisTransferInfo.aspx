<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowHisTransferInfo.aspx.cs" Inherits="Flow_WorkFlow_FlowHisTransferInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>流程流转历史明细表</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>

<script>
  window.moveTo(0,0);  
  window.resizeTo(screen.availWidth,screen.availHeight);
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">

<table class="table">

    <tr>
        <td colspan="4" valign="top" align="center" class="td_Frame2" ><span class="left_bt" id="spanTitle" runat="server">流程流转历史明细表</span>
            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" OnClientClick="window.close();">关    闭</asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td valign="top" colspan="2">
            <table border="0" class="table" width="95%" id="tb1" align="center" style="height:auto">
		      <tr>
		        <td class="edit_label"align = "center" style="width:15%">
		            <asp:Label ID="Label2" runat="server" Text="待办事项名称"></asp:Label>    
		        </td>
		        <td colspan="3">
                    <asp:Label ID="lbFlowName" runat="server" Text="待办事项名称"></asp:Label>
		        </td>
		      </tr>
	        </table>
        </td>
    </tr>
    <tr id="trGrid" runat="server">
        <td valign="top" bgcolor="#F7F8F9" align="left" style="white-space:nowrap">
            <table class="warp_table" id="changecolor" width="100%">
                <tr width="100%">
                    <td>
                        <asp:DataGrid ID="DataGrid1" BorderWidth="0" CellPadding="0" CellSpacing="0" runat="server" Visible="true" Width="100%" AllowPaging="false" 
                            HorizontalAlign="Center" AutoGenerateColumns="false" AllowSorting="True" ShowFooter="false" DataKeyField="SWFDCODE">
                            <ItemStyle CssClass="tableContent" />
                            <HeaderStyle CssClass="tableTitle"></HeaderStyle>
                            <Columns>
								<asp:BoundColumn DataField="NNUMBER" SortExpression="NNUMBER" HeaderText="序号">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SSOURPOSTNAME" SortExpression="SSOURPOSTNAME" HeaderText="移交岗位">
									<HeaderStyle HorizontalAlign ="left" Width="20%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="left" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SSOURUSERNAME" SortExpression="SSOURUSERNAME" HeaderText="移交人">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DTSOURDATE" SortExpression="DTSOURDATE" HeaderText="移交时间" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SDESTPOSTNAME" SortExpression="SDESTPOSTNAME" HeaderText="接收岗位">
									<HeaderStyle HorizontalAlign ="left" Width="20%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="left" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SDESTUSERNAME" SortExpression="SDESTUSERNAME" HeaderText="接收人">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DTDESTDATE" SortExpression="DTDESTDATE" HeaderText="接收时间" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SMEMO" SortExpression="SMEMO" HeaderText="移交意见">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
      <tr height="18" align="center">
        <TD align ="center" colspan="4" class="td_Frame2" >
            <asp:LinkButton ID="aReset" runat="server" CssClass="a_Left" OnClientClick="window.close();">关    闭</asp:LinkButton>
        </TD>
       </tr>
</table>
</form>
</body>
</html>

<script language="javascript">
	//初始化结果表格
	DefineTableCss("changecolor");
</script>