<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllPaibanIndex.aspx.cs" Inherits="AppFunction_Attendance_AllPaibanIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link type="text/css" href="../../common/css/main.css" rel="stylesheet"  media="all" />
<link type="text/css" href="../../common/css/topStyle.css" rel="stylesheet"  media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<title>Attendance Verify</title>
<style>
    .imgBtn {width:36px; height:36px;cursor:hand;}
</style>
</head>

<body>

<form runat="server" id = "form1">

<div id="divWaitting" runat="server" style="display:none;float:left;z-index:100;position:absolute;text-align:center; 
	width:100%;height:100%;filter:alpha(opacity=60);background-color:#707070;">
    <font color="red" >Loading.......</font>
</div>

<table width="100%" height="43px"  cellpadding="0" cellspacing="0" border="0" background="common/images/topFrame/bg.jpg" >
    <tr width="100%" style="height:3px;" valign="middle"><td style="height:3px"></td></tr>
    <tr width="100%" style="height:98%;" valign="middle">
      <td height="100%" width="100%" align="left" valign="middle">
          <div class="topBox">
              <table width="100%" cellpadding="0" cellspacing="0" border="0">
                  <tr width="100%">
                      <td height="100%" style="width:5%" align="left">
                          <asp:Label ID="Label_User" runat="server" Text="考勤员:" Font-Bold=true></asp:Label>
                      </td>
                      <td style="width:20%" >
                          <asp:DropDownList ID="ddListUser" runat="server" Width = "100%"  
                              onselectedindexchanged="ddListUser_SelectedIndexChanged" AutoPostBack="true">
                          </asp:DropDownList>
                      </td>
                      <td colspan="2">
                          <asp:Label ID="Label_ChargeSections" runat="server" Text="负责考勤部门：" Font-Bold=true Visible="false"></asp:Label>
                          <asp:Label ID="Label_Sections" runat="server" Text="" Font-Bold=true></asp:Label>
                      </td>
                  </tr>
                  <tr width="100%" style="display:none">
                      <td height="100%" style="width:100" align="left">
                          <asp:Label ID="Label_Dept" runat="server" Text="Deprtment"></asp:Label>
                      </td>
                      <td style="width:90%" colspan="2">
                          <asp:DropDownList ID="ddListDivision" runat="server" Width = "20%"  
                              onselectedindexchanged="ddListDivision_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                          </asp:DropDownList>
                          <asp:DropDownList ID="ddListDept" runat="server" Width = "20%"  
                              onselectedindexchanged="ddListDept_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                          </asp:DropDownList>
                          <asp:DropDownList ID="ddListSection" runat="server" Width = "20%"  
                              onselectedindexchanged="ddListSection_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                          </asp:DropDownList>
                      </td>
                  </tr>
                  
                  <tr width="100%">
                      <td height="100%" style="width:5%" align="left">
                          <asp:Label ID="Label_YearMonth" runat="server" Text="YearMonth" Font-Bold=true></asp:Label>
                      </td>
                      <td style="width:20%">
                          <asp:DropDownList ID="ddListYearMonth" runat="server" Width = "65%"  
                              onselectedindexchanged="ddListYearMonth_SelectedIndexChanged" AutoPostBack="true">
                          </asp:DropDownList>
                          <asp:LinkButton ID="aLockPerd"  Width = "35%" Height="10px"  runat="server" CssClass="a_Right" Visible="false" onclick="aLockPerd_Click">
                              <asp:Label ID="Label_Lock" runat="server" Text="锁定该期间"></asp:Label>
                          </asp:LinkButton>
                      </td>
                      <td id="tdSetWeekLock" height="100%" style="width:30%" align="left" runat="server" >
                          <asp:Label ID="Label_WeekLock" runat="server" Text="YearMonth" Font-Bold="true" Visible="false">&nbsp;&nbsp;&nbsp;&nbsp;周锁定</asp:Label>
                          <asp:DropDownList ID="ddListWeekLock" runat="server" Width = "50%"   Visible="false"
                              onselectedindexchanged="ddListWeekLock_SelectedIndexChanged"  AutoPostBack="true">
                          </asp:DropDownList>
                      </td>
                      <td style="width:45%" align="right">
                          <asp:LinkButton ID="aVerify" runat="server" CssClass="a_Center" Visible="false" onclick="aVerify_Click"><asp:Label ID="Label_Verify" runat="server" Text="审核该部门"></asp:Label></asp:LinkButton>
                          <asp:LinkButton ID="aFinish" runat="server" CssClass="a_Center" Visible="false" onclick="aFinish_Click"><asp:Label ID="Label_Finish" runat="server" Text="该部门审核完毕"></asp:Label></asp:LinkButton>
                          <asp:LinkButton ID="aReturn" runat="server" CssClass="a_Center" Visible="false" onclick="aReturn_Click"><asp:Label ID="Label_Return" runat="server" Text="退回该部门"></asp:Label></asp:LinkButton>
                          <asp:Label ID="Label6" runat="server" Text="||"></asp:Label>
                          <asp:LinkButton ID="aVerifyAll" runat="server" CssClass="a_Center" Visible="false" onclick="aVerifyAll_Click"><asp:Label ID="Label_VerifyAll" runat="server" Text="审核所有部门"></asp:Label></asp:LinkButton>
                          <asp:LinkButton ID="aFinishAll" runat="server" CssClass="a_Center" Visible="false" onclick="aFinishAll_Click"><asp:Label ID="Label_FinishAll" runat="server" Text="所有审核完毕"></asp:Label></asp:LinkButton>
                          <asp:LinkButton ID="aReturnAll" runat="server" CssClass="a_Center" Visible="false" onclick="aReturnAll_Click"><asp:Label ID="Label_ReturnAll" runat="server" Text="退回所有部门"></asp:Label></asp:LinkButton>
                          <asp:Label ID="Label1" runat="server" Text="||"></asp:Label>
                          <asp:LinkButton ID="aCloase" runat="server" CssClass="a_Center" OnClientClick="window.close();"><asp:Label ID="lbCloase" runat="server" Text="Close"></asp:Label></asp:LinkButton>
                      </td>
                  </tr>
              </table>
          </div>
      </td>
    </tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="left" style="width:80%; height:100%; padding:0px;">
        <iframe id="mainFrame" name="mainFrame" width="100%" height="600" frameborder="0" src="" style="border: 0px solid #cecece;" scrolling="auto"></iframe>
	</td>
  </tr>
  <tr>
  </tr>
</table>

</form>
</body>
</html>

<script language="javascript">
function SetFrameHeight(obj) 
{ 
    var win=obj; 
    if (document.getElementById) 
    { 
        if (win && !window.opera) 
        { 
            if (win.contentDocument && win.contentDocument.body.offsetHeight) 
                win.height = win.contentDocument.body.offsetHeight; 
            else if(win.Document && win.Document.body.scrollHeight) 
                win.height = win.Document.body.scrollHeight; 
        } 
    }
}

</script>
