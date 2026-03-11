
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownFileList.aspx.cs" Inherits="UpDownLoad_DownFileList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>DownLoad Files</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/tableStyle.js"></script>
<script src="../common/js/MainUtil.js" type="text/javascript"></script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="from1post" name="from1post" method="post" runat="server">

<table id="changecolor" class="table">
	<tr align="right">
		<td align="center" id="tdTitle" runat="server" class="td_Frame1" colspan="8">
            <asp:Label ID="lbTitle" runat="server" Text="Label"></asp:Label>
        </td>
	</tr>
    <div id="divFileListArea" runat="server">
    
    </div>

    
</table>

<asp:HiddenField ID="hf_strSureDelete" runat="server" />
<asp:HiddenField ID="hf_strDeleteSuccess" runat="server" />
<asp:HiddenField ID="hf_strLbFile" runat="server" />
</form>
  <iframe id="iddownframe"  name="iddownframe"  style="width:0px;height:0px;display:none;"></iframe>   
    <script language="javascript" type="text/javascript">
    function downthisfile(path,filename){    
        var sPath = 'DownLoadFile.aspx?path=' + path + '&fileName=' + filename + '&optype=down&ran=' + Math.random();
        sPath = sPath + "&folder=<%=this.strFileFolder%>&token=<%=this.strToken%>";
        from1post.action = sPath;
        from1post.target = "iddownframe";
        from1post.submit(); 
    }
    function deletethisfile(path,filename){
        if (confirm(document.getElementById("hf_strSureDelete").value)) { 
            var sPath = 'DownLoadFile.aspx?path=' + path + '&fileName=' + filename + '&optype=del&ran=' + Math.random();
            sPath = sPath + "&folder=<%=this.strFileFolder%>&token=<%=this.strToken%>";
            from1post.action = sPath;
            from1post.target = "iddownframe";
            from1post.submit();
            alert(document.getElementById("hf_strDeleteSuccess").value + '（' + document.getElementById("hf_strLbFile").value + decodeURI(filename) + ')');
            window.location.reload();
        }
    }
    </script>
   
</body>
</html>

<script language="javascript">
    //兼容Edge时跳转到新页面
<%--    if (myBrowser() != 'IE') {
        var edgeToUrl = "FileInput.aspx?folder=<%=strFileFolder %>&edit=<%=strIsEdit %>";
        window.location.href = edgeToUrl;
    }--%>
	//初始化结果表格
	DefineNoTitleTableCss("changecolor");
</script>
