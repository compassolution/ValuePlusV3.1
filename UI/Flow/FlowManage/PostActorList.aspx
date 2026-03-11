<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostActorList.aspx.cs" Inherits="Flow_FlowManage_PostActorList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>岗位参与者列表</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" />
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
                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="a_Left" onclick="Button2_Click">返    回</asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" onclick="Button1_Click">新    增</asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="a_Left" onclick="Button3_Click">岗位活动列表</asp:LinkButton>
                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="a_Left" onclick="Button4_Click">下岗位列表</asp:LinkButton>
		        <asp:Label ID="Label1" runat="server" Text="岗位参与者列表，当前岗位编码："></asp:Label>
		        <asp:Label ID="lbPostCode" runat="server" Text="："></asp:Label>
            </TD>
  	        
          </tr>
          
          <!-- 列表部分 -->  
          <tr>
            
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="SACTORID"  OnItemCreated="DataGrid1_ItemCreated" OnSortCommand = "DataGrid1_SortCommand"
                                ondeletecommand="DataGrid1_DeleteCommand" onHorizontalAlign="Center" >
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
									<asp:BoundColumn DataField="NINDEX" SortExpression="NINDEX" ReadOnly="True" HeaderText="序号">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SUSERID" SortExpression="SUSERID" ReadOnly="True" HeaderText="参与者编码">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SUSERNAME" SortExpression="SUSERNAME" HeaderText="参与者英文名">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SUSERNAMECN" SortExpression="SUSERNAMECN" HeaderText="参与者中文名">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SDEPTID" SortExpression="SDEPTID" HeaderText="所属部门编码">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SDEPTNAMECN" SortExpression="SDEPTNAMECN" HeaderText="所属部门名称">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/delete.gif" 
                                                    AlternateText="Delete" CommandName="Delete" CausesValidation="False" 
                                                    ID="Imagebutton2" ></asp:imagebutton>
										            
										</ItemTemplate>
									</asp:TemplateColumn>
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
