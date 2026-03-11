
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreeList.aspx.cs" Inherits="Tree_TreeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tree Data List</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
<script  src="../common/js/waitProcess.js"></script>

<script language="javascript" type="text/javascript">
	function OpenDetail(strParamString){
	    var url = "TreeDetail.aspx?" + strParamString;
        window.open(url, 'TreeDetail', 'width=900,height=600,top=50,left=200,scrollbars,resizable=yes,toolbar=no,location=no');//最大化打开

    }
    function RefreshParent() {
        if (window.parent != null) {
            if (window.parent.document.getElementById("aRefreshTreeData") != null) {
                window.parent.document.getElementById("aRefreshTreeData").click();
            }
        }
    }
</script>
</head>
<body>

<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
    
<table class="table" width="100%">
	<tr id = "trTitle" runat="server" >
		<td class="edit_label" align = "center"  colspan="1" style="width:20%">
	        <asp:Label ID="Label_Title" runat="server" Text="Label" >当前节点编码</asp:Label>
        </td>
		<td colspan="1" align="left">
	        <asp:Label ID="Label_Code" runat="server" Text="Label"></asp:Label>----
            <asp:Label ID="Label_Name" runat="server" Text="Label"></asp:Label>
	    </td>
		<td align="right" colspan="2">
            <asp:LinkButton ID="aRefresh" runat="server" CssClass="a_Center" OnClick = "aRefresh_Click" Text = "重新加载数据">
            </asp:LinkButton>
            <asp:LinkButton ID="btnExportExl" runat="server" CssClass="a_Left" Font-Bold="true">Excel
            </asp:LinkButton>
            <asp:LinkButton ID="aDetail" runat="server" CssClass="a_Center" Text = "明细">
            </asp:LinkButton>
            <asp:LinkButton ID="aAddSub" runat="server" CssClass="a_Center" Text = "新增子目录">
            </asp:LinkButton>
        </td>
	</tr>
    <tr id="tr1" runat="server" width="100%">
		<td align="left" colspan="4">
	        <asp:Label ID="Label_SubTitle" runat="server" Text="Label" CssClass="left_ts">当前节点下的子节点：</asp:Label>
        </td>
    </tr>
    <tr id="trGrid" runat="server" width="100%">
        <td valign="top"  align="left" style="white-space:nowrap" width="100%" colspan="4">
            <table  id="changecolor" width="100%">
                <tr width="100%">
                    <td id="tdGrid">
                        <asp:DataGrid ID="DataGrid1" BorderWidth="0" CellPadding="0" CellSpacing="0" runat="server" Visible="true" Width="100%" AllowPaging="false" 
                            HorizontalAlign="Center" AutoGenerateColumns="false" AllowSorting="True" ShowFooter="false" 
                            OnItemCreated = "DataGrid1_ItemCreated" OnItemDataBound = "DataGrid1_ItemDataBound"  OnItemCommand = "DataGrid1_ItemCommand"
                            OnSortCommand="DataGrid1_SortCommand">
                            <ItemStyle CssClass="tableContent" />
                            <HeaderStyle CssClass="tableTitle"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="Edit">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ImageUrl="../common/images/icon/view.gif" AlternateText="View" Visible="false"
                                            CommandName="View" CausesValidation="false" ID="Imagebutton_View"></asp:ImageButton>
                                        <asp:ImageButton runat="server" ImageUrl="../common/images/icon/icon-delete.gif" AlternateText="Delete" Visible="true"
                                            CommandName="Delete" CausesValidation="False" ID="Imagebutton_Delete"></asp:ImageButton>
                                        <asp:CheckBox ID="CB_Select" runat="server" Checked="false" Visible="false"/>
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
<form id="from10000post" name="from10000post" method="post">
<input name="hidden" type="hidden" />
</form>
  <iframe id="iddownframe000000"  name="iddownframe000000"  style="width:0px;height:0px;display:none;"></iframe>   
  <iframe id="iddownframe000001"  name="iddownframe000001"  style="width:0px;height:0px;display:none;"></iframe> 
  <script language="javascript" type="text/javascript">
      function exportexcel(filename) {
          //        var sPath = filename + '?type=excel&ran=' + Math.random();
          var sPath = filename + '?type=nopiExcel&ran=' + Math.random();
          from10000post.action = sPath;
          from10000post.target = "iddownframe000000";
          from10000post.submit();
      }
  </script>  
</body>
</html>
<script language="javascript">
	//初始化结果表格
	DefineTableCss("changecolor");
</script>
