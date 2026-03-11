<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostActionList.aspx.cs" Inherits="Flow_FlowManage_PostActionList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>岗位活动列表</title>
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
                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="a_Left" onclick="Button3_Click">岗位参与者</asp:LinkButton>
                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="a_Left" onclick="Button4_Click">下岗位列表</asp:LinkButton>
		        <asp:Label ID="Label1" runat="server" Text="岗位活动列表，当前岗位编码："></asp:Label>
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
                                AutoGenerateColumns="False" DataKeyField="SFLOWACTIONCODE"  OnItemCreated="DataGrid1_ItemCreated" OnSortCommand = "DataGrid1_SortCommand"
                                ondeletecommand="DataGrid1_DeleteCommand" OnItemCommand="DataGrid1_ItemCommand" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="NINDEX" SortExpression="NINDEX" ReadOnly="True" HeaderText="序号">
										<HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFLOWACTIONCODE" SortExpression="SFLOWACTIONCODE" ReadOnly="True" HeaderText="岗位活动编码">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFLOWACTIONNAME" SortExpression="SFLOWACTIONNAME" HeaderText="岗位活动名称">
										<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFLOWACTIONNAMECN" SortExpression="SFLOWACTIONNAMECN" HeaderText="岗位活动中文名">
										<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SACTIONDETAIL" SortExpression="SACTIONDETAIL" HeaderText="活动内容">
										<HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SBIZSTATUS" SortExpression="SBIZSTATUS" HeaderText="业务状态">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton runat="server" ImageUrl="../../common/images/icon/edt.gif" 
								                    AlternateText="Detail" CommandName="Detail" CausesValidation="false" 
								                    ID="Imagebutton1" ></asp:imagebutton>
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
