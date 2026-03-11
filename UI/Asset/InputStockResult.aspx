<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="InputStockResult.aspx.cs" Inherits="Asset_InputStockResult" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8"/>
    <title>资产盘点录入</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <link href="../common/css/main.css" rel="stylesheet" type="text/css" />
    <link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
    <link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css"/>
    <link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
    <link href="../common/bootstrap/bootstrap.css" rel="stylesheet"/>
    <link href="../common/bootstrap-table/bootstrap-table.css" rel="stylesheet"/>
    <link href="../common/mmGrid/mmGrid.css" rel="stylesheet"/>
    <link href="../common/mmGrid/mmPaginator.css" rel="stylesheet"/>
    <style>
        .mmGrid,
        .mmPaginator{
            font-size: 12px;
        }
    </style>
</head>

<script src="../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script> 
<script src="../common/bootstrap/bootstrap.js" type="text/javascript"></script> 
<script src="../common/bootstrap-table/bootstrap-table.js" type="text/javascript"></script> 
<script src="../common/js/waitProcess.js" type="text/javascript"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
<script src="../common/mmGrid/mmGrid.js" type="text/javascript"></script>
<script src="../common/mmGrid/mmPaginator.js" type="text/javascript"></script>

<body style = "background-color:#F7F8F9">

<div class="container">
<div>
<form id="formList" class="form-horizontal" method="POST">
    <br />
    <%--<div>
        <h3 >盘点结果录入</h3>
    </div>--%>
    <div class="row">
        <div class="col-xs-8">
            <div class="col-xs-12">
                <label class="control-label" id = "lbListCount" style="color:green">列表记录数:0</label>&nbsp;&nbsp;&nbsp;&nbsp;
                <label class="control-label" id = "lbSelectCount" style="color:Red">已选择记录数：0</label>
            </div>
            <div class="col-xs-12">
                <table id="tbList" style="width:90%"></table>
            </div>
            <%--<div style="text-align:right;">
                <div id="paginator"></div>
            </div>--%>
        </div>
        <div class="col-xs-3">
            <br />
            <div class="col-xs-11">
                <label class="control-label" style="width:30%" for="sel_StockPlan">盘点计划</label>
                <select class = "input-block-level" style = "width:65%;" id="sel_StockPlan" name="sel_StockPlan" > 
                    <option value=""></option> 
                </select>
            </div>
            <div class="col-xs-11">
                <label id = "lbTipSelectPlan" class="control-label right_bt" style="width:100%; color:Red">请首先选择一个盘点计划</label>
            </div>
            <br />
            <br />
            <div class="col-xs-11">
                <label class="control-label" style="width:30%" for="sel_Dept">使用部门</label>
                <select class = "input-block-level" style = "width:65%;" id="sel_Dept" name="sel_Dept" > 
                    <option value=""></option> 
                </select>
            </div>
            <br />
            <br />
            <div class="col-xs-11">
                <label class="control-label"style="width:30%" for="sel_Location">存放地址</label>
                <select class = "input-block-level" style = "width:65%;" id="sel_Location" name="sel_Location" > 
                    <option value=""></option> 
                </select> 
            </div>
            <br />
            <br />
            <div class="col-xs-11">
                <label class="control-label" style="width:30%" for="sel_Category">资产分类</label>
                <select class = "input-block-level" style = "width:65%;" id="sel_Category" name="sel_Category" > 
                    <option value=""></option> 
                </select> 
            </div>
            <br />
            <br />
            <div class="col-xs-11">
                <label class="control-label" style="width:30%" for="txt_ACode">资产编码</label>
                <input type="text" style = "width:64%;" id = "txt_ACode" name="txt_ACode">
            </div>
            <br />
            <br />
            <div class="col-xs-11">
                <label class="control-label" style="width:30%" for="txt_AName">资产名称</label>
                <input type="text" style = "width:64%;" id = "txt_AName" name="txt_AName">
            </div>
            <br />
            <br />
            <div class="col-xs-11">
                <label class="control-label" style="width:30%" for="txt_AModel">资产型号</label>
                <input type="text" style = "width:64%;" id = "txt_AModel" name="txt_AModel">
            </div>
            <br />
            <br />
            <div class="col-xs-11 text-left">
                <label class="control-label" style="width:30%" for="cb_Stocked">已盘点资产</label>
                <input type="checkbox" style=" vertical-align:top" id = "cb_Stocked" name="cb_Stocked">
            </div>
            <br />
            <br />
            <div class="col-xs-11 text-center">
                <button id = "btnQuery" type="button" class="a_Left">查询</button>&nbsp;&nbsp;&nbsp;&nbsp;
                <button id = "btnSave" type="button" class="a_Left">保存盘点数据</button>
            </div>
            <br />
            <br />
            <div class="col-xs-11 text-center">
                <label class="control-label" style="color:Red" id="txtNoticeTip"></label>
            </div>
        </div >
    </div>
    
    <input type="text" style = "display:none" id = "txt_SelectSaveSACODE" name="txt_SelectSaveSACODE">
