
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EdgeIndex.aspx.cs" Inherits="EdgeIndex" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Index</title>
    <link href="common/bootstrap/bootstrap.css" rel="stylesheet">
    <link href="common/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="common/plugins/jqueryEject/css/Eject.css" rel="stylesheet">
    <link href="common/bootstrapSelect/bootstrap-select.css" rel="stylesheet" />
    <style type="text/css">
    </style>
</head>
<body>
<form runat="server" id = "form1">
    <div class="container " style="width:100%;margin-bottom:50px;">
        <div class="row">
            <div class="span12">
                <nav id="navTop" class="navbar navbar-default navbar-fixed-top navbar-inverse">
                    <div class="container-fluid">
                        <!-- Brand and toggle get grouped for better mobile display -->
                        <div class="navbar-header">
                            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                                <span class="sr-only">Toggle navigation</span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                            </button>
                            <%--<img alt="Brand" src="common/images/topFrame/bg_log.jpg">--%>
                            <a id="aBrand" class="navbar-brand" href="#">VP HRMS</a>
                        </div>

                        <!-- Collect the nav links, forms, and other content for toggling -->
                        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                            <ul id="ulFirstLevelMenu" class="nav navbar-nav">
                            </ul>
                            <ul id="ulMoreMenu" class="nav navbar-nav">
                                <li class="dropdown">
                                    <a href="#" class="dropdown-toggle small" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">
                                        <span id="spanMore">更多</span>
                                        <span class="caret"></span>
                                    </a>
                                    <ul class="dropdown-menu" id="ulMoreMenuList">
                                    </ul>
                                </li>
                            </ul>
                            <ul class="nav navbar-nav navbar-right" style="margin-right:5px">
                                <li id="liToHome" title="Home"><a href="#" id="aToHome"><span class="glyphicon glyphicon-home" aria-hidden="true"></span></a></li>
                                <li id="liHelpList" class="dropdown" title="Help List">
                                    <a href="#" class="dropdown-toggle small" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">
                                        <span class="glyphicon glyphicon-question-sign" aria-hidden="true"></span>
                                    </a>
                                    <ul id="ulIndexTools" class="dropdown-menu">
                                    </ul>
                                </li>
                                <li class="dropdown">
                                    <a href="#" class="dropdown-toggle small" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">
                                        <%--<span class="glyphicon glyphicon-text-background" aria-hidden="true"></span>--%>
                                        <span id="spanLanguageDesc">English</span> 
                                        <span class="caret"></span>
                                    </a>
                                    <ul class="dropdown-menu" id ="ulLanguageList">
                                        
                                    </ul>
                                </li>
                                <li class="dropdown">
                                    <a href="#" class="dropdown-toggle small" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">
                                        <span class="glyphicon glyphicon-user" aria-hidden="true"></span>
                                        <span id="spanUserName"></span> 
                                        <span class="caret"></span>
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a id="aToInnerMsg" href="#"><span class="glyphicon glyphicon-lock" aria-hidden="true"></span>&nbsp;&nbsp;消息通知</a></li>
                                        <li role="separator" class="divider"></li>
                                        <li><a id="aChangePassword" href="#"><span class="glyphicon glyphicon-lock" aria-hidden="true"></span>&nbsp;&nbsp;修改密码</a></li>
                                        <li role="separator" class="divider"></li>
                                        <li><a href="#" id="aLogout"><span class="glyphicon glyphicon-log-out" aria-hidden="true"></span>&nbsp;&nbsp;退出</a></li>
                                    </ul>
                                </li>
                            </ul>
                        </div><!-- /.navbar-collapse -->
                    </div><!-- /.container-fluid -->
                </nav>
            </div>
        </div>
    </div>
    
    <nav id="navActionPath" class="navbar" style="max-height:16px;min-height:16px;display:none"></nav>

    <nav class="navbar navbar-default">
        <div class="container-fluid" style="margin-bottom:30px">
            <div class="row-fluid" id="div_MainPage">
                <div class="span12">
                    <iframe id="frame_MainPage" name="mainFrame"  width="100%" src="" style="border: 0px solid #cecece;" scrolling="auto"></iframe>
                </div>
            </div>
        </div>
    </nav>

    <nav class="navbar navbar-default navbar-fixed-bottom" style="min-height:30px">
        <div class="container">  
        </div>
        <div class="container-fluid">
            <div class="row">   
                <div id="divPendingAlert" class="alert alert-danger alert-dismissible fade in" role="alert" style="display:none">
                    <strong id="spanHelloUser">Hello</strong> 
                    <span id="spanYouHavePending" >您有待办事项未处理</span>
                    <a id="aOpenAlertPage" href="Archive/Pending/PendingList.aspx" target="_blank">
                        <span id="spanClickToIn" >请点击进入</span>
                    </a>
                    <div class="pull-right">
                        <button id="btnRejectShow" type="button" class="btn btn-primary btn-xs" >不再显示</button>
                        <button id="btnCloseThisTime" type="button" class="btn btn-default btn-xs" >关闭</button>
                    </div> 
                </div> 
            </div>     
            <div class="row-fluid" style="min-height:30px;padding-top:5px">
                <div class="span12">
                    <div class="pull-left">
                        <a id="aProductVersion" href="#" class="h5 text-left label label-default">
                            <span id="spanProductVersion" class=""></span>
                        </a>
                            <span id="spanSysUpdateCount" class="badge" style="color:white;background-color:red"></span>
                    </div>
                    <div class="pull-right">
                        <span id="spanLicAuthorName_Label" class="h6" ></span>
                        <span id="spanLicAuthorName_Value" class="h6 text-info" ></span> ---
                        <span id="spanLicAuthorDate_Label" class="h6" ></span>
                        <span id="spanLicAuthorDate_Value" class="h6 text-info" ></span>
                    </div>
                </div>
            </div>
        </div>
    </nav>

