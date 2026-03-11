<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoCodeList.aspx.cs" Inherits="SystemManager_AutoCode_AutoCodeList" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>清单字典定义配置页</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script> 
<script  src="../../common/js/tableStyle.js"></script>
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
        window.open(url, 'newwindow', 'width=600,height=400,top=200,left=300, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }

--></script>
<body>
<!--#include   file= "../../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
          <!-- 操作行 -->
          <tr height="30" align="center">
  	        <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" background="../../common/images/welcome/content-bg.gif" >
                <asp:Button ID="Button1" runat="server" Text="新    增" width="100px" height="25px" CssClass="btn_2k3" OnClientClick = "javascript:showOpenWindow('AddAutoItem.aspx');"/>
                <asp:Button ID="Button2" runat="server" Text="查    询" width="100px" height="25px" CssClass="btn_2k3" OnClientClick = "javascript:showSearchTR();"/>
            </TD>
  	        <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
          </tr>
          <!-- 查询部分--> 
          <tr height="18" align="center" id="trSearch" style="display:none">
  	        <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
  	        <td>
  		        <table class="warp_table" width = "50%" heiht = "200" bgcolor="#F7F8F9">
  			        <tr>
  				        <td width="90%">
  					        <table>
  						        <tr>
							      <td width="5%">
								    <asp:CheckBox ID="cb_search1" runat="server" Checked="true" />
							      </td>
			                      <td width="30%">
			          	            <asp:DropDownList ID="dList_Condition1" runat="server">
                                        <asp:ListItem Value="AID">编码</asp:ListItem>
                                        <asp:ListItem Value="ADESC">英文名</asp:ListItem>
                                        <asp:ListItem Value="ADESCCHS">中文名</asp:ListItem>
                                        <asp:ListItem Value="APREFIX">前缀</asp:ListItem>
                                        <asp:ListItem Value="ADATE">日期类型</asp:ListItem>
                                    </asp:DropDownList>
			                      </td>
			                      <td width="60%">
			          	            <asp:TextBox ID="txtCondition1" runat="server"  Width = "100%"></asp:TextBox>
			          	          </td>
			                    </tr>
			  			        <tr>
							        <td width="5%">
                                        <asp:CheckBox ID="cb_search2" runat="server" />
                                    </td>
		                            <td width="30%">
		          	                    <asp:DropDownList ID="dList_Condition2" runat="server">
                                        <asp:ListItem Value="AID">编码</asp:ListItem>
                                        <asp:ListItem Value="ADESC">英文名</asp:ListItem>
                                        <asp:ListItem Value="ADESCCHS">中文名</asp:ListItem>
                                        <asp:ListItem Value="APREFIX">前缀</asp:ListItem>
                                        <asp:ListItem Value="ADATE">日期类型</asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
			                    <td width="60%">
			          	            <asp:TextBox ID="txtCondition2" runat="server" Width = "100%"></asp:TextBox>
                                    </td>
			                </tr>
  					        </table>
  				        </td>
  				        <td width="10%">
                              <asp:imagebutton runat="server" ImageUrl="../../common/images/search_big.png" style= "cursor:hand "
                                    CausesValidation="false" ID="ImageBtnSearch" onclick="ImageBtnSearch_Click"></asp:imagebutton>
  				        </td>
  			        </tr>
    	        </table>
  	        </td>
  	        <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
          </tr>
          <!-- 换一行 -->  
          <tr align="center">
  	        <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD>
            </TD>
  	        <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
          </tr>
          <!-- 列表部分 -->  
          <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <td valign="top" bgcolor="#F7F8F9">
                <table class="warp_table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False" DataKeyField="AID"  OnItemCreated="DataGrid1_ItemCreated"
                                ondeletecommand="DataGrid1_DeleteCommand" oneditcommand="DataGrid1_EditCommand"  OnSortCommand="DataGrid1_SortCommand"
                                oncancelcommand="DataGrid1_CancelCommand" OnUpdateCommand="DataGrid1_UpdateCommand"
                                OnItemDataBound="DataGrid1_ItemDataBound" onHorizontalAlign="Center" >
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="AID" SortExpression="AID" ReadOnly="True" HeaderText="AutoID">
										<HeaderStyle HorizontalAlign ="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ADESC" SortExpression="ADESC" HeaderText="English Name">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ADESCCHS" SortExpression="ADESCCHS" HeaderText="Chinese Name">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="APREFIX" SortExpression="APREFIX" HeaderText="Prefix">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn SortExpression="ADATE" HeaderText="Date Type" FooterText="ADATE">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
										<ItemTemplate>
											<asp:DropDownList id="Dropdownlist3" runat="server" Enabled="False">
												<asp:ListItem Selected="True"></asp:ListItem>
												<asp:ListItem Value="YYMM">YYMM</asp:ListItem>
												<asp:ListItem Value="YYYYMM">YYYYMM</asp:ListItem>
												<asp:ListItem Value="YY">YY</asp:ListItem>
												<asp:ListItem Value="YYYY">YYYY</asp:ListItem>
												<asp:ListItem Value="YYMMDD">YYMMDD</asp:ListItem>
												<asp:ListItem Value="YYYYMMDD">YYYYMMDD</asp:ListItem>
											</asp:DropDownList>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:DropDownList id="Dropdownlist3" runat="server" >
												<asp:ListItem Selected="True"></asp:ListItem>
												<asp:ListItem Value="YYMM">YYMM</asp:ListItem>
												<asp:ListItem Value="YYYYMM">YYYYMM</asp:ListItem>
												<asp:ListItem Value="YY">YY</asp:ListItem>
												<asp:ListItem Value="YYYY">YYYY</asp:ListItem>
												<asp:ListItem Value="YYMMDD">YYMMDD</asp:ListItem>
												<asp:ListItem Value="YYYYMMDD">YYYYMMDD</asp:ListItem>
											</asp:DropDownList>
										</EditItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="ALENGTH" SortExpression="ALENGTH" HeaderText="Length">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ANEXTNO" SortExpression="ANEXTNO" HeaderText="Next No">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ALASTDATE" SortExpression="ALASTDATE" HeaderText="Last Date">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Edit">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton runat="server" ImageUrl="../../common/images/icon/edt.gif" 
										                    AlternateText="Edit" CommandName="Edit" CausesValidation="false" 
										                    ID="Imagebutton1" ></asp:imagebutton>
										    <asp:imagebutton Runat="server" ImageUrl="../../common/images/icon/delete.gif" 
                                                            AlternateText="Delete" CommandName="Delete" CausesValidation="False" 
                                                            ID="Imagebutton2" ></asp:imagebutton>
										</ItemTemplate>
										<EditItemTemplate>
											<asp:imagebutton runat="server" ImageUrl="../../common/images/icon/floppy.gif" AlternateText="Update" CommandName="Update" CausesValidation="False" ID="Imagebutton3"></asp:imagebutton>
											<img src="../../common/images/icon/spacer.gif" width="3">
											<asp:imagebutton runat="server" ImageUrl="../../common/images/icon/stop.gif" AlternateText="Cancel" CommandName="Cancel" CausesValidation="False" ID="Imagebutton4"></asp:imagebutton>
										</EditItemTemplate> 
									</asp:TemplateColumn>
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
             <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
          </tr>
        </table>
    </form>
</body>
</html>
<script language="javascript">
	//初始化结果表格
	DefineTableCss("changecolor");
</script>
