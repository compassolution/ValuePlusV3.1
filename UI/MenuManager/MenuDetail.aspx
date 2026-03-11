<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuDetail.aspx.cs" Inherits="MenuManager_MenuDetail" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>栏目菜单明细页</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" />
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script> 
<script language="javascript">
    function showOpenWindow(url)
    {
        verPatch = "../common/images/fuctionIcon/";
        url = url + '?PATH='+verPatch;
        window.open(url, 'newwindow', 'width=800,height=500,top=200,left=300, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }
    
    function zoomin()
	{
		document.getElementById("Image1").height=68;
		document.getElementById("Image1").width=50;
	}
	function zoomout()
	{
		document.getElementById("Image1").height=16;
		document.getElementById("Image1").width=16;
	}
	
	function selectMenuType()
	{
	    var select1 = document.getElementById("DropDownList1") ; 
        var select1value  = select1.options[select1.selectedIndex].value;
        if(select1value=='D'){//单据
            document.getElementById("TextBox4").value= "Archive/Archive.aspx?DOCU=";
        } else if (select1value == 'T') {//树形页面模板
            document.getElementById("TextBox4").value = "Tree/TreeIndex.aspx?TREE=&OPTYPE=ADD";
        } else if (select1value == 'R') {//报表
            document.getElementById("TextBox4").value= "Report/ReportMain.aspx?RPT=";
        }else if(select1value=='Q'){//普通查询
            document.getElementById("TextBox4").value= "Query/QueryMain.aspx?QRY=";
        }else if(select1value=='S'){//存储查询
            document.getElementById("TextBox4").value= "Query/SPQuery.aspx?SP=";
        }else if(select1value=='U'){//文件上传
            document.getElementById("TextBox4").value= "UpDownLoad/UploadIndex.aspx?folder=";
        }else{
            document.getElementById("TextBox4").value= "";
        }
	}
	function selectParentMenu()
	{
	    var select1 = document.getElementById("DropDownList3") ; 
        var select1value  = select1.options[select1.selectedIndex].value; 
        var strArr = select1value.split('*');
        var strLevel = strArr[1];
        var strOrder = strArr[2];
        var iLevel = parseInt(strLevel)+1;
        document.getElementById("TextBox6").value= iLevel;
        //如果选择根目录，则将TextBox7置为空
        if(iLevel>1){
            document.getElementById("TextBox7").value= strOrder;
        }else{
            document.getElementById("TextBox7").value= "";
        }

	}
	
</script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
    <div>
        <table border="0" class="warp_table" width="95%" id="tb1" align="center" >
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label1" runat="server" Text="栏目明细信息"></asp:Label>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="栏目编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox1" runat="server" Width="90%" MaxLength="20" ReadOnly=true></asp:TextBox><font color="red">*</font>
            </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="栏目英文名"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="TextBox2" runat="server" Width="90%" MaxLength="50"></asp:TextBox><font color="red">*</font></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="栏目中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox3" runat="server" Width="90%" MaxLength="25"></asp:TextBox><font color="red">*</font></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label7" runat="server" Text="栏目类型"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList1" runat="server" Width="200px" onchange="JavaScript:selectMenuType();">
					<asp:ListItem Value="F">Folder</asp:ListItem>
					<asp:ListItem Value="P">Page</asp:ListItem>
					<asp:ListItem Value="D">Document</asp:ListItem>
					<asp:ListItem Value="T">Tree</asp:ListItem>
					<asp:ListItem Value="R">Report</asp:ListItem>
					<asp:ListItem Value="Q">Query</asp:ListItem>
					<asp:ListItem Value="S">SP Query</asp:ListItem>
					<asp:ListItem Value="E">Explorer</asp:ListItem>
					<asp:ListItem Value="C">Client Report</asp:ListItem>
					<asp:ListItem Value="U">Upload</asp:ListItem>
                    <asp:ListItem Value="A">Application</asp:ListItem>
                    <asp:ListItem Value="L">Line feed</asp:ListItem><%--换行 add by sammen 20230211--%>
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="栏目链接地址"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="TextBox4" runat="server" Width="90%" MaxLength="200"></asp:TextBox></td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label8" runat="server" Text="显示图片"></asp:Label>
		    </td>
		    <td>
		        <img runat="server"  ID="Image1" src="" onmouseover="zoomin();" onmouseout="zoomout();" />
		        <asp:TextBox ID="TextBox5" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
		        <asp:imagebutton runat="server" ImageUrl="../common/images/icon/selectImage.gif" style= "cursor:hand "
                                    CausesValidation="false" ID="ImageButton1" OnClientClick = "javascript:showOpenWindow('SelectImage.aspx');"/></asp:imagebutton>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label9" runat="server" Text="父栏目"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList3" runat="server" Width="400px" onchange="JavaScript:selectParentMenu();">
				</asp:DropDownList>		        
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label10" runat="server" Text="栏目级别"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox6" runat="server" Width="90%" MaxLength="20">1</asp:TextBox>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label11" runat="server" Text="显示顺序" ></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox7" runat="server" Width="10%" MaxLength="18"></asp:TextBox>+
		        <asp:TextBox ID="TextBox8" runat="server" Width="75%" MaxLength="2"></asp:TextBox><font color="red">*</font>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label2" runat="server" Text="打开位置"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="ddList_ShowLocation" runat="server" Width="200px">
					<asp:ListItem Value="001">主窗口</asp:ListItem>
					<asp:ListItem Value="002">弹出窗口</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label12" runat="server" Text="是否停用"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList2" runat="server" Width="200px">
					<asp:ListItem Value="0">正常</asp:ListItem>
					<asp:ListItem Value="1">停用</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr height="30" align="center">
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
                  <asp:LinkButton ID="Button1" runat="server" CssClass="a_Center" OnClick="Button1_Click" >修    改</asp:LinkButton>
                  <asp:LinkButton ID="Button2" runat="server" CssClass="a_Center" OnClick="Button2_Click" >新    增</asp:LinkButton>
                  <asp:LinkButton ID="Button4" runat="server" CssClass="a_Center" OnClick="Button4_Click" >删    除</asp:LinkButton>
                  <asp:LinkButton ID="Button3" runat="server" CssClass="a_Center" OnClick="Button3_Click" >返回列表</asp:LinkButton>
            </TD>
            <td background="../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