</form>
<script src="common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="common/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
<script src="common/plugins/jqueryEject/js/index.js" type="text/javascript"></script>
<script src="common/js/MainUtil.js" type="text/javascript"></script>
<script src="common/js/stringUtil.js" type="text/javascript"></script>
<script src="common/js/waitProcess.js"></script>

<script type="text/javascript">
    var projectId = "";
    var remoteServer = "";
    var homePageUrl = "";
    var jsonLanguage = [];
    var curLanguage = "zh-cn";
    var screenHeight = screen.availHeight;
    var screenWidth = screen.availWidth;
    //页面基本信息的所有合集json
    var jsonPageData = {};
    //是否开启实时提醒功能
    var IsRealtimeAlert = "";
    //实时提醒间隔时间
    var RealtimeAlertInterval = "";
    //定时执行当前用户的提醒信息
    var handleAlert;
    var handleAlertTimer;
    //弹出框的实例化
    var Ealt_Main = new Eject();

    function RedirectMainPage(url) {
        $("#frame_MainPage").attr("src", url);
        $("#frame_MainPage").height(screenHeight - 220);
    }
    $(document).ready(function () {
        //初始化页面获取数据
        GetPageBasicData();
        $("#myPendingAlertModal").modal();
        $("#navActionPath").hide();

        $("#aBrand").click(function () {
            $('#aToHome').click()
        })
        $("#aToHome").click(function () {
            $("#navActionPath").hide();
            RedirectMainPage(homePageUrl);
        })
        $("#aBrand").click(function () {
            $("#navActionPath").hide();
            RedirectMainPage(homePageUrl);
        })
        $("#aProductVersion").click(function () {
            ShowUpdateList();
        })
        $("#btnRejectShow").click(function () {
            RejectShowPendingAlert();
        })
        $("#btnCloseThisTime").click(function () {
            HidePendingAlert();
        })
        $("#aChangePassword").click(function () {
            $("#navActionPath").hide();
            RedirectMainPage('UserManager/ChangePwd.aspx');
        })
        $("#aToInnerMsg").click(function () {
            $("#navActionPath").hide();
            RedirectMainPage('Archive/Archive.aspx?DOCU=INNERMSG&ROLE=MySelf');
        })        
        $("#aLogout").bind('click', function () {
            //弹出框的实例化
            var Ealt_logout = new Eject();
            Ealt_logout.Econfirm({
                title: jsonPageData.LanguageTips.SystemTips,
                message: jsonPageData.LanguageTips.ExitTips,
                confirmText: jsonPageData.LanguageTips.ConfirmTips,
                cancelText: jsonPageData.LanguageTips.CancelTips,
                define: function () {
                    //退出系统
                    ExitApplication();
                },
                cancel: function () {
                    Ealt_logout = null;
                    //alert('您点击了取消')
                }
            })
        })
    });
