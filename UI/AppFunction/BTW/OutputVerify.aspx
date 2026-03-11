<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OutputVerify.aspx.cs" Inherits="AppFunction_BTW_OutputVerify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Outlet Verify Page</title>
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
    
    //单价文本框输入校验
    function VerifyPriceTextBox(idVal,txtVal, qty) {
        var idAmount = idVal.replace('txtPrice', 'txtAmount');
        var varValue = document.getElementById(idVal).value;
        if (isNaN(varValue)) {
            document.getElementById(idVal).value = txtVal;
        } else {
            document.getElementById(idAmount).value = varValue * qty;
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
          <tr height="35" align="left">
            <TD align ="center" valign="middle">
                <div class="topBox" runat="server" id="divForm1">
                    <asp:Label ID="Label1" runat="server" Text="Outlet Customs Number"></asp:Label>
                        <asp:TextBox ID="txtBGDH" runat="server" Width="150" MaxLength="50"></asp:TextBox><font color=red>*</font>
                    <asp:Label ID="Label2" runat="server" Text="Outlet Customs Date"></asp:Label>
                        <asp:TextBox ID="txtBGSJ" runat="server" Width="150" MaxLength="20" onfocus="vpCalendar_ShowDate(this);"></asp:TextBox><font color=red>*</font>
                    <asp:LinkButton ID="btnSave1" runat="server" CssClass="a_Left" Font-Bold="true"  OnClick="btnSave1_Click">Save</asp:LinkButton>
                    <asp:LinkButton ID="btnClose1" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "window.close()">Close</asp:LinkButton>
                </div>
            </TD>
          </tr>
     </table>
     
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tableNoHover">
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:100%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%" DataKeyField="POSCODE"  OnItemDataBound="DataGrid1_ItemDataBound"
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
						            <asp:BoundColumn DataField="OUTNO" SortExpression="OUTNO"  HeaderText="OutCode">
							            <HeaderStyle HorizontalAlign ="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="POSCODE" SortExpression="POSCODE" HeaderText="Pos">
							            <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="BWNO" SortExpression="BWNO" HeaderText="BW No.">
							            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MCODE" SortExpression="MCODE" HeaderText="Article">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MNAMECHS" SortExpression="MNAMECHS" HeaderText="Name">
							            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MTYPE" SortExpression="MTYPE" HeaderText="Type" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="QTY" SortExpression="QTY"  HeaderText="Qty">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="NUNIT" SortExpression="NUNIT"  HeaderText="Unit">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="LQTY" SortExpression="LQTY"  HeaderText="L Qty">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="LUNIT" SortExpression="LUNIT"  HeaderText="L Unit">
							            <HeaderStyle HorizontalAlign ="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:TemplateColumn HeaderText="Price">
							            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle>
							            <ItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" Width="50"></asp:TextBox>									            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:TemplateColumn HeaderText="Amount">
							            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle>
							            <ItemTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" Width="70"></asp:TextBox>									            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:TemplateColumn HeaderText="Currency">
							            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle>
							            <ItemTemplate>
                                            <asp:TextBox ID="txtCurrency" runat="server" Width="50"></asp:TextBox>									            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="MPRICE" SortExpression="MPRICE" HeaderText="MPRICE" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="MAMOUNT" SortExpression="MAMOUNT" HeaderText="MAMOUNT" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="CURRENCY" SortExpression="CURRENCY" HeaderText="CURRENCY" Visible="false">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="PROJECTNO" SortExpression="PROJECTNO" HeaderText="Project No.">
							            <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="CUSNUMBER" SortExpression="CUSNUMBER" HeaderText="Customs No.">
							            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="OUTNUMBER" SortExpression="OUTNUMBER" HeaderText="Outlet No.">
							            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="OUTDATE" SortExpression="OUTDATE" HeaderText="Outlet Date" DataFormatString="{0:yyyy-MM-dd}">
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
