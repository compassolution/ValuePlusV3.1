<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectPlateImage.aspx.cs" Inherits="News_Manage_SelectPlateImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
<HEAD id="HEAD1" runat="server">
<title>Select Images</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script>
    function ImageDbClick(path,filename){
        window.opener.document.getElementById("txtImage").value = filename;
        window.opener.document.getElementById("imgPlate").src = path + "" + filename;
    }
</script>
</HEAD>
<body>
	<form id="Form1" method="post" runat="server">
	    <table border="0" class="warp_table" width="95%" id="tb1" align="center" >
	      <tr>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
	        <td class="left_bt2" style="color:Blue" colspan="2">
	            <asp:Label ID="Label1" runat="server" Text="请双击选择图片"></asp:Label>
            </td>
            <td background="../../common/images/welcome/mail_rightbg.gif" style="width:1px">&nbsp;</td>
	      </tr>
	    </table>
	</form>
</body>
</HTML>