</script>

<script type="text/javascript">
    function GetPageBasicData() {
        $("#navActionPath").hide();
        var urlQuery = "M=" + Math.random() + "&param=getpagebasicdata";
        //alert(urlQuery);
        urlQuery = escape(urlQuery);
        var url = "EdgeIndex.aspx?" + urlQuery;
        $.ajax({
            cache: false,
            url: url,
            async: false,
            error: function (request) {
                HideWaitting();
            },
            success: function (data) {
                //alert(data);
                //$("#txt_ResultText").val(data);
                if (data == "") return false;
                //var dataJson = eval("(" + data + ")");
                var dataJson = JSON.parse(data);
                jsonPageData = dataJson;
                //console.log('GetPageBasicData().dataJson', dataJson);

                //获取ProjectId和RemoteServer
                projectId = dataJson.ProjectId;
                remoteServer = dataJson.RemoteServer;

                $("#aBrand").html(dataJson.ProductShortName);

                //加载当前用户
                $("#spanUserName").html(dataJson.UserName);
                $("#spanHelloUser").html('Hello '+dataJson.UserName);

                //加载语言选择区域
                jsonLanguage = dataJson.LanguageJson;
                BuildLanguageArea();

                //加载主页
                homePageUrl = dataJson.HomePageUrl;
                RedirectMainPage(homePageUrl);
                
                //设置元素的中英文显示
                $("#spanMore").html(dataJson.LanguageTips.MoreMenu);
                $("#liToHome").attr("title", dataJson.LanguageTips.ToHomeTips);
                $("#liHelpList").attr("title", dataJson.LanguageTips.ToolAndDoc);
                $("#spanYouHavePending").html(dataJson.LanguageTips.YouHavePendingTips);
                $("#spanClickToIn").html(dataJson.LanguageTips.ClickToInTips);
                $("#btnRejectShow").html(dataJson.LanguageTips.RejectShowTips);
                $("#btnCloseThisTime").html(dataJson.LanguageTips.CloseThisTimeTips);
                $("#aChangePassword").html(dataJson.LanguageTips.ChangePasswordTips);
                $("#aToInnerMsg").html(dataJson.LanguageTips.InnerMsgTips);
                $("#spanLicAuthorName_Label").html(dataJson.LanguageTips.LicAuthorNameTips);
                $("#spanLicAuthorDate_Label").html(dataJson.LanguageTips.LicAuthorDateTips);
                $("#aLogout").html(dataJson.LanguageTips.LogoutBtnTips);

                //加载首页文档及工具区域
                BuildIndexToolsList(dataJson.IndexToolsData);

                //加载第一级菜单区域
                var showMenuCount = dataJson.ShowMenuCount;
                BuildFirstLevelMenu(dataJson.UserFirstLevelMenu, showMenuCount);

                //获取系统授权数据
                var jsonLicData = dataJson.LicenseData;
                $("#spanProductVersion").html(jsonLicData.ProductName + " " + jsonLicData.Version);                
                $("#spanLicAuthorName_Value").html(jsonLicData.ClientName);
                $("#spanLicAuthorDate_Value").html(jsonLicData.ValidDate);
                if (parseInt(jsonLicData.ValidDateDiffDays) <= parseInt(jsonLicData.DiffDaysNoticeDays)) {
                    Ealt_Main.Ealert({
                        title: dataJson.LanguageTips.SystemTips,
                        message: jsonLicData.DiffDaysNoticeMsg + jsonLicData.ValidDateDiffDays.toString(),
                        confirmText: dataJson.LanguageTips.ConfirmTips
                    })
                    $("#spanLicAuthorName_Value").removeClass("text-info").addClass("text-danger");
                    $("#spanLicAuthorDate_Value").removeClass("text-info").addClass("text-danger");
                }

                //加载实时提醒框
                IsRealtimeAlert = dataJson.IsRealtimeAlert;
                RealtimeAlertInterval = dataJson.RealtimeAlertInterval;
                if (IsRealtimeAlert == "1") {
                    handleAlertTimer = setInterval(function () {
                        getUserPengdingCount();
                    }, RealtimeAlertInterval);
                }
                //获取可更新的远程推送
                GetUpdateCount(dataJson);
            }
        });
    }
    //加载语言选择区域
    function BuildLanguageArea() {
        var tempHtml = "";
        $.each(jsonLanguage,function (index, item) {
            var varValue = item.value;
            var varName = item.name;
            var varActive = item.active;
            var varClass = "aLanguageList";
            if (varActive == '1') {
                curLanguage = varValue;
                $("#spanLanguageDesc").html(varName);
                varClass = varClass + " label-primary"
            }
            tempHtml = tempHtml + "<li><a href=\"#\" class=\"" + varClass+"\" lvalue=\"" + varValue+"\">" + varName+"</a></li>";
        });
        $("#ulLanguageList").html(tempHtml);

        $(".aLanguageList").click(function () {
            curLanguage = $(this).attr("lvalue");
            ChangeLanguage();
        })
    }

    //加载首页文档及工具区域
    function BuildIndexToolsList(jsonIndexToolsData) {
        var tempHtml = "";
        var tempHtml_More = "";
        //console.log(jsonIndexToolsData);

        $.each(jsonIndexToolsData, function (index, item) {
            var varValue = item.DCODE;
            var varName = (curLanguage == 'zh-cn') ? item.DNAMECHS : item.DNAME;
            var varHrefUrl = item.HrefUrl;
            var varIsNewWindow = item.IsNewWindow;
            var varIsDivider = item.IsDivider;

            var varClass = "aIndexTools";
            var varTarget = varIsNewWindow == '1' ? '_blank' : '';


            //传递固定参数[为跳转到新CS-VUE平台新增功能]
            varHrefUrl = varHrefUrl.ReplaceAll('%ProjectId%', '<%=this.GetProjectId()%>');
            varHrefUrl = varHrefUrl.ReplaceAll('%USERCODE%', '<%=this.GetUserCode()%>');
            varHrefUrl = varHrefUrl.ReplaceAll('%USERPASSWORD%', '<%=this.GetAccountPassword()%>');
            switch (varValue) {
                case "Feedback"://新CS-VUE平台的问题反馈功能入口
                    var urlArray = varHrefUrl.split('post=')
                    var tempHrefUrl = varHrefUrl
                    //console.log('urlArray:', urlArray);
                    if (urlArray.length == 2) {
                        tempHrefUrl = urlArray[0] + "post=" + encodeURIComponent(urlArray[1])
                    }
                    varHrefUrl = tempHrefUrl
                    //console.log('varHrefUrl:', varHrefUrl);
                    break;
            }

            //alert(varHrefUrl);
            if (varIsDivider == '1') {
                tempHtml = tempHtml + "<li role=\"separator\" class=\"divider\"></li>";
            } else if (varHrefUrl == '#') {
                tempHtml = tempHtml + "<li><a href=\"javascript:void(0);\" class=\"" + varClass + "\" >" + varName + "</a></li>";
            } else {
                tempHtml = tempHtml + "<li><a href=\"" + varHrefUrl + "\" class=\"" + varClass + "\" target=\"" + varTarget + "\">" + varName + "</a></li>";
            }

        });
        if (tempHtml != '') {
            $("#ulIndexTools").empty();
            $("#ulIndexTools").html(tempHtml);
        }
    }

    //加载第一级菜单区域
    function BuildFirstLevelMenu(jsonFirstLevelMenu, showMenuCount) {
        var tempHtml = "";
        var tempHtml_More = "";
        $.each(jsonFirstLevelMenu, function (index, item) {
            var varValue = item.SMENUCODE;
            var varName = (curLanguage == 'zh-cn') ? item.SMENUNAMECN : item.SMENUNAME;
            //var varActive = item.active;
            var varClass = "aFirstLevelMenuList small";
            //if (index == '0') {
            //    varClass = varClass + " active label-danger"
            //}
            if (index < showMenuCount) {
                tempHtml = tempHtml + "<li><a href=\"#\" class=\"" + varClass + "\" menucode=\"" + varValue + "\">" + varName + "</a></li>";
            } else {
                tempHtml_More = tempHtml_More + "<li><a href=\"#\" class=\"" + varClass + "\" menucode=\"" + varValue + "\">" + varName + "</a></li>";
            }
        });
        if (tempHtml!='') {
            $("#ulFirstLevelMenu").empty();
            $("#ulFirstLevelMenu").html(tempHtml);
        }
        if (tempHtml_More != '') {
            //$("#ulMoreMenuList").show();
            $("#ulMoreMenuList").empty();
            $("#ulMoreMenuList").html(tempHtml_More);
        }
        if (showMenuCount >= jsonFirstLevelMenu.length){
            $("#ulMoreMenu").hide();
        }

        $(".aFirstLevelMenuList").click(function () {
            $(".aFirstLevelMenuList").removeClass("label-primary");
            $(this).addClass("label-primary");

            var menuId = $(this).attr("menucode");
            ClickFirstLevelMenu(menuId,"0");
        })
    }

    function ClickFirstLevelMenu(menuId,isLastLevel) {
        //加载面包屑
        BuildActionPath(menuId);
        if (isLastLevel != "1") {
            var mainUrl = "FunctionList.aspx?sCode=" + menuId
            //加载主界面
            RedirectMainPage(mainUrl);
        }
    }
