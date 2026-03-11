
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreeDetail.aspx.cs" Inherits="Tree_TreeDetail" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detail</title>

<link href="../common/css/ArchiveStyle.css" rel="stylesheet" type="text/css" />
<link href="../common/css/fixAreaStyle.css" rel="stylesheet" type="text/css" />
<script  src="../common/js/waitProcess.js"type="text/javascript"></script>

<%--<script src="../common/JS/vpCalendar.js" type="text/javascript"></script>--%>
<script type="text/javascript" src="../common/jquery-calendar/laydate.js"></script>

<script src="../Archive/JS/ArchiveDetailSave.js" type="text/javascript"></script>

  
 <script type="text/javascript">
    window.focus();
    
    function closeWindow(){
        window.close();
    }

    function RefreshOpener() {
        if(window.opener!=null){
            if(window.opener.document.getElementById("aRefresh")!=null){
                window.opener.document.getElementById("aRefresh").click();
                window.close();
            }
        }
    }

    //主控字段下拉框控件ONCHANGE事件(屏蔽档案模板中的相关事件)
    function onChangeCtrlValue(ctrlId, keyValue) {
        var ddList = document.getElementById(ctrlId);
//        try {
//            Archive_Detail_ArchiveDetailAjax.GetBeMastControlList(ctrlId, ddList.value, keyValue, get_CtrlDataSet_CallBack);
//        } catch (e) {
//            alert(e.ToString());
//            throw e;
//        }
    }
    
</script>
</head>
<body>
<!--#include   file= "../common/CurPageWaiting.htm"--> 
    <form id="form1" runat="server">
<table width="99%">
    <tr>
        <td valign="top" colspan="2" class="td_Frame2" >
            <div class="fixActionAreaTop">
                <div id="div1"  runat="server" style=" float:left; margin :auto">
                    <asp:LinkButton ID="aClose" runat="server" CssClass="a_Center" OnClientClick = "closeWindow();">
                        <asp:Label ID="Label_Close" runat="server" Text="Label">关  闭</asp:Label>
                    </asp:LinkButton>
                    <asp:LinkButton ID="aSave" runat="server" CssClass="a_Center" OnClick = "aSave_Click" OnClientClick="return onPreSaved();">
                        <asp:Label ID="Label_Save" runat="server" Text="Label">保  存</asp:Label>
                    </asp:LinkButton>
                </div>
                <div style="float:right; margin:auto">
                    <asp:ImageButton ID="imgBtnPre" ImageUrl="../common/images/turnLeft.png" runat="server"  OnClick="imgBtnPre_Click" Height="16" Width="16" />
                    <asp:DropDownList id="ddListKeyValue" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddListKeyValue_SelectedIndexChanged">
		            </asp:DropDownList>
                    <asp:ImageButton ID="imgBtnNext" ImageUrl="../common/images/turnRight.png" runat="server" OnClick="imgBtnNext_Click" Height="16" Width="16"  />
                </div>
            </div>
        </td>
    </tr>
    <tr align="center" id="trAchiveContent">
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
<script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
</body>
</html>

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
    });

</script>
