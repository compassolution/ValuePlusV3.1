<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OutWarehouse.aspx.cs" Inherits="AppFunction_BTW_OutWarehouse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OutWarehouse Page</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
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
//                   if(chkVal == true) 
//                   {
//                        frm.elements[i].checked = true;
//                   } 
//                   else 
//                   {
//                        frm.elements[i].checked = false;
//                   }
                   frm.elements[i].click();
                }
            } 
        }
    }

    //CheckBox选择与不选时，文本框隐藏控制
    function CheckRow(chkVal, idVal) {
        if (idVal.indexOf('cbox') != -1) {
            var txt1 = idVal.replace('cbox', 'txtCount');
            var txt2 = idVal.replace('cbox', 'txtPrice');
            if (chkVal == true) {
                document.getElementById(txt1).disabled = false;
                document.getElementById(txt2).disabled = false;
            }
            else {
                document.getElementById(txt1).disabled = true;
                document.getElementById(txt2).disabled = true;
            }
        }
    }
    //数量文本框输入校验
    function VerifyCountTextBox(idVal,maxValue) {
        var varValue = document.getElementById(idVal).value;
        if (isNaN(varValue)) {
            document.getElementById(idVal).value = maxValue;
        } else {
            if (varValue > maxValue) {
                document.getElementById(idVal).value = maxValue;
            } else if (varValue < 0) {
                document.getElementById(idVal).value = "0";
            }
        }
    }
    //单价文本框输入校验
    function VerifyPriceTextBox(idVal, maxValue) {
        var varValue = document.getElementById(idVal).value;
        if (isNaN(varValue)) {
            document.getElementById(idVal).value = maxValue;
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
                <div class="topBox" runat="server" id="divForm1">
  		            <table width = "750 >
  			            <tr style="width:750">
  			                <td align="right" style="width:100" >
                                <asp:Label ID="Label1" runat="server" Text="Invoice No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtBCODE" runat="server" style="width:150"></asp:TextBox>
                            </td>
  			                <td align="right" style="width:100" >
                                <asp:Label ID="Label2" runat="server" Text="Article No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtMCODE" runat="server" style="width:150"></asp:TextBox>
                            </td>
                            <td align="center" style="width:100" >
                                <asp:Label ID="Label3" runat="server" Text="Bonded Number："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtBondedNo" runat="server" style="width:150"></asp:TextBox>
                            </td>
                         </tr>
  			            <tr style="width:750">
                            <td align="center" style="width:100" >
                                <asp:Label ID="Label4" runat="server" Text="Project No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtProject" runat="server" style="width:150"></asp:TextBox>
                            </td>
  			                <td align="right" style="width:100" >
                                <asp:Label ID="Label5" runat="server" Text="Bin No.："></asp:Label>
                            </td>
                            <td style="width:150" >
                                <asp:TextBox ID="txtBin" runat="server" style="width:150"></asp:TextBox>
                            </td>
                            <td align="right" colspan="2" style="width:250" >
                                <asp:LinkButton ID="btnQuery" runat="server" CssClass="a_Left" Font-Bold="true"  OnClick="btnQuery_Click" >Query</asp:LinkButton>
                                <asp:LinkButton ID="btnSave" runat="server" CssClass="a_Left" Font-Bold="true"  OnClick="btnSave_Click" >Save</asp:LinkButton>
                                <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick="javascript:window.close();">Close</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </TD>
          </tr>
          <tr valign="bottom" align="right">
              <td height="85" >
                <asp:Label ID="Label6" runat="server" Text="Record Count：" ForeColor="Red"></asp:Label>
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
                                AutoGenerateColumns="false"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" 
                                OnItemCreated = "DataGrid1_ItemCreate" OnItemDataBound="DataGrid1_ItemDataBound">
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
					            <ItemStyle CssClass="tableContent"  />
					            <Columns>
						            <asp:TemplateColumn HeaderText="<input   type='checkbox'  id='CheckAll' onclick='return select_deselectAll(this.checked, this.id);'>">
							            <HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
            						    
							            <ItemTemplate>
                                            <asp:CheckBox ID="cbox" runat="server"/>										            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="MCODE" SortExpression="MCODE" HeaderText="Article">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MNAME" SortExpression="MNAME" HeaderText="English" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MNAMECHS" SortExpression="MNAMECHS" HeaderText="Chinese">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MTYPE" SortExpression="MTYPE" HeaderText="Type" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="QWNO" SortExpression="QWNO" HeaderText="Bonded No." Visible="false">
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
						            <asp:TemplateColumn HeaderText="Out Qty">
							            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle>
							            <ItemTemplate>
                                            <asp:TextBox ID="txtCount" runat="server" Width="50"></asp:TextBox>									            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="MPRICE" SortExpression="MPRICE"  HeaderText="Price">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="QWNO" SortExpression="QWNO"  HeaderText="Bonded No.">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <%--<asp:TemplateColumn HeaderText="Out Price">
							            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
							            <ItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" Width="60"></asp:TextBox>									            
							            </ItemTemplate>
						            </asp:TemplateColumn>--%>
						            <asp:BoundColumn DataField="BCODE" SortExpression="BCODE"  HeaderText="Invoice">
							            <HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PROJECTNO" SortExpression="PROJECTNO" HeaderText="Project No.">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="CUSNUMBER" SortExpression="CUSNUMBER" HeaderText="Customs No.">
							            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="CUSDATE" SortExpression="CUSDATE" HeaderText="CustomsDate" DataFormatString="{0:yyyy-MM-dd}">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="WHCODE" SortExpression="WHCODE" HeaderText="Bin No.">
							            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="RKDATE" SortExpression="RKDATE" HeaderText="StorageDate" DataFormatString="{0:yyyy-MM-dd}">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
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

</form>
</body>
</html>

<script language="javascript">
	//初始化结果表格
	DefineTableCssNoCursorOver("changecolor");
</script>
