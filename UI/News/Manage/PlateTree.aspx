<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlateTree.aspx.cs" Inherits="News_Manage_PlateTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html style="height:100%">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<title>新闻版块管理页面</title>
<link rel="STYLESHEET" type="text/css" href="../../common/codebase/dhtmlxtree.css">
<link rel="STYLESHEET" type="text/css" href="../../common/codebase/dhtmlxtabbar.css">

<script language="javascript"  src="../../common/codebase/dhtmlxcommon.js"></script>
<script language="javascript"  src="../../common/codebase/dhtmlxtabbar.js"></script>
<script language="javascript"  src="../../common/codebase/dhtmlxtree.js"></script>
<script language="javascript"  src="../../common/codebase/dhtmlxtabbar_start.js"></script>

<script language="javascript"  src="../../common/js/biuldTree.js"></script>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>

</head>

<body onLoad="loadTree()" style="padding: 0; margin: 0;overflow:hidden;height:100%;">
<!--#include   file= "../../common/WaitProccess.htm"-->
	
<!--- <a href="javascript:void(0)" onclick="updateTreeSize()">update Size</a> --->
<form id="form1" runat="server">
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;">
<tr>
	<td width="100%" height="100%" colspan="3">
		<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
		    <tr>
                <td valign="top" style="width:350px">
                    <div id="a_tabbar" 
						class="dhtmlxTabBar"
						style="width: 100%; height: 100%; border:0px solid;overflow:auto;
						tabstyle="scbr"
						tabheight="0"
						imgpath="../../common/codebase/imgs/" 
						mode="left"
						offset="3"
						oninit="a_tabbar.setTabActive(type01+'tab')"
						select="doctree_box">
	                            <div runat="server" id="doctree_box" style="width:100%; height:520px;background-color:#CAE1FF; border :1px solid Silver; overflow:auto;"></div>
	                </div>
                </td>
		        <td valign="top" style="height:auto" align="center">
				    <iframe id="contentFrame" name="contentFrame" frameborder="0" src="NewsList.aspx" style="height:520px;width:100%;border: 0px solid #cecece;"></iframe>
			    </td>
			</tr>
		</table>
	</td>
</tr>
</table>
</form>
</body>
<script language="javascript" >
	
	/* init tree */
	var tree_menu;
	var tree_smpl
	function loadTree(){
		tree_menu=new dhtmlXTreeObject("doctree_box","100%","100%",0);
		tree_menu.setImagePath("../../common/images/treeIcon/");
		tree_menu.setOnClickHandler(function(id){openPathDocs(id);});
		tree_menu.attachEvent("onOpenEnd",updateTreeSize);
		tree_menu.loadXML("NewsPlateTreeHandler.ashx?nu=1", autoselectNode);
	}	
 
</script>
</html>