</script>

<script type="text/javascript">
    //切换语言
    function ChangeLanguage() {
        var urlQuery = "M=" + Math.random() + "&param=changelanguage&language=" + curLanguage;
        //alert(urlQuery);
        urlQuery = escape(urlQuery);
        var url = "EdgeIndex.aspx?" + urlQuery;
        $.ajax({
            cache: false,
            url: url,
            async: false,
            error: function (request) {
                HideWaitting();
            },
            success: function (data) {
                //alert(data);
                //$("#txt_ResultText").val(data);
                if (data == "") return false;
                //var dataJson = eval("(" + data + ")");
                var dataJson = JSON.parse(data);
                //console.log('ChangeLanguage().dataJson', dataJson);
                if (dataJson.ReturnCode == '1') {
                    GetPageBasicData();
                }

            }
        });
    }
    //获取栏目完整路径并加载面包屑
    function BuildActionPath(menuId) {
        var urlQuery = "M=" + Math.random() + "&param=getmenupathbymenuid&menuid=" + menuId;
        //alert(urlQuery);
        urlQuery = escape(urlQuery);
        var url = "EdgeIndex.aspx?" + urlQuery;
        $.ajax({
            cache: false,
            url: url,
            async: false,
            error: function (request) {
                HideWaitting();
            },
            success: function (data) {
                //alert(data);
                //$("#txt_ResultText").val(data);
                if (data == "") return false;
                //var dataJson = eval("(" + data + ")");
                var dataJson = JSON.parse(data);
                //console.log('BuildActionPath().dataJson', dataJson);

                $("#navActionPath").empty();
                var tempHtml = "<ol class=\"breadcrumb\" >";
                $.each(dataJson.ReturnData, function (index, item) {
                    var varMenuCode = item.SMENUCODE;
                    var varMenuName = dataJson.ReturnLanguage == 'zh-cn' ? item.SMENUNAMECN : item.SMENUNAME;
                    if (index == 0) {
                        varMenuName = "<span class=\"glyphicon glyphicon-home\" aria-hidden=\"true\" style=\" margin-right:5px\"></span>" +varMenuName;
                    }
                    var varParentCode = item.SPARENTCODE;
                    var varClass = "aActionPathList small";
                    if (varMenuCode == dataJson.ReturnMenuCode) {
                        tempHtml = tempHtml + "<li class=\"small\">" + varMenuName + "</li>";
                    } else {
                        tempHtml = tempHtml + "<li><a href=\"#\" class=\"" + varClass + "\" menuId=\"" + varMenuCode + "\">" + varMenuName + "</a></li>";
                    }
                });
                tempHtml = tempHtml + "</ol>";
                $("#navActionPath").html(tempHtml);
                $("#navActionPath").show();

                
            }
        });

        $(".aActionPathList").click(function () {
            var MenuId = $(this).attr("menuId");
            var mainUrl = "FunctionList.aspx?sCode=" + MenuId
            RedirectMainPage(mainUrl);
        })
    }
    //退出系统
    function ExitApplication() {
        var urlQuery = "M=" + Math.random() + "&param=exitapplication";
        //alert(urlQuery);
        urlQuery = escape(urlQuery);
        var url = "EdgeIndex.aspx?" + urlQuery;
        $.ajax({
            cache: false,
            url: url,
            async: false,
            error: function (request) {
                HideWaitting();
            },
            success: function (data) {
                //alert(data);
                //$("#txt_ResultText").val(data);
                if (data == "") return false;
                //var dataJson = eval("(" + data + ")");
                var dataJson = JSON.parse(data);
                //console.log('ExitApplication().dataJson', dataJson);
                if (dataJson.ReturnCode == '1') {
                    if (window.parent != null) {
                        window.parent.location.href = "login.aspx";
                    } else {
                        window.location.href = "login.aspx";
                    }
                } else {
                    var Ealt_logout = new Eject();
                    Ealt_logout.Etoast(dataJson.ReturnMsg, 3)//默认三秒
                }

            }
        });
    }

