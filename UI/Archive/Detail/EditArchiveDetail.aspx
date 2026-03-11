<%@ page language="C#" autoeventwireup="true" CodeFile="EditArchiveDetail.aspx.cs" inherits="Archive_Detail_EditArchiveDetail" enableeventvalidation="false" maintainscrollpositiononpostback="true" validaterequest="false" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>档案明细编辑页面</title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/ArchiveStyle.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/fixAreaStyle.css" rel="stylesheet" type="text/css" />
<link href="../../common/jImagePreview/index.css" rel="stylesheet" type="text/css" />

<script src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>
<script src="../../common/js/stringUtil.js" type="text/javascript"></script>
<script src="../../common/js/MainUtil.js" type="text/javascript"></script>

<script src="../JS/ArchiveDetailAjax.js" type="text/javascript"></script>
<script src="../JS/ArchiveDetailStyle.js" type="text/javascript"></script>
<script src="../JS/ArchiveDetailSave.js" type="text/javascript"></script>
<script src="../JS/SavePagePosition.js" type="text/javascript"></script>

<%--<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../../common/jquery-calendar/laydate.js"></script>

<script type="text/javascript">

    window.focus();

    function showOpenWindow(url){
        window.open(url, 'ArchiveDetailPage', 'width=1000,height=800,top=100,left=100, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
    }
    
    function OpenGridDetailPage(url){
        window.open(url, 'GridDetailPage', 'width=1000,height=680,top=20,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
    }

    function selectOneAction(actionPage, isAutoSave, actionType) {//选择某个动作
        document.getElementById("hfCurActionUrl").value = actionPage;
        //如果需要自动保存，则首先执行保存按钮操作。
        if (isAutoSave == '1') {
            document.getElementById("hfIsAutoSave").value = '1'; //设置是否自动保存
            document.getElementById("aSave").click(); //自动保存主界面
        } else {
            //如果此动作无需自动保存，则直接继续动作执行
            doExcuteAction(actionType);
        }
    }

    function doExcuteAction(actionType) {//执行动作
        var randamNum = (Math.floor((Math.random() * 100) + 1)).toString();//100之内的随机数,可同时打开多个动作页面
        if (actionType == '2') {
            //如果是执行存储过程类型的动作，则只允许打开一个窗口，及随机数固定为200
            randamNum = '200';
        }
        var actionPage = document.getElementById("hfCurActionUrl").value
        window.open(actionPage, 'doAction' + randamNum, 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no');
        
    }

	function OpenDbTextBoxWindow(url, ctrlId) {
	    var keyValue = document.getElementById(ctrlId).value;
        var varUrl = url + "&KEYVALUE=" + keyValue;
        //兼容Edge浏览器的特殊设置
        varUrl = varUrl + "&TextCtrlId=" + ctrlId;
	    window.open(varUrl, 'DbTextBoxWindow', 'width=900,height=500,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=YES,location=no, status=no');
	}

	function AddDetail(strParamString, isOpen) {
	    var url = "EditArchiveDetail.aspx?" + strParamString;
	    if (isOpen == 0) {
	        window.location.href = url;
	    } else {
	        window.open(url, 'ArchiveDetail', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no');//最大化打开
	    }

	}
</script>
 
<script type="text/javascript">
    /*
       在页面中回车事件
   */
    document.onkeydown = keyDownSearch;

    function keyDownSearch(e) {
        // 兼容FF和IE和Opera    
        var theEvent = e || window.event;
        var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
        if (code == 13) {
            return false;
        }
        return true;
    }
     /*
        在页面中引用该脚本，页面过长时，页面回传后滚动条位置保持不变
    */
    document.onclick=function()
    {
        //写入Coolie进行编码
        document.cookie="scrollTop="+encodeURIComponent(document.body.scrollTop);
        document.cookie="scrollLeft="+encodeURIComponent(document.body.scrollLeft);
    }
    document.onselectionchange=function()
    {
        document.cookie="scrollTop="+encodeURIComponent(document.body.scrollTop);
        document.cookie="scrollLeft="+encodeURIComponent(document.body.scrollLeft);
    }
    window.onload=function()
    {
        var allcookies=document.cookie;
    
        if(allcookies==null) return;
        var pos=allcookies.indexOf("scrollTop=");
        if(pos!=-1)
        {
            var start=pos+10;
            var end=allcookies.indexOf(";",start);
            if(end==-1) end=allcookies.length;
            var value=allcookies.substring(start,end);
            //读取Coolie进行编码
            value=decodeURIComponent(value);            
            document.body.scrollTop=value;
        }
        pos=allcookies.indexOf("scrollLeft=");
        if(pos!=-1)
        {
            var start=pos+11;
            var end=allcookies.indexOf(";",start);
            if(end==-1) end=allcookies.length;
            var value=allcookies.substring(start,end);
            value=decodeURIComponent(value);
            document.body.scrollLeft=value;
        }
    }

    //返回按钮的客户端事件
    function DoBackPage() {
        if (IsInputed) {
            if (confirm(document.getElementById("hfIsSureLeaveWhenChanged").value)) {
                ShowWaitingDiv();
                return true;
            } else {
                return false;
            }
        } else {
            ShowWaitingDiv();
            return true;
        }
    }
</script>
<style type="text/css">
    
</style>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<!--#include   file= "../../common/CurPageWaiting.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfGirdTableName" runat="server" />
<asp:HiddenField ID="hfIsOpenAtCurPage" runat="server" />
<asp:HiddenField ID="hfIsSaveSuccess" runat="server" /><%--是否保存成功（主要是在自动保存时，如果不成功则不进入动作页面）--%>
<asp:HiddenField ID="hfIsAutoSave" runat="server" /><%--是否是通过动作来自动保存--%>
<asp:HiddenField ID="hfCurActionUrl" runat="server" /><%--当前执行的操作链接--%>
<asp:HiddenField ID="hfIsSureLeaveWhenChanged" runat="server" /><%--本页面存在数据修改尚未保存，是否确定要离开?--%>
<table width="100%" >
    <tr style="height:60" align="center" id="divTopToolBar" runat="server">
        <td align ="left" class="td_Frame2" >
            <div id="divTop" class="fixActionAreaTop">
                <div id="div1" runat="server" style="float:left; margin:auto">
                    <asp:LinkButton ID="aBack" runat="server" CssClass="a_Left" OnClick = "aBack_Click" Visible="false" OnClientClick="return DoBackPage();">
                        <asp:Label ID="Label_Back" runat="server" Text="Label">返   回</asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton ID="aClose" runat="server" CssClass="a_Left" OnClick = "aClose_Click" OnClientClick="return ShowWaitingDiv();">
                        <asp:Label ID="Label_Close" runat="server" Text="Label">关  闭</asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton ID="aSave" runat="server" CssClass="a_Left" OnClick = "aSave_Click" OnClientClick="return onPreSaved();">
                        <asp:Label ID="Label_Save" runat="server" Text="Label">保  存</asp:Label>
                    </asp:LinkButton>
                </div>
                <div id="div2"  style=" display:none">
                    <asp:LinkButton ID="aAddDetail" runat="server" CssClass="a_Right">
                        <asp:Label ID="Label_Add" runat="server" Text="Label">新增</asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton ID="aRefreshDetail" runat="server" CssClass="a_Left" OnClick = "aRefresh_Click">
                        <asp:Label ID="Label_Refresh" runat="server" Text="Label">重新加载数据</asp:Label>
                    </asp:LinkButton>
                </div>
                <div id="divActionArea" runat="server" style="float:left; margin:auto">
                </div>
                <div style="float:right; margin:auto">
                    <asp:ImageButton ID="imgBtnPreRefresh" ImageUrl="../../common/images/icon/refresh.gif" runat="server" ToolTip = "Refresh"  OnClick="aRefresh_Click" Height="16" Width="16" />
                    <asp:ImageButton ID="imgBtnPre" ImageUrl="../../common/images/turnLeft.png" runat="server" ToolTip = "Forward"  OnClick="imgBtnPre_Click" Height="16" Width="16" />
                    <asp:DropDownList id="ddListKeyValue" runat="server" OnSelectedIndexChanged="ddListKeyValue_SelectedIndexChanged" AutoPostBack="true">
		            </asp:DropDownList>
                    <asp:ImageButton ID="imgBtnNext" ImageUrl="../../common/images/turnRight.png" runat="server" ToolTip = "Next" OnClick="imgBtnNext_Click" Height="16" Width="16"  />
                </div>
            </div>
        </td>
    </tr>
    <tr id="trAchiveContent" oncontextmenu=return(true) style = "height:100%">
        <td valign="top" colspan="2">
            <div id="div_All">
                <table border="0" class="table_archive" width="100%" id="tbTabs" align="center" style="height:auto" runat="server">
		          <tr class="fixTabStyle">
		            <td>
		                <iewc:TabStrip id="TabStrip1" runat="server" TargetID="MultiPage1" OnSelectedIndexChange="TabStrip1_SelectedIndexChange"
			                    TabDefaultStyle="padding:3px 4px; border:1px solid #ccc; color:#888;border-bottom: 1px solid #6c6;text-decoration:none;background:#f7f7f7;"
			                    TabHoverStyle="background:#6c6; border-bottom-color:#DDDDDD; border-bottom-width:1PX; border-bottom-style:solid; cursor:hand;color:#666666;font-weight:bolder;"
		                        TabSelectedStyle="background:#ddd; border-width:1px; border-style:solid; border-color:#DDDDDD; border-bottom-width:0px; color:Red; cursor:default;font-weight:bolder;"  >
                        </iewc:TabStrip>
		            </td>
		          </tr>
			      <tr>
			        <td>
				        <div><iewc:MultiPage id="MultiPage1" runat="server"></iewc:MultiPage></div>
				    </td>
			      </tr>
	            </table>
	            <div id ="divCheckGroup" runat="server" style="margin:auto">
                    <table border="0" class="table_archive" width="100%" align="center" style="height:auto">
		              <tr>
		                <td style="width:30px" class="td_Normal_Label" align="center">
                            <input id="ckb_SelectAll" type="checkbox" checked="checked" style="cursor:hand" onclick="AllGroupCheck();" />
                            <asp:Label  ID="lbSelcetAll" runat="server" Text="Label">全选</asp:Label>
		                </td>
		                <td id = "tdGroupCheckBox" runat="server" class="td_Normal_Label">
		                </td>
		              </tr>
		            </table>
                </div>
                <asp:Panel ID="Panel_View" runat="server"></asp:Panel>
            </div>
        </td>
    </tr>
</table>
</form>
  

  <%--  Jquery引入的位置不能随意修改--%>
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../../common/plugins/jquerytimexz/js/jquery-clock-timepicker.min.js" type="text/javascript"></script>

<%--jQuery鼠标点击图片预览插件--%>
<script src="../../common/jImagePreview/preview-photo.js" type="text/javascript"></script>

<%--jQuery鼠标悬停图片预览插件 add by sammen 20220727--%>
<script src="../../common/imgPreview/imgpreview.full.0.22.jquery.js" type="text/javascript"></script>

<%--模板设置中的事件配置区域--%>
<div id="divClientEvent" runat="server">

</div>

</body>
</html>

<script language="javascript">
    var oTable = document.getElementById("hfGirdTableName").value;
    var Arr1 = oTable.split("*");
    
    for( i = 0; i< Arr1.length; i++ )
    {
        var oTableId = Arr1[i];
        
        //设置存在列名的table表格样式("表格名称","奇数行背景","偶数行背景","鼠标经过背景","点击后背景","列名行背景");
        biuldTableCss(oTableId,"#ffffff","#f0f0f0","#E3EEFD","#C2DAF1","#ddd");
    }
</script>

<script type="text/javascript">
    var IsInputed = false;
    $(document).ready(function () {
        //PBMS系统中CRM项目管理中的模板明细页面的客户端额外处理 add by sammen 20170514
        //如果其他模板有需要，也可以扩展
        if ('<%=this.TID%>' == 'CProject') {
            $.getScript("../JS/ArchiveDetail_CProject.js");
        }else if ('<%=this.TID%>' == 'CContact') {
            $.getScript("../JS/ArchiveDetail_CContact.js");
        }
		
        //HR系统的扩展
        else if ('<%=this.TID%>' == 'FLOT') {//加班申请流程
            $.getScript("../JS/ArchiveDetail_FLOT.js");
        }
        else if ('<%=this.TID%>' == 'FLMP') {//临时用工加班申请
            $.getScript("../JS/ArchiveDetail_FLMP.js");
        }

        //输入是否存在变更
        $('input,select,textarea').change(function () {
            IsInputed = true; 
        });

        $('select').each(function () {
            var varId = $(this).attr('id');
            console.log(varId);
        });

        //输入框的回车事件(跳转到下一元素)
        $('input,select,textarea').keydown(function (e) {
            // 兼容FF和IE和Opera    
            var theEvent = e || window.event;
            var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
            if (code == 13) {
                var inputs = $("#trAchiveContent").find("input,select,textarea"); // 获取表单中的所有输入框  
                var idx = inputs.index(this); // 获取当前焦点输入框所处的位置  
                if (idx == inputs.length - 1) {// 判断是否是最后一个输入框  

                } else {
                    inputs[idx + 1].focus(); // 设置焦点  
                    inputs[idx + 1].select(); // 选中文字  
                }
                return false;// 取消默认的提交行为  
            }
            return true;
        });

    });

    //更新日期控件
    !function () {
        laydate.skin('molv');//切换皮肤，请查看skins下面皮肤库
        $("input[datetype='date']").each(function () {
            var varId = $(this).attr('id');
            $(this).focus(function () {
                laydate({
                    elem: '#' + varId,
                    format: 'YYYY-MM-DD',
                    choose: function (datas) { //选择日期完毕的回调
                        $("#" + varId).trigger("input");
                    }
                });
            })
        });
        $("input[datetype='datetime']").each(function () {
            $(this).focus(function () {
                var varId = $(this).attr('id');
                laydate({
                    elem: '#' + varId,
                    istime: true,
                    format: 'YYYY-MM-DD hh:mm:ss',
                    choose: function (datas) { //选择日期完毕的回调
                        $("#" + varId).trigger("input");
                    }
                });
            })
        });

        if (myBrowser() != 'IE') {
            //仅时间选择HH:mm
            $("input[ptype='time']").clockTimePicker({});
        }

    }();

</script>

<%--//设置Edge兼容时需更新此方法内容--%>
<script type="text/javascript">
    $(document).ready(function () {
        //数据文本类型的控件
        $("input[ctrltype='2']").each(function () {
            var varId = $(this).attr('id');
            //兼容Edge浏览器的特殊设置[由于Edge浏览器不支持onpropertychange事件，在图片选择框选择后触发click事件，此处捕捉click事件进行处理]
            $(this).click(function () {
                onChangeCtrlValue(varId, $(this).val());
            })
        })

        // 文本框（图片）鼠标悬停图片预览插件 add by sammen 20220727
        $('.Image_IMAGETEXT_Archive').imgPreview({
            imgCSS: { width: 400 },
            srcAttr: "src",
        });
        // 文本框（图片）预览初始化，鼠标点击显示
        window.PreviewPhoto({
            imageContainerType: 1 // 图片承载类型 1、iframe  2、div
        });


    }) 
</script>