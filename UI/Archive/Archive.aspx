<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Archive.aspx.cs" Inherits="Archive_Archive" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>档案首页</title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<script  src="../common/js/waitProcess.js"></script>
<script type="text/javascript" src="../common/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript">
$(document).ready(
    function()
	{
		$(".content").hide();
		$('#divRoleArea li').hover(
		    
			function()
			{
			    var curRoleLi = $(this);
				$(this).addClass("cli");
				if($(this).index()!=0) $("#liRole1").removeClass("cli");
				$("#idSceneDiv"+($(this).index()+1)).show();
				$("#idSceneDiv"+($(this).index()+1)).hover(
					function()
					{
						$(this).show();
						curRoleLi.addClass("cli");
					},
					function()
					{
						$(this).hide();
						curRoleLi.removeClass("cli");
					}
				);
			},
			function()
			{
			    var curRoleLi = $(this);
				$(this).removeClass("cli");
				$("#idSceneDiv"+($(this).index()+1)).hide();
				$("#idSceneDiv"+($(this).index()+1)).hover(
					function()
					{
						$(this).show();
						curRoleLi.addClass("cli");
					},
					function()
					{
						$(this).hide();
						curRoleLi.removeClass("cli");
					}
				);
			}
		);
		
		$("#idSceneDiv1").show();
		$("#liRole1").addClass("cli");
		
	}
)
</script>
<SCRIPT language="javascript">
	
	function toArchiveListPage(strParamString)
	{
	    document.getElementById("divSceneArea").style.display = "none";
	    if((window.parent!=null)&&(window.parent.frames.contentFrame!=null)){
	        window.parent.frames.contentFrame.location.href = "ArchiveMain.aspx?"+strParamString;
	    }else{
	        window.location.href = "ArchiveMain.aspx?"+strParamString;
	    }
	}
</SCRIPT>
<style type="text/css">
    ul,li{padding:0;margin:0;}
    li{list-style:none;}
    #page{width:1000px;height:500px;margin:1px auto 0 auto;}
    #header{width:90%;border-bottom:3px solid #3970CE;}
    #center{width:90%; height:490px;overflow:hidden;position:relative;border-left:1px solid #CAE1FF;border-bottom:1px solid #CAE1FF;border-right:1px solid #CAE1FF;}
    #divRoleArea{position:relative;width:200px;height:480px;border:2px solid #CAE1FF;border-right:2px solid #CAE1FF;float:left;}
    #divRoleArea li{height:30px;line-height:30px;text-indent:30px;border-bottom:1px solid #CAE1FF; cursor:hand;}
    .cli{background:#C2DAF1;width:212px; font-weight:bold;}
    .content{position:absolute;width:760px;height:480px;border:1px solid #CAE1FF;margin:0 0 0 5px;z-index:999;background:#C2DAF1;left:200px;}
    .post{margin:5px;width:680px;height:470px;background:#FFF;}
    #divDefault{width:680px;height:322px;float:left;margin:8px 0 0 10px;z-index:-1; font-weight:bold;}
</style>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server" >
 
<div id="page">
	<div id="header"></div>
	<div id="center">
		<ul>
		    <div id="divRoleArea" runat="server">
		    </div>
		</ul>
		<div id="divDefault" style="vertical-align:middle; text-align:center">
            <asp:Label ID="Label1" runat="server" Text="Label">温馨提示：请移动鼠标至左边角色列表区域查看角色对应场景列表！</asp:Label></div>
        <div id="divSceneArea" runat="server">
        </div>
	</div>	
</div>

</form>

</body>
</html>
