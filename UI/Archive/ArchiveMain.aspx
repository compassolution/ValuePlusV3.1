
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArchiveMain.aspx.cs" Inherits="Archive_ArchiveMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>档案列表</title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 

<%--<script src="../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../common/jquery-calendar/laydate.js"></script>

<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
<script src="JS/ArchiveMainAjax.js" type="text/javascript"></script>
<style type="text/css">
    

</style>
<script language="javascript" type="text/javascript">

    window.focus();

	function selectOtherRole(strParamString){
	    window.location.href = "ArchiveMain.aspx?"+strParamString;
	}
	
	function selectOtherScene(strParamString){
	    window.location.href = "ArchiveMain.aspx?"+strParamString;
	}
	
	function OpenDetail(strParamString,isOpen){
	    var url = "Detail/EditArchiveDetail.aspx?"+strParamString;
	    if(isOpen==0){
	        window.location.href = url;
	    }else{
            window.open(url, 'ArchiveDetail', 'left=0,top=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-50) +',scrollbars,resizable=yes,toolbar=no,location=no');//最大化打开
        }
        
	}
	
	function selectOneAction(actionPage,actionType){
	    //	    window.open(actionPage,'doAction','height=300, width=500,toolbar=no,scrollbars=auto,location=no,resizable=yes,smenubar=no,center:yes');
	    var randamNum = (Math.floor((Math.random() * 100) + 1)).toString();//100之内的随机数,可同时打开多个动作页面
	    if (actionType == '2') {
	        //如果是执行存储过程类型的动作，则只允许打开一个窗口，及随机数固定为200
	        randamNum = '200';
	    }
	    window.open(actionPage, 'doAction' + randamNum, 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no');//最大化打开
	}
</script>
<script language="javascript">
    function EnterSimpleSearchTextBox()
      {
         if(event.keyCode == 13 && document.all["txtSimpleSearchValue"].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["aSimpleSearch"].click();
         }
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
    function EnterGroupSearchTextBox(txtBoxId)
      {
         if(event.keyCode == 13 && document.all[txtBoxId].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["aGroupSearch"].click();
         }
    }
</script>

</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfCheckBoxValue"  runat="server"/>
<asp:HiddenField ID="hfIsHaveOpener"  runat="server"/>
<table class="table">
 
	<tr id = "trFirst" align="left" runat="server">
		<td align="left" id="tdTitle" runat="server" >
		    <div id="divRoleScene" class="navMenu"><ul>
	            <li id="liRole">
	                <a id="aRole" href="#" style="cursor:hand" class="a_Left"><asp:Label ID="Label_Role" runat="server" Text="Label">角色</asp:Label>>>>></a>
                    <ul>
                        <div id="subNav_aRole" runat="server">
                        </div>
                    </ul>
	            </li>
	            <li id="liSence">
	                <a id="aScene" href="#" style="cursor:hand" class="a_Left"><asp:Label ID="Label_Scene" runat="server" Text="Label">场景</asp:Label></a>
                    <ul>
                        <div id="subNav_aScene" runat="server">
                        </div>
                    </ul>
	            </li>
		    </ul></div>
        </td>
	</tr>
    <tr align="center" id="trGroupSearch" runat="server">
        <td valign="top" bgcolor="#F7F8F9" style="white-space:nowrap" class="td_Frame3">
            <asp:Table width="80%" runat="server" ID="tbSearchCondition" class="table" >
            </asp:Table>

            <table width="80%" style="margin-top:6px">
                <tr>
                    <td style="width:15%" colspan="4" align="center">
                        <asp:LinkButton ID="aGroupReset" runat="server" CssClass="a_Center" OnClick="GroupReset_Click" Width="60px">
                            <asp:Label ID="Label_Reset" runat="server" Text="Label" ForeColor="Gray">Reset/重置</asp:Label>
                        </asp:LinkButton> 
                        <asp:LinkButton ID="aGroupSearch" runat="server" CssClass="a_Center" OnClick="GroupSearch_Click" Width="120px">
                            <asp:Label ID="Label_Query" runat="server" Text="Label">Search/查询</asp:Label>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
 
	<tr id = "tr1" valign="middle">
		<td align="right" valign="middle" id="td2" runat="server" class="td_Frame2">
		    <div style="vertical-align:middle; float:left">
                <asp:DropDownList ID="DDListSimpleSearch" runat="server" Visible="true"></asp:DropDownList>
                <asp:TextBox ID="txtSimpleSearchValue" runat="server" Visible="true" Width="130px" BorderWidth=1></asp:TextBox>
                <asp:LinkButton ID="aSimpleSearch" runat="server" CssClass="a_Right" onclick="SimpleSearch_Click"><asp:Label ID="Label_Search" runat="server" Text="Label">搜索</asp:Label></asp:LinkButton>
            </div>
		    <div style="vertical-align:middle; float:right">
                <div id="divActionNav" runat="server" class="navMenu" style="vertical-align:middle; float:left; margin:auto">
                    <ul style="float:left; margin:auto; vertical-align:bottom">
                        <li id="liAction"><a href="#" class="a_Right" style="font-weight:bold;"><asp:Label ID="Label_Operation" runat="server" Text="Label">可进行操作</asp:Label></a>
                            <ul>
	                            <div id="subNav_aAction" runat="server">
	                            </div>
                            </ul>
                        </li>
                    </ul>
                </div>
                <div style="display:none; vertical-align:middle; float:left; margin:auto" class="navMenu">
                    <asp:LinkButton ID="aSaveChecked" runat="server" CssClass="a_Right" OnClientClick="SaveCheckBoxValue();return false;">保存复选框</asp:LinkButton>
                </div>
                <div style="vertical-align:middle; float:left; margin:auto">
                    <asp:LinkButton ID="aAdd" runat="server" CssClass="a_Right"><asp:Label ID="Label_Add" runat="server" Text="Label">新增</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aRefresh" runat="server" CssClass="a_Right" onclick="Refresh_Click"><asp:Label ID="Label_Refresh" runat="server" Text="Label">刷新</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aExportExcel" runat="server" CssClass="a_Right"><asp:Label ID="Label_ExportExcel" runat="server" Text="Label_ExportExcel">导出Excel</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aShowGroupSearch" runat="server" CssClass="a_Right" onclick="ShowGroupSearch_Click"><asp:Label ID="Label_ShowGroupSearch" runat="server" Text="Label">组合查询</asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aShowDataList" runat="server" CssClass="a_Right" onclick="ShowDataList_Click" Visible="false"><asp:Label ID="Label_ShowDataList" runat="server" Text="Label">显示数据</asp:Label></asp:LinkButton>
                    <a id="aClose" style="display:none" href="javascript:window.close();" class="a_Right">close</a>

                </div>
		    </div>
        </td>
	</tr>

    <tr id="trGrid" runat="server">
        <td valign="top" bgcolor="#F7F8F9" align="left" style="white-space:nowrap">
            <table class="warp_table" id="changecolor" width="100%">
                <tr width="100%">
                    <td id="tdGrid">
                        <asp:DataGrid ID="DataGrid1" BorderWidth="0" CellPadding="0" CellSpacing="0" runat="server" Visible="true" Width="100%" AllowPaging="false" 
                            HorizontalAlign="Center" AutoGenerateColumns="false" AllowSorting="True" ShowFooter="false" 
                            OnItemCreated = "DataGrid1_ItemCreated" OnItemDataBound = "DataGrid1_ItemDataBound"  OnItemCommand = "DataGrid1_ItemCommand"
                            OnSortCommand="DataGrid1_SortCommand">
                            <ItemStyle CssClass="tableContent" />
                            <HeaderStyle CssClass="tableTitle"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="">
                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ImageUrl="../common/images/icon/view.gif" AlternateText="View" Visible="false"
                                            CommandName="View" CausesValidation="false" ID="Imagebutton_View"></asp:ImageButton>
                                        <asp:ImageButton runat="server" ImageUrl="../common/images/icon/edt.gif" AlternateText="Edit" Visible="false"
                                            CommandName="Edit" CausesValidation="false" ID="Imagebutton_Edit"></asp:ImageButton>
                                        <asp:ImageButton runat="server" ImageUrl="../common/images/icon/icon-delete.gif" AlternateText="Delete" Visible="false"
                                            CommandName="Delete" CausesValidation="False" ID="Imagebutton_Delete"></asp:ImageButton>
                                        <asp:CheckBox ID="CB_Select" runat="server" Checked="false" Visible="false"/>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" ImageUrl="../common/images/icon/refresh.gif" ToolTip="Refresh Columns"
                                            Width="20" Height="20" CommandName="Move" CausesValidation="False" ID="Imagebutton_Refresh">
                                        </asp:ImageButton>
                                    </FooterTemplate>
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
                <asp:LinkButton ID="aFirstPage" runat="server" CssClass="a_Right" OnClick="FirstPage_Click"><asp:Label ID="Label_FirstPage" runat="server" Text="Label">首页</asp:Label></asp:LinkButton>
                <asp:LinkButton ID="aPrePage" runat="server" CssClass="a_Right" OnClick="PrePage_Click"><asp:Label ID="Label_PrePage" runat="server" Text="Label">上一页</asp:Label></asp:LinkButton>
                <asp:LinkButton ID="aNextPage" runat="server" CssClass="a_Right" OnClick="NextPage_Click"><asp:Label ID="Label_NextPage" runat="server" Text="Label">下一页</asp:Label></asp:LinkButton>
                <asp:LinkButton ID="aLastPage" runat="server" CssClass="a_Right" OnClick="LastPage_Click"><asp:Label ID="Label_LastPage" runat="server" Text="Label">末页</asp:Label></asp:LinkButton>
		        <asp:Label ID="Label_Page1" runat="server" Text="Label">共</asp:Label>
		            <font color="red"><asp:Label ID="Label_AllCount" runat="server" Text="Label"></asp:Label></font>
		        <asp:Label ID="Label_Page2" runat="server" Text="Label">条记录</asp:Label>
		        ---<asp:Label ID="Label_Page3" runat="server" Text="Label">分</asp:Label>
		            <font color="red"><asp:Label ID="Label_AllPage" runat="server" Text="Label"></asp:Label></font>
		        <asp:Label ID="Label_Page4" runat="server" Text="Label">页</asp:Label>
		        ---<asp:Label ID="Label_Page5" runat="server" Text="Label">当前页</asp:Label>
		            <asp:DropDownList ID="DDList_CurPage" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="DDList_CurPage_SelectedIndexChanged" ></asp:DropDownList>
                ---<asp:Label ID="Label_Page6" runat="server" Text="Label">每页显示</asp:Label><asp:TextBox ID="txtPageSize" runat="server" Visible="true" Width="30px" BorderWidth="1"></asp:TextBox>
                <asp:LinkButton ID="aToPageSize" runat="server" CssClass="a_Right" OnClick="GoPageSize_Click"><asp:Label ID="Label_GO" runat="server" Text="Label">GO</asp:Label></asp:LinkButton>
		    </div>
        </td>
	</tr>
                                 
</table>

 <div id="divGroupSearchArea" runat="server">
 </div>
</form>    
<form id="from10000post" name="from10000post" method="post">
    <input name="hidden" type="hidden" />
</form>
<iframe id="iddownframe000000" name="iddownframe000000" style="width: 0px; height: 0px; display: none;"></iframe>
<script language="javascript" type="text/javascript">
    function exportexcel(filename) {
        //        var sPath = filename + '?type=excel&ran=' + Math.random();
        var sPath = filename + '?type=nopiExcel&ran=' + Math.random();  
        from10000post.action = sPath;
        from10000post.target = "iddownframe000000";
        from10000post.submit(); 
    }
</script>
  <%--  Jquery引入的位置不能随意修改--%>
<script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
</body>
</html>

<script language="javascript">
	//初始化结果表格
    DefineTableCss("changecolor");

    if (window.opener != null) {
        if (window.opener.document.getElementById('aRefresh_PendingList') != null) {//待办页面刷新
            window.opener.document.getElementById('aRefresh_PendingList').click();
        }
        document.getElementById('hfIsHaveOpener').value = "1"; //具有Opener
        document.getElementById('aClose').style.display = "" ;
    } else {
        document.getElementById('aClose').style.display = "none";
    }
    
        
<%--        alert('<%=this.GetProjectId()%>');
        alert('<%=this.TID%>');
        alert('<%=this.RID%>');
        alert('<%=this.SID%>');
        alert('<%=this.GetUserCode()%>');--%>
    
</script>

<script type="text/javascript">
    $(document).ready(function () {
        //更新日期控件
        laydate.skin('molv');//切换皮肤，请查看skins下面皮肤库
        $("input[datetype='date']").each(function () {
            var varId = $(this).attr('id');
            $(this).focus(function () {
                laydate({
                    elem: '#' + varId,
                    format: 'YYYY-MM-DD'
                });
            })
        });
        $("input[datetype='datetime']").each(function () {
            $(this).focus(function () {
                var varId = $(this).attr('id');
                laydate({
                    elem: '#' + varId,
                    format: 'YYYY-MM-DD hh:mm:ss'
                });
            })
        });

        //界面选择框的实时保存到数据库字段值
        //$("input[type='checkbox']").each(function () {
        //    $(this).change(function () {
        //        //console.log($(this));
        //        //var varId = $(this).attr('id');
        //        //var varValue = $(this).attr('checked');
        //        //alert(varId);
        //        //alert(varValue);
        //        setTimeout(function () {
        //            SaveCheckBoxValue();
        //        },50)
        //    })
        //});

        $.getScript("JS/ArchiveMain_Entry.js", function(){
            //可扩展该页面的一些逻辑 ADD BY SAMMEN 20180504
            ExcuteSpecialScript('<%=this.GetProjectId()%>','<%=this.TID%>','<%=this.RID%>','<%=this.SID%>','<%=this.KEY%>','<%=this.GetUserCode()%>');
        });

    });

</script>
