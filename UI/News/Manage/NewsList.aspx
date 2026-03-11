<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsList.aspx.cs" Inherits="News_Manage_NewsList"  EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>新闻列表</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script> 
<script  src="../../common/js/tableStyle.js"></script>
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

    function showNewsEditor() {
        var plateId = document.getElementById("hfPlateId").value;
        var url = 'EditNews.aspx?newsId=&plateId=' + plateId;
        showOpenWindow(url);
    }
    function showOpenWindow(url) {
        window.open(url, 'newwindow', 'width=1000,height=800,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }
--></script>
<body>
<!--#include   file= "../../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfPlateId" runat="server" />
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
          <!-- 操作行 -->
          <tr height="20" align="center" class="titlebt">
            <TD align ="left">
                <asp:Label ID="Label1" runat="server" Text="当前版块："></asp:Label>
                <asp:Label ID="lbCurPlateId" runat="server" Text="当前版块ID"></asp:Label>
            </TD>
            <TD align ="right">
                <asp:LinkButton ID="btnDeletePlate" runat="server" CssClass="a_Center" OnClick="btnDeletePlate_Click" >删除当前版块</asp:LinkButton>
                <asp:LinkButton ID="btnModifyPlate" runat="server" CssClass="a_Center" OnClick="btnModifyPlate_Click" >修改当前版块</asp:LinkButton>
                <asp:LinkButton ID="btnAddSubPlate" runat="server" CssClass="a_Center" OnClick="btnAddSubPlate_Click" >新增子版块</asp:LinkButton>
            </TD>
          </tr>
          <tr height="20" align="center">
            <TD align ="right" colspan="2">
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="a_Center" OnClick="btnRefresh_Click">刷  新</asp:LinkButton>
                <asp:LinkButton ID="btnAddNews" runat="server" CssClass="a_Center" OnClientClick="showNewsEditor();">发布新闻</asp:LinkButton>
            </TD>
          </tr>
          <!-- 列表部分 -->  
          <tr>
            <td valign="top" bgcolor="#F7F8F9" colspan="2">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False"  DataKeyField="SNEWSID" OnSortCommand="DataGrid1_SortCommand"
                                HorizontalAlign="Center" OnItemCreated="DataGrid1_ItemCreated" onitemcommand="DataGrid1_ItemCommand">
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="SNEWSID" SortExpression="SNEWSID" ReadOnly="True" HeaderText="News Id">
										<HeaderStyle HorizontalAlign ="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="STITLECHS" SortExpression="STITLECHS" HeaderText="Title">
										<HeaderStyle HorizontalAlign="Center" Width="50%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DTTIME" SortExpression="DTTIME" HeaderText="DateTime" DataFormatString="{0:yyyy-MM-dd}">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BISSTOP" SortExpression="BISSTOP" HeaderText="Is Stop">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton Runat="server" ImageUrl="../../common/images/search1.png" 
										                    AlternateText="Detail/明细" CommandName="Detail" CausesValidation="False" 
										                    ID="imgDetail"></asp:imagebutton>
										    <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/del.gif" 
										                    AlternateText="Delete/删除" CommandName="Delete" CausesValidation="False" 
										                    ID="imgDelete"></asp:imagebutton>
										     
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
