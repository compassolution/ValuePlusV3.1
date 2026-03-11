
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArchiveGridRecordDetail.aspx.cs" Inherits="Archive_Detail_ArchiveGridRecordDetail"  enableeventvalidation="false" maintainscrollpositiononpostback="true" validaterequest="false" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>模板明细表格记录明细信息</title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
    
<link href="../../common/css/ArchiveStyle.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/fixAreaStyle.css" rel="stylesheet" type="text/css" />
<script src="../../common/js/waitProcess.js"type="text/javascript"></script>
<script src="../../common/js/MainUtil.js" type="text/javascript"></script>

<%--<script src="../../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../../common/jquery-calendar/laydate.js"></script>

<script src="../JS/ArchiveDetailAjax.js" type="text/javascript"></script>
<script src="../JS/ArchiveDetailStyle.js" type="text/javascript"></script>
<script src="../JS/ArchiveDetailSave.js" type="text/javascript"></script>
<script type="text/javascript">
    window.focus();
    
    function closeWindow(){
        window.close();
    }
    
    function OpenDbTextBoxWindow(url, ctrlId) {
        var keyValue = document.getElementById(ctrlId).value;
        var varUrl = url + "&KEYVALUE=" + keyValue;
        //兼容Edge浏览器的特殊设置
        varUrl = varUrl + "&TextCtrlId=" + ctrlId;
        window.open(varUrl, 'newwindow', 'width=900,height=500,top=100,left=250, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');
    }
</script>
</head>
<body>
<!--#include   file= "../../common/CurPageWaiting.htm"--> 
    <form id="form1" runat="server">
<table width="99%">
    <tr>
        <td valign="top" colspan="2" class="td_Frame2" >
            <div id="divTop" class="fixActionAreaTop">
                <div>
                    <div id="div1"  runat="server" style=" float:left; margin :auto">
                        <asp:LinkButton ID="aClose" runat="server" CssClass="a_Center" OnClientClick = "closeWindow();">
                            <asp:Label ID="Label_Close" runat="server" Text="Label">关  闭</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="aSaveT" runat="server" CssClass="a_Center" OnClick = "aSaveT_Click" OnClientClick="return onPreSaved();">
                            <asp:Label ID="Label_SaveT" runat="server" Text="Label">保存并关闭</asp:Label>
                        </asp:LinkButton>
                        <asp:LinkButton ID="aSaveF" runat="server" CssClass="a_Center" OnClick = "aSaveF_Click" OnClientClick="return onPreSaved();">
                            <asp:Label ID="Label_SaveF" runat="server" Text="Label">保存后继续</asp:Label>
                        </asp:LinkButton>
                    </div>
                    <div id="div2"  runat="server" style="display:none">
                        <asp:LinkButton ID="aReloadPage" runat="server" CssClass="a_Center" OnClick = "aReloadPage_Click" OnClientClick="return onPreSaved();">
                            <asp:Label ID="Label3" runat="server" Text="Label">重新加载</asp:Label>
                        </asp:LinkButton>
                    </div>
                    <div style="float:right; margin:auto">
                        <asp:ImageButton ID="imgBtnPre" ImageUrl="../../common/images/turnLeft.png" runat="server"  OnClick="imgBtnPre_Click" Height="16" Width="16" />
                        <asp:DropDownList id="ddListKeyValue" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddListKeyValue_SelectedIndexChanged">
		                </asp:DropDownList>
                        <asp:ImageButton ID="imgBtnNext" ImageUrl="../../common/images/turnRight.png" runat="server" OnClick="imgBtnNext_Click" Height="16" Width="16"  />
                    </div>
                </div>
            </div>
        </td>
    </tr>
    <tr align="center" id="trAchiveContent" oncontextmenu=return(false)>
        <td align ="left">
            <Table ID="Table1"  class="table_archive" border="0" style="width:90%;">
			  <tr>
			    <td>
				    <div><iewc:MultiPage id="MultiPage1" runat="server"></iewc:MultiPage></div>
				</td>
			  </tr>
            </Table>
        </td>
    </tr>
</table>
    </form>
  <%--  Jquery引入的位置不能随意修改--%>
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../../common/plugins/jquerytimexz/js/jquery-clock-timepicker.min.js" type="text/javascript"></script>
<%--模板设置中的事件配置区域--%>
<div id="divClientEvent" runat="server">

</div>
</body>
</html>

<script type="text/javascript">
    $(document).ready(function () {
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

        //更新日期控件
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
    });

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
    }) 
</script>