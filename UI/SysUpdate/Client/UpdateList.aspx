<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateList.aspx.cs" Inherits="SysUpdate_Client_UpdateList" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8"/>
    <title><%=strLbUpdateList %></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
    <link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
    <link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css"/>
    <link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
    <link href="../../common/bootstrap/bootstrap.css" rel="stylesheet"/>
    <link href="../../common/bootstrap-table/bootstrap-table.css" rel="stylesheet"/>
</head>
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script> 
<script src="../../common/layer/layer.min.js" type="text/javascript"></script>
<script src="../../common/layer/layerLoading.js" type="text/javascript"></script>
<script src="../../common/bootstrap/bootstrap.js" type="text/javascript"></script> 
<script src="../../common/bootstrap/bootstrap-tab.js" type="text/javascript"></script>
<script src="../../common/bootstrap-table/bootstrap-table.js" type="text/javascript"></script> 
<script src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<body>
<div class="container-fluid">
<div class="row-fluid">
<div class="span12">
    <form id="formList" class="form-horizontal" method="post">
        <div class="h4"><%=strLbUpdateList %>
        </div>

        <div class="row-fluid">
            <div class="tabbable" style="margin:8px;"> <!-- Only required for left/right tabs -->
                <ul class="nav nav-tabs">
                    <li id="liPage1" name = "liPage"><a id = "aPage1" class="pagetab" href="#tabPage1" data-toggle="tab"><%=strLbNeedUpdate %></a></li>
                    <li id="liPage2" name = "liPage"><a id = "aPage2" class="pagetab" href="#tabPage2" data-toggle="tab"><%=strLbHadUpdated %></a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="tabPage1">
                        <div id = "divListTableNeedUpdate" class="row-fluid">
                            <table id="tbListNeedUpdate" 
                                    data-toolbar="#divCustomToolbar" 
                                    data-classes="table table-bordered table-striped table-hover"
                                    data-toggle="table"
                                    data-show-columns="true"
                                    data-show-toggle="false"
                                    data-pagination="true"
                                    data-height="450"
                                    data-cache = "false"
                                    data-search = "true"
                                    data-sortable="true">
                                <thead>
                                <tr>
                                    <th data-width = "15%" data-field="DEPLOYNO" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbPackageCode %></th>
                                    <th data-width = "15%" data-field="DeployTime" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbReleaseTime %></th>
                                    <th data-width = "30%" data-field="DEPLOYDESC" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbReleaseDesc %></th>
                                    <th data-width = "30%" data-field="DEPLOYKEY" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbReleaseKey %></th>
                                    <th data-width = "10%" data-field="operate" data-formatter="operateFormatter" data-align = "center" data-events="operateEvents"><%=strLbOperation %></th>
                                </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="tab-pane active" id="tabPage2">  
                        <div id = "divListTableHadUpdated" class="row-fluid">
                            <table id="tbListHadUpdated" 
                                    data-toolbar="#divCustomToolbar" 
                                    data-classes="table table-bordered table-striped table-hover"
                                    data-toggle="table"
                                    data-show-columns="true"
                                    data-show-toggle="false"
                                    data-pagination="true"
                                    data-height="450"
                                    data-cache = "false"
                                    data-search = "true"
                                    data-sortable="true">
                                <thead>
                                <tr>
                                    <th data-width = "10%" data-field="DEPLOYNO" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbPackageCode %></th>
                                    <th data-width = "20%" data-field="DeployTime" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbReleaseTime %></th>
                                    <th data-width = "30%" data-field="DEPLOYDESC" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbReleaseDesc %></th>
                                    <th data-width = "30%" data-field="DEPLOYKEY" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbReleaseKey %></th>
                                    <th data-width = "5%" data-field="UpdateUserId" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbUpdateUser %></th>
                                    <th data-width = "20%" data-field="UpdateTime" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbUpdateTime %></th>
                                    <th data-width = "10%" data-field="UpdateClientIP" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbUpdateClientIP %></th>
                                    <th data-width = "10%" data-field="ProjectId" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbAimProjectId %></th>
                                    <th data-width = "10%" data-field="AppWebSite" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbAimWebSite %></th>
                                    <th data-width = "10%" data-field="RemoteServer" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbRemoteServer %></th>
                                    <th data-width = "5%" data-field="IsSuccess" data-sortable = "true" data-align = "center" data-order = "asc"><%=strLbIsSuccess %></th>
                                    <th data-width = "10%" data-field="SEQNO" data-sortable = "true" data-align = "center" data-order = "asc" data-visible ="false"><%=strLbUpdateFlag %></th>
                                </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <textarea rows="3" cols="20" id="txtCurDEPLOYNO" name="txtCurDEPLOYNO" style="display:none"> </textarea>
        <textarea rows="3" cols="20" id="txtDeployTime" name="txtDeployTime" style="display:none"> </textarea>
        <textarea rows="3" cols="20" id="txtDEPLOYDESC" name="txtDEPLOYDESC" style="display:none"> </textarea>
        <textarea rows="3" cols="20" id="txtDEPLOYKEY" name="txtDEPLOYKEY" style="display:none"> </textarea>

        <textarea rows="3" cols="20" id="txtUpdateFile" name="txtUpdateFile" style="display:none"> </textarea>
    </form>
