<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetPostActor.aspx.cs" Inherits="Flow_FlowManage_SetPostActor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>选择参与者</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
          <!-- 操作行 -->
          <tr height="30" align="center">
  	        
            <TD align ="left" style="color:Blue" class="td_Frame2" >
		        <div style="vertical-align:middle; float:right" class="topBox">
                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="a_Left" onclick="Button2_Click">返    回</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" onclick="Button1_Click">选择后确定</asp:LinkButton>
		            <asp:Label ID="Label1" runat="server" Text="选择岗位参与者，当前岗位编码："></asp:Label>
		            <asp:Label ID="lbPostCode" runat="server" Text="："></asp:Label>
		        </div>
            </TD>
  	        
          </tr>
          
          <!-- 列表部分 -->  
          <tr>
            
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="SUSERID"   OnSortCommand = "DataGrid1_SortCommand" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:TemplateColumn HeaderText="选择">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" />										            
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="SUSERID" SortExpression="SUSERID" ReadOnly="True" HeaderText="用户编码">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SUSERNAME" SortExpression="SUSERNAME" HeaderText="用户英文名">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SUSERNAMECN" SortExpression="SUSERNAMECN" HeaderText="用户中文名">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SDEPT" SortExpression="SDEPT" HeaderText="部门名称">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SDEPTCN" SortExpression="SDEPTCN" HeaderText="部门中文名">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPOSI" SortExpression="SPOSI" HeaderText="职位名称">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPOSICN" SortExpression="SPOSICN" HeaderText="职位中文名">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
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
	DefineTableCssNoCursorOver("changecolor");
</script>
