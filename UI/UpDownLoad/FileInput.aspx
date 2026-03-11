
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileInput.aspx.cs" Inherits="UpDownLoad_FileInput" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <title>文件上传下载</title>
    <link href="../common/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../common/bootstrap-fileinput/css/fileinput.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../common/Font-Awesome/css/font-awesome.min.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../common/bootstrap-fileinput/themes/explorer-fa/theme.css" media="all" rel="stylesheet" type="text/css" />
    <link href="../common/plugins/jqueryEject/css/Eject.css" rel="stylesheet">
    
    <style type="text/css">
        .myscroll_wrap {
            max-height:500px;
            overflow:auto;
        }
        /*修改滚动条样式*/
        .myscroll_wrap::-webkit-scrollbar,.pop .pop_content::-webkit-scrollbar{width:7px;height:7px;}
        .myscroll_wrap::-webkit-scrollbar,.pop .pop_content::-webkit-scrollbar-track{border-radius:5px;background:#ddd;}
        .myscroll_wrap::-webkit-scrollbar,.pop .pop_content::-webkit-scrollbar-thumb{border-radius:5px;background:rgba(153,153,153,.5);}
        .myscroll_wrap::-webkit-scrollbar:hover,.pop .pop_content::-webkit-scrollbar-thumb:hover{background:rgba(102,102,102,.6);}
        .myscroll_wrap::-webkit-scrollbar:active,.pop .pop_content::-webkit-scrollbar-thumb:active{background:rgba(102,102,102,.6);}
    </style>

    <script src="../common/JQuery/jquery-1.10.2.js"></script>
    <script src="../common/plugins/jqueryEject/js/index.js" type="text/javascript"></script>
    <script src="../common/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/js/plugins/piexif.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/js/plugins/sortable.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/js/fileinput.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/js/locales/zh.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/js/locales/es.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/themes/fa/theme.js" type="text/javascript"></script>
    <script src="../common/bootstrap-fileinput/themes/explorer-fa/theme.js" type="text/javascript"></script>
    <script src="../common/js/MainUtil.js" type="text/javascript"></script>
</head>

<body>
    <div class="container-fluid" >
        <div class="row-fluid" style="margin-bottom:0px">
            <div class="span12">
                <div class="alert text-left" id="lb_Tips" style="max-height:20px">
                    <ins id ="spanTitle" class="h4">文件上传及下载</ins>&nbsp;&nbsp;
                    <span id ="spanFolderName" class="h4"></span>
                    <button id="btnInitAssetsImageData" type="button" class="btn btn-info btn-xs pull-right" onclick="javascript:InitAssetsImageData();" style="display:none ;margin-left:10px">批量初始化图片数据</button>
                    <button id="btnQueryAssetsImageName" type="button" class="btn btn-info btn-xs pull-right" onclick="javascript:QueryAssetsImageName();" style="display:none; margin-left:10px">查看上传规则</button>
                </div>
            </div>
        </div>
        <div class="row-fluid ">
            <div id="divUpload" class="col-sm-5 myscroll_wrap">
                <form enctype="multipart/form-data">
                    <div class="form-group">
                        <div class="file-loading">
                            <input id="fileTempInput" type="file" multiple class="file" 
                                data-overwrite-initial="false" 
                                data-show-upload="true" 
                                data-show-preview="true"
                                data-auto-replace ="true"
                                >
                        </div>
                    </div>
                </form> 
            </div>
            <div id="divDownload" class="col-sm-7">
                <div id="divDownTitle" class="row col-sm-12" style="display:none">
                    <div class=" col-sm-6"> 
                        <div class="input-group"> 
                            <input type="text" class="form-control" id="txt_Search" placeholder="search">
                            <div class="input-group-addon"><a id="aSearchFileList" href="#"><span class="glyphicon glyphicon-search"></span></a></div>
                        </div>
                    </div>
                    <button id="btnDeleteAllFile" type="button" class="btn btn-danger btn-sm pull-left">清空所有文件</button>
                    <span id="spanFileCount" class ="badge pull-right">0</span>
                    <ins id ="spanDownloadTitle" class="h5 pull-right" style="margin-left:20px" >已上传文件列表：</ins>
                </div>
                <div id="divNoFileList" class="jumbotron" style="display:">
                  <h2 id="spanNoFileList" class="text-center">暂无上传文件</h2>
                </div>
                <div class="col-sm-12 myscroll_wrap">
                    <ul id ="ulFileList" class="list-group" style="margin-top:5px">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <!-- 模态框（Modal） -->
    <div class="modal fade" id="myWaittingModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myWaittingModalTitle">
                        正在处理中,请稍候.....
                    </h4>
                </div>
                <div id="myWaittingModalContent" class="modal-body">

                </div>
            </div>
        </div>
    </div>
    <%--专为下载区域--%>
    <form id="from1post" name="from1post" method="post" runat="server">
        <iframe id="iddownframe"  name="iddownframe"  style="width:0px;height:0px;display:none;"></iframe> 
    </form>  
    <script type="text/javascript">
        function downthisfile(path, filename) {
            var sPath = 'DownLoadFile.aspx?path=' + path + '&fileName=' + filename + '&optype=down&ran=' + Math.random();
            sPath = sPath + "&folder=<%=this.strFileFolder%>&token=<%=this.strToken%>";
            from1post.action = sPath;
            from1post.target = "iddownframe";
            from1post.submit();
        }
    </script>
    <%--专为下载区域--%>
    <script type="text/javascript">
        var screenHeight = screen.availHeight;
        var postFolder = GetUrlQueryString("folder");
        var postEdit = GetUrlQueryString("edit");
        if (postFolder == null) { postFolder = "";}
        var varFilePath = "";
        var varAllowType = ['txt', 'doc', 'docx', 'pdf', 'xls', 'xlsx', 'jpg', 'png', 'xml', 'rpt', 'bak'];//默认可上传文件类型，上传下载配置中可再配置
        var varCurLanguage = "zh-cn";
        var pageBasicJsonData = {};
        var fileListData = {};
        //弹出框的实例化
        var Ealt = new Eject();

        function ShowWaitting() {
            $('#myWaittingModal').modal();
        }
        function HideWaitting() {
            $('#myWaittingModal').modal('hide');
        }

        $(document).ready(function () {
            $(".myscroll_wrap").css("max-height", (screenHeight - 280).toString() + 'px');

            if (postFolder == 'AIMAGE' || postFolder == 'OEIMAGE') {
                $("#btnQueryAssetsImageName").show();
                $("#btnInitAssetsImageData").show();
            }

            //$("#divDownTitle").hide();
            //$("#divNoFileList").hide();
            //判断Edit参数
            if (postEdit == '1') {
                $("#divUpload").removeClass("col-sm-5").addClass("col-sm-5").show();
                $("#divDownload").removeClass("col-sm-7").removeClass("col-sm-12").addClass("col-sm-7");

                $("#btnDeleteAllFile").show();
            } else {
                $("#divUpload").removeClass("col-sm-5").hide();
                $("#divDownload").removeClass("col-sm-7").removeClass("col-sm-12").addClass("col-sm-12");

                $("#btnDeleteAllFile").hide();
            }
            $("#txt_Search").keyup(function () {
                FilterFileList();
            })
            $("#aSearchFileList").click(function () {
                FilterFileList();
            })
            $("#btnDeleteAllFile").click(function () {
                Ealt.Econfirm({
                    title: pageBasicJsonData.LanguageTips.SystemTips,
                    message: pageBasicJsonData.LanguageTips.DeleteAllFileTips + "(" + $("#spanFileCount").html() + ") ?",
                    confirmText: pageBasicJsonData.LanguageTips.ConfirmTips,
                    cancelText: pageBasicJsonData.LanguageTips.CancelTips,
                    define: function () {
                        $.each(fileListData, function (index, item) {
                            var varFileName = unescape(item.FileName);
                            OperateOneFile(postFolder,varFilePath, varFileName, 'delete');
                        });
                        Ealt.Etoast(pageBasicJsonData.LanguageTips.DeleteAllFileTips, 2)   //默认三秒
                        //加载文件列表区域
                        GetDownloadFileList(postFolder, varFilePath);
                    }
                })
            })
        })

        function DeleteAllFiles() {
        }
    </script>
    
    <script type="text/javascript">
        //初始化fileinput控件（第一次初始化）
        function initFileInput(ctrlName, uploadUrl, lang) {
            //console.log('varAllowType:', varAllowType);
            var control = $('#' + ctrlName);
            //alert(control);
            control.fileinput({
                theme: 'explorer-fa',
                language: lang,
                uploadUrl: uploadUrl, // you must set a valid URL here else you will get an error
                //allowedFileTypes:['image', 'html', 'text', 'video', 'audio', 'flash', 'object'],
                allowedFileExtensions: varAllowType,
                overwriteInitial: false,
                initialPreviewAsData: true,
                maxFileSize: 100000,//kb
                maxFilesNum: 10,
                showUpload: true,
                slugCallback: function (filename) {
                    //return filename.replace('(', '_').replace(']', '_');
                    return filename;
                }
            }).on("filebatchselected", function (event, data, previewId, index) {
                //console.log('------filebatchselected---')
                //console.log(data)
                for (var i in data) {
                    var file = data[i]
                    if (file.name && (file.name.indexOf('+') > -1 || file.name.indexOf('#') > -1 || file.name.indexOf('&') > -1)) {
                        Ealt.Etoast('文件《' + file.name + '》包含了特殊字符+#&，上传成功后将该文件无法下载及使用，建议修改文件名后重试！', 4)   //默认三秒
                    }
                }

            }).on("fileuploaded", function (event, data) {
                //console.log('File Uploaded.data', data);
                var result = data.response;
                if (result.ReturnCode == '1') {
                    //上传成功
                    Ealt.Etoast(result.ReturnMsg, 3)   //默认三秒
                } else {
                    //上传失败
                    //清除历史上传失败文件
                    $(event.target).fileinput('clear').fileinput('unlock')
                    $(event.target).parent().siblings('.fileinput-remove').hide();
                    //清除历史上传失败文件
                    Ealt.Etoast(result.ReturnMsg, 3)   //默认三秒
                }
            }).on('fileuploaderror', function (event, data, msg) {
                //console.log('File Upload Error', 'ID: ' + data.fileId + ', Thumb ID: ' + data.previewId);
            }).on('filebatchuploadcomplete', function (event, preview, config, tags, extraData) {
                //console.log('File Uploaded.extraData', extraData);
                //Ealt.Etoast(pageBasicJsonData.LanguageTips.UoloadSuccessTips, 2)   //默认三秒
                //加载文件列表区域
                GetDownloadFileList(postFolder, varFilePath);

                //console.log('File Batch Uploaded', preview, config, tags, extraData);
            });
        }
        //获取文件列表
        function GetDownloadFileList(folder, filePath) {
            var urlQuery = "M=" + Math.random() + "&param=getfilelist&folder=" + folder + "&filepath=" + filePath;
            //alert(urlQuery);
            urlQuery = escape(urlQuery);
            var url = "FileInput.aspx?" + urlQuery;
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
                    //console.log('GetDownloadFileList().dataJson', dataJson);
                    $("#ulFileList").empty();
                    if (dataJson.ReturnCode == '1') {
                        $("#spanFileCount").html(dataJson.ReturnCount);
                        if (dataJson.ReturnCount > 0) {
                            $("#divDownTitle").show();
                            $("#divNoFileList").hide();
                        } else {
                            $("#divDownTitle").hide();
                            $("#divNoFileList").show();
                        }
                        fileListData = dataJson.FileListData;
                        //加载文件列表区域
                        BuildFileList(fileListData, filePath)

                        $(".lifile").click(function () {
                            $(".lifile").removeClass("label-danger");
                            $(this).addClass("label-danger");
                        })
                        $(".downloadfile").click(function () {
                            var varFileName = $(this).attr("filename");

                            Ealt.Econfirm({
                                title: pageBasicJsonData.LanguageTips.SystemTips,
                                message: pageBasicJsonData.LanguageTips.DownloadTips + "\"" + varFileName + "\" ?",
                                confirmText: pageBasicJsonData.LanguageTips.ConfirmTips,
                                cancelText: pageBasicJsonData.LanguageTips.CancelTips,
                                define: function () {
                                    //OperateOneFile(folder,filePath, varFileName, 'down');
                                    //还是沿用老版本的下载模式
                                    downthisfile(filePath, varFileName);
                                }
                            })

                        })
                        $(".deletefile").click(function () {
                            var varFileName = $(this).attr("filename");
                            Ealt.Econfirm({
                                title: pageBasicJsonData.LanguageTips.SystemTips,
                                message: pageBasicJsonData.LanguageTips.SureDeleteFile + "\"" + varFileName+"\" ?",
                                confirmText: pageBasicJsonData.LanguageTips.ConfirmTips,
                                cancelText: pageBasicJsonData.LanguageTips.CancelTips,
                                define: function () {
                                    OperateOneFile(folder,filePath, varFileName, 'delete');
                                }
                            })

                        })
                    }

                }
            });
        }
        //加载文件列表区域
        function BuildFileList(fileListData, filePath) {
            //利用js中的sort方法
            fileListData.sort(function (a, b) {
                //根据LastWriteTime排序
                return b.LastWriteTime - a.LastWriteTime
            });
            var tempHtml = "";
            //alert(unescape(filePath));
            $.each(fileListData, function (index, item) {
                var varFileName = unescape(item.FileName);
                var varFilePathAndName = unescape(filePath) + "\\" + varFileName;
                var varFileLength = item.FileLength;
                var varCreateTime = item.CreateTime;
                var varLastWriteTime = item.LastWriteTime;

                var varClass = "aLanguageList";
                tempHtml = tempHtml + "<li class=\"list-group-item lifile\"> \r\n";
                tempHtml = tempHtml + "<span class=\"text-info small\" style=\"margin-right:5px\">" + (index + 1).toString() + "、</span>\r\n";
                tempHtml = tempHtml + "<a href=\"#\" filename=\"" + varFileName + "\" class=\"text-primary small downloadfile\" title=\"Delete\">" + varFileName + "</a>\r\n";
                tempHtml = tempHtml + "<a href=\"#\" filename=\"" + varFileName + "\" class=\"glyphicon glyphicon-cloud-download pull-right text-primary large downloadfile\" aria-hidden=\"true\" style=\"margin-left:10px\"></a>\r\n";
                if (postEdit == '1') {
                    tempHtml = tempHtml + "<a href=\"#\" filename=\"" + varFileName + "\" class=\"glyphicon glyphicon-remove pull-right text-danger large deletefile\" aria-hidden=\"true\"></a>\r\n";
                }
                tempHtml = tempHtml + "<span class=\" pull-right text-info small\" style=\"margin-right:20px\">" + varLastWriteTime + "</span>\r\n";
                tempHtml = tempHtml + "<span class=\" pull-right text-info small\" style=\"margin-right:20px\">" + varFileLength + " KB</span>\r\n";
                tempHtml = tempHtml + "</li>\r\n";

            });
            $("#ulFileList").html(tempHtml);
        }
        //过滤文件列表
        function FilterFileList() {
            var searchText = $("#txt_Search").val();
            if (searchText == '') {
                //重新刷新文件夹
                GetDownloadFileList(postFolder, varFilePath);
            } else {
                var filterarray = $.grep(fileListData, function (item, idx) {
                    console.log('aSearchFileList.filterarray.item', item);
                    return unescape(item.FileName).indexOf(searchText) > -1
                });
                //加载文件列表区域
                BuildFileList(filterarray, varFilePath);

            }
        }

        //操作某一个文件
        function OperateOneFile(folder,filePath, fileName,opType) {
            var urlQuery = "M=" + Math.random() + "&param=operateonefile&folder=" + folder + "&filepath=" + filePath + "&filename=" + fileName + "&optype=" + opType;
            //alert(urlQuery);
            urlQuery = escape(urlQuery);
            var url = "FileInput.aspx?" + urlQuery;
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
                    //console.log('OperateOneFile().dataJson', dataJson);
                    $("#ulFileList").empty();
                    if (dataJson.ReturnCode == '1') {
                        if (opType == 'delete') {
                            //加载文件列表区域
                            GetDownloadFileList(postFolder, varFilePath);
                        }
                    }

                }
            });
        }
    </script>
    <%--//加载界面中的基本数据--%>
    <script type="text/javascript">
        //加载界面中的基本数据
        function GetPageBasicData(folder) {
            var urlQuery = "M=" + Math.random() + "&param=getpagebasicdata&folder=" + folder;
            urlQuery = escape(urlQuery);
            var url = "FileInput.aspx?" + urlQuery;
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
                    //var pageBasicJsonData = eval("(" + data + ")");
                    pageBasicJsonData = JSON.parse(data);
                    //console.log('FileInput.GetPageBasicData().pageBasicJsonData', pageBasicJsonData);
                    if (pageBasicJsonData.ReturnCode != '1') {
                        //alert(pageBasicJsonData.ReturnMsg);
                        //Ealt.Etoast(pageBasicJsonData.ReturnMsg, 3)   //默认三秒
                        Ealt.Ealert({
                            title: pageBasicJsonData.LanguageTips.SystemTips,
                            message: pageBasicJsonData.ReturnMsg,
                            confirmText: pageBasicJsonData.LanguageTips.ConfirmTips
                        })
                    } else {
                        varFilePath = pageBasicJsonData.FilePath;
                        varCurLanguage = pageBasicJsonData.Language;
                        var allowType = unescape(pageBasicJsonData.AllowType);
                        allowType = JSON.parse(JSON.stringify(allowType))
                        varAllowType = eval("(" + allowType + ")");

                        //加载文件列表区域
                        GetDownloadFileList(folder, varFilePath);

                        ////设置元素的中英文显示
                        $("title").html(pageBasicJsonData.LanguageTips.TitleTips);
                        $("#spanTitle").html(pageBasicJsonData.LanguageTips.TitleTips);                        
                        $("#spanNoFileList").html(pageBasicJsonData.LanguageTips.NoFileListTips);
                        $("#spanDownloadTitle").html(pageBasicJsonData.LanguageTips.FileListHadUploadedTips);
                        $("#spanFolderName").html("【" + pageBasicJsonData.FolderName + "】");
                        $("#btnDeleteAllFile").text(pageBasicJsonData.LanguageTips.DeleteAllFileTips);
                        $("#btnInitAssetsImageData").text(pageBasicJsonData.LanguageTips.InitImageDataTips);
                        $("#btnQueryAssetsImageName").text(pageBasicJsonData.LanguageTips.ViewUploadRuleTips);

                        //初始化fileinput控件
                        var varUploadUrl = "FileInput.aspx?param=savefile&folder=" + folder + "&filepath=" + varFilePath;
                        initFileInput("fileTempInput", varUploadUrl, varCurLanguage.substring(0, 2));

                    }
                }
            });
        }
        //执行加载界面中的基本数据
        GetPageBasicData(postFolder);
    </script>
    
    <%--//特殊自定义事件--%>
    <script type="text/javascript">    
        //查询资产图片规则页面
        function QueryAssetsImageName() {
            var objType = (postFolder == 'AIMAGE' ? 'AM' : 'OE');
            var url = "../Query/SPQuery.aspx?SP=USP_AM_QRY_UploadImageFileRules&P0=" + objType;
            window.open(url, 'newwindow', 'left=0,top=100,width=' + (screen.availWidth - 10) + ',height=600,scrollbars,resizable=yes,toolbar=no');
        }
        function InitAssetsImageData() {
            ShowWaitting()
            setTimeout(function () {
                var urlQuery = "M=" + Math.random() + "&param=initassetsimagedata&folder=" + postFolder;
                urlQuery = escape(urlQuery);
                var url = "FileInput.aspx?" + urlQuery;
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
                        //var pageBasicJsonData = eval("(" + data + ")");
                        var jsonData = JSON.parse(data);
                        //console.log('FileInput.GetPageBasicData().pageBasicJsonData', pageBasicJsonData);
                        if (jsonData.ReturnCode == '1') {
                        }
                        Ealt.Etoast(jsonData.ReturnMsg, 2)   //默认三秒
                        HideWaitting();
                    }
                });
            },100)
        }
    </script>
</body>
</html>