</div>
</div>
</div>

<!--页面初始化区域-->
<script type="text/javascript">
    var projectId = '<%=this.GetProjectId()%>';
    //var projectId = '待填';
    var remoteServer = '<%=this.GetRemoteServer()%>';
    var canUpdateDeployNo = "";
    var iListRowCount = 0;

    //获取远程服务器中的更新包列表
    function GetAndSetNeedUpdateList() {
        ShowLoadingLevel1();
        iListRowCount = 0;
        canUpdateDeployNo = "";
        var strParam = "jsonCallback=&projectid=" + projectId+"&updatewebsite=<%=this.GetSiteWebAddress() %>";
        var urlQuery = escape("M=" + Math.random() + "&param=getlist&" + strParam);
        var url = remoteServer + "/SysUpdate/Server/UpdateServerHandler.ashx?" + urlQuery;

        try{
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
					alert('<%=strErrorGetList%>');
                    HideLoadingLevel1();
                },
                success: function (result) {
                    //alert(result);
                    if (result.ResultData == 'error'){
						alert('<%=strErrorGetList%>');
						HideLoadingLevel1();
						return false;
					} 

                    var row, data = [];
                    iListRowCount = result.ResultData.length;
                    $.each(result.ResultData, function (i, v) {
                        row = {};
                        for (var key in v) {
                            //对值进行编码处理特殊字符，如引号等// 
                            row[key] = unescape(v[key]);
                        }
                        if (i == iListRowCount - 1) {
                            canUpdateDeployNo = unescape(v["DEPLOYNO"]);
                        }
                        data.push(row);
                    });

                    $('#tbListNeedUpdate').bootstrapTable('destroy').bootstrapTable({
                        data: data
                    });
                    $('#tbListNeedUpdate').bootstrapTable('review');
                    HideLoadingLevel1();

                }
            });
        }
        catch (err) {
            HideLoadingLevel1();
            return;
        }
    }

    //定义每列的操作事件
    function operateFormatter(value, row, index) {
        var RowData = JSON.parse(JSON.stringify(row));
        var deployNo = RowData.DEPLOYNO;
        if (deployNo != canUpdateDeployNo) {
            return [
                '<label id ="lb_' + deployNo + '">',
                    'waiting',
                '</label>'
            ].join('');
        } else {////最早的更新包才能被更新
            return [
                '<a class="update ml10" href="javascript:void(0)" title="update" id ="a_' + deployNo + '">',
                    '<i class="icon-remove"></i><%=strBtnUpdate%>',
                '</a>'
            ].join('');
        }
    }
    window.operateEvents = {
        'click .update': function (e, value, row, index) {
            var RowData = JSON.parse(JSON.stringify(row));
            var deployNo = RowData.DEPLOYNO;
            ShowLoadingLevel1();
            $("#txtCurDEPLOYNO").text("");
            $("#txtDeployTime").text("");
            $("#txtDEPLOYDESC").text("");
            $("#txtDEPLOYKEY").text("");
            $("#txtCurDEPLOYNO").text(RowData.CurDEPLOYNO);
            $("#txtDeployTime").text(RowData.DeployTime);
            $("#txtDEPLOYDESC").text(RowData.DEPLOYDESC);
            $("#txtDEPLOYKEY").text(RowData.DEPLOYKEY);

            GetUpdateFile(deployNo);
        }
    };

    //从远程服务器获取更新包文件
    function GetUpdateFile(deployNo) {
        var strParam = "jsonCallback=&projectid=" + projectId + "&deployno=" + deployNo;
        var urlQuery = escape("M=" + Math.random() + "&param=getfile&" + strParam);
        var url = remoteServer+"/SysUpdate/Server/UpdateServerHandler.ashx?" + urlQuery;

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
				alert('<%=strErrorGetFile%>');
                HideLoadingLevel1();
            },
            success: function (result) {
                //alert(result.fileString);
                if (result.fileString == ""){
					alert('<%=strErrorNoFile%>');
					HideLoadingLevel1();
					return false;
				} 
                if (result.fileString == "error"){
					alert('<%=strErrorGetFile%>');
					HideLoadingLevel1();
					return false;
				} 
                $("#txtUpdateFile").text("");
                $("#txtUpdateFile").text(result.fileString);

                //更新当前服务器
                DoUpdate(deployNo);

            }
        });

    }

    //更新当前服务器
    function DoUpdate(deployNo) {
        var strParam = "projectid=" +projectId+ "&deployno=" + deployNo;
        var urlQuery = escape("M=" + Math.random() + "&param=update&" + strParam);
        var url = "SysUpdate.ashx?" + urlQuery;

        //alert(url);
        $.ajax({
            cache: false,
            url: url,
            async: false,
            type: "post",
            data: $("#formList").serialize(), // 你的formid
            error: function (jqXHR, textStatus, errorThrown) {
                //alert(jqXHR);
                //alert(textStatus);
                //alert(errorThrown);
				alert('<%=strErrorUpdateFailed%>');
                HideLoadingLevel1();
            },
            success: function (data) {
                //alert(data);
                if (data == '1') {
                    UpdateSuccess(deployNo);
                }else {
					alert('<%=strErrorUpdateFailed%>');
					HideLoadingLevel1();
					return false;
				} 
                //var dataobj = eval("(" + data + ")");
            }
        });

    }
    
    //更新完成后设置远程服务器标记
    function UpdateSuccess(deployNo) {
        var strParam = "jsonCallback=&projectid=" + projectId + "&deployno=" + deployNo;
        strParam = strParam+"&updatewebsite=<%=this.GetSiteWebAddress() %>"+"&updateuser=<%=this.GetUserCode() %>"+"&updateip=<%=this.GetClientIPAddress() %>";
        var urlQuery = escape("M=" + Math.random() + "&param=success&" + strParam);
        var url = remoteServer+"/SysUpdate/Server/UpdateServerHandler.ashx?" + urlQuery;

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
                alert('<%=strErrorAfterUpdate%>');
                HideLoadingLevel1();
            },
            success: function (result) {
                //alert(result.flag);
                if (result.flag == "-1") {
					alert('<%=strErrorAfterUpdate%>');
					HideLoadingLevel1();
					return false;
				} 
                if (result.flag == '1') {
                    GetAndSetNeedUpdateList();
                    GetAndSetHadUpdatedList();
                    alert("<%=strLbPackageCode %>(" + deployNo + ")<%=strLbUpdateSuccess %>!");
                }

                HideLoadingLevel1();

            }
        });

    }

    //获取本地服务器中的已更新列表
    function GetAndSetHadUpdatedList() {
        var urlQuery = escape("M=" + Math.random() + "&param=getupdatedlist");
        var url = "SysUpdate.ashx?" + urlQuery;

        try {
            $.ajax({
                cache: false,
                url: url,
                async: false,
                type: "post",
                data: $("#formList").serialize(), // 你的formid
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert(jqXHR);
                    //alert(textStatus);
                    //alert(errorThrown);
                    HideLoadingLevel1();
                },
                success: function (result) {
                    //                                alert(data);
                    if (result == "") return false;
                    var dataobj = eval("(" + result + ")");
                    //                alert(dataobj);
                    //alert(dataobj.ResultData.length); //输出ResultData的子对象数量// 
                    var item = dataobj.ResultData[0];
                    var rows = dataobj.ResultData.length;
                    //构建表格####### Start
                    var i, j, row, columns = [], data = [];

                    for (i = 0; i < rows; i++) {
                        row = {};
                        for (var key in dataobj.ResultData[i]) {
                            row[key] = unescape(dataobj.ResultData[i][key]);
                        }
                        data.push(row);

                    }

                    $('#tbListHadUpdated').bootstrapTable('destroy').bootstrapTable({
                        data: data
                    });
                    HideLoadingLevel1();
                    $('#tbListHadUpdated').bootstrapTable('review');

                }
            });
        }
        catch (err) {
            HideLoadingLevel1();
            return;
        }
    }
</script>
    
<script type="text/javascript">
    $(document).ready(function () {
        $("#aPage1").click();
        if (projectId != '') {
            ShowLoadingLevel1();
            GetAndSetNeedUpdateList();
            GetAndSetHadUpdatedList();
            HideLoadingLevel1();
        }

    });
</script>
    
</body>
</html>
