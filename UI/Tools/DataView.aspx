<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataView.aspx.cs" Inherits="Tools_DataView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据查看器</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/tableStyle.js"></script>
    
<script language="javascript">

  function EnterSqlTextBox()
  {
     if(event.keyCode == 13 && document.all["txtSql"].value != "")
     {
         event.keyCode = 9;
         event.returnValue = false;
         document.all["btnQuery"].click();
     }
  }
</script> 
</head>
<body>
<form id="form1" runat="server">
    <table class="tableNoHover"  align="center" width="100%">
        <tr id="trResult" >
            <td width="80%">
                <asp:TextBox ID="txtSql" runat="server"  Width = "100%" TextMode="MultiLine" Rows=2 ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <!-- 结果列表部分 --> 
            <td style ="width:98%; white-space:nowrap; color:red" align="left">
                <asp:Button ID="btnQuery" runat="server" Text="查    询" OnClick="Button1_Click" />
                <asp:Button ID="btnExport" runat="server" Text="导出Excel"/>
                <asp:Label ID="lbCount" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <!-- 结果列表部分 --> 
            <td style ="width:100%; white-space:nowrap">
                <table id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server"  Width="100%"  onsortcommand="DataGrid1_SortCommand" AllowSorting="true"
                                AutoGenerateColumns="true"  onHorizontalAlign="Center">
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
			                    <ItemStyle CssClass="tableContent"  />
			                    <Columns>
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
<iframe id="iddownframe000000" name="iddownframe000000" style="width: 0px; height: 0px; display: none;"></iframe>
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
	DefineTableCssNoCursorOver("changecolor");
</script>