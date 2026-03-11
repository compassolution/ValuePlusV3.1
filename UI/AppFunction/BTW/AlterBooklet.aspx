<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlterBooklet.aspx.cs" Inherits="AppFunction_BTW_AlterBooklet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Alter Booklet Page</title>
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
          <tr height="30" align="left">
            <TD align ="center" valign="middle">
                <div class="topBox" runat="server" id="divForm1">
                    <asp:Label ID="Label1" runat="server" Text="Booklet Number"></asp:Label>
                        <asp:dropdownlist id="ddListQuery" runat="server">
		                </asp:dropdownlist>
                    <asp:LinkButton ID="btnQuery" runat="server" CssClass="a_Left" Font-Bold="true"  OnClick="btnQuery_Click">Query</asp:LinkButton>
                    <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick="javascript:window.close();">Close</asp:LinkButton>
                </div>
            </TD>
          </tr>
          <tr valign="bottom" align="right">
              <td height="3" >
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
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%" DataKeyField="BCODE" 
                                AutoGenerateColumns="false"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" >
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
					            <ItemStyle CssClass="tableContent"  />
					            <Columns>
						            <asp:TemplateColumn HeaderText="<input   type='checkbox'   id='CheckAll'   onclick='return select_deselectAll(this.checked, this.id);'>">
							            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
							            <ItemTemplate>
                                            <asp:CheckBox ID="cbox" runat="server"/>										            
							            </ItemTemplate>
						            </asp:TemplateColumn>
						            <asp:BoundColumn DataField="BCODE" SortExpression="BCODE"  HeaderText="Invoice">
							            <HeaderStyle HorizontalAlign ="Center" Width="20%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="FDATE" SortExpression="FDATE" HeaderText="Invoice Date" DataFormatString="{0:yyyy-MM-dd}">
							            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount">
							            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="WEIGHT" SortExpression="WEIGHT" HeaderText="Weight">
							            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
						                <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
						            </asp:BoundColumn>
						            <asp:BoundColumn DataField="BOOKLETNO" SortExpression="BOOKLETNO" HeaderText="Booklet NO.">
							            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
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
                    <asp:Label ID="Label2" runat="server" Text="From："></asp:Label>
                        <asp:dropdownlist id="ddListOld" runat="server">
		                </asp:dropdownlist>
                    <asp:Label ID="Label4" runat="server" Text="To："></asp:Label>
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

