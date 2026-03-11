<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingList.aspx.cs" Inherits="Flow_WorkFlow_PendingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>待办事项列表</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>

<script language="javascript">
    function EnterSearchTextBox(txtId)
      {
         if(event.keyCode == 13 && document.all[txtId].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["aSearch"].click();
         }
    }
    function EnterPageSizeTextBox()
      {
         if(event.keyCode == 13 && document.all["txtPageSize"].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["aToPageSize"].click();
         }
    }
    
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<table class="table">

    <tr>
        <td colspan="4" valign="top" align="center"><span class="left_bt" id="spanTitle" runat="server">流程处理业务待办事项列表</span><br>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table border="0" class="table" width="95%" id="tb1" align="center" style="height:auto">
		      <tr>
		        <td class="edit_label"align = "center" style="width:15%">
		            <asp:Label ID="Label11" runat="server" Text="流程类型"></asp:Label>
		        </td>
		        <td colspan="3">
				    <div id="divFlowDefine" runat="server">
                        <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
				    </div>
		        </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center" style="width:15%">
		            <asp:Label ID="Label3" runat="server" Text="移交人"></asp:Label>
		        </td>
		        <td style="width:25%">
		            <asp:TextBox ID="txtSendUser" runat="server" Width="200"></asp:TextBox>
                </td>
		        <td class="edit_label"align = "center" style="width:15%">
		            <asp:Label ID="Label1" runat="server" Text="移交时间"></asp:Label>    
		        </td>
		        <td style="width:45%">
		            From<asp:TextBox ID="txtSendDate_Start" runat="server" Width="160" onfocus="vpCalendar_ShowDate(this);"></asp:TextBox> To
		            <asp:TextBox ID="txtSendDate_End" runat="server" Width="160" onfocus="vpCalendar_ShowDate(this);"></asp:TextBox>
		        </td>
		      </tr>
		      <tr>
		        <td class="edit_label"align = "center">
		            <asp:Label ID="Label4" runat="server" Text="待办事项名称"></asp:Label>    
		        </td>
		        <td colspan="3">
		            <asp:TextBox ID="txtFlowName" runat="server" Width="85%"></asp:TextBox>
		        </td>
		      </tr>
		      <tr height="18" align="center">
                <TD align ="center" colspan="4" class="td_Frame2" >
                    <asp:LinkButton ID="aSearch" runat="server" CssClass="a_Left" OnClick="Search_Click">查    询</asp:LinkButton>
                    <asp:LinkButton ID="aReset" runat="server" CssClass="a_Left" OnClick="Reset_Click">重    置</asp:LinkButton>
                </TD>
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
                            HorizontalAlign="Center" AutoGenerateColumns="false" AllowSorting="True" ShowFooter="false" DataKeyField="SWORKFLOWCODE"
                            OnItemCreated = "DataGrid1_ItemCreated" OnItemDataBound = "DataGrid1_ItemDataBound"  OnItemCommand = "DataGrid1_ItemCommand"
                            OnSortCommand="DataGrid1_SortCommand">
                            <ItemStyle CssClass="tableContent" />
                            <HeaderStyle CssClass="tableTitle"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									<ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />										            
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SWORKFLOWNAME" SortExpression="SWORKFLOWNAME" ReadOnly="True" HeaderText="待办事项名称">
									<HeaderStyle HorizontalAlign ="left" Width="35%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="left" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SFLOWCODE" SortExpression="SFLOWCODE" HeaderText="待办类型">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SSOURPOSTNAMECN" SortExpression="SSOURPOSTNAMECN" HeaderText="当前岗位">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SSOURUSERNAME" SortExpression="SSOURUSERNAME" HeaderText="移交人">
									<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DTSOURDATE" SortExpression="DTSOURDATE" HeaderText="移交时间" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}">
									<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
								    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
								</asp:BoundColumn>
								
                                <asp:TemplateColumn HeaderText="操作">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="toDeal" CommandName="toDeal" runat="server" CssClass="a_Right"><asp:Label ID="Label_ToDeal" runat="server" Text="Label">进入处理</asp:Label></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </td>
    </tr>	
    <tr id = "trPageArea" valign="middle" runat="server">
		<td align="right" valign="middle" id="td3" runat="server" class="td_Frame2">
		    <div style="vertical-align:middle; float:right" class="bottomBox">
                <asp:LinkButton ID="aFirstPage" runat="server" CssClass="a_Right" OnClick="FirstPage_Click"><asp:Label ID="Label_FirstPage" runat="server" Text="Label">首页</asp:Label></asp:LinkButton>
                <asp:LinkButton ID="aPrePage" runat="server" CssClass="a_Right" OnClick="PrePage_Click"><asp:Label ID="Label_PrePage" runat="server" Text="Label">上一页</asp:Label></asp:LinkButton>
                <asp:LinkButton ID="aNextPage" runat="server" CssClass="a_Right" OnClick="NextPage_Click"><asp:Label ID="Label_NextPage" runat="server" Text="Label">下一页</asp:Label></asp:LinkButton>
                <asp:LinkButton ID="aLastPage" runat="server" CssClass="a_Right" OnClick="LastPage_Click"><asp:Label ID="Label_LastPage" runat="server" Text="Label">末页</asp:Label></asp:LinkButton>
		        <asp:Label ID="Label_Page1" runat="server" Text="Label">共</asp:Label>
		            <font color="red"><asp:Label ID="Label_AllCount" runat="server" Text="Label"></asp:Label></font>
		        <asp:Label ID="Label_Page2" runat="server" Text="Label">条记录</asp:Label>
		        ---<asp:Label ID="Label_Page3" runat="server" Text="Label">分</asp:Label>
		            <font color="red"><asp:Label ID="Label_AllPage" runat="server" Text="Label"></asp:Label></font>
		        <asp:Label ID="Label_Page4" runat="server" Text="Label">页</asp:Label>
		        ---<asp:Label ID="Label_Page5" runat="server" Text="Label">当前页</asp:Label>
		            <asp:DropDownList ID="DDList_CurPage" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="DDList_CurPage_SelectedIndexChanged" ></asp:DropDownList>
                ---<asp:Label ID="Label_Page6" runat="server" Text="Label">每页显示</asp:Label><asp:TextBox ID="txtPageSize" runat="server" Visible="true" Width="30px" BorderWidth="1"></asp:TextBox>
                <asp:LinkButton ID="aToPageSize" runat="server" CssClass="a_Right" OnClick="GoPageSize_Click"><asp:Label ID="Label_GO" runat="server" Text="Label">GO</asp:Label></asp:LinkButton>
		    </div>
        </td>
	</tr>
</table>
</form>
</body>
</html>

<script language="javascript">
	//初始化结果表格
	DefineTableCss("changecolor");
</script>