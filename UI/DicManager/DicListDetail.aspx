<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DicListDetail.aspx.cs" Inherits="DicManager_DicListDetail" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>清单字典明细项</title>

<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
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
    
    function showOpenWindow(url){
        window.open(url, 'newwindow', 'width=400,height=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }
    
    function deleteDic(url){
        if (confirm('Delete？')){
            
        }
    }
--></script>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
          <!-- 换一行 -->  
          <tr align="center">
            <td valign="middle" align ="center" background="common/images/welcome/content-bg.gif">
               <asp:Label ID="lbLDESC" class="left_ts" runat="server" Text="清单定义名称"></asp:Label>
                
            </td>
          </tr>
          <!-- 列表部分 -->  
          <tr>
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" AllowPaging="False"  DataKeyField="LID" 
                                HorizontalAlign="Center"  OnItemCreated="DataGrid1_ItemCreated"   OnSortCommand="DataGrid1_SortCommand"
                                ondeletecommand="DataGrid1_DeleteCommand" oneditcommand="DataGrid1_EditCommand" 
                                oncancelcommand="DataGrid1_CancelCommand" OnUpdateCommand="DataGrid1_UpdateCommand"
                                onHorizontalAlign="Center">
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="LID" SortExpression="LID" ReadOnly="True" HeaderText="LID">
										<HeaderStyle HorizontalAlign ="Center" Width="8%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CID" SortExpression="CID"  ReadOnly="True" HeaderText="CID">
										<HeaderStyle HorizontalAlign ="Center" Width="8%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CDESC" SortExpression="CDESC" HeaderText="CDESC">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CDESCCHS" SortExpression="CDESCCHS" HeaderText="CDESCCHS">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CUID" SortExpression="CUID" HeaderText="CUID">
										<HeaderStyle HorizontalAlign ="Center" Width="8%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="P9" SortExpression="P9" HeaderText="Order" Visible="true">
										<HeaderStyle HorizontalAlign ="Center" Width="3%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BISSTOP" SortExpression="BISSTOP" HeaderText="Is Stop">
										<HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
								            <asp:imagebutton runat="server" ImageUrl="../common/images/icon/edt.gif" 
								                    AlternateText="Edit" CommandName="Edit" CausesValidation="false" 
								                    ID="Imagebutton1"></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../common/images/icon/delete.gif" 
                                                    AlternateText="Delete" CommandName="Delete" CausesValidation="False" 
                                                    ID="Imagebutton2"></asp:imagebutton>
										            
										</ItemTemplate>
										<EditItemTemplate>
											<asp:imagebutton runat="server" ImageUrl="../common/images/icon/floppy.gif" AlternateText="Update" CommandName="Update" CausesValidation="False" ID="Imagebutton3"></asp:imagebutton>
											<img src="../common/images/icon/spacer.gif" width="3">
											<asp:imagebutton runat="server" ImageUrl="../common/images/icon/stop.gif" AlternateText="Cancel" CommandName="Cancel" CausesValidation="False" ID="Imagebutton4"></asp:imagebutton>
										</EditItemTemplate> 
									</asp:TemplateColumn>
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>
        </table>
        <!-- 新增清单明细内容部分 --> 
        <table border="0" class="warp_table" width="60%" id="tb1" align = "center">
          <tr align = "center">
            <td width="100" background="../common/images/editPage/level2-title-bg.gif" colspan="2" align="center">
                <asp:Label ID="Label1" runat="server" Text="新增清单内容明细"></asp:Label>
            </td>
          </tr>
          <tr align = "center">
            <td class="edit_label" style="width:40%">
                <asp:Label ID="Label2" runat="server" Text="清单定义编码"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBox1" runat="server" Width="85%" Enabled=false></asp:TextBox>
            </td>
          </tr>
          <tr align = "center">
            <td class="edit_label">
                <asp:Label ID="Label3" runat="server" Text="明细内容编码"></asp:Label>    
            </td>
            <td align="left"><asp:TextBox ID="TextBox2" runat="server" Width="85%" MaxLength="50"></asp:TextBox><font color=red>*</font></td>
          </tr>
          <tr align = "center">
            <td class="edit_label">
                <asp:Label ID="Label4" runat="server" Text="明细内容英文名"></asp:Label>
            </td>
            <td align="left"><asp:TextBox ID="TextBox3" runat="server" Width="85%" MaxLength="100"></asp:TextBox><font color=red>*</font></td>
          </tr>
          <tr align = "center">
            <td class="edit_label">
                <asp:Label ID="Label5" runat="server" Text="明细内容中文名"></asp:Label>
            </td>
            <td align="left"><asp:TextBox ID="TextBox4" runat="server" Width="85%" MaxLength="50"></asp:TextBox><font color=red>*</font></td>
          </tr>
          <tr align = "center">
            <td class="edit_label">
                <asp:Label ID="Label6" runat="server" Text="CUID"></asp:Label>
            </td>
            <td align="left"><asp:TextBox ID="TextBox5" runat="server" Width="85%" MaxLength="30"></asp:TextBox></td>
          </tr>
          <tr align = "center">
            <td class="edit_label">
                <asp:Label ID="Label7" runat="server" Text="Order"></asp:Label>
            </td>
            <td align="left"><asp:TextBox ID="TextBox6" runat="server" Width="85%" MaxLength="50" Text = "10"></asp:TextBox></td>
          </tr>
          <tr height="18" align="center">
            <TD align ="center" colspan="2">
                  <asp:Button ID="Button1" runat="server" Text="保    存" width="100px" 
                      height="25px" CssClass="btn_2k3" OnClick="Button1_Click"/>
                  <asp:Button ID="Button2" runat="server" Text="返    回" width="100px" Visible = "false" 
                      height="25px" CssClass="btn_2k3" OnClick="Button2_Click"/>
            </TD>
           </tr>
        </table>
    </form>
</body>
</html>
<script language="javascript">
	//初始化结果表格
	DefineTableCss("changecolor");
</script>
