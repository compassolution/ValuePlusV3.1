<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostNextList.aspx.cs" Inherits="Flow_FlowManage_PostNextList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>岗位下岗位路径列表</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" />
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/tableStyle.js"></script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
          <!-- 操作行 -->
          <tr height="30" align="center">
  	        
            <TD align ="left" style="color:Blue" class="td_Frame2" >
                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="a_Left" onclick="Button2_Click">返    回</asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Left" onclick="Button1_Click">新    增</asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="a_Left" onclick="Button3_Click">岗位参与者</asp:LinkButton>
                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="a_Left" onclick="Button4_Click">岗位活动列表</asp:LinkButton>
		        <asp:Label ID="Label1" runat="server" Text="岗位下一岗位列表，当前岗位编码："></asp:Label>
		        <asp:Label ID="lbPostCode" runat="server" Text="："></asp:Label>
            </TD>
  	        
          </tr>
          
          <!-- 列表部分 -->  
          <tr>
            
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="SFLOWPATHCODE"  OnItemCreated="DataGrid1_ItemCreated" OnSortCommand = "DataGrid1_SortCommand"
                                ondeletecommand="DataGrid1_DeleteCommand" OnItemCommand="DataGrid1_ItemCommand" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="NINDEX" SortExpression="NINDEX" ReadOnly="True" HeaderText="序号">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPOSTCODE_PRE" SortExpression="SPOSTCODE_PRE" ReadOnly="True" HeaderText="当前岗位编码">
										<HeaderStyle HorizontalAlign ="Center" Width="25%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPOSTCODE_NEXT" SortExpression="SPOSTCODE_NEXT" HeaderText="下一岗位编码">
										<HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="NWORKDAY" SortExpression="NWORKDAY" HeaderText="发送接收时限">
										<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SBIZSTATUS" SortExpression="SBIZSTATUS" HeaderText="业务状态">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton runat="server" ImageUrl="../../common/images/icon/edt.gif" 
								                    AlternateText="Detail" CommandName="Detail" CausesValidation="false" 
								                    ID="Imagebutton1" ></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/delete.gif" 
                                                    AlternateText="Delete" CommandName="Delete" CausesValidation="False" 
                                                    ID="Imagebutton2" ></asp:imagebutton>
										            
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
        
        <table border="0" class="table" id="tb1" align="center" style="height:auto; width:70%">
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label2" runat="server" Text="下一岗位路径明细信息："></asp:Label>
		        <asp:Label ID="lbOption" runat="server" Text="新增"></asp:Label>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center" style="width:35%">
		        <asp:Label ID="Label3" runat="server" Text="岗位路径编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtPathCode" runat="server" Width="90%" Enabled="false"></asp:TextBox><font color=red>*</font>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center" style="width:35%">
		        <asp:Label ID="Label4" runat="server" Text="当前岗位编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtCurPostCode" runat="server" Width="90%" Enabled="false"></asp:TextBox><font color=red>*</font>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center">
		        <asp:Label ID="Label5" runat="server" Text="下岗位编码"></asp:Label>    
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList1" runat="server" Width="80%">
				</asp:DropDownList><font color=red>*</font>
			</td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center">
		        <asp:Label ID="Label6" runat="server" Text="发送接收时限"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="txtWorkDay" runat="server" Width="90%" MaxLength="10">0</asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label7" runat="server" Text="路径英文描述"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="txtDesc" runat="server" Width="90%" MaxLength="500" TextMode="MultiLine" Rows=2></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center">
		        <asp:Label ID="Label8" runat="server" Text="路径英文描述"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtDescCN" runat="server" Width="90%" MaxLength="500" TextMode="MultiLine" Rows=2></asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center">
		        <asp:Label ID="Label10" runat="server" Text="顺序号"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtIndex" runat="server" Width="90%" MaxLength="9" >10</asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label" align = "center">
		        <asp:Label ID="Label12" runat="server" Text="业务状态"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtBizStatus" runat="server" Width="90%" MaxLength="10" >0</asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  
		  <tr height="18" align="center">
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2" class="td_Frame2" >
                <asp:LinkButton ID="Button1" runat="server" CssClass="a_Left" onclick="Button5_Click">保    存</asp:LinkButton>

            </TD>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </form>
</body>
</html>
<script language="javascript">
	//初始化结果表格
	DefineTableCssNoCursorOver("changecolor");
</script>