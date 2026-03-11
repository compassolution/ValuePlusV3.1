<%@ Page Language="C#" AutoEventWireup="true" CodeFile="topFrame.aspx.cs" Inherits="topFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link type="text/css" href="common/css/main.css" rel="stylesheet"  media="all" />
<link type="text/css" href="common/css/topStyle.css" rel="stylesheet"  media="all" />
<script type="text/javascript" src="common/JQuery/jquery-1.10.2.js"></script>
<title></title>
<style>
    .imgBtn {width:36px; height:36px;cursor:hand;}
    
    #divPendingAlert {position: absolute;right: 0px;bottom: 0px;height: 0px;width: 280px;border: 1px solid #CCCCCC;background-color: #eeeeee;padding: 1px;overflow:hidden;display:none;font-size:12px;z-index:10;}
    #divPendingAlert p {padding:6px;}
    #divPendingAlert h1 {font-size:14px;height:25px;line-height:25px;background-color:#33FF00;color:#FF0000;padding:0px 3px 0px 3px;}
    #divPendingAlert h1 a {float:right;text-decoration:none;color:#FFFFFF;}
</style>

<script language="javascript">
    
    function doRedirect(url){
        window.document.getElementById("mainFrame").src  = url;
    }
    
    function redirectMenuFunction(menuId){
        window.document.getElementById("mainFrame").src = "leftFrame.html?menuId="+menuId;
    }
    function redirectHomePage() {
        window.document.getElementById("mainFrame").src = window.document.getElementById("hfHomePageUrl").value;
    }
    
    function BeforeChangeLanguge(){
        if(window.document.getElementById("mainFrame")!=null){
            document.getElementById("hfCurMainFrameUrl").value = window.document.getElementById("mainFrame").src;
            var mainObj = window.frames["mainFrame"];
            if(mainObj.document.frames["topContentFrame"]!=null)
            {
                document.getElementById("hfCurTopFrameUrl").value = mainObj.document.frames["topContentFrame"].window.location.href;
            }
            if(mainObj.document.frames["contentFrame"]!=null)
            {
                document.getElementById("hfCurContentFrameUrl").value = mainObj.document.frames["contentFrame"].window.location.href;
            }
        }
    }

    //设置Edge兼容时需更新此方法内容
    function AfterChangeLanguge(curUrl) {
        if (window.document.getElementById("mainFrame") != null) {
            if (document.getElementById("hfCurMainFrameUrl").value != '') {
                window.document.getElementById("mainFrame").src = document.getElementById("hfCurMainFrameUrl").value;
            } else {
                window.document.getElementById("mainFrame").src = window.document.getElementById("hfHomePageUrl").value;
            }
            var mainObj = window.frames["mainFrame"];
//                alert(mainObj.document.frames["topContentFrame"]);
            if(mainObj.document.frames["topContentFrame"]!=null)
            {
                mainObj.document.frames["topContentFrame"].window.location.href = document.getElementById("hfCurTopFrameUrl").value;
            }
            if(mainObj.document.frames["contentFrame"]!=null)
            {
                mainObj.document.frames["contentFrame"].window.location.href = document.getElementById("hfCurContentFrameUrl").value;
            }
        }
        if(window.parent.frames.bottomFrame!=null){
            window.parent.frames.bottomFrame.location.href = window.parent.frames.bottomFrame.location.href;
        }
    }
    //设置Edge兼容时需更新此方法内容
    
    function ExitSystem(msg){
        document.getElementById("divWaitting").style.display="";
        if(confirm(msg)){
            document.getElementById("aDoExit").click();
        }else{
            document.getElementById("divWaitting").style.display="none";
            return false;
        }
    }
</script>

 <script language="JavaScript" type="text/JavaScript">
 <!--
    function MM_reloadPage(init) {  //reloads the window if Nav4 resized
       if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
         document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
       else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
     }
     MM_reloadPage(true);

     function MM_findObj(n, d) { //v4.01
       var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
         d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
       if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
       for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
       if(!x && d.getElementById) x=d.getElementById(n); return x;
     }

     function MM_showHideLayers() { //v6.0
       var i,p,v,obj,args=MM_showHideLayers.arguments;
       for (i=0; i<(args.length-2); i+=3) if ((obj=MM_findObj(args[i]))!=null) { v=args[i+2];
         if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v=='hide')?'hidden':v; }
         obj.visibility=v; }
     }
 //-->
 </script>

</head>

<body onload="document.getElementById('divPendingAlert').style.height='0px'">
<form runat="server" id = "form1">
<asp:HiddenField ID="hfCurMainFrameUrl"  runat="server"/>
<asp:HiddenField ID="hfCurTopFrameUrl"  runat="server"/>
<asp:HiddenField ID="hfCurContentFrameUrl"  runat="server"/>
<asp:HiddenField ID="hfHomePageUrl"  runat="server"/>
<asp:HiddenField ID="hfIsRealtimeAlert"  runat="server"/>
<asp:HiddenField ID="hfRealtimeAlertInterval"  runat="server"/>


