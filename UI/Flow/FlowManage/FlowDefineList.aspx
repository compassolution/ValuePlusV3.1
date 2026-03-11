<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FlowDefineList.aspx.cs" Inherits="Flow_FlowManage_FlowDefineList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>流程定义列表</title>
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
                <asp:LinkButton ID="Button1" runat="server" CssClass="a_Left" onclick="Button1_Click">新    增</asp:LinkButton>
		        <asp:Label ID="Label1" runat="server" Text="流程定义列表"></asp:Label>
            </TD>
          </tr>
          
          <!-- 列表部分 -->  
          <tr>
            
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="SFLOWCODE"  OnItemCreated="DataGrid1_ItemCreated" OnSortCommand = "DataGrid1_SortCommand"
                                ondeletecommand="DataGrid1_DeleteCommand" OnItemCommand="DataGrid1_ItemCommand" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="SFLOWCODE" SortExpression="SFLOWCODE" ReadOnly="True" HeaderText="流程定义编码">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFLOWNAME" SortExpression="SFLOWNAME" HeaderText="流程定义名称">
										<HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFLOWNAMECN" SortExpression="SFLOWNAMECN" HeaderText="流程定义中文名">
										<HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BISNEEDACCEPT" SortExpression="BISNEEDACCEPT" HeaderText="是否需要接收">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BSTOP" SortExpression="BSTOP" HeaderText="是否停用">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton runat="server" ImageUrl="../../common/images/icon/edt.gif" 
								                    AlternateText="Detail" CommandName="Detail" CausesValidation="false" 
								                    ID="imgBtnDetail" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/calendar.gif" 
                                                    AlternateText="Instance" CommandName="Instance" CausesValidation="False" 
                                                    ID="imgBtnInstance" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/view.gif" 
                                                    AlternateText="Post" CommandName="Post" CausesValidation="False" 
                                                    ID="imgBtnPost" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/DatabaseFields.bmp" 
                                                    AlternateText="Create" CommandName="Create" CausesValidation="False" 
                                                    ID="imgBtnCreate" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/delete.gif" 
                                                    AlternateText="Delete" CommandName="Delete" CausesValidation="False" 
                                                    ID="imgBtnDel" ></asp:imagebutton>
										            
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
