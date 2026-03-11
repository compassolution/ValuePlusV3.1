<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InputOEStock.aspx.cs" Inherits="Asset_InputOEStock" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8"/>
    <title>OE资产盘点录入</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
    <link href="../common/css/main.css" rel="stylesheet" type="text/css" />
    <link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
    <link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css"/>
    <link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
    <link href="../common/bootstrap/bootstrap.css" rel="stylesheet"/>
    <link href="../common/bootstrap-table/bootstrap-table.css" rel="stylesheet"/>
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
                <input type="text" style="display:none" id="txtListCount" name="txtListCount" value="0"/>
                <div class="btn-group pull-right btn-group-sm" role="group">
                    <button type="button" class="btn btn-default dropdown-toggle " data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                      批量设置盘点数量
                      <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu">
                      <li><a href="javascript:void(0);" id="aSetAllCount">设置全部</a></li>
                      <li><a href="javascript:void(0);" id="aCancelAllCount">取消全部</a></li>
                    </ul>
              </div>
               <%-- <label class="control-label" id = "lbSelectCount" style="color:Red">已选择记录数：0</label>--%>
            </div>
            <div class="col-xs-12">
                <table id="tbList" class="tableNoHover table-hover col-xs-12" >
                    <thead>
                        <tr style=" font-weight:bold">
                            <td data-width = "10">序号</td>
                            <td data-width = "90">资产编码</td>
                            <td data-width = "90">资产名称</td>
                            <td data-width = "90">资产类别</td>
                            <td data-width = "90">使用部门</td>
                            <td data-width = "90">存放地址</td>
                            <td data-width = "90">帐存数量</td>
                            <td data-width = "90">盘点数量</td>
                        </tr>
                    </thead>
                    <tbody id = "tbodyItem" class="tableContent">

                    </tbody>
                
                </table>
                <br /><br /><br />
            </div>

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
            <div class="col-xs-11 text-center">
                <button id = "btnQuery" type="button" class="a_Left">查询</button>&nbsp;&nbsp;&nbsp;&nbsp;
                <button id = "btnSave" type="button" class="a_Left">保存盘点数据</button>
            </div>
        </div >
        
    </div>
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
    $(document).ready(function () {
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

        $("#btnQuery").click(function () {
            if ($("#sel_StockPlan").val() == '') {
                $("#lbTipSelectPlan").show();
                return false;
            } else {
                $("#lbTipSelectPlan").hide();
            }

            $("#btnQuery").attr("disabled", true);
            $("#btnSave").attr("disabled", true);
            
            setTimeout(function () {
                var varStockPlan = $("#sel_StockPlan").val();
                var varDept = $("#sel_Dept").val();
                var varLocation = $("#sel_Location").val();
                var varCategory = $("#sel_Category").val();
                var varACode = $("#txt_ACode").val();
                var varAName = $("#txt_AName").val();
                var varAModel = $("#txt_AModel").val();
                var strParam = "stockplan=" + varStockPlan + "&dept=" + varDept + "&location=" + varLocation + "&category=" + varCategory + "&acode=" + varACode + "&aname=" + varAName + "&amodel=" + varAModel;
                var urlQuery = escape("M=" + Math.random() + "&param=queryAssetsList&" + strParam);
                var url = "InputOEStock.ashx?" + urlQuery;
                $.ajax({
                    cache: false,
                    url: url,
                    async: false,
                    type: "post",
                    error: function (request) {
                        $("#btnQuery").attr("disabled", false);
                        $("#btnSave").attr("disabled", false);
                    },
                    success: function (data) {
                        //                                        alert(data);
                        if (data == "") return false;
                        items = [];

                        var dataobj = eval("(" + data + ")");
                        //alert(dataobj.root.length); //输出root的子对象数量
                        //                alert(dataobj.ResultData);
                        $("#tbodyItem").empty();
                        $("#lbListCount").text("列表记录数:0");
                        $("#txtListCount").val("0");

                        if (dataobj.ResultData != "") {
                            var rows = dataobj.ResultData.length;
                            //构建表格####### Start
                            var i, j, row;
                            for (i = 0; i < rows; i++) {
                                var tempHTML = "";
                                tempHTML = tempHTML + "<tr id = \"trItemRow\"" + i.toString() + ">";
                                tempHTML = tempHTML + "    <td>" + (i+1).toString() + "</td>";
                                tempHTML = tempHTML + "    <td>" + unescape(dataobj.ResultData[i]["SACODE"]) + "</td>";
                                tempHTML = tempHTML + "    <td style = \"display:none\"><input type=\"text\" class=\"input-mini\" id=\"txt_Item_SACODE_" + i.toString() + "\" name=\"txt_Item_SACODE_" + i.toString() + "\" value=\"" + unescape(dataobj.ResultData[i]["SACODE"]) + "\" /> </td>";
                                tempHTML = tempHTML + "    <td>" + unescape(dataobj.ResultData[i]["SANAMECHS"]) + "</td>";
                                tempHTML = tempHTML + "    <td>" + unescape(dataobj.ResultData[i]["CLASSNAMECN"]) + "</td>";
                                tempHTML = tempHTML + "    <td>" + unescape(dataobj.ResultData[i]["SUSEDEPTNAME"]) + "</td>";
                                tempHTML = tempHTML + "    <td style = \"display:none\"><input type=\"text\" class=\"input-mini\" id=\"txt_Item_SLCODE_" + i.toString() + "\" name=\"txt_Item_SLCODE_" + i.toString() + "\" value=\"" + unescape(dataobj.ResultData[i]["SLCODE"]) + "\" /> </td>";
                                tempHTML = tempHTML + "    <td>" + unescape(dataobj.ResultData[i]["SLCODENAME"]) + "</td>";
                                tempHTML = tempHTML + "    <td class=\"tdNQUANTITY\" id = \"tdCanInput_" + i.toString() + "\">" + unescape(dataobj.ResultData[i]["NQUANTITY"]) + "</td>";
                                tempHTML = tempHTML + "    <td><input type=\"text\" style=\"width:80px\" class=\"classStockQty\" trIndex = \"" + i.toString() + "\" id=\"txt_Item_StockQty_" + i.toString() + "\" name=\"txt_Item_StockQty_" + i.toString() + "\" value=\"" + unescape(dataobj.ResultData[i]["StockQty"]) + "\" /> </td>";
                                tempHTML = tempHTML + "</tr>";

                                $("#tbodyItem").append(tempHTML);
                            }
                            $("#lbListCount").text("列表记录数:" + rows.toString());
                            $("#txtListCount").val(rows.toString());

                            $('#formList').height($('#tbodyItem').height() + 150);
                        } else {
                            $('#formList').height(300);
                        }
                        $("#btnQuery").attr("disabled", false);
                        $("#btnSave").attr("disabled", false);

                        $(".classStockQty").blur(function (e) {
                            var pattern = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;
                            var flag = pattern.test($(this).val());

                            if (flag) {
                                var rowIndex = $(this).attr("trIndex");
                                var inputValue = parseFloat($(this).val());
                                var canSotck = parseFloat($("#tdCanInput_" + rowIndex.toString()).html());

                                if (inputValue > canSotck) {
                                    if (confirm("输入的盘点数量大于帐存数量，是否确认?")) {
                                    } else {
                                        $(this).val(canSotck.toString());
                                    }
                                }
                            } else {
                                alert("请输入数值型数据!");
                                $(this).val("0.00");
                            }
                        });
                    }
                });
            }, 500);
        });

        $("#aSetAllCount").click(function () {
            $(".classStockQty").each(function () {
                $trIndex = $(this).attr('trIndex');
                $(this).val($('#tdCanInput_' + $trIndex).html());
            });
        });
        $("#aCancelAllCount").click(function () {
            $(".classStockQty").val('0.0000');
        });

        $("#btnSave").click(function () {
            if ($("#sel_StockPlan").val() == '') {
                $("#lbTipSelectPlan").show();
                return false;
            } else {
                $("#lbTipSelectPlan").hide();
            }
            setTimeout(function () {
                //请求服务器进行保存
                var varStockPlan = $("#sel_StockPlan").val();
                var strParam = "stockplan=" + varStockPlan;
                var urlQuery = escape("M=" + Math.random() + "&param=saveResult&" + strParam);
                var url = "InputOEStock.ashx?" + urlQuery;
                $.ajax({
                    cache: false,
                    url: url,
                    async: false,
                    type: "post",
                    data: $('#formList').serialize(), // 你的formid
                    error: function (request) {
                        $("#btnSave").attr("disabled", false);
                    },
                    success: function (data) {
                        $("#btnSave").attr("disabled", false);
                        //                            alert(data);
                        if (data == "") return false;

                        if (data == "1") {
                            alert("保存盘点结果成功!");
                            $("#btnQuery").click();
                        } else {
                            alert("保存盘点结果失败，请稍后重试!");
                        }
                    }
                });
            }, 500);
        });

    });
    
</script>

<script type="text/javascript">
    function LoadStockPlanList() {
        ///加载盘点计划下拉框
        var urlStockPlanQuery = escape("M=" + Math.random() + "&param=getOEPlan");
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

    function LoadLocationList() {
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

    function LoadCategoryList() {
        ///加载资产分类下拉框
        var urlCategoryQuery = escape("M=" + Math.random() + "&param=getAllOEClass");
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

<script language="javascript">
    //初始化结果表格
    DefineTableCssNoCursorOver("tbList");
</script>
</body>
</html>
