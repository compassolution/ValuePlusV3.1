<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NoticeList.aspx.cs" Inherits="Notice_NoticeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>公告信息列表</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<script  src="../common/js/waitProcess.js"></script> 
<script  src="../common/js/tableStyle.js"></script>
</head>
<script language="javascript">
    function showNoticeInfo(key){
        var url = "NoticeView.aspx?noticeKey="+key
        window.open(url, 'newwindow', 'width=800,height=650,top=50,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }

    var varSearchTRFlag = 0;
    function showSearchTR(){
	    if (varSearchTRFlag==0){
		    this.trSearch.style.display = "";
		    varSearchTRFlag = 1;}
	    else if (varSearchTRFlag==1){
		    this.trSearch.style.display = "none";
		    varSearchTRFlag = 0}
    		
    }
--></script>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
          
          <div id="divAdd" runat="server">
              <!-- 操作行 -->
              <tr height="30" align="center">
                <TD align ="center" background="../common/images/welcome/content-bg.gif" >
                    <asp:Button ID="Button1" runat="server" Text="新    增" width="100px" height="25px" CssClass="btn_2k3" OnClick="Button1_Click"/>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="条件过滤" width="100px" height="25px" CssClass="btn_2k3" OnClientClick = "javascript:showSearchTR();"/>
                </TD>
              </tr>
              <!-- 换一行 -->  
              <tr align="center">
                <TD>
                </TD>
              </tr>
          </div><!-- 查询部分--> 
          <tr height="18" align="center" id="trSearch" style="display:none">
  	        <td>
  		        <table class="warp_table" width = "50%" heiht = "200" bgcolor="#F7F8F9">
  			        <tr>
  				        <td width="90%">
  					        <table>
  					        <div id="divSearchArea" runat="server">
  						        <tr style="width:100%">
							      <td width="5%">
								    <asp:CheckBox ID="cb_search1" runat="server" Checked="true"/>
							      </td>
			                      <td width="35%">
			          	            <asp:DropDownList ID="dList_Condition1" runat="server">
                                        <asp:ListItem Value="STITLE">公告标题</asp:ListItem>
                                        <asp:ListItem Value="SPUBLISHOR">公告发布者</asp:ListItem>
                                        <asp:ListItem Value="DTPUBLISHTIME">公告发布时间</asp:ListItem>
                                        <asp:ListItem Value="BISSTOP">是否停用</asp:ListItem>			          	            
                                    </asp:DropDownList>
			                      </td>
			                      <td width="10%">
			          	            <asp:DropDownList ID="DDList_rule1" runat="server">
			          	                <asp:ListItem Value="=">=</asp:ListItem>
	                                    <asp:ListItem Value=">">></asp:ListItem>
	                                    <asp:ListItem Value=">=">>=</asp:ListItem>
	                                    <asp:ListItem Value="<"><</asp:ListItem>
	                                    <asp:ListItem Value="<="><=</asp:ListItem>
	                                    <asp:ListItem Value="<>"><></asp:ListItem>
	                                    <asp:ListItem Value="like">like</asp:ListItem>
                                    </asp:DropDownList>
			                      </td>
			                      <td width="50%">
			          	            <asp:TextBox ID="txtCondition1" runat="server"  Width = "90%"></asp:TextBox>
			          	          </td>
			                    </tr>
			  			        <tr>
							        <td width="5%">
                                        <asp:CheckBox ID="cb_search2" runat="server" />
                                    </td>
		                            <td width="35%">
		          	                    <asp:DropDownList ID="dList_Condition2" runat="server">
                                            <asp:ListItem Value="STITLE">公告标题</asp:ListItem>
                                            <asp:ListItem Value="SPUBLISHOR">公告发布者</asp:ListItem>
                                            <asp:ListItem Value="DTPUBLISHTIME">公告发布时间</asp:ListItem>
                                            <asp:ListItem Value="BISSTOP">是否停用</asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
		                            <td width="10%">
		          	                    <asp:DropDownList ID="DDList_rule2" runat="server">
		          	                        <asp:ListItem Value="=">=</asp:ListItem>
                                            <asp:ListItem Value=">">></asp:ListItem>
                                            <asp:ListItem Value=">=">>=</asp:ListItem>
                                            <asp:ListItem Value="<"><</asp:ListItem>
                                            <asp:ListItem Value="<="><=</asp:ListItem>
                                            <asp:ListItem Value="<>"><></asp:ListItem>
                                            <asp:ListItem Value="like">like</asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
			                        <td width="50%">
			          	                <asp:TextBox ID="txtCondition2" runat="server" Width = "90%"></asp:TextBox>
                                    </td>
			                    </tr>
			                </div>
  					        </table>
  				        </td>
  				        <td width="10%">
                              <asp:imagebutton runat="server" ImageUrl="../common/images/search_big.png" style= "cursor:hand "
                                    CausesValidation="false" ID="ImageBtnSearch" onclick="ImageBtnSearch_Click"></asp:imagebutton>
  				        </td>
  			        </tr>
    	        </table>
  	        </td>
          </tr>
          <div id="divTemp" runat="server"></div>
          <!-- 列表部分 -->  
          <tr>
            <td valign="top" bgcolor="#F7F8F9">
                <table class="table" id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True"  Width="100%"  
                                AutoGenerateColumns="False"  DataKeyField="SKEY" OnSortCommand="DataGrid1_SortCommand" OnItemDataBound = "DataGrid1_ItemDataBound"
                                HorizontalAlign="Center" OnItemCreated="DataGrid1_ItemCreated" OnItemCommand = "DataGrid1_ItemCommand">
								<ItemStyle CssClass="tableContent"  />
								<HeaderStyle CssClass="tableTitle" ></HeaderStyle>
                                <Columns>
									<asp:BoundColumn DataField="STITLE" SortExpression="STITLE" ReadOnly="True" HeaderText="Notice Title">
										<HeaderStyle HorizontalAlign ="Center" Width="60%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="left" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPUBLISHOR" SortExpression="SPUBLISHOR" HeaderText="SPUBLISHOR" Visible="false">
										<HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DTPUBLISHTIME" SortExpression="DTPUBLISHTIME" HeaderText="DTPUBLISHTIME">
										<HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BISSTOP" SortExpression="BISSTOP" HeaderText="BISSTOP" Visible="true">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Option">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
										    <asp:imagebutton Runat="server" ImageUrl="../common/images/icon/view.gif" 
										                    AlternateText="view/查看" CommandName="Detail" CausesValidation="False" 
										                    ID="Imagebutton1"></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../common/images/icon/edt.gif" 
								                    AlternateText="edit/修改" CommandName="Edit" CausesValidation="False" 
								                    ID="Imagebutton2"></asp:imagebutton>
								            <asp:imagebutton Runat="server" ImageUrl="../common/images/icon/delete.gif" 
                                                    AlternateText="Delete/删除" CommandName="Delete" CausesValidation="False" 
                                                    ID="Imagebutton3" ></asp:imagebutton>
										            
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
<script language="javascript">
	//初始化结果表格
	DefineTableCss("changecolor");
</script>
