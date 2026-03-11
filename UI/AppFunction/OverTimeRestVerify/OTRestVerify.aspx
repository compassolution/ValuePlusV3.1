<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTRestVerify.aspx.cs" Inherits="AppFunction_OverTimeRestVerify_OTRestVerify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>加班调休核销</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
<script  src="../../common/js/waitProcess.js"></script>

<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>
</head>
<script language="javascript">
    function EnterDateTextBox()
      {
         if(event.keyCode == 13 && document.all["txtDate"].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["aPreview"].click();
         }
    }
</script>
<body>
<!--#include   file= "../../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
        <table id="tbAction" width="100%" style="height:72px" border="0" cellpadding="0" cellspacing="0" class="table" runat="server">
          <tr style="vertical-align:text-bottom; height:10%">
            <td align="center">
		        <div style="vertical-align:middle; float:right; height:70px; width:99%" class="topBox">
                    <table border="0" class="table" width="950%" align="center" style="height:auto;">
                       <tr>
                            <td> 
                                <table border="0" class="table" width="48%" align="center" style="height:auto;">
                                   <tr>
                                      <td class="edit_label"align = "center" style="width:40%">
                                        <asp:Label ID="lbInputDate" runat="server" Text="核销截止日期"></asp:Label>
                                      </td>
                                      <td style="width:60%">
                                         <asp:TextBox ID="txtDate" runat="server" Width="60%" onfocus="vpCalendar_ShowDate(this);"></asp:TextBox>YYYY-MM-DD
                                      </td>
                                   </tr>
                                  <tr height="18" align="center">
                                      <td align ="center" colspan="2">
                                        <asp:LinkButton ID="aPreview" runat="server" CssClass="a_Left" Font-Bold="true" onclick="aPreview_Click">预览核销记录</asp:LinkButton>
                                        <asp:LinkButton ID="aConfirm" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="aVerify_Click">确定进行核销</asp:LinkButton>
                                      </td>
                                  </tr>
                              </table>
                            </td>
                            <td>
                                <table border="0" class="table" width="48%" align="center" style="height:auto;">
                                   <tr>
                                      <td class="edit_label" align = "center" style="width:40%">
                                        <asp:Label ID="LabelOTTotal" runat="server" Text="加班合计小时数"></asp:Label>
                                      </td>
                                      <td style="width:10%"><asp:Label ID="lbOTTotal" runat="server" Text="" CssClass="left_ts"></asp:Label>
                                      </td>
                                      <td class="edit_label" align = "center" style="width:40%">
                                        <asp:Label ID="LabelOTHave" runat="server" Text="已核销调休小时数"></asp:Label>
                                      </td>
                                      <td style="width:10%"><asp:Label ID="lbOTHave" runat="server" Text="" CssClass="left_ts"></asp:Label>
                                      </td>
                                   </tr>
                                   <tr>
                                      <td class="edit_label" align = "center" style="width:40%">
                                        <asp:Label ID="LabelLVTotal" runat="server" Text="调休合计小时数"></asp:Label>
                                      </td>
                                      <td style="width:10%"><asp:Label ID="lbLVTotal" runat="server" Text="" CssClass="left_ts"></asp:Label>
                                      </td>
                                      <td class="edit_label" align = "center" style="width:40%">
                                        <asp:Label ID="LabelLVHave" runat="server" Text="已核销加班小时数"></asp:Label>
                                      </td>
                                      <td style="width:10%"><asp:Label ID="lbLVHave" runat="server" Text="" CssClass="left_ts"></asp:Label>
                                      </td>
                                   </tr>
                              </table>
                            </td>
                       </tr>
                       
                    </table>
                </div>
            </td>
          </tr>
        </table>
        <table id="tbOTLVList" width="100%" border="0" cellpadding="0" cellspacing="0" class="table" runat="server">
          <tr style="vertical-align:top"><td colspan="2">
                <asp:Label ID="lbTip1" runat="server" Text="请点击左边列表选择某一员工" CssClass="left_ts" Visible="false"></asp:Label> 
                <asp:Label ID="lbTip2" runat="server" Text="下面列表显示某员工（stuffName）的加班及调休记录：" CssClass="left_ts"></asp:Label> </td>
          </tr>
          <!-- 列表部分 -->  
          <tr style="vertical-align:top; height:90%" runat="server">
            <td align="center" valign="top" bgcolor="#F7F8F9" style ="width:54%; white-space:nowrap; vertical-align:top">
                <table class="warp_table" id="Table1" width="100%">
                    <tr style="vertical-align:top">
                        <td>
                            <asp:Label ID="lbTipOT" runat="server" Text="加班记录" CssClass="sec1"></asp:Label>
                            <img src="../../common/images/down_list.gif" width="14" height="14">
                        </td>
                    </tr>
                    <tr width="100%">
                        <td>
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="OTNO" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="OTDATEF" SortExpression="OTDATEF" HeaderText="OTDATEF" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
										<HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
<%--									<asp:BoundColumn DataField="OTDATET" SortExpression="OTDATET" HeaderText="OTDATET" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
										<HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>--%>
									<asp:BoundColumn DataField="OTTIME" SortExpression="OTTIME" HeaderText="OTTIME" DataFormatString="{0:N1}">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="OTCTIME" SortExpression="OTCTIME" HeaderText="OTCTIME" DataFormatString="{0:N1}">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="OTTYPE" SortExpression="OTTYPE" ReadOnly="True" HeaderText="OTTYPE">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="OTNO" SortExpression="OTNO" ReadOnly="True" HeaderText="OTNO">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
            <td align="center" valign="top" bgcolor="#F7F8F9" style ="width:44%; white-space:nowrap; vertical-align:top">
                <table class="warp_table" id="Table2" width="100%">
                    <tr style="vertical-align:top;">
                        <td>
                            <asp:Label ID="lbTipLV" runat="server" Text="调休记录" CssClass="sec1"></asp:Label>
                            <img src="../../common/images/down_list.gif" width="14" height="14">
                        </td>
                    </tr>
                    <tr width="100%">
                        <td>
                            <asp:DataGrid ID="DataGrid2" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="LVNO" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="LVDATEF" SortExpression="LVDATEF" HeaderText="LVDATEF" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
										<HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="LVDATET" SortExpression="LVDATET" HeaderText="LVDATET" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
										<HeaderStyle HorizontalAlign="Center" Width="35%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="LVTIME" SortExpression="LVTIME" HeaderText="LVTIME" DataFormatString="{0:N1}">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="LVSST" SortExpression="LVSST" HeaderText="LVSST" DataFormatString="{0:N1}">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="LVNO" SortExpression="LVNO" ReadOnly="True" HeaderText="LVNO">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
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
    </form>
</body>
</html>

<script language="javascript">
	//初始化结果表格
	DefineTableCss("Table1");
	DefineTableCss("Table2");
</script>