</form>
</div>
</div>

<script type="text/javascript">
    //保留两位小数
    var fixed2 = function (val) {
        return val.toFixed(2);
    }

    //加百分号
    var fixed2percentage = function (val) {
        return fixed2(val) + '%';
    }
    //高亮
    var highliht = function (val) {
        if (val > 0) {
            return '<span style="color: #b00">' + fixed2(val) + '</span>';
        } else if (val < 0) {
            return '<span style="color: #0b0">' + fixed2(val) + '</span>';
        }
        return fixed2(val);
    };

</script>

<!--页面初始化区域-->
<script type="text/javascript">
    var IsBuildList = false;
    var varMMGrdid;
    $(document).ready(function () {
        var columns = [
            { title: '资产编码', name: 'SACODE', sortable: true, align: 'center' },
        //            { title: '标签类型', name: 'LABELTYPENAMECHS', width: 60, align: 'center' },
            {title: '资产名称', name: 'SANAMECHS', sortable: true, align: 'center' },
            { title: '资产类别', name: 'CLASSNAMECN', sortable: true, align: 'center' },
            { title: '规格型号', name: 'GuigeXinghao', sortable: true, align: 'center' },
            { title: '应存放地址', name: 'LOCATIONNAMECN', sortable: true, align: 'center' },
            { title: '使用部门', name: 'SUSEDEPTNAMECN', sortable: true, align: 'center' },
            { title: '责任人', name: 'SUSER', sortable: true, align: 'center' },
            { title: '标签编码', name: 'SBARCODE', sortable: true, align: 'center' },
            { title: '资产净值', name: 'NVALUE', sortable: true, align: 'center' },
        ];
        var items = [];

        //首先加载盘点计划列表
        LoadStockPlanList();
        if (!IsBuildList) {
            LoadDeptList();
            LoadLocationList();
            LoadCategoryList();
        }

        $("#sel_StockPlan").click(function () {
            if ($("#sel_StockPlan").val() == '') {
                $("#lbTipSelectPlan").show();
            } else {
                $("#lbTipSelectPlan").hide();
            }
        });

        function InitMMGrid() {
            $('#tbList').empty();
            //定义表格
            varMMGrdid = $('#tbList').mmGrid({
                height: 440,
                indexCol: true,
                indexColWidth: 35,
                fullWidthRows: true,
                multiSelect: true,
                checkCol: true,
                nowrap: true,
                cols: columns,
                //                        plugins: [
                //                            $('#paginator').mmPaginator()
                //                        ],
                items: items
            });
        }
        InitMMGrid();

        $("#cb_Stocked").change(function () {
            InitMMGrid();
            $("#lbListCount").text("列表记录数:0")
            if ($(this).prop("checked")) {
                $("#btnSave").text("删除选中盘点数据");
            } else {
                $("#btnSave").text("保存盘点数据");
            }
        });

        $("#btnQuery").click(function () {
            $("#txtNoticeTip").html('数据加载中......');
            if ($("#sel_StockPlan").val() == '') {
                $("#lbTipSelectPlan").show();
                return false;
            } else {
                $("#lbTipSelectPlan").hide();
            }
            $("#btnQuery").attr("disabled", true);
            $("#btnSave").attr("disabled", true);

            setTimeout(function () {
                var varStocked = $("#cb_Stocked").prop("checked");
                var strParam = "stocked=" + varStocked;
                var urlQuery = escape("M=" + Math.random() + "&param=queryAssetsList&" + strParam);
                var url = "InputStockResult.ashx?" + urlQuery;
                $.ajax({
                    cache: false,
                    url: url,
                    async: false,
                    type: "post",
                    data: $("#formList").serialize(), // 你的formid
                    error: function (request) {
                        $("#btnQuery").attr("disabled", false);
                        $("#btnSave").attr("disabled", false);
                    },
                    success: function (data) {
                        //                    alert(data);
                        if (data == "") return false;
                        items = [];

                        var dataobj = eval("(" + data + ")");
                        //alert(dataobj.root.length); //输出root的子对象数量
                        //                alert(dataobj.ResultData);
                        if (dataobj.ResultData != "") {
                            var rows = dataobj.ResultData.length;
                            //构建表格####### Start
                            var i, j, row;
                            for (i = 0; i < rows; i++) {
                                row = {};
                                for (var key in dataobj.ResultData[i]) {
                                    row[key] = unescape(dataobj.ResultData[i][key]);
                                }
                                items.push(row);
                            }
                        }
                        varMMGrdid.load(items);
                        $("#btnQuery").attr("disabled", false);
                        $("#btnSave").attr("disabled", false);
                    }
                });
            }, 500);

            $("#lbListCount").text("列表记录数:" + varMMGrdid.rowsLength())
            $("#lbSelectCount").text("已选择记录数: 0 ");
            $("#txtNoticeTip").html('');
        });

        //        //选择事件
        //        varMMGrdid.on("cellSelected", function (e, item, rowIndex, colIndex) {
        //            alert(item);
        //            alert(item[0]);
        //            alert(item.SACODE);
        //            var selectedRows = varMMGrdid.selectedRows();
        //            $("#lbSelectCount").text("已选择记录数:" + (parseInt(selectedRows.length) + 1).toString());
        //        });

        //        $("#tbList :checkbox").click(function (e) {
        //            alert($(this));
        //            //            $("#lbSelectCount").text("已选择记录数:" + selectedRows.length);
        //        });

        $("#btnSave").click(function () {
            $("#txtNoticeTip").html('正在保存......');
            if ($("#sel_StockPlan").val() == '') {
                $("#lbTipSelectPlan").show();
                return false;
            } else {
                $("#lbTipSelectPlan").hide();
            }
            setTimeout(function () {
                var selectedRows = varMMGrdid.selectedRows();
                $("#btnSave").attr("disabled", true);
                $("#lbSelectCount").text("已选择记录数:" + selectedRows.length);
                if (selectedRows.length > 0) {
                    var varStocked = $("#cb_Stocked").prop("checked");
                    var showString = "并将保存为盘点数据";
                    if (varStocked) {
                        showString = "并将删除所选盘点数据";
                    }

                    if (confirm("已选择" + selectedRows.length + "条记录，" + showString + ",是否确认？")) {
                        var varSelectedACODE = "";
                        for (var idx in selectedRows) {
                            var item = selectedRows[idx];
                            var sAcode = item.SACODE;
                            if (idx == 0) {
                                varSelectedACODE = sAcode;
                            } else {
                                varSelectedACODE = varSelectedACODE + "," + sAcode;
                            }
                        }
                        $("#txt_SelectSaveSACODE").val(varSelectedACODE);
                        //请求服务器进行保存
                        var varStockPlan = $("#sel_StockPlan").val();
                        var urlQuery = escape("M=" + Math.random() + "&param=saveResult&stocked=" + varStocked);
                        var url = "InputStockResult.ashx?" + urlQuery;
                        $.ajax({
                            cache: false,
                            url: url,
                            async: false,
                            type: "post",
                            data: $("#formList").serialize(), // 你的formid
                            error: function (request) {
                                $("#btnSave").attr("disabled", false);
                            },
                            success: function (data) {
                                $("#btnSave").attr("disabled", false);
                                //                            alert(data);
                                if (data == "") return false;


                                var showString = "保存盘点数据";
                                if (varStocked) {
                                    showString = "删除盘点数据";
                                }
                                if (data == "1") {
                                    alert(showString + "成功!");
                                    $("#btnQuery").click();
                                } else {
                                    alert(showString + "失败，请稍后重试!");
                                }
                            }
                        });
                    }
                } else {
                    $("#btnSave").attr("disabled", false);
                    alert("请至少选择一条记录后再进行保存！");
                }
            }, 500);
            $("#btnSave").attr("disabled", false);
            $("#txtNoticeTip").html('');

        });
    });
    
