<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlterWarehouse.aspx.cs" Inherits="AppFunction_BTW_AlterWarehouse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Alter Warehouse Location</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>
<!--JavaScript部分-->
<script language="javascript">

    window.focus();
    
    //CheckBox全选And反全选
    function select_deselectAll (chkVal, idVal) 
    {
      var frm = document.forms[0];
      for (i=0; i<frm.length; i++) 
      {
           if (idVal.indexOf ('CheckAll') != -1)
           {
                if(frm.elements[i].type == "checkbox" && frm.elements[i].id.indexOf("cbox") != -1)
               { 
                   if(chkVal == true) 
                    {
                         frm.elements[i].checked = true;
                     } 
                    else 
                    {
                         frm.elements[i].checked = false;
                     }
                }
            } 
        }
    }
</script> 
<style type="text/css">
.customBox1{
	padding:5px;
	border:1px solid #aaa;
	background-color:#fee;
	font-size:12px;
	width:100%;
	left:0;
	top:30px;	
	position:fixed;	
}
</style>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<!--#include   file= "../../common/CurPageWaiting.htm"-->
 <form id="form1" runat="server">
<asp:HiddenField ID="hfLocalUrl" runat="server" />
<table width="100%" border="0" cellpadding="0" cellspacing="0" >
          <!-- 操作行 -->
          <tr align="left">
            <TD align ="center" valign="middle">
                <div class="topBox" runat="server" id="div2">
  		            <table width = "750 >
  			            <tr style="width:750">
  			                <td align="right" style="width:100" >
                                <asp:Label ID="Label2" runat="server" Text="Invoice No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtBCODE" runat="server" style="width:150"></asp:TextBox>
                            </td>
  			                <td align="right" style="width:100" >
                                <asp:Label ID="Label6" runat="server" Text="Article No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtMCODE" runat="server" style="width:150"></asp:TextBox>
                            </td>
                            <td align="center" style="width:100" >
                                <asp:Label ID="Label7" runat="server" Text="Bonded Number："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtBondedNo" runat="server" style="width:150"></asp:TextBox>
                            </td>
                         </tr>
  			            <tr style="width:750">
                            <td align="center" style="width:100" >
                                <asp:Label ID="Label8" runat="server" Text="Project No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtProject" runat="server" style="width:150"></asp:TextBox>
                            </td>
  			                <td align="right" style="width:100" >
                                <asp:Label ID="Label9" runat="server" Text="Bin No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:dropdownlist id="ddListQuery" runat="server" Width="150">
		                        </asp:dropdownlist>
                            </td>
                            <td align="right" colspan="2" style="width:250" >
                                <asp:LinkButton ID="btnQuery" runat="server" CssClass="a_Left" Font-Bold="true"  OnClick="btnQuery_Click" >Query</asp:LinkButton>
                                <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick="javascript:window.close();">Close</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </TD>
          </tr>
          <tr valign="bottom" align="right">
              <td height="85" >
                    <asp:Label ID="Label1" runat="server" Text="Record Count：" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbListCount" runat="server" Text="0" ForeColor="Red"></asp:Label>
              </td>
          </tr>
     </table>
     
     
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tableNoHover">
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:100%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%" DataKeyField="QWNO" 
                                AutoGenerateColumns="false"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" >
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
					            <ItemStyle CssClass="tableContent"  />
					            <Columns>
						            <asp:TemplateColumn HeaderText="<input   type='checkbox'   id='CheckAll'   onclick='return select_deselectAll(this.checked, this.id);'>">
							            <HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
							            <ItemTemplate>
                                            <asp:CheckBox ID="cbox" runat="server"/>										            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="QWNO" SortExpression="QWNO" HeaderText="BondedNumber">
						                <ItemStyle HorizontalAlign="Center"  Width="10%"></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="WHCODE" SortExpression="WHCODE" HeaderText="Bin No.">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MCODE" SortExpression="MCODE" HeaderText="Article No.">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="QTY" SortExpression="QTY"  HeaderText="Total">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="OUTQTY" SortExpression="OUTQTY"  HeaderText="Had Out">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PREQTY" SortExpression="PREQTY"  HeaderText="Pre Out">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="BCODE" SortExpression="BCODE"  HeaderText="Invoice No.">
							            <HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PROJECTNO" SortExpression="PROJECTNO" HeaderText="Project No.">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="CUSNUMBER" SortExpression="CUSNUMBER" HeaderText="CustomsNumber">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="CUSDATE" SortExpression="CUSDATE" HeaderText="CustomsDate" DataFormatString="{0:yyyy-MM-dd}">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="RKDATE" SortExpression="RKDATE" HeaderText="StorageDate" DataFormatString="{0:yyyy-MM-dd}">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
					            </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>	
    </table>

    <table width="100%" border="0" cellpadding="0" cellspacing="0" >
          <!-- 操作行 -->
          <tr height="35" align="left">
            <TD align ="center" valign="middle">
                <div class="bottomBox" runat="server" id="div1">
                    <asp:Label ID="Label4" runat="server" Text="Change the selection Warahouse To："></asp:Label>
                        <asp:dropdownlist id="ddListNew" runat="server">
		                </asp:dropdownlist>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="a_Left" Font-Bold="true"  OnClick="btnSave_Click">Save</asp:LinkButton>
                    <asp:LinkButton ID="btnClose1" runat="server" CssClass="a_Left" Font-Bold="true" OnClick = "btnClose_Click">Close</asp:LinkButton>
                </div>
            </TD>
          </tr>
     </table>
     
</form>
</body>
</html>

<script language="javascript">
	//初始化结果表格
	DefineTableCssNoCursorOver("changecolor");
</script>


