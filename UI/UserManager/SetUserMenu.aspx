<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetUserMenu.aspx.cs" Inherits="UserManager_SetUserMenu" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<title>栏目树授权页面</title>
<link rel="STYLESHEET" type="text/css" href="../common/codebase/dhtmlxtree.css">
<link rel="STYLESHEET" type="text/css" href="../common/codebase/dhtmlxtabbar.css">

<script language="javascript"  src="../common/codebase/dhtmlxcommon.js"></script>
<script language="javascript"  src="../common/codebase/dhtmlxtabbar.js"></script>
<script language="javascript"  src="../common/codebase/dhtmlxtree.js"></script>
<script language="javascript"  src="../common/codebase/dhtmlxtabbar_start.js"></script>

<script language="javascript"  src="../common/js/biuldTree.js"></script>
<script  src="../common/js/waitProcess.js"></script>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 

</head>
<body onLoad="loadTree()" style="padding: 0; margin: 0;overflow:hidden;height:100%;">
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfUserId" runat ="server" />
<asp:HiddenField ID="hfMenuCodeArr" runat ="server" />
    <div>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;height:100%">
        <tr><td>
            <asp:Label ID="Label1" runat="server" Text="UserId:" class="left_bt2" style="color:red"></asp:Label>
            <asp:Label ID="lbUserId" runat="server" Text="UserId" class="left_bt2" style="color:red"></asp:Label>
        </td></tr>
        <tr style="height:500px">
	        <td width="100%"  colspan="3">
		        <table width="100%"  cellpadding="0" cellspacing="0" border="0">
			        <tr style="width:250" >
			            <td valign="top" style="width:80%">
				            <div runat=server id="treebox_menu" style="width:100%; height:490px;background-color:#ccffff; border :1px solid Silver; overflow:auto;"></div>
			            </td>
			            <td style="width:2%"></td>
			            <td valign="top" style="width:18%" align=center>
                            <asp:Button ID="Button1" runat="server" Text="授    权" height="25px" Width = "100px" CssClass="btn_2k3" OnClick="Button1_Click" />
                            </br></br>
                            <asp:Button ID="Button4" runat="server" Text="Copy" height="25px" Width = "100px" CssClass="btn_2k3" OnClientClick="openCopyPage()" />
                            </br></br></br></br></br></br></br></br></br></br>
                            <asp:Button ID="Button2" runat="server" Text="全    选" height="25px" Width = "100px" CssClass="btn_2k3" OnClientClick="selCheckAll();" />
                            </br></br>
                            <asp:Button ID="Button3" runat="server" Text="全 不 选" height="25px" Width = "100px" CssClass="btn_2k3" OnClientClick="unSelCheckAll();" />
			            </td>
			        </tr>
		        </table>
	        </td>
        </tr>
    </table>
    </div>
    </form>
 <script language="javascript" >
	
	/* init tree */
	var tree_menu;
	var tree_smpl
	function loadTree(){
		tree_menu=new dhtmlXTreeObject("treebox_menu","100%","100%",0);
		tree_menu.setImagePath("../common/codebase/imgs/csh_bluebooks/");
		tree_menu.enableCheckBoxes(1);
		tree_menu.enableThreeStateCheckboxes(true);
		tree_menu.loadXML("../HandleData/SetUserMenuTreeHandle.ashx?nu=1&userId="+document.getElementById("hfUserId").value,autoselectNode);
	}
	
	function getMenuArr(){
	    document.getElementById( "hfMenuCodeArr").value = tree_menu.getAllCheckedBranches();
	}
	
	function selCheckAll(){
	    tree_menu.setCheck(tree_menu.getSelectedItemId(),true);
	}
	function unSelCheckAll(){
	    tree_menu.setCheck(tree_menu.getSelectedItemId(),false);
	}
	function openCopyPage() {
	    var url = 'CopyUserRole.aspx?userId=' + document.getElementById("hfUserId").value + '&opType=1'
	    window.open(url, 'copyUser', 'width=520,height=500,top=100,left=350, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
	}
</script>
</body>
</html>
