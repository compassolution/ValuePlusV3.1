<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormList.aspx.cs" Inherits="Flow_FormManage_FormList"  EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>流程表单定义列表</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" />
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
<!--JavaScript部分-->
<script language="javascript">
    var varSearchTRFlag = 0;
    function showSearchTR(){
	    if (varSearchTRFlag==0){
		    this.trSearch.style.display = "";
		    varSearchTRFlag = 1;}
	    else if (varSearchTRFlag==1){
		    this.trSearch.style.display = "none";
		    varSearchTRFlag = 0}
    		
    }
    
    function showOpenWindow(url){
        window.open(url, 'newwindow', 'width=400,height=200,top=300,left=400, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }

--></script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
          <!-- 操作行 -->
          <tr height="30" align="center">
  	        
            <TD align ="center" >
                <asp:Button ID="Button1" runat="server" Text="新    增" width="100px" height="25px" CssClass="btn_2k3" OnClick="Button1_Click"/>
            </TD>
  	        
          </tr>
          
          <!-- 列表部分 -->  
          <tr>
            
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="SFORMCODE"  OnItemCreated="DataGrid1_ItemCreated" OnSortCommand = "DataGrid1_SortCommand"
                                ondeletecommand="DataGrid1_DeleteCommand" OnItemCommand="DataGrid1_ItemCommand" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="SFORMCODE" SortExpression="SFORMCODE" ReadOnly="True" HeaderText="表单编码">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFORMNAME" SortExpression="SFORMNAME" HeaderText="表单英文名">
										<HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SFORMAMECN" SortExpression="SFORMAMECN" HeaderText="表单中文名">
										<HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BISVERSION" SortExpression="BISVERSION" HeaderText="是否版本控制">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BISSTOP" SortExpression="BISSTOP" HeaderText="是否停用">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton runat="server" ImageUrl="../../common/images/icon/edt.gif" 
								                    AlternateText="Detail" CommandName="Detail" CausesValidation="false" 
								                    ID="Imagebutton1" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/delete.gif" 
                                                    AlternateText="Delete" CommandName="Delete" CausesValidation="False" 
                                                    ID="Imagebutton2" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/calendar.gif" 
                                                    AlternateText="Field" CommandName="Field" CausesValidation="False" 
                                                    ID="Imagebutton3" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/DatabaseFields.bmp" 
                                                    AlternateText="Create" CommandName="Create" CausesValidation="False" 
                                                    ID="Imagebutton4" ></asp:imagebutton>
										            
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
