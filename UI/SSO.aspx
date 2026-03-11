<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SSO.aspx.cs" Inherits="SSO" %>

<!DOCTYPE html>
<!--[if lt IE 7 ]> <html lang="en" class="no-js ie6 lt8"> <![endif]-->
<!--[if IE 7 ]>    <html lang="en" class="no-js ie7 lt8"> <![endif]-->
<!--[if IE 8 ]>    <html lang="en" class="no-js ie8 lt8"> <![endif]-->
<!--[if IE 9 ]>    <html lang="en" class="no-js ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang="zh-CN" class="no-js"> <!--<![endif]-->
<head>
    <meta charset="utf-8" />
    <title>SSO Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
    <meta name="description" content="Login and Registration Form with HTML5 and CSS3" />
    <meta name="keywords" content="html5, css3, form, switch, animation, :target, pseudo-class" />
    <meta name="author" content="Codrops" />
    <link href="common/bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="common/bootstrap-table/bootstrap-table.css" rel="stylesheet" />
    <link href="common/bootstrapSelect/bootstrap-select.css" rel="stylesheet" />
    <link rel="shortcut icon" href="../favicon.ico"> 
    <link rel="stylesheet" type="text/css" href="common/SSOLogin/css/demo.css" />
    <link rel="stylesheet" type="text/css" href="common/SSOLogin/css/style.css" />
	<link rel="stylesheet" type="text/css" href="common/SSOLogin/css/animate-custom.css" />
</head>

<body>
    <script src="common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="common/layer/layer.min.js" type="text/javascript"></script>
    <script src="common/layer/layerLoading.js" type="text/javascript"></script>
    <script src="common/bootstrap/bootstrap.js" type="text/javascript"></script>
    <script src="common/bootstrap/bootstrap-tab.js" type="text/javascript"></script>
    <script src="common/bootstrap-table/bootstrap-table.js" type="text/javascript"></script>
    <script src="common/bootstrapSelect/bootstrap-select.js" type="text/javascript"></script>
    <script src="common/js/waitProcess.js" type="text/javascript"></script>
     
    <div class="container">
        <!-- Codrops top bar -->
        <header>
            <h1>Select One Of <span>Hotel Application</span></h1>
            <div class="row">
                <div class="col-xs-6 col-xs-offset-3">
                <div class="form-group">
                    <select id ="sel_Application" class="selectpicker form-control  show-tick" data-style="btn-info"  data-live-search="true" runat="server">
                    </select>
                </div>
                </div>
            </div>
        </header>
        <section>				
            <div id="container_demo" >
                <div id="wrapper" class="row">
                    <div class="col-xs-12">
                        <div id="login" class="animate form">
                            <form id ="formSSOLogin" runat="server"> 
                                <input id="txt_DBConnect" name="txt_DBConnect"  style="display:none" runat="server"/> 
                                <h1>Log in</h1> 
                                <p> 
                                    <label for="txt_UserName" class="uname" data-icon="u" > Your account name </label>
                                    <input id="txt_UserName" name="txt_UserName" required="required" type="text" placeholder="" runat="server"/>
                                </p>
                                <p> 
                                    <label for="txt_Password" class="youpasswd" data-icon="p"> Your password </label>
                                    <input id="txt_Password" name="txt_Password" required="required" type="password" placeholder=""  runat="server"/> 
                                </p>
                                <p class="login button"> 
							        <asp:button ID="btnLogin" src="common/images/login/login_btn.gif" runat="server" onclick="LoginButton_Click"  Text="Login"/>
                                    <%--<input id ="btnLogin" type="button" value="Login" /> --%>
								</p>
                            </form>
                        </div>
                    </div>
                </div>
            </div>  
        </section>
    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            PageReady();
            setTimeout(function () {
                GetMultiDBConnetct();
            }, 500);

            $('#btnLogin').click(function () {
                //DoSSOLogin();
            });

            $('#sel_Application').change(function () {
                $('#txt_DBConnect').val($('#sel_Application').val());
                PageReady();
            });

            $('body').click(function () {
            });
        });


        function PageReady() {
            if (($('#sel_Application').val()==null) || ($('#sel_Application').val() == '')) {
                $('#txt_UserName').attr('disabled', true);
                $('#txt_Password').attr('disabled', true);
                $('#btnLogin').attr('disabled', true);
            } else {
                $('#txt_UserName').removeAttr('disabled');
                $('#txt_Password').removeAttr('disabled');
                $('#btnLogin').removeAttr('disabled');
            }
        }

        //获取需单点登录的多账套列表
        function GetMultiDBConnetct() {
            var urlQuery = escape("M=" + Math.random() + "&ajaxparam=getdbconnect");
            var url = "SSO.aspx?" + urlQuery;
            $.ajax({
                cache: false,
                type: "POST",
                url: url,
                async: false,
                success: function (data) {
                    //alert(data);
                    if (data == "") return false;
                    var dataobj = eval("(" + data + ")");

                    //表列数量
                    var rowCount_ResultData = dataobj.ResultData.length;
                    //构建表格####### Start
                    var i, j, tableRow_ResultData, tableData = [];

                    $("#sel_Application").empty();
                    $("#sel_Application").append("<option value=''>----------------------Please Select----------------------</option>");
                    //加载下拉列表sel_Month
                    $.each(dataobj.ResultData, function (idx, item) {
                        $("#sel_Application").append("<option value='" + unescape(item.CID) + "'>" + unescape(item.CDESC) + "</option>");
                    });
                    //更新内容刷新到相应的位置(t动态加载)
                    $('#sel_Application').selectpicker('render');
                    $('#sel_Application').selectpicker('refresh');
                }
            });

        }
    </script>
    
</body>
</html>