<div id="divWaitting" runat="server" style="display:none;float:left;z-index:100;position:absolute;text-align:center; 
	width:100%;height:100%;filter:alpha(opacity=60);background-color:#707070;">
    <font color="red" >Loading.......</font>
</div>

<table width="100%" height="40px"  cellpadding="0" cellspacing="0" border="0" background="common/images/topFrame/bg.jpg" >
    <tr width="100%" style="height:3px;" valign="middle"><td style="height:3px"></td></tr>
    <tr width="100%" style="height:98%;" valign="middle">
      <td height="100%" width="40%" valign="middle" align="left">
          <%--<input type="image" name="imgLogo" id="imgLogo" style="height:100%" src="common/images/topFrame/accor.jpg" />--%>
          <input type="image" name="imgLogo" id="imgLogo" style="height:100%" src="common/images/topFrame/bg_log.jpg" />
      </td>
      <td height="100%" width="50%" align="right" valign="middle">
		  <img id="imgBtn_help400" src="common/images/topFrame/hotline400.png" height="35" alt="help" />
          <div id="menu1" style="position:absolute; top:40px; width:111px; height:91px; z-index:1; visibility: hidden;" onMouseOver="MM_showHideLayers('menu1','','show')"  onMouseOut="MM_showHideLayers('menu1','','hide')"> 
               <table style="border:1px solid #b2b2b2" width="126" border="0" background="common/images/topFrame/bg.jpg"  cellspacing="1" cellpadding="2">
                 <tr  align="center">
                   <td><a href="./UserFile/Doctool/ValuePlus User Guid.pdf"  target="_blank"><font color="#FFFFFF">操作手册</font></a></td>
                 </tr>
                 <tr  align="center">
                   <td><a href="./UserFile/Doctool/AttendanceTraining.pdf"  target="_blank"><font color="#FFFFFF">考勤培训文档</font></a></td>
                 </tr>
               <tr align="center">
                   <td><a href="#" onclick="javascript:doRedirect('./UserFile/Doctool/TeamViewerQS_zhCN-idcy8qb5ta.exe');"><font color="#FFFFFF">远程工具Teamview</font></a></td>
                 </tr>
                 <tr align="center">
                   <td><a href="https://compassolution.beyondtrustcloud.com" target="_blank"><font color="#FFFFFF">远程工具Bomgar</font></a></td>
                 </tr>
               </table>
          </div>
		  <img id="imgBtn_help" src="common/images/topFrame/easyhelpdesk.png" height=35 alt="help" onclick="javascript:doRedirect('./UserFile/Doctool/TeamViewerQS_zhCN-idcy8qb5ta.exe');"  onMouseOver="MM_showHideLayers('menu1','','show')"  onMouseOut="MM_showHideLayers('menu1','','hide')"/>
          <img id="imgBtn_toHome" src="common/images/topFrame/toHome.jpg"  class="imgBtn" alt="toHome" onclick="javascript:redirectHomePage();"/>
          <img id="imgBtn_changePwd" src="common/images/topFrame/changePwd.jpg" class="imgBtn" alt="change password" onclick="javascript:doRedirect('UserManager/ChangePwd.aspx');"/>
          <asp:ImageButton ID="imgBtn_exit" runat="server" ImageUrl="common/images/topFrame/exit.jpg"  CssClass="imgBtn" alt="exit"/>
          <div style="display:none">
              <asp:ImageButton ID="aDoExit" runat="server" ImageUrl="common/images/topFrame/exit.jpg"  CssClass="imgBtn" alt="exit" OnClick="imgBtn_exit_Click"/>
          </div>
          
            <%--//设置Edge兼容时需更新以下内容--%>
          <asp:DropDownList ID="DropDownList1" runat="server" Width = "70"
              onselectedindexchanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" Visible="false">
          </asp:DropDownList>
          
          <asp:ListBox ID="ListBox1" runat="server"  Width="70" Height="40" ToolTip = "Change Language" Visible ="true" style ="border:0;overflow:auto;"
            BackColor="#3d6cc1" AutoPostBack="true" onselectedindexchanged="ListBox1_SelectedIndexChanged">
	      </asp:ListBox>
            <%--//设置Edge兼容时需更新以上内容--%>
      </td>
    </tr>
</table>
<table width="100%" style="height:10px"  border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="menu">
        <div id="dirMenuArea" runat="server">
        
        </div><%--
        <a href="javascript:doRedirect('FunctionList.aspx')" style="display:inline;">系统工具</a>
        <a href="javascript:doRedirect('FunctionList.aspx')" style="display:inline;">聊天室</a>--%>
        
        <div style="text-align:right; vertical-align:middle">
            <asp:Label ID="Label1" runat="server" Text="你好：登陆用户" ForeColor="white" Font-Bold=true ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
	</td>
  </tr>
