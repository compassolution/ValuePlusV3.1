<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollReport.aspx.cs" Inherits="AppFunction_HRSalary_PayRollReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Pay Roll Report</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<%--<link href="../../common/css/ArchiveStyle.css" rel="stylesheet" type="text/css" />--%>

    <!--media=print 这个属性可以在打印时有效-->
    <style media="print">
        .Noprint
        {
            display: none;
        }
        .PageNext
        {
            page-break-after: always;
        }
    </style>
    <style>
        .tdp
        {
            border-bottom: 1 solid #000000;
            border-left: 1 solid #000000;
            border-right: 0 solid #ffffff;
            border-top: 0 solid #ffffff;
        }
        .tabp
        {
            border-color: #000000 #000000 #000000 #000000;
            border-style: solid;
            border-top-width: 2px;
            border-right-width: 2px;
            border-bottom-width: 1px;
            border-left-width: 1px;
        }
        .NOPRINT
        {
            font-family: "宋体";
            font-size: 9pt;
        }
    </style>
<script  src="../../common/js/waitProcess.js" ></script>
<script  src="../../common/js/tableStyle.js" ></script>
<script>

    function EnterPageSizeTextBox() {
        if (event.keyCode == 13 && document.all["txtPageSize"].value != "") {
            event.keyCode = 9;
            event.returnValue = false;
            document.all["aChangePageSize"].click();
        }
    }
    function OpenWindowMax(actionPage) {
        window.open(actionPage, 'SalaryReport', 'left=0,top=0,width=' + (screen.availWidth - 10) + ',height=' + (screen.availHeight - 50) + ',scrollbars,resizable=yes,toolbar=no,location=no'); //最大化打开
    }
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<!--#include   file= "../../common/CurPageWaiting.htm"-->
 <object id="WebBrowser" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" height="0" width="0"> </object> 
<form id="form1" runat="server">
    <div class="Noprint">
        <asp:DropDownList ID="DDList_YearMonth" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="DDList_YearMonth_SelectedIndexChanged" Height="22px" Width="100"></asp:DropDownList>
        <asp:DropDownList ID="DDList_ReportType" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="DDList_ReportType_SelectedIndexChanged" Height="22px" Width="100"></asp:DropDownList>
        <asp:TextBox ID="txtPageSize" runat="server" Visible="true" Width="50px" Height="18px" BorderWidth=1 Text=20></asp:TextBox>
        <asp:LinkButton ID="aChangePageSize" runat="server" CssClass="a_Right" onclick="aChangePageSize_Click" Text="GO"></asp:LinkButton>
        <asp:DropDownList ID="DDList_TableCount" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="DDList_TableCount_SelectedIndexChanged" Height="22px" Width="70"></asp:DropDownList>
        <input onclick="document.all.WebBrowser.ExecWB(6,1)" type="button" value="Print">     
        <input onclick="document.all.WebBrowser.ExecWB(8,1)" type="button" value="Page Setting">     
        <input onclick="document.all.WebBrowser.ExecWB(7,1)" type="button" value="Print Preview">
        <input type="button" value="Export Excel" onclick="javascript:exportexcel('../../Export/exportindex.aspx')">   
        <input type="button" value="New Window" onclick="javascript:OpenWindowMax('PayRollReport.aspx')">    
        <input type="button" value="Close" onclick="javascript:window.close()">    

    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tableNoHover"> 
        <tr >
            <div style="display:none" runat="server">
                <asp:Label ID="lb_TableName_Pre" runat="server" Text="TB_PayRollReport"></asp:Label>
                <asp:Label ID="lb_SPName_Detail" runat="server" Text="USP_HR_PR_BuildPayRollReportData"></asp:Label>
                <asp:Label ID="lb_SPName_Summary" runat="server" Text="USP_HR_PR_BuildPayRollReportData_Summary"></asp:Label>
            </div>
        </tr>
        <!-- 结果列表部分 --> 
        <tr>
            <td align="left" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" >
                    <tr>
                        <td style ="white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  
                                AutoGenerateColumns="true"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" OnItemDataBound = "DataGrid1_ItemDataBound" >
                                <ItemStyle CssClass="tableContent" />
                                <HeaderStyle CssClass="fixGridHeaderStyle"></HeaderStyle>
                                <Columns>
							    </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>	

    </table>
    </form>
<form id="from10000post" name="from10000post" method="post">
<input name="hidden" type="hidden" />
</form>
  <iframe id="iddownframe000000"  name="iddownframe000000"  style="width:0px;height:0px;display:none;"></iframe>   
  <iframe id="iddownframe000001"  name="iddownframe000001"  style="width:0px;height:0px;display:none;"></iframe> 
  <script language="javascript" type="text/javascript">
      function exportexcel(filename) {
          //        var sPath = filename + '?type=excel&ran=' + Math.random();
          var sPath = filename + '?type=nopiExcel&ran=' + Math.random();
          from10000post.action = sPath;
          from10000post.target = "iddownframe000000";
          from10000post.submit();
      }
      function exportpdf(filename) {
          var sPath = filename + '?type=pdf&ran=' + Math.random();
          from10000post.action = sPath;
          from10000post.target = "iddownframe000002";
          from10000post.submit();
      }
      function exporttxt(filename) {
          var sPath = filename + '?type=txt&ran=' + Math.random();
          from10000post.action = sPath;
          from10000post.target = "iddownframe000001";
          from10000post.submit();
      }
  </script>  
</body>
</html>

<script language="javascript">
    //初始化结果表格
    DefineTableCssNoCursorOver("changecolor");
</script>