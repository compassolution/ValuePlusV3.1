<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArchiveAttachment.aspx.cs" Inherits="Archive_Detail_ArchiveAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模板附件页面</title>
<meta http-equiv="X-UA-Compatible" content="IE=edge,Chrome=1" />
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<link rel="stylesheet" type="text/css" href="../../common/jqueryBoxImg/boxImg.css" />
<script src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../../common/jqueryBoxImg/boxImg.js" type="text/javascript"></script>


<script type="text/javascript">

    window.focus();
    
//    function ShowFile(url){
//        window.open(url, 'AttachFliePage', 'width=600,height=500,top=50,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=auto,location=no, status=no');
//    }

    function DeleteFile(fileName) {
        if (confirm(document.getElementById("hf_strSureDelete").value)) {
            document.getElementById("hfDeleteFile").value = fileName;
            document.getElementById("btnDelete").click();
        }
    }

    function ShowFile(object, HomeDir, fileName, FileList) {
        var myrow = object.parentElement.parentElement;

        //var fileName = myrow.innerText;
        //if (fileName == "undefined") {
        //    fileName = myrow.firstChild.innerText;
        //}
        ////去掉前后空格
        //fileName = fileName.replace(/(^\s*)|(\s*$)/g, "");
        //获取当前文件扩展名
        var extName = fileName.substring(fileName.lastIndexOf('.') + 1, fileName.length);
        //console.log('extName:', extName);
        //图片类型文件扩展名数组
        var imageFileType = "jpg;png;gif;bmp;jpeg;"
        ////根据扩展名判断是否为要求的图片
        if (imageFileType.indexOf(extName + ";") > -1) {
            //如果是图片类型则进行预览
            ReviewImageFiles(HomeDir, FileList, imageFileType);
        } else {
            $("#divImagePreview").hide();
            window.open(HomeDir + fileName, 'AttachFliePage', 'width=600,height=500,top=50,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no', false);
        }
        return false;
    }

    //add by sammen 20220208增加图片格式的预览放大旋转功能
    function ReviewImageFiles(HomeDir, FileList, imageFileType) {
        //console.log('HomeDir:', HomeDir);
        var arrayFileList = FileList.split(';');
        if (arrayFileList.length > 0) {
            $("#divImagePreview").show();
            $("#divImageList").empty();
            for (i = 0; i < arrayFileList.length; i++) {
                //console.log('FileList:', arrayFileList[i]);
                var fileName = arrayFileList[i];
                var extName = fileName.substring(fileName.lastIndexOf('.') + 1, fileName.length).toLowerCase();
                if (imageFileType.indexOf(extName + ";") > -1) {
                    var filePathAndName = HomeDir + fileName;
                    var tempHTML = "";
                    tempHTML = tempHTML + "<div style=\"display: inline-block;padding:5px;\">";
                    tempHTML = tempHTML + "<div><img modal=\"zoomImg\" src=\"" + filePathAndName + "\" height=\"150px\" fileName=\"" + fileName + "\" alt=\"\" /></div>";
                    tempHTML = tempHTML + "<div style=\"text-align: center\">" + fileName + "</div>";
                    tempHTML = tempHTML + "</div>";
                    $("#divImageList").append(tempHTML);
                }
            }
            //console.log('$("#divImageList").html():', $("#divImageList").html());
            InitImageZoom();
        }

        return false;
    }
	
</script>
</head>
<body>
 <!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfDeleteFile" runat="server" />
<asp:HiddenField ID="hf_strSureDelete" runat="server" />
<asp:HiddenField ID="hf_strDeleteSuccess" runat="server" />
<asp:HiddenField ID="hf_strLbFile" runat="server" />
    <table width="80%" border="0" cellpadding="0" cellspacing="0" style="text-align:center" class="table">
          <!-- 操作行 -->
          <tr height="30">
            <td align ="center" valign="middle">
                <div class="topBox" style="vertical-align:middle; float:left; margin:auto">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:LinkButton ID="btnOK" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="OK_Click">上    传</asp:LinkButton>
                    <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:window.close();">关   闭</asp:LinkButton>
                </div>
                <div  style="display:none">
                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="Delete_Click">删除</asp:LinkButton>
                </div>
            </td>
          </tr>
          <tr align="center">
            <td align="center">
                <table  class="table" style="text-align:center" style="width:80%" >
                  <!-- 结果列表部分 --> 
                  <div id="divFileList" runat="server"></div>
                </table>
            </td>
          </tr>
          
    </table>
</form>

<div id ="divImagePreview" style="padding:15px;margin-top:10px;overflow:auto;display:none">
    <div style="font-size:x-large;padding-bottom:10px">
        图片格式文件预览:<span style="font-size:small">（如需下载请点击右键另存）</span>
    </div>
    <div id ="divImageList">
        <%--<div style="display:inline-block;">
            <div><img modal="zoomImg" src="../../UserFile/ArchiveAtt/FLLVAttachFile/FLLV-1/LV20220100282/QQ20220208160114.jpg" height="150px" alt="" /></div>
            <div style="text-align:center">QQ20220208160114.jpg</div>
        </div>--%>
    </div>
</div>


<script type="text/javascript">
    $(document).ready(function () {
        //InitImageZoom();
    })
</script>

</body>
</html>