</table>

<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="left" style="width:80%; height:100%; padding:0px;">
        <iframe id="mainFrame" name="mainFrame" width="100%" height="600" frameborder="0" src="<%=strHomePage %>" style="border: 0px solid #cecece;" scrolling="auto"></iframe>
	</td>
  </tr>
  <tr>
  </tr>
</table>

<%----Start--待办事项提醒弹出框--Start--%>
<div id="divPendingAlert">
    <h1>
        <asp:LinkButton ID="aClosePendingAlert" runat="server" OnClientClick = "HidePendingAlert();" ToolTip = "Close" Font-Bold="true" Font-Size=XX-Large>×</asp:LinkButton>
        <asp:Label ID="lbPendingAlert" runat="server" Text="实时提醒" Font-Bold="true" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:CheckBox ID="cbNeverPengdingAlert" runat="server" Text="不再提醒" ForeColor="white" />
    </h1>
    <iframe id="AlertFrame" name="AlertFrame" width="100%" height="600" frameborder="0" src="Alert.aspx" style="border: 0px solid #cecece;" scrolling="auto"></iframe>
</div>

<script language="javascript" type="text/javascript">
    function getUserPengdingCount() {
        $.ajax({
            //要用post方式      
            type: "Post",
            //方法所在页面和方法名      
            url: "topFrame.aspx/GetCurUserPendingCount",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                //返回的数据用data.d获取内容  
                var jsonData = eval("(" + result.d + ")");
                if (jsonData[0].iPendingCount > 0) {
                    startPendingAlert();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }

    //定时执行当前用户的提醒信息
    var handle;
    if(document.getElementById("hfIsRealtimeAlert").value=="1") {
        var handleTimer = setInterval(function () { getUserPengdingCount(); }, document.getElementById("hfRealtimeAlertInterval").value);
    }

    function startPendingAlert() {
        var obj = document.getElementById("divPendingAlert");
        if (parseInt(obj.style.height) == 0) {
            obj.style.display = "block";
            handle = setInterval("ChangePendingAlertHeight('up')", 50);
        } 
//        else {
//            handle = setInterval("ChangePendingAlertHeight('down')", 50)
//        }
    }
    function HidePendingAlert() {
        handle = setInterval("ChangePendingAlertHeight('down')", 50);
        document.getElementById("divPendingAlert").style.display = "none";
        //如果选择拒绝提醒
        if (document.getElementById("cbNeverPengdingAlert").checked) {
            clearInterval(handleTimer);
        }
    }
    function ChangePendingAlertHeight(str) {
        var obj = document.getElementById("divPendingAlert");
        if (str == "up") {
            if (parseInt(obj.style.height) > 200)
                clearInterval(handle);
            else
                obj.style.height = (parseInt(obj.style.height) + 8).toString() + "px";
        }
        if (str == "down") {
            if (parseInt(obj.style.height) < 8) {
                clearInterval(handle);
                obj.style.display = "none";
            }
            else
                obj.style.height = (parseInt(obj.style.height) - 8).toString() + "px";
            //如果选择拒绝提醒
            if (document.getElementById("cbNeverPengdingAlert").checked) {
                clearInterval(handleTimer);
            }
        }
    }

</script>
<%----End--待办事项提醒弹出框--End--%>

<script language="javascript" type="text/javascript">
    <%--//设置Edge兼容时需更新以下内容--%>
    //function SetFrameHeight(obj) {
    //    var win = obj;
    //    if (document.getElementById) {
    //        if (win && !window.opera) {
    //            if (win.contentDocument && win.contentDocument.body.offsetHeight)
    //                win.height = win.contentDocument.body.offsetHeight;
    //            else if (win.Document && win.Document.body.scrollHeight)
    //                win.height = win.Document.body.scrollHeight;
    //        }
    //    }
    //}

    //document.getElementById("mainFrame").style.height = document.body.offsetHeight - 110 + "px";
    var screenHeight = screen.availHeight;
    $(document).ready(function () {
        $("#mainFrame").height(screenHeight - 210);
    });
    <%--//设置Edge兼容时需更新以下内容--%>
</script>

</form>
    
<!--<%--Group Function Addtional--%>-->
<script src="common/Group/jquery.cookies.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        window.attachEvent("onbeforeunload", function () {
            //alert('index.html onbeforeunload' + $.cookie('vp_isDoLogin'));
            if ($.cookie('vp_isDoLogin') == '1') {
                $.cookie('vp_isDoLogin', "0");
            }
        });
    });
</script>
<!--<%--Group Function Addtional--%>-->

</body>
</html>