</script>

<%----Start-待办事项提醒弹出框--Start--%>
<script type="text/javascript">
    function getUserPengdingCount() {
        var urlQuery = "M=" + Math.random() + "&param=getcuruserpendingcount";
        //alert(urlQuery);
        urlQuery = escape(urlQuery);
        var url = "Archive/Pending/Pending.ashx?" + urlQuery;
        $.ajax({
            cache: false,
            url: url,
            async: false,
            error: function (request) {
                HideWaitting();
            },
            success: function (data) {
                //alert(data);
                //$("#txt_ResultText").val(data);
                if (data == "") return false;
                //var dataJson = eval("(" + data + ")");
                var dataJson = JSON.parse(data);
                //console.log('getUserPengdingCount().dataJson', dataJson);
                if (dataJson.ReturnCode == '1') {
                    //console.log('getUserPengdingCount().dataJson.ReturnData.iPendingCount', dataJson.ReturnData.iPendingCount);
                    if (parseInt(dataJson.ReturnData.iPendingCount) > 0) {
                        ShowPendingAlert();
                    }
                }

            }
        });
    }

    function ShowPendingAlert() {
        $("#divPendingAlert").show();
    }
    function HidePendingAlert() {
        $("#divPendingAlert").hide();
    }
    //如果选择拒绝提醒
    function RejectShowPendingAlert() {
        $("#divPendingAlert").hide();
        clearInterval(handleAlertTimer);
    }
