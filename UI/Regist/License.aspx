
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="License.aspx.cs" Inherits="Regist_License" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>License Info</title>
<link href="../common//css/topStyle.css" rel="stylesheet" type="text/css" />
    
<script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../common/JS/MainUtil.js" type="text/javascript"></script>

<SCRIPT language="javascript">
	function showWarningPage(msg)
	{
	    window.open('LicenseWarning.aspx?msg='+msg, 'LicenseWarning', 'left=0,top=0,width=400,height=200,scrollbars=no,resizable=no,toolbar=no,location=no');
	}
	
</SCRIPT>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" class="table" width="100%" id="tb1" align="center" style="height:auto">
		  <tr>
		    <td align="left" id="tdProduct" style="font-size:4">
		        <asp:LinkButton ID="lb_Product" runat="server" Text="" OnClientClick="javascript:ShowUpdateList();return false;"></asp:LinkButton>
                <span id="lb_SysUpdateNotice"></span>
            </td>
		    <td align="right" id="tdInvalid" runat="server" style="font-size:4">
		        <font color="red"><asp:Label ID="Label_Invalid" runat="server" Text=""></asp:Label></font>
            </td>
		    <td align="right" id="tdValid" runat="server" style="font-size:4">
		        <asp:Label ID="Label_Name" runat="server" Text="软件授权使用者："></asp:Label>
		        <asp:Label ID="lb_Name" runat="server" Text=""></asp:Label>----
		        <asp:Label ID="Label_Date" runat="server" Text="有效期至："></asp:Label>
		        <asp:Label ID="lb_Date" runat="server" Text=""></asp:Label>
            </td>
		  </tr>
        </table>
    </form>
    
<!--系统更新监听-->
<script type="text/javascript">
    var projectId = '<%=this.GetProjectId()%>';
    //var projectId = '待填';
    var remoteServer = '<%=this.GetRemoteServer()%>';

    function GetUpdateCount() {
        $("#lb_Product").css("color", "black");
        $("#lb_Product").text($("#lb_Product").text().replace("(New)",""));
        var strParam = "jsonCallback=&projectid=" + projectId;
        var urlQuery = escape("M=" + Math.random() + "&param=getcount&" + strParam);
        var url = "<%=this.GetRemoteServer()%>"+"/SysUpdate/Server/UpdateServerHandler.ashx?" + urlQuery;

        try{
            //alert(url);
            $.ajax({
                cache: false,
                url: url,
                contentType: "application/json; charset=utf-8",
                crossDomain: true,//支持跨站请求
                async: false,
                type: "post",
                dataType: "jsonp",
                //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback) 
                jsonp: "callback",
                //自定义的jsonp回调函数名称"jsonpCallback"，返回的json也必须有这个函数名称
                jsonpCallback: "jsonpCallback",
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(jqXHR);
                    //alert(textStatus);
                    //alert(errorThrown);
                },
                success: function (result) {
                    //alert(result.count);
                    if (result == "") return false;
                    if (parseInt(result.count) > 0) {
                        $("#lb_Product").css("color", "red");
                        $("#lb_Product").append("(New)");
                        if ('<%=IsAlertSysUpdate%>'=='1'){
                            if (confirm("<%=strLbUpdateAlert%>")){
                                ShowUpdateList();
                            }
                        }
                    }
                }
            });
        } catch (err) {
            return;
        }

    }

    function ShowUpdateList() {
        var url = "../SysUpdate/Client/UpdateList.aspx";
        var obj = new Object();
        obj.name = "SysUpdateList";
        var varWidth = screen.availWidth - 300;
        var varHeight = screen.availHeight - 200;
        var varParam = "dialogWidth=" + varWidth + "px;dialogHeight=" + varHeight + "px";
        window.showModalDialog(url, obj, varParam);
        //window.open(url, "", "height=500, width=860,top=270,left=530");
    }
    
    $(document).ready(function () {
        if (projectId != '') {
            GetUpdateCount();
        }
    });
</script>

</body>
</html>