</script>

<script type="text/javascript">
    function LoadStockPlanList() {
        ///加载盘点计划下拉框
        var urlStockPlanQuery = escape("M=" + Math.random() + "&param=getStockPlan");
        var urlStockPlan = "AssetsCommon.ashx?" + urlStockPlanQuery;
        $.ajax({
            cache: false,
            url: urlStockPlan,
            async: false,
            error: function (request) {
            },
            success: function (data) {
                //                                alert(data);
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");
                //alert(dataobj.root.length); //输出root的子对象数量
                //                alert(dataobj.ResultData);
                if (dataobj.ResultData == "") return false;

                //加载下拉列表sel_StockPlan
                $.each(dataobj.ResultData, function (idx, item) {
                    $("#sel_StockPlan").append("<option value='" + item.CID + "'>" + unescape(item.CDESCCHS) + "</option>");
                });

            }
        });
    }

    function LoadDeptList() {
        ///加载部门下拉框
        var urlDeptQuery = escape("M=" + Math.random() + "&param=getAllDept");
        var urlDept = "AssetsCommon.ashx?" + urlDeptQuery;
        $.ajax({
            cache: false,
            url: urlDept,
            async: false,
            error: function (request) {
            },
            success: function (data) {
                //                                alert(data);
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");
                //alert(dataobj.root.length); //输出root的子对象数量
                //                alert(dataobj.ResultData);
                if (dataobj.ResultData == "") return false;

                //加载下拉列表sel_Dept
                $.each(dataobj.ResultData, function (idx, item) {
                    $("#sel_Dept").append("<option value='" + item.CID + "'>" + unescape(item.CDESCCHS) + "</option>");
                });
                //设置下拉列表sel_Dept默认选中当前登录用户
                var curDept = '<%=this.GetUserDept()%>';
                $("#sel_Dept option[value='" + curDept + "']").attr("selected", true);   //设置Select的Text值为jQuery的项选中 ;

                IsBuildList = true;
            }
        });
    }
        
    function LoadLocationList(){
        ///加载存放地址下拉框
        var urlLocationQuery = escape("M=" + Math.random() + "&param=getAllLocation");
        var urlLocation = "AssetsCommon.ashx?" + urlLocationQuery;
        $.ajax({
            cache: false,
            url: urlLocation,
            async: false,
            error: function (request) {
            },
            success: function (data) {
                //                                alert(data);
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");
                //alert(dataobj.root.length); //输出root的子对象数量
                //                alert(dataobj.ResultData);
                if (dataobj.ResultData == "") return false;

                //加载下拉列表sel_Location
                $.each(dataobj.ResultData, function (idx, item) {
                    $("#sel_Location").append("<option value='" + item.CID + "'>" + unescape(item.CDESCCHS) + "</option>");
                });

            }
        });
    }

    function LoadCategoryList(){
        ///加载资产分类下拉框
        var urlCategoryQuery = escape("M=" + Math.random() + "&param=getAllAMClass");
        var urlCategory = "AssetsCommon.ashx?" + urlCategoryQuery;
        $.ajax({
            cache: false,
            url: urlCategory,
            async: false,
            error: function (request) {
            },
            success: function (data) {
                //                                alert(data);
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");
                //alert(dataobj.root.length); //输出root的子对象数量
                //                alert(dataobj.ResultData);
                if (dataobj.ResultData == "") return false;

                //加载下拉列表sel_Category
                $.each(dataobj.ResultData, function (idx, item) {
                    $("#sel_Category").append("<option value='" + item.CID + "'>" + unescape(item.CDESCCHS) + "</option>");
                });

            }
        });
    }
    
</script>

</body>
</html>
