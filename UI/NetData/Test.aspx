<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="NetData_Test" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8"/>
    <title>测试页面</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
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

<body>
    <form id="form1" runat="server">
    <div>
        <input type="text" style = "width:64%;" id = "txt_Input" name="txt_Input">
        <button id = "btnTest" type="button" class="a_Left">测试</button>
    </div>
    </form>
</body>
</html>

<script type="text/javascript">

    $(document).ready(function () {
        $("#btnTest").click(function () {
            LoadStockPlanList();
        });
    });

    function LoadStockPlanList() {
        ///测试
        var urlStockPlanQuery = escape("M=" + Math.random() + "&param=querymobileinfo&mobileno=13924207569");
        var urlStockPlan = "Handler/MobileHandler.ashx?" + urlStockPlanQuery;
//        var urlStockPlanQuery = escape("M=" + Math.random() + "&param=getStockPlan");
//        var urlStockPlan = "../Asset/AssetsCommon.ashx?" + urlStockPlanQuery;
        alert(urlStockPlan);
        $.ajax({
            cache: false,
            url: urlStockPlan,
            async: false,
            error: function (request) {
            },
            success: function (data) {
                                                alert(data);
                if (data == "") return false;
                var dataobj = eval("(" + data + ")");


            }
        });
    }
    
</script>
