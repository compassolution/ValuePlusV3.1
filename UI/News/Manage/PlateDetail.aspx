<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlateDetail.aspx.cs" Inherits="News_Manage_PlateDetail"  EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>新闻版块菜单明细页</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" />
<link href="../../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script> 
<script language="javascript">
    var imgPath = "../../UserFile/News/PlateImg/";
    function showOpenWindow() {
        url = "SelectPlateImage.aspx";
        url = url + '?PATH=' + imgPath;
        window.open(url, 'newwindow', 'width=400,height=200,top=300,left=400, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
    }
    
    function zoomin()
	{
	    document.getElementById("imgPlate").height = 136;
	    document.getElementById("imgPlate").width = 100;
	}
	function zoomout()
	{
	    document.getElementById("imgPlate").height = 16;
	    document.getElementById("imgPlate").width = 16;
	}
	
	function selectParentMenu()
	{
	    var select1 = document.getElementById("ddListParent"); 
        var select1value  = select1.options[select1.selectedIndex].value; 
        var strArr = select1value.split('*');
        var strLevel = strArr[1];
        var iLevel = parseInt(strLevel)+1;
        document.getElementById("txtLevel").value = iLevel;
	}
	
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"-->
    <form id="form1" runat="server">
<asp:HiddenField ID = "hfImagePath" runat="server" />
    <div>
        <table border="0" class="warp_table" width="95%" id="tb1" align="center" >
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		    <td class="left_bt2" style="color:Blue" colspan="2" align=center>
		        <asp:Label ID="Label1" runat="server" Text="新闻版块明细信息"></asp:Label>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label3" runat="server" Text="新闻版块编码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtCode" runat="server" Width="90%" MaxLength="20"></asp:TextBox><font color="red">*</font>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label4" runat="server" Text="新闻版块英文名"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="txtName" runat="server" Width="90%" MaxLength="50"></asp:TextBox><font color="red">*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label5" runat="server" Text="新闻版块中文名"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="txtNameChs" runat="server" Width="90%" MaxLength="25"></asp:TextBox><font color="red">*</font></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label2" runat="server" Text="版块英文描述"></asp:Label>    
		    </td>
		    <td><asp:TextBox ID="txtDesc" runat="server" Width="90%" MaxLength="50"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="版块中文描述"></asp:Label>
		    </td>
		    <td><asp:TextBox ID="txtDescChs" runat="server" Width="90%" MaxLength="25"></asp:TextBox></td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label8" runat="server" Text="显示图片"></asp:Label>
		    </td>
		    <td>
		        <img runat="server"  ID="imgPlate" src="" onmouseover="zoomin();" onmouseout="zoomout();"/>
		        <asp:TextBox ID="txtImage" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
		        <asp:imagebutton runat="server" ImageUrl="../../common/images/icon/selectImage.gif" style= "cursor:hand "
                                    CausesValidation="false" ID="ImageButton1" OnClientClick = "javascript:showOpenWindow();"/></asp:imagebutton>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label9" runat="server" Text="父新闻版块"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="ddListParent" runat="server" Width="400px" onchange="JavaScript:selectParentMenu();">
				</asp:DropDownList>		        
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label10" runat="server" Text="新闻版块级别"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtLevel" runat="server" Width="90%" MaxLength="20">1</asp:TextBox>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label11" runat="server" Text="显示顺序" ></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtOrder" runat="server" Width="10%" MaxLength="18"></asp:TextBox><font color="red">*</font>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label12" runat="server" Text="是否停用"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="ddListIsStop" runat="server" Width="200px">
					<asp:ListItem Value="0">正常</asp:ListItem>
					<asp:ListItem Value="1">停用</asp:ListItem>
				</asp:DropDownList>
		    </td>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
		  </tr>
		  <tr height="30" align="center">
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
            <TD align ="center" colspan="2">
                  <asp:LinkButton ID="btnEdit" runat="server" CssClass="a_Center" OnClick="btnEdit_Click" >保    存</asp:LinkButton>
                  <asp:LinkButton ID="btnReturn" runat="server" CssClass="a_Center" OnClick="btnReturn_Click" >返回列表</asp:LinkButton>
            </TD>
            <td background="../../common/images/welcome/mail_rightbg.gif">&nbsp;</td>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>

<script language="javascript">
    document.getElementById("hfImagePath").value = imgPath;
</script>
    