<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuList.aspx.cs" Inherits="MenuManager_MenuList" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>清单字典定义配置页</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script> 
<script  src="../common/js/tableStyle.js"></script>
</head>
<!--JavaScript部分-->
<script language="javascript"><!--
    var varSearchTRFlag = 0;
    function showSearchTR(){
	    if (varSearchTRFlag==0){
		    this.trSearch.style.display = "";
		    varSearchTRFlag = 1;}
	    else if (varSearchTRFlag==1){
		    this.trSearch.style.display = "none";
		    varSearchTRFlag = 0}
    		
    }
    
--></script>
<body>
<!--#include   file= "../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
          <!-- 操作行 -->
          <tr height="20" align="center">
            <TD align ="right">
                <asp:LinkButton ID="Button1" runat="server" CssClass="a_Center" OnClick="Button1_Click" >新    增</asp:LinkButton>
            </TD>
          </tr>
          <!-- 列表部分 -->  
          <tr>
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False"  DataKeyField="SMENUCODE" OnSortCommand="DataGrid1_SortCommand"
                                HorizontalAlign="Center"  OnItemCreated="DataGrid1_ItemCreated" onitemcommand="DataGrid1_ItemCommand">
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="SMENUCODE" SortExpression="SMENUCODE" ReadOnly="True" HeaderText="Menu Code">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SMENUNAME" SortExpression="SMENUNAME" HeaderText="SMENUNAME" Visible="false">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SMENUNAMECN" SortExpression="SMENUNAMECN" HeaderText="SMENUNAMECN">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SURLDETAIL" SortExpression="SURLDETAIL" HeaderText="SURLDETAIL">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SMENUTYPE" SortExpression="SMENUTYPE" HeaderText="SMENUTYPE" Visible="false">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPARENTCODE" SortExpression="SPARENTCODE" HeaderText="SPARENTCODE" Visible="false">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SLEVEL" SortExpression="SLEVEL" HeaderText="SLEVEL">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SORDER" SortExpression="SORDER" HeaderText="ISSTOP" Visible="false">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton Runat="server" ImageUrl="../common/images/icon/calendar.gif" 
										                    AlternateText="Detail/明细" CommandName="Detail" CausesValidation="False" 
										                    ID="Imagebutton1"></asp:imagebutton>
										     
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
	DefineTableCss("changecolor");
</script>