</script>
<%----End--待办事项提醒弹出框--End--%>

<%----Start-获取可更新的远程推送--Start--%>
<script type="text/javascript">
    function GetUpdateCount(dataJson) {
        var strParam = "jsonCallback=&projectid=" + projectId;
        var urlQuery = escape("M=" + Math.random() + "&param=getcount&" + strParam);
        var url = remoteServer + "/SysUpdate/Server/UpdateServerHandler.ashx?" + urlQuery;
        try {
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
                    $("#spanSysUpdateCount").html("");
                    if (parseInt(result.count) > 0) {
                        $("#spanSysUpdateCount").html(result.count);
                        if (dataJson.IsAlertSysUpdate == '1') {
                            //弹出框的实例化
                            var Ealt_sysUpdate = new Eject();
                            Ealt_sysUpdate.Econfirm({
                                title: dataJson.LanguageTips.SystemTips,
                                message: dataJson.LanguageTips.UpdateAlertTips,
                                confirmText: dataJson.LanguageTips.ConfirmTips,
                                cancelText: dataJson.LanguageTips.CancelTips,
                                define: function () {
                                    ShowUpdateList();
                                },
                                cancel: function () {
                                    //alert('您点击了取消')
                                }
                            })
                        }
                    }
                }
            });
        } catch (err) {
            return;
        }
    }

    function ShowUpdateList() {
        var url = "SysUpdate/Client/UpdateList.aspx";
        var obj = new Object();
        obj.name = "SysUpdateList";
        var varWidth = screen.availWidth - 300;
        var varHeight = screen.availHeight - 200;
        var varParam = "dialogWidth=" + varWidth + "px;dialogHeight=" + varHeight + "px";
        window.showModalDialog(url, obj, varParam);
    }
</script>
<%----End--获取可更新的远程推送--End--%>

</body>
</html>
