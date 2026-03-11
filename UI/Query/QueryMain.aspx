<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryMain.aspx.cs" Inherits="Query_QueryMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查询主页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<%--<link href="../common/css/ArchiveStyle.css" rel="stylesheet" type="text/css" />--%>

<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/tableStyle.js"></script>
<!--JavaScript部分-->
<script language="javascript">

    window.focus();

    var varSearchTRFlag = 0;
    var varFilterColTRFlag = 0;
    function showSearchTR(){
        if (varSearchTRFlag==0){
	        this.trSearch.style.display = "";
	        varSearchTRFlag = 1;
	        this.changecolor.style.display = "none";
	    }
        else if (varSearchTRFlag==1){
            this.trSearch.style.display = "none";
            this.changecolor.style.display = "";
	        varSearchTRFlag = 0;
	    }
	    this.trFilterCol.style.display = "none";

	}
	function showFilterColTR() {
	    if (varFilterColTRFlag == 0) {
	        this.trFilterCol.style.display = "";
	        this.changecolor.style.display = "none";
	        varFilterColTRFlag = 1;
	    }
	    else if (varFilterColTRFlag == 1) {
	        this.trFilterCol.style.display = "none";
	        this.changecolor.style.display = "";
	        varFilterColTRFlag = 0
	    }
	    this.trSearch.style.display = "none";

	}
	function showOrderWindow() {
	    var tbName = document.getElementById("hfTableName").value;
	    var varOrder = document.getElementById("hfSortString").value;
	    var url = "QuerySort.aspx?tbName=" + tbName + "&order=" + varOrder;
	    window.open(url, 'newwindow', 'width=520,height=500,top=100,left=250, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');

//	    window.showModalDialog(url, 'window', 'dialogWidth:420px;dialogHeight:500px;location:no;edge:raised;resizable:yes;scroll:auto;status:no;center:yes;help:no;minimize:yes;maximize:yes;');
	}
    function showOpenWindow(){
        var url = document.getElementById("hfLocalUrl").value;
        window.open(url, 'newwindow', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no');
    }
    
    function EnterPageSizeTextBox()
      {
         if(event.keyCode == 13 && document.all["txtPageSize"].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["aToPageSize"].click();
         }
    } 
    function EnterConditionTextBox(txtBoxId)
      {
         if(event.keyCode == 13 && document.all[txtBoxId].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["ImageBtnSearch"].click();
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
<!--#include   file= "../common/WaitProccess.htm"--> 
<!--#include   file= "../common/CurPageWaiting.htm"-->
 <form id="form1" runat="server">
<asp:HiddenField ID="hfLocalUrl" runat="server" />
<asp:HiddenField ID="hfTableName" runat="server" />
<asp:HiddenField ID="hfSortString" runat="server" />
<asp:HiddenField ID="hfViewOrSpIsShowDataList" runat="server" />

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tableNoHover">
          <!-- 操作行 -->
          <tr height="22" align="left">
            <TD align ="center" valign="middle">
                <%--<asp:Button ID="btnQueryAll" runat="server" Text="查询所有" width="100px" height="25px" CssClass="btn_2k3" OnClick = "btnQueryAll_Click"/>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnFilterCon" runat="server" Text="条件过滤" width="100px" height="25px" CssClass="btn_2k3" OnClientClick = "javascript:showSearchTR();"/>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnExportExl" runat="server" Text="导出excel文件" Visible="true"   Width="100px" Height="25px"  class="btn_2k3" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnExportTxt" runat="server" Text="导出txt文件" Visible="true"   Width="100px" Height="25px"  class="btn_2k3" />&nbsp;&nbsp;&nbsp
                <asp:Button ID="btnNewWindow" runat="server" Text="新窗口打开" width="100px" height="25px" CssClass="btn_2k3" OnClientClick = "javascript:showOpenWindow();"/>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClose" runat="server" Text="关闭" width="100px" height="25px" CssClass="btn_2k3" OnClientClick = "javascript:window.close();"/>--%>
                
                <div class="topBox">
                    <asp:Label ID="lb_QueryTileName" runat="server" Text="查询标题名称" CssClass="left_bt"></asp:Label>----(
                    <span id = "trCountArea" align="right" runat="server">
                    <!-- 查询结果数显示 -->  
                        <asp:Label ID="lbLabelResult" runat="server" Text="本次查询结果数为：" CssClass="left_ts"></asp:Label> 
                        <asp:Label ID="lbResultCount" runat="server" Text="0" CssClass="left_ts"></asp:Label>
                        <asp:Label ID="lbTipToExportExcel" runat="server" Text="" CssClass="left_ts"></asp:Label>
                    </span>)
                    <asp:LinkButton ID="btnQueryAll" runat="server" CssClass="a_Left" Font-Bold="true" OnClick = "btnQueryAll_Click">查询所有</asp:LinkButton>
                    <asp:LinkButton ID="btnFilterCon" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:showSearchTR();">条件过滤</asp:LinkButton>
                    <asp:LinkButton ID="btnFilterCol" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:showFilterColTR();">列过滤</asp:LinkButton>
                    <asp:LinkButton ID="btnOrder" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:showOrderWindow();">排序规则</asp:LinkButton>
                    <div style="display:none"><asp:LinkButton ID="btnOrderOk" runat="server" CssClass="a_Left" Font-Bold="true" OnClick = "btnOrderOk_Click">排序确定</asp:LinkButton></div>
                    <asp:LinkButton ID="btnExportExl" runat="server" CssClass="a_Left" Font-Bold="true">导出excel文件</asp:LinkButton>
                    <div style="display:none"><asp:LinkButton ID="btnExportPdf" runat="server" CssClass="a_Left" Font-Bold="true">导出PDF文件</asp:LinkButton></div>
                    <asp:LinkButton ID="btnExportTxt" runat="server" CssClass="a_Left" Font-Bold="true">导出txt文件</asp:LinkButton>
                    <asp:LinkButton ID="btnNewWindow" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:showOpenWindow();">新窗口打开</asp:LinkButton>
                    <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" Visible="false" OnClientClick = "javascript:window.close();">关闭</asp:LinkButton>
                     &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                </div>
            </TD>
          </tr>
          <!-- 查询部分--> 
          <tr height="18" align="left" id="trSearch" style="display:none">
  	        <td align="center">
  	        
                <div class="customBox1">
  		            <table width = "600" >
  			        <tr>
  				        <td width="90%">
  					        <table>
  					        <div id="divSearchArea" runat="server">
  						        <tr style="width:100%">
							      <td width="5%">
								    <asp:CheckBox ID="cb_search1" runat="server" Checked="false" />
							      </td>
			                      <td width="35%">
			          	            <asp:DropDownList ID="dList_Condition1" runat="server">
			          	            
                                    </asp:DropDownList>
			                      </td>
			                      <td width="10%">
			          	            <asp:DropDownList ID="DDList_rule1" runat="server">
	                                    <asp:ListItem Value="like">like</asp:ListItem>
			          	                <asp:ListItem Value="=">=</asp:ListItem>
	                                    <asp:ListItem Value=">">></asp:ListItem>
	                                    <asp:ListItem Value=">=">>=</asp:ListItem>
	                                    <asp:ListItem Value="<"><</asp:ListItem>
	                                    <asp:ListItem Value="<="><=</asp:ListItem>
	                                    <asp:ListItem Value="<>"><></asp:ListItem>
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
		          	                    
                                        </asp:DropDownList>
		                            </td>
		                            <td width="10%">
		          	                    <asp:DropDownList ID="DDList_rule2" runat="server">
	                                        <asp:ListItem Value="like">like</asp:ListItem>
		          	                        <asp:ListItem Value="=">=</asp:ListItem>
                                            <asp:ListItem Value=">">></asp:ListItem>
                                            <asp:ListItem Value=">=">>=</asp:ListItem>
                                            <asp:ListItem Value="<"><</asp:ListItem>
                                            <asp:ListItem Value="<="><=</asp:ListItem>
                                            <asp:ListItem Value="<>"><></asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
			                        <td width="50%">
			          	                <asp:TextBox ID="txtCondition2" runat="server" Width = "90%"></asp:TextBox>
                                    </td>
			                    </tr>
			  			        <tr>
							        <td width="5%">
                                        <asp:CheckBox ID="cb_search3" runat="server" />
                                    </td>
		                            <td width="35%">
		          	                    <asp:DropDownList ID="dList_Condition3" runat="server">
		          	                    
                                        </asp:DropDownList>
		                            </td>
		                            <td width="10%">
		          	                    <asp:DropDownList ID="DDList_rule3" runat="server">
	                                        <asp:ListItem Value="like">like</asp:ListItem>
		          	                        <asp:ListItem Value="=">=</asp:ListItem>
                                            <asp:ListItem Value=">">></asp:ListItem>
                                            <asp:ListItem Value=">=">>=</asp:ListItem>
                                            <asp:ListItem Value="<"><</asp:ListItem>
                                            <asp:ListItem Value="<="><=</asp:ListItem>
                                            <asp:ListItem Value="<>"><></asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
			                        <td width="50%">
			          	                <asp:TextBox ID="txtCondition3" runat="server" Width = "90%"></asp:TextBox>
                                    </td>
			                    </tr>
			  			        <tr>
							        <td width="5%">
                                        <asp:CheckBox ID="cb_search4" runat="server" />
                                    </td>
		                            <td width="35%">
		          	                    <asp:DropDownList ID="dList_Condition4" runat="server">
		          	                    
                                        </asp:DropDownList>
		                            </td>
		                            <td width="10%">
		          	                    <asp:DropDownList ID="DDList_rule4" runat="server">
	                                        <asp:ListItem Value="like">like</asp:ListItem>
		          	                        <asp:ListItem Value="=">=</asp:ListItem>
                                            <asp:ListItem Value=">">></asp:ListItem>
                                            <asp:ListItem Value=">=">>=</asp:ListItem>
                                            <asp:ListItem Value="<"><</asp:ListItem>
                                            <asp:ListItem Value="<="><=</asp:ListItem>
                                            <asp:ListItem Value="<>"><></asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
			                        <td width="50%">
			          	                <asp:TextBox ID="txtCondition4" runat="server" Width = "90%"></asp:TextBox>
                                    </td>
			                    </tr>
			  			        <tr>
							        <td width="5%">
                                        <asp:CheckBox ID="cb_search5" runat="server" />
                                    </td>
		                            <td width="35%">
		          	                    <asp:DropDownList ID="dList_Condition5" runat="server">
		          	                    
                                        </asp:DropDownList>
		                            </td>
		                            <td width="10%">
		          	                    <asp:DropDownList ID="DDList_rule5" runat="server">
	                                        <asp:ListItem Value="like">like</asp:ListItem>
		          	                        <asp:ListItem Value="=">=</asp:ListItem>
                                            <asp:ListItem Value=">">></asp:ListItem>
                                            <asp:ListItem Value=">=">>=</asp:ListItem>
                                            <asp:ListItem Value="<"><</asp:ListItem>
                                            <asp:ListItem Value="<="><=</asp:ListItem>
                                            <asp:ListItem Value="<>"><></asp:ListItem>
                                        </asp:DropDownList>
		                            </td>
			                        <td width="50%">
			          	                <asp:TextBox ID="txtCondition5" runat="server" Width = "90%"></asp:TextBox>
                                    </td>
			                    </tr>
			                </div>
  					        </table>
  				        </td>
  				        <td width="10%" align="left">
                              <asp:imagebutton runat="server" ImageUrl="../common/images/search_big.png" style= "cursor:hand "
                                    CausesValidation="false" ID="ImageBtnSearch" onclick="ImageBtnSearch_Click"></asp:imagebutton>
  				        </td>
  			        </tr>
    	        </table>
    	        </div>
  	        </td>
          </tr>
          <!-- 列过滤部分--> 
          <tr height="18" align="left" id="trFilterCol" style="display:none">
  	        <td align="center">
                <div class="customBox1">
                  <asp:ListBox ID="lstBoxGridCol" runat="server"  Width="400" Height="150" ToolTip = "Select Show Column" SelectionMode="Multiple">
	              </asp:ListBox>
                  <asp:imagebutton runat="server" ImageUrl="../common/images/search_big.png" style= "cursor:hand "
                        CausesValidation="false" ID="ImageBtnFilterCol" onclick="ImageBtnFilterCol_Click"></asp:imagebutton>
	            </div>
  	        </td>
  	      </tr>
          <!-- 结果列表部分 --> 
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" width="100%">
                    <tr width="100%">
                        <td style ="width:98%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%"
                                AutoGenerateColumns="true"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand"
                                OnItemCreated="DataGrid1_ItemCreate"  OnItemDataBound = "DataGrid1_ItemDataBound" >
                                <HeaderStyle CssClass="fixGridHeaderStyle"></HeaderStyle>
								<ItemStyle CssClass="tableContent"  />
                                <Columns>
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>	
          <!-- 分页部分-->
          <tr id = "trPageArea" valign="middle" runat="server">
		    <td align="center" valign="middle" id="td3" runat="server" class="td_Frame1">
		        <div style="vertical-align:middle; float:right" class="bottomBox">
		            <asp:Label ID="Label_Page1" runat="server" Text="Label">共</asp:Label>
		                <font color="red"><asp:Label ID="Label_AllCount" runat="server" Text="Label"></asp:Label></font>
		            <asp:Label ID="Label_Page2" runat="server" Text="Label">条记录</asp:Label>
		            ---<asp:Label ID="Label_Page3" runat="server" Text="Label">分</asp:Label>
		                <font color="red"><asp:Label ID="Label_AllPage" runat="server" Text="Label"></asp:Label></font>
		            <asp:Label ID="Label_Page4" runat="server" Text="Label">页</asp:Label>
                    <asp:LinkButton ID="aFirstPage" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="FirstPage_Click"><asp:Label ID="Label_FirstPage" runat="server" Text="Label">首页</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aPrePage" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="PrePage_Click"><asp:Label ID="Label_PrePage" runat="server" Text="Label">上一页</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aNextPage" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="NextPage_Click"><asp:Label ID="Label_NextPage" runat="server" Text="Label">下一页</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aLastPage" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="LastPage_Click"><asp:Label ID="Label_LastPage" runat="server" Text="Label">末页</asp:Label></asp:LinkButton>
		            ---<asp:Label ID="Label_Page5" runat="server" Text="Label">当前页</asp:Label>
		                <asp:DropDownList ID="DDList_CurPage" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="DDList_CurPage_SelectedIndexChanged"  Height="19px"></asp:DropDownList>
                    ---<asp:Label ID="Label_Page6" runat="server" Text="Label">每页显示</asp:Label><asp:TextBox ID="txtPageSize" runat="server" Visible="true" Width="30px" Height="13px"></asp:TextBox>
                    <asp:LinkButton ID="aToPageSize" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="GoPageSize_Click"><asp:Label ID="Label_GO" runat="server" Text="Label">GO</asp:Label></asp:LinkButton>
		        </div>
            </td>
	      </tr>
    </table>
     
    <script language="javascript" type="text/javascript">
        //add by sammen 20201104
        //alert(document.getElementById('hfViewOrSpIsShowDataList').value);
        if (document.getElementById('hfViewOrSpIsShowDataList').value != '1') {
            //如果不默认显示数据列表，则默认打开条件过滤框
            showSearchTR();

        }
    </script>  
</form>
<form id="from10000post" name="from10000post" method="post">
<input name="hidden" type="hidden" />
</form>
  <iframe id="iddownframe000000"  name="iddownframe000000"  style="width:0px;height:0px;display:none;"></iframe>   
  <iframe id="iddownframe000001"  name="iddownframe000001"  style="width:0px;height:0px;display:none;"></iframe> 
  <script language="javascript" type="text/javascript">
    function exportexcel(filename){
//        var sPath = filename + '?type=excel&ran=' + Math.random();
        var sPath = filename + '?type=nopiExcel&ran=' + Math.random();  
        from10000post.action = sPath;
        from10000post.target = "iddownframe000000";
        from10000post.submit();
    }
    function exportpdf(filename) {
        var sPath = filename + '?type=pdf&ran=' + Math.random();
        from10000post.action = sPath;
        from10000post.target = "iddownframe000002";
        from10000post.submit();
    }
    function exporttxt(filename){    
        var sPath = filename + '?type=txt&ran=' + Math.random(); 
        from10000post.action = sPath;
        from10000post.target = "iddownframe000001";
        from10000post.submit(); 
    }
  </script>  
</body>
</html>

<script language="javascript">
	//初始化结果表格
	DefineTableCssNoCursorOver("changecolor");
</script>