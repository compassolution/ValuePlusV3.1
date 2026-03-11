<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardTopUp.aspx.cs" Inherits="ICCard_CardTopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
    <title>员工卡充值</title>
</head>
<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/tableStyle.js"></script>
<script language=javascript>
    function getCardInfo() {
        document.getElementById("btnGetInfo").click();
    }
</script>
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
        <td valign="top" align ="center" style="width:60%;height:100%">
		    <table width="60%" class="table">
    			<tr align="center">
		            <td class="edit_label" align = "center" style="width:20%">
		                <asp:Label ID="Label3" runat="server" Text="">员工卡号</asp:Label>
		            </td>
	                <td align ="left" colspan = "3" style="width:80%">                  
		                <asp:TextBox ID="txtCardNo" runat="server" Width="400" MaxLength="10" ReadOnly="false" ForeColor="Red" Font-Bold="true" Font-Size="XX-Large" onchange ="javascript:getCardInfo();" >
                        </asp:TextBox>
		                <asp:LinkButton ID="btnGetInfo" runat="server" CssClass="a_Center" OnClick="btnGetInfo_Click">获取信息</asp:LinkButton>
	                </td>
                </tr>
                <tr align="center">
		            <td class="edit_label" align = "center" style="width:20%">
		                <asp:Label ID="Label1" runat="server" Text="">持卡员工</asp:Label>
		            </td>
	                <td align ="left" colspan = "3" style="width:80%">                  
		                <asp:Label ID="lbStaff" runat="server" Text=""></asp:Label>
	                </td>
                </tr>
                <tr align="center">
		            <td class="edit_label" align = "center" style="width:20%">
		                <asp:Label ID="Label4" runat="server" Text="">当前余额</asp:Label>
		            </td>
	                <td align ="left" colspan = "3" style="width:80%">     
                        <asp:Label ID="Label5" runat="server" Text="￥" ForeColor="Blue" Font-Bold="true" Font-Size="X-Large"></asp:Label>             
		                <asp:Label ID="lbBalance" runat="server" Text="" ForeColor="Blue" Font-Bold="true" Font-Size="X-Large"></asp:Label>
	                </td>
                </tr>
                <tr align="center">
		            <td class="edit_label" align = "center">
		                <asp:Label ID="Label2" runat="server" Text="">充值金额</asp:Label>
		            </td>
	                <td align ="left">                                      
                        <asp:DropDownList ID="ddListAmount" runat="server" Width = "100" AutoPostBack=false>
					        <asp:ListItem Value="10">￥10.00</asp:ListItem>
					        <asp:ListItem Value="20">￥20.00</asp:ListItem>
					        <asp:ListItem Value="30">￥30.00</asp:ListItem>
					        <asp:ListItem Value="40">￥40.00</asp:ListItem>
					        <asp:ListItem Value="50">￥50.00</asp:ListItem>
					        <asp:ListItem Value="100" Selected>￥100.00</asp:ListItem>
					        <asp:ListItem Value="200">￥200.00</asp:ListItem>
					        <asp:ListItem Value="300">￥300.00</asp:ListItem>
                        </asp:DropDownList>
                        <asp:LinkButton ID="btnTopUp" runat="server" CssClass="a_Center" OnClick="btnTopUp_Click" Visible = "false">确定充值</asp:LinkButton>
	                </td>
                </tr>
		    </table>
	    </td>
      </tr>
      <tr align="center">
	    <TD align ="left" >
                  
        <div id="div1" runat="server" class="titlebt" style="float:left">
            <span class="left_ts">充值记录</span>
        </div>
	    </TD>
      </tr>
      <tr>
         <td align="center" valign="top" bgcolor="#F7F8F9">	    
            <table id="changecolor" width="100%" class="tableNoHover">
                <tr width="100%">
                    <td style ="width:98%; ">
                        <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="false"  Width="100%"
                            AutoGenerateColumns="false"  onHorizontalAlign="Center" >
                            <HeaderStyle CssClass="tableTitle"></HeaderStyle>
								<ItemStyle CssClass="tableContent"  />
                            <Columns>
								<asp:BoundColumn DataField="CARDNO" SortExpression="CARDNO" ReadOnly="True" HeaderText="卡号">
									<HeaderStyle HorizontalAlign ="Center" Width="15%"></HeaderStyle> 
									<ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SEQNO" SortExpression="SEQNO" HeaderText="序号">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									<ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="NAMOUNT" SortExpression="NAMOUNT" HeaderText="充值金额">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									<ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="NBALANCE" SortExpression="NBALANCE" HeaderText="充值后余额">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									<ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SYSTIME" SortExpression="SYSTIME" HeaderText="充值时间"  DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									<ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SYSUSER" SortExpression="SYSUSER" HeaderText="充值操作人">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									<ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DCNO" SortExpression="DCNO" HeaderText="持卡员工">
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

<script language="javascript">
    //初始化结果表格
    DefineTableCss("changecolor");
</script>