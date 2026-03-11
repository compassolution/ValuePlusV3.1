<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArchiveDetailBatch.aspx.cs" Inherits="Archive_Detail_ArchiveDetailBatch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>批量选择页面</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 

<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
<script language="javascript">

    window.focus();
    function AfterSave()
    {
        alert('Successfully!');
        if (window.opener.document.all("aRefreshDetail") != null) {
            window.opener.document.all("aRefreshDetail").click();
        }
        window.close();
    } 
    
     
    function EnterFilterTextBox()
      {
         if(event.keyCode == 13)
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["btnFilter"].click();
         }
     }
     
     //CheckBox全选And反全选
     function select_deselectAll(chkVal, idVal) {
         if (idVal.indexOf('CheckAll') != -1) {
             var oTb = document.getElementById('changecolor');

             var oSel = oTb.getElementsByTagName('input');
             for (i = 0; i < oSel.length; i++) {
                 if (oSel[i].type == "checkbox" && oSel[i].id.indexOf("cbox") != -1) {
                     var checkB = oSel[i];
                     if (chkVal == true) {
                         checkB.checked = true;
                     }
                     else {
                         checkB.checked = false;
                     }
                 }
             }
         }

     }

  </script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfKeyValue" runat="server" />
    <table width="95%" border="0" cellpadding="0" cellspacing="0" align="center" >
          <!-- 操作行 -->
          <tr height="30" align="center">
            <td align ="center" valign="middle">
                <div class="topBox"style="vertical-align:middle; float:left; margin:auto">
                    <asp:TextBox ID="txtFilter" runat="server" Width="100"></asp:TextBox>
                    
                    <%--<div style="display:none; float:left; margin:auto">
                        <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="Filter_Click">定   位</asp:LinkButton>
                    </div>--%>
                    <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="Filter_Click">定   位</asp:LinkButton>
                    <asp:LinkButton ID="btnOK" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="OK_Click">确   定</asp:LinkButton>
                    <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:window.close();">关   闭</asp:LinkButton>
                </div>
            </td>
          </tr>
          
          <!-- 结果列表部分 --> 
          <tr align="center">
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" class="table" style="width:100%" align="center">
                    <tr width="100%" align="center">
                        <td style ="width:100%; white-space:nowrap" align="center">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%"
                                AutoGenerateColumns="true"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" OnItemDataBound = "DataGrid1_ItemDataBound"  >
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
								<ItemStyle CssClass="tableContent"  />
                                <Columns>
									<asp:TemplateColumn HeaderText="<input   type='checkbox'  id='CheckAll'  onclick='return select_deselectAll(this.checked, this.id);'>">
										<HeaderStyle  CssClass="tableTitle" HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
                                            <asp:CheckBox ID="cbox" runat="server"/>										            
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>
        <tr id = "trPageArea" valign="middle" runat="server">
		    <td align="right" valign="middle" id="td3" runat="server" class="td_Frame2">
		        <div style="vertical-align:middle; float:right" class="bottomBox">
                    <asp:LinkButton ID="aFirstPage" runat="server" CssClass="a_Right" OnClick="FirstPage_Click"><asp:Label ID="Label_FirstPage" runat="server" Font-Bold = "true" Text="|<"></asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aPrePage" runat="server" CssClass="a_Right" OnClick="PrePage_Click"><asp:Label ID="Label_PrePage" runat="server" Font-Bold = "true" Text="<<"></asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aNextPage" runat="server" CssClass="a_Right" OnClick="NextPage_Click"><asp:Label ID="Label_NextPage" runat="server" Font-Bold = "true" Text=">>"></asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aLastPage" runat="server" CssClass="a_Right" OnClick="LastPage_Click"><asp:Label ID="Label_LastPage" runat="server" Font-Bold = "true" Text=">|"></asp:Label></asp:LinkButton>
		            <asp:Label ID="Label_Page1" runat="server" Text="Label">Total</asp:Label>
		            <font color="red"><asp:Label ID="Label_AllCount" runat="server" Text="Label"></asp:Label></font>
		            ---<asp:Label ID="Label_Page3" runat="server" Text="Label">Page:</asp:Label>
		                <font color="red"><asp:Label ID="Label_CurPage" runat="server" Text="Label"></asp:Label></font>
		            <asp:Label ID="Label_Page4" runat="server" Text="Label">/</asp:Label>
		                <font color="red"><asp:Label ID="Label_AllPage" runat="server" Text="Label"></asp:Label></font>
		            ------
		        </div>
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
