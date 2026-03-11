<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalculateFlow.aspx.cs" Inherits="AppFunction_SalaryCalculate_CalculateFlow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>薪资计算流程页面</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script>
    function redirectToDeal(params){
        document.form1.action = "CalculateDealPage.aspx?"+params;
        document.form1.submit();
    }
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<div>
    <table border="0"  width="95%" id="tb1" align="center" style="height:auto; height:100%; ">
	  <tr valign="middle">
        <td style="width:60%; height:100%" valign="TOP" align="center">
            <table>
                <tr style="height:10%"></tr>
                <tr valign="middle">
                    <td align="center">
                        <img alt="" src="../../common/images/SalaryFlow/SalaryMain.jpg" width="650" height="400" /><br>
                        
                        <div  class="left_txt">
                            薪资计算及支付由人事部门与财务部门共同完成。<br>
                            人事部门主要负责提供完整而准确的薪资计算基础数据，如员工基础信息，保险信息，薪资信息等；<br>
                            财务部门主要根据人事部门所提供的基础数据进行核对及薪资计算，然后完成支付操作。<br>
                            提示：右图中<font color="red">红色标记</font>当前可进行的操作。
                        </div>
                    </td>
                </tr>
                <tr style="height:10%"></tr>
                <div runat="server" id="divFlowDescArea">
                    
                </div>
            </table>
        </td>
        <td style="width:40%; height:100%" valign="top" align="center">
            <table class="table">
                <div runat="server" id="divFlowImageArea">
                    
                </div>
            </table>
        </td>
	  </tr>
    </table>
</div>
</form>
</body>
</html>
