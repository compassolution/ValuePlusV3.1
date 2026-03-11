<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuerySort.aspx.cs" Inherits="Query_QuerySort" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>数据列表选择查询主页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/tableStyle.js"></script>
<script language="javascript">

    window.focus();
    function SetOpenerSorting(strResult)
    {
        if (strResult != null && strResult != "") {
            if (window.opener != null) {
                if (window.opener.document.getElementById("hfSortString") != null) {
                    window.opener.document.getElementById("hfSortString").value = strResult;
                }
                if (window.opener.document.getElementById("btnOrderOk") != null) {
                    window.opener.document.getElementById("btnOrderOk").click();
                }
                
            }
        }
        window.close();
    } 
    
  </script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfKeyValue" runat="server" />
    <table width="95%" border="0" cellpadding="0" cellspacing="0" >
          <!-- 操作行 -->
          <tr height="30" align="left">
            <td align ="center" valign="middle">
                <div class="topBox"style="vertical-align:middle; float:left; margin:auto">
                    <asp:LinkButton ID="btnOK" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="OK_Click">OK</asp:LinkButton>
                    <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:window.close();">Close</asp:LinkButton>
                </div>
            </td>
          </tr>
          <!-- 结果列表部分 --> 
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" class="table" style="width:90%">
                    <tr width="100%">
                        <td style ="width:98%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%"
                                AutoGenerateColumns="false"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" OnItemDataBound = "DataGrid1_ItemDataBound"  >
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
								<ItemStyle CssClass="tableContent"  />
                                <Columns>
									<asp:TemplateColumn HeaderText="Order">
										<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
                                            <asp:DropDownList id="ddListNumber" runat="server" Width="80%"></asp:DropDownList>									            
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="column" SortExpression="column" HeaderText="Column Name" Visible=false>
										<HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="caption" SortExpression="caption" HeaderText="Caption">
										<HeaderStyle HorizontalAlign="Center" Width="40%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Is Reverse">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>	
                                            <asp:CheckBox ID="cbOrder" runat="server" />DESC							            
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
