
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreeIndex.aspx.cs" Inherits="Tree_TreeIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="pageTitle">树形信息数据首页</title>
<link rel="STYLESHEET" type="text/css" href="../common/codebase/dhtmlxtree.css"/>
<link rel="STYLESHEET" type="text/css" href="../common/codebase/dhtmlxtabbar.css"/>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 

<script type="text/javascript" src="../common/JQuery/jquery-1.10.2.js"></script>
<script type="text/javascript" src="../common/JS/Resize.js"></script>
<script type="text/javascript" src="../common/codebase/dhtmlxcommon.js"></script>
<script type="text/javascript" src="../common/codebase/dhtmlxtabbar.js"></script>
<script type="text/javascript" src="../common/codebase/dhtmlxtree.js"></script>
<script type="text/javascript" src="../common/codebase/dhtmlxtabbar_start.js"></script>

<script type="text/javascript" src="../common/js/biuldTree.js"></script>

</head>

<body onload="loadTree()" style="padding: 0; margin: 0;overflow:hidden;">
<form id="form1" runat="server">
<asp:HiddenField ID="hfFieldTreeCode" runat="server" />
<asp:HiddenField ID="hfOpType" runat="server" />
<table id="tableMain" width="100%" cellpadding="0" cellspacing="0" border="0">
<tr>
	<td width="100%" colspan="3">
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr id= "trTop" style="display:none">
                <td colspan = "3"> 
                    <asp:LinkButton ID="aRefreshTreeData" runat="server" CssClass="a_Center" OnClick = "aRefreshTreeData_Click" Text = "重新加载数据"></asp:LinkButton>

                    <div style="width:100%; text-align:right; height:80px;background-color:#CAE1FF; border :1px solid Silver; overflow:auto;">
                        <table style="width:100%">
                            <tr>
                                <td class="edit_label" align = "center" style="width:30%">
                                    <asp:Label ID="Label3" runat="server" Text="Code/编码"></asp:Label>
                                </td>
                                <td style="width:50%" align="left">
	                                <asp:Label ID="Label_Code" runat="server" Text="Label"></asp:Label>
                                </td>
                                <td style="width:20%" align="right"> 
                                    <a id="aExpandAll" href="javascript:ExpandAll();" class="a_Left">
                                        <img id="imgExpand" src="../common/codebase/imgs/dhtmlxgantt_icon.gif" border="0" height="15" width="15" alt="Expand/Close" />
                                    </a>
                                    &nbsp;
                                    <asp:LinkButton ID="aReload" runat="server" CssClass="a_Left" OnClientClick="loadTree()">
                                        <img id="imgLoad" src="../common/images/icon/refresh.gif" border="0"  height="15" width="15" alt="Reload"/>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="edit_label" align = "center" style="width:30%">
                                    <asp:Label ID="Label1" runat="server" Text="Chinese/中文名称"></asp:Label>
                                </td>
                                <td align="left" colspan="2">
	                                <asp:Label ID="Label_NameCn" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="edit_label" align = "center"  style="width:30%">
                                    <asp:Label ID="Label4" runat="server" Text="English/英文名称"></asp:Label>
                                </td>
                                <td align="left" colspan="2">
	                                <asp:Label ID="Label_NameEn" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
		    <tr id="trContent">
                <td valign="top" style="width:25%" id="frmTitle">
                    <div id="a_tabbar" 
							tabstyle="scbr"
							tabheight="0"
							imgpath="common/codebase/imgs/" 
							style="width: 100%; height: 100%; border:0px solid;scrollbar-face-color: #FFFFFF; scrollbar-shadow-color: #D2E5F4; 
                                    scrollbar-highlight-color: #D2E5F4; scrollbar-3dlight-color: #FFFFFF; scrollbar-darkshadow-color: #FFFFFF; 
                                    scrollbar-track-color: #FFFFFF; scrollbar-arrow-color: #D2E5F4"
							skinColors="#FFFFFF,#F4F3EE"
							mode="left"
							offset="3"
							oninit="a_tabbar.setTabActive(type01+'tab')"
							select="doctree_box">
                        
							<div id="doctree_box" style="width: 100%; height: 100%;"></div>		
	                </div>                
                </td>
				<td id="tdSwitchPoint" class="navTd" onclick="switchSysBar()">
					<SPAN class="navPoint" id="switchPoint" title="open/close"><img src="../common/images/fold.gif" name="img1" id=img1>
					</SPAN>
				</td>    
		        <td valign="top" style="height:100%;" align="center">
				    <iframe id="contentFrame" name="contentFrame" frameborder="0" src="TreeList.aspx" style="width:100%;border: 0px solid #cecece;" runat="server" scrolling="auto" ></iframe>
			    </td>
			</tr>
		</table>
	</td>
</tr>
</table>
</form>
</body>
<script type="text/javascript">

    /* init tree */
    var varFlag = "closed";
	var tree_menu;
	var tree_smpl
	function loadTree(){
	    tid = document.getElementById("hfFieldTreeCode").value;
	    optype = document.getElementById("hfOpType").value;
		tree_menu=new dhtmlXTreeObject("doctree_box","","",0);
		tree_menu.setImagePath("../common/images/treeIcon/");
		tree_menu.setOnClickHandler(function(id){openPathDocs(id);});
		tree_menu.attachEvent("onOpenEnd",updateTreeSize);
		tree_menu.loadXML("TreeDataHandle.ashx?TID=" + tid + "&OPTYPE=" + optype, autoselectNode);
	}

	function ExpandAll() {
	    if (varFlag == "closed") {
	        tree_menu.openAllItems(0);
	        varFlag = "opened";
	    } else {
	        tree_menu.closeAllItems(0);
	        varFlag = "closed";
	    }
	}
	function CloseAll() {
	    tree_menu.closeAllItems(0);
	}


	function switchSysBar() {
	    if (document.all("frmTitle").style.display == "") {
	        document.all("img1").src = "../common/images/unfold.gif";
	        document.all("frmTitle").style.display = "none";
	    }
	    else {
	        document.all("img1").src = "../common/images/fold.gif";
	        document.all("frmTitle").style.display = "";
	    }
	} 

</script>
<script language="javascript">

//    function reinitIframe() {//设置iframe的自适应高度
//        var iframe = document.getElementById("contentFrame");
//        try {
////            alert(document.documentElement.clientHeight);
////            alert(document.all["trTop"].offsetHeight);
//            var bHeight = document.documentElement.clientHeight;//可见区域高度
//            var dHeight = document.all["trTop"].offsetHeight;

//            iframe.style.height = bHeight - dHeight;
//        } catch (ex) { }
//    }
//    reinitIframe();
</script>

<script type="text/javascript">
    var screenHeight = screen.availHeight;
    var parentFrameHeight = $("#frame_MainPage", window.parent.document).height();
    if (parentFrameHeight == null) {
        parentFrameHeight = screenHeight-160
    }
    var setHeight = parentFrameHeight
	$(document).ready(function () {
        //$("#trContent").height(screenHeight + 400);
        //$("#contentFrame").height(screenHeight + 40);
        $("#doctree_box").resize(function () {
            var treeHeight = $("#doctree_box").height();
            //alert(parentFrameHeight + '-->'+treeHeight);

            if (setHeight < treeHeight) { setHeight = treeHeight }

            $("#frame_MainPage", window.parent.document).height(setHeight);
            $("#trContent").height(setHeight);
            $("#contentFrame").height(setHeight);
        })
    });
</script>

</html>