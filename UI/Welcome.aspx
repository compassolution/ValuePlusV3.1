<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" Inherits="Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

<html>
<head>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="common/css/main.css" rel="stylesheet" type="text/css" />
<link href="common/css/button.css" type="text/css" rel="stylesheet" />
<link href="common/css/topStyle.css" rel="stylesheet" type="text/css" />
<link href="common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" />
<link href="common/bootstrap/bootstrap.css" rel="stylesheet" />
<link href="common/bootstrap-table/bootstrap-table.css" rel="stylesheet" />

<script src="common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="common/bootstrap/bootstrap.js" type="text/javascript"></script>
<script src="common/bootstrap-table/bootstrap-table.js" type="text/javascript"></script>
<script  src="common/js/waitProcess.js" type="text/javascript"></script>
<title>欢迎页面</title>
</head>
<body>
<!--#include   file= "common/WaitProccess.htm"--> 
<!--JavaScript部分-->
<script  type="text/javascript">
	function secBoard(n)
	{
		for(i=0;i<secTable.cells.length;i++)
			secTable.cells[i].className="sec1";
			secTable.cells[n].className="sec2";
		for(i=0;i<mainTable.tBodies.length;i++)
			mainTable.tBodies[i].style.display="none";
			mainTable.tBodies[n].style.display="block";
	}
	
</script>
<form id="form1" runat="server">
<table id="tbMain" width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-top:10px">
  <tr>
    <td valign="middle">&nbsp;</td>
    <td valign="top" bgcolor="#F7F8F9" style="width:100%">
    <table width="98%" style="height:95%" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr><td colspan="4" valign="top">&nbsp;</td></tr>
      <tr>
        <td colspan="4" valign="top"><span class="left_bt" id="spanTitle" runat="server">欢迎使用ValuePlus酒店人事管理系统程序</span><br>
        </td>
      </tr>
      <tr><td colspan="4">&nbsp;</td></tr>
      
      <tr>
        <td colspan="4" valign="top" style="width:100%;height:100%">
          <!--HTML部分-->
          <table width="100%" border="0" id="secTable">
            <tbody>
              <tr align="center">
                <td align="center" class="sec2" onclick="javascripts:secBoard(0)" style="width:15%"><span id="spanSec_Pending" runat="server">待办事项</span></td>
                <td align="center" class="sec1" onclick="javascripts:secBoard(1)" style="width:15%;display:none"><span id="spanSec_Notice" runat="server">公告信息</span></td>
                <%--<td align="center" class="sec1" onclick=secBoard(2) style="width:15%"><span id="spanSec_CopyRight" runat="server">版权说明</span></td>--%>
                <%--<td align="center" class="sec1" onclick=secBoard(3) style="width:70%"></td>--%>
              </tr>
            </tbody>
          </table>
          
          <table class="main_tab" id="mainTable" width="100%">
                <!--关于待办事项页面-->
                <tbody style="display:">
                  <tr>
                    <td valign="top">
                    <table id="tbPending" width="100%">
                        <tbody>
                          <tr><td height="5" colspan="3"></td></tr>
                          <tr><td height="5" colspan="3" bgcolor="#FAFBFC"></td>
                          </tr>
                          <tr>
                            <td bgcolor="#FAFBFC" align="center" valign="top">
                                <iframe id="frmPending" name="frmPending" width="100%" height="380" frameborder="0" src="Archive\Pending\PendingList.aspx" style="border: 0px solid #cecece;"></iframe>
                            </td>
                          </tr>
                          <tr>
                            <td height="5" colspan="3"></td>
                          </tr>
                        </tbody>
                    </table>

                    </td>
                  </tr>
                </tbody>
                <!--关于公告信息页面-->
                <tbody style="DISPLAY: none">
                  <tr>
                    <td valign="top" align="center">
                        <table width="100%" align="center">
                            <tbody>
                              <tr>
                                <td height="5" colspan="3"></td>
                              </tr>
                              <tr>
                                <td height="5" colspan="3" bgcolor="#FAFBFC"></td>
                              </tr>
                              <tr>
                                <td bgcolor="#FAFBFC" align="center" valign="top">
                                    <iframe id="frmNotice" name="frmNotice" width="100%" height="380" frameborder="0" src="Notice/NoticeList.aspx?opType=view" style="border: 0px solid #cecece;"></iframe>
                                </td>
                              </tr>
                              <tr>
                                <td height="5" colspan="3"></td>
                              </tr>
                            </tbody>
                        </table>
                    </td>
                  </tr>
                </tbody>
                <!--关于版权说明信息-->
                <%--<tbody style="DISPLAY: none">
                  <tr>
                    <td vAlign=top align=middle><table width=98% height="380" border=0 align="center" cellPadding=0 cellSpacing=0>
                        <tbody>
                          <tr>
                            <td height="5" colspan="3"></td>
                          </tr>
                          <tr>
                            <td height="5" colspan="3" bgcolor="#FAFBFC"></td>
                          </tr>
                          <tr>
                            <td bgcolor="#FAFBFC" style="width:1%"></td>
                            <td bgcolor="#FAFBFC" align="center" valign="top">
                                <iframe id="frmCopyright" name="frmCopyright" width="100%" height="380" frameborder="0" src="Regist/CopyrightInfo.aspx" style="border: 0px solid #cecece;"></iframe>
                            </td>
                            <td bgcolor="#FAFBFC" style="width:1%"></td>
                          </tr>
                          <tr>
                            <td height="5" colspan="3"></td>
                          </tr>
                        </tbody>
                    </table></td>
                  </tr>
                </tbody>--%>
          </table>
        </td>
        <td>&nbsp;</td>
        
      </tr>
      <%--<tr>
        <td width="2%">&nbsp;</td>
        <td width="51%" class="left_txt"><img src="common/images/welcome/icon-mail2.gif" width="16" height="11"> 客户服务邮箱：21338671@qq.com<br>
              <img src="common/images/welcome/icon-phone.gif" width="17" height="14"> 官方网站：http://www.valueplus.cn</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
      </tr>--%>
    </table>
    </td>
  </tr>
</table>
</form>
<script type="text/javascript">
    var screenHeight = screen.availHeight;
    $(document).ready(function () {
        //$("#tbMain").height(screenHeight - 220); 
        //$("#mainTable").height(screenHeight - 320); 
        //$("#tbPending").height(screenHeight - 320);
        //$("#frmPending").height(screenHeight - 330);
    });
</script>
</body>
</html>

