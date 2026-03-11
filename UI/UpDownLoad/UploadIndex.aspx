
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadIndex.aspx.cs" Inherits="UpDownLoad_UploadIndex" %>
<%@ Register TagPrefix="CuteEditor" Namespace="CuteEditor" Assembly="CuteEditor" %>
<!DOCTYPE   HTML   PUBLIC  "-//W3C//DTD   HTML   4.0   Transitional//EN"   >
<html>
<head>
<title>Upload Files</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js"></script>
<script src="../common/js/MainUtil.js" type="text/javascript"></script>
<script>
    function redirectFrame(folder) {
        document.getElementById("fileListFrame").src = "DownFileList.aspx?folder=" + folder + "&edit=1";
    }
</script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
<form id="Form1" method="post" runat="server">
<asp:HiddenField ID="hfPleaseSelectFile" runat="server" />
<asp:HiddenField ID="hfFileMaxSize" runat="server" />
    <table width="95%" border="0" cellpadding="0" cellspacing="0" class="table" align="center" style=" height:100%">
        <tr align="left">
		    <td align="left" id="tdTitle" runat="server" class="td_Frame1" colspan="8">
                <asp:Label ID="lbTitle" runat="server" Text=""></asp:Label>
                <asp:LinkButton ID="btnQueryAssetsImageName" CssClass="a_Left" runat="server" Font-Bold="true" OnClientClick="javascript:QueryAssetsImageName('AM');return false;">查看上传规则</asp:LinkButton>
                <asp:LinkButton ID="btnInitAssetsImageData" CssClass="a_Left" runat="server" Font-Bold="true" OnClick="InitAssetsImageData_Click">批量初始化图片数据</asp:LinkButton>
                <asp:LinkButton ID="btnQueryOEImageName" CssClass="a_Left" runat="server" Font-Bold="true" OnClientClick="javascript:QueryAssetsImageName('OE');return false;">查看上传规则</asp:LinkButton>
                <asp:LinkButton ID="btnInitOEImageData" CssClass="a_Left" runat="server" Font-Bold="true" OnClick="InitOEImageData_Click">批量初始化图片数据</asp:LinkButton>
                <a href="../UserFile/Doctool/Silverlight_x64.exe" target="_blank" class="a_Right"><font color="gray">下载上传插件</font></a>
            </td>
        </tr>
        <tr align="center" style="width:80%;height:48%;background-color:#C2DAF1;">
            <td style="width:45%;height:98%"  valign="top" align="center">
                <%-- UploadType
                Flash--需要安装插件Flash，但是有些集团屏蔽了Flash
                Silverlight--需要安装插件Silverlight
                IFrame--无需插件但只能单个上传
                --%>
                <CuteEditor:UploadAttachments runat="server" UploadType="Silverlight" ManualStartUpload="true" ID="Uploader1"  AutoUseSystemTempFolder="true" 
                    InsertText="选择上传文件 " InsertButtonStyle-CssClass ="btn_rb1"
                    CancelAllMsg="取消所有的上传" 
                    CancelText="取消"  CancelButtonStyle-CssClass="btn_rb1"
                    CancelUploadMsg="取消上传" FileTooLargeMsg="{0} 不能被上传!文件大小 ({1}) 太大.可以上传的最大大小是: {2}." 
                    ShowCheckBoxes="true" ShowRemoveButtons="False" TableHeaderTemplate="&lt;td nowrap='nowrap'&gt;&lt;/td&gt;&lt;td&gt;文件列表&lt;/td&gt;"
                    UploadingMsg="正在上传。。。">
                    <ValidateOption MaxSizeKB="5120" />
                    <TableStyle  CssClass="btn_2k3"></TableStyle>
                </CuteEditor:UploadAttachments>
            </td>
	        <td align="center" valign="middle" width="5%" height = "100%">
	             <img id="img1" src="../common/images/right.png"/>
	        </td>
            <td style="width:50%;height:98%" valign="top" align="center">
                <table style="width:100%; height:100%;">
                    <tr align="center" style="width:80%; height:20%;">
                        <td>
                            <asp:Button runat="server" ID="SubmitButton" OnClientClick="return submitbutton_click()"
                                Text="上传" OnClick="SubmitButton_Click"  CssClass="btn_rb1" />
                        </td>
                    </tr>
                    <tr align="center" style="width:80%; height:100%;">
                        <td>
                        <div>
                            <asp:ListBox runat="server" ID="ListBoxEvents" Width="95%" Height="200"></asp:ListBox>
                        </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center" style="width:80%">
            <td colspan="3">
                <iframe runat="server" id="fileListFrame" name="fileListFrame" height="300" width="100%" frameborder="0" src="DownFileList.aspx" style="border: 0px solid #cecece;" scrolling="auto"></iframe>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
    //兼容Edge时跳转到新页面
    if (myBrowser() != 'IE') {
        var edgeToUrl = "FileInput.aspx?folder=<%=strFileFolder %>&edit=1";
        window.location.href = edgeToUrl;
    }

    function submitbutton_click() {
        var submitbutton = document.getElementById('<%=SubmitButton.ClientID %>');
        var uploadobj = document.getElementById('<%=Uploader1.ClientID %>');
	    if(!window.filesuploaded)
	    {
		    if(uploadobj.getqueuecount()>0)
		    {
			    uploadobj.startupload();
		    }
		    else
		    {
			    var uploadedcount=parseInt(submitbutton.getAttribute("itemcount"))||0;
			    if(uploadedcount>0)
			    {
				    return true;
			    }
			    alert(document.getElementById("hfPleaseSelectFile").value);
		    }
		    return false;
	    }
	    window.filesuploaded=false;
	    return true;
    }
    function CuteWebUI_AjaxUploader_OnPostback()
    {
	    window.filesuploaded=true;
	    var submitbutton=document.getElementById('<%=SubmitButton.ClientID %>');
        submitbutton.click();
        return false;
    }

    //查询资产图片规则页面
    function QueryAssetsImageName(objType) {
        var url = "../Query/SPQuery.aspx?SP=USP_AM_QRY_UploadImageFileRules&P0=" + objType;
        window.open(url, 'newwindow', 'left=0,top=100,width=' + (screen.availWidth - 10) + ',height=600,scrollbars,resizable=yes,toolbar=no');
    }
</script>
