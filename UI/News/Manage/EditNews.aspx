<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditNews.aspx.cs" Inherits="News_Manage_EditNews" ValidateRequest="false"  %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新闻内容明细页面</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script> 
<script  src="../../common/js/tableStyle.js"></script>
<script type="text/javascript">  
<!--
    function RefreshList() {
        if (window.opener != null) {
            if (window.opener.document.getElementById("btnRefresh") != null) {//列表页面刷新按钮
                window.opener.document.all["btnRefresh"].click();
            }
        }
        alert("Successfully!");
    }
//-->
</script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr >
            <td valign="top" style="height:auto">
                <table border="0" class="table" width="95%" id="tb1" align="center" class="table">
		          <tr>
		            <td class="edit_label"align = "center" style="width:15%">
		                <asp:Label ID="Label11" runat="server" Text="版块ID"></asp:Label>
		            </td>
		            <td colspan="3" >
		                <asp:Label ID="lbPlateId" runat="server" Text="版块ID"></asp:Label>
		            </td>
		          </tr>
		          <tr>
		            <td class="edit_label"align = "center" style="width:15%">
		                <asp:Label ID="Label3" runat="server" Text="版块名称"></asp:Label>
		            </td>
		            <td colspan="3">
		                <asp:Label ID="lbName" runat="server" Text="版块名称"></asp:Label>
                    </td>
		          </tr>
		          <tr>
		            <td class="edit_label"align = "center">
		                <asp:Label ID="Label4" runat="server" Text="版块描述"></asp:Label>    
		            </td>
		            <td colspan="3">
		                <asp:Label ID="lbDesc" runat="server" Text="版块描述"></asp:Label>
		            </td>
		          </tr>
	            </table>
            </td>
        </tr>
        <tr >
            <td valign="top" style="height:auto">
                <table border="0" class="table" width="95%" id="Table2" align="center" class="table">
		          <tr>
		            <td class="edit_label"align = "center" style="width:15%">
		                <asp:Label ID="Label1" runat="server" Text="新闻标题(中文)"></asp:Label>
		            </td>
		            <td  colspan="3">
                        <asp:TextBox ID="txtTitleCn" runat="server" Width="90%" MaxLength="100"></asp:TextBox><font color="red">*</font>
		            </td>
		          </tr>
		          <tr>
		            <td class="edit_label"align = "center" style="width:15%">
		                <asp:Label ID="Label2" runat="server" Text="新闻标题(英文)"></asp:Label>
		            </td>
		            <td colspan="3">
                        <asp:TextBox ID="txtTitleEn" runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                    </td>
		          </tr>
		          <tr>
		            <td class="edit_label" align = "center" style="width:15%">
		                <asp:Label ID="Label5" runat="server" Text="新闻内容："></asp:Label>
		            </td>
		            <td style="width:35%">
                        <asp:RadioButtonList ID="rdBtnListLanguage" runat="server" RepeatDirection="Horizontal" 
                            OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="cn" Selected="True">中文</asp:ListItem>
                            <asp:ListItem Value="en" >英文</asp:ListItem>
                        </asp:RadioButtonList>                   
		            </td>
		            <td class="edit_label" align = "center" style="width:15%">
		                <asp:Label ID="Label6" runat="server" Text="是否停用"></asp:Label>
		            </td>
		            <td>
		                <asp:DropDownList id="ddListIsStop" runat="server" Width="100px">
					        <asp:ListItem Value="0">正常</asp:ListItem>
					        <asp:ListItem Value="1">停用</asp:ListItem>
				        </asp:DropDownList>                
		            </td>
                      
		          </tr>
	            </table>
            </td>
        </tr>
        <tr align="center" id = "trContentCn" runat="server">
            <TD align ="right" colspan="2" style="height:auto">
                <div style="width:100%; height:100%; text-align:center">
                <FTB:FreeTextBox ID="FreeTextBox_Cn" runat="server" Language="zh-CN"  ToolbarStyleConfiguration="OfficeMac" Width="100%" Height="500"
                    ImageGalleryUrl = "ftb.imagegallery.aspx?rif={0}&cif={0}"  ImageGalleryPath="~/UserFile/News/UploadImg/" 
                    Toolbarlayout="ParagraphMenu, FontFacesMenu, FontSizesMenu, FontForeColorsMenu,
                                FontBackColorsMenu, FontForeColorPicker, FontBackColorPicker| Bold, Italic, Underline,
                                Strikethrough, Superscript, Subscript, RemoveFormat| JustifyLeft, JustifyRight, JustifyCenter,
                                JustifyFull; BulletedList, NumberedList, Indent, Outdent; CreateLink, Unlink| Cut,
                                Copy, Paste, Delete, Undo, Redo, Print, Save| SymbolsMenu, StyleMenu, InsertHtmlMenu| InsertRule,
                                InsertDate, InsertTime| InsertTable, EditTable; InsertTableRowBefore, InsertTableRowAfter,
                                DeleteTableRow; InsertTableColumnBefore, InsertTableColumnAfter, DeleteTableColumn| InsertForm,
                                InsertDiv, InsertTextBox, InsertTextArea, InsertRadioButton, InsertCheckBox, InsertDropDownList,
                                InsertButton| InsertImageFromGallery, Preview, SelectAll, WordClean, EditStyle">
                </FTB:FreeTextBox>
                </div>
            </TD>
        </tr>
        <tr align="center" id = "trContentEn" runat="server">
            <TD align ="right" colspan="2" style="height:auto">
                <div style="width:100%; height:100%; text-align:center">
                <FTB:FreeTextBox ID="FreeTextBox_En" runat="server" Language="zh-CN"  ToolbarStyleConfiguration="OfficeMac" Width="100%" Height="500" 
                    ImageGalleryUrl = "ftb.imagegallery.aspx?rif={0}&cif={0}"  ImageGalleryPath="~/UserFile/News/UploadImg/"
                    Toolbarlayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker|InsertRule,InsertDate,InsertTime|Bold,Italic,Underline,Strikethrough,Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage|InsertImageFromGallery,Preview,SelectAll,WordClean">
                </FTB:FreeTextBox>
                </div>
            </TD>
        </tr>
        <tr >
            <td valign="top" style="height:auto" colspan="2">
                <table border="0" class="table" width="95%" id="Table1" align="center" class="table">
		          <tr>
		            <td class="edit_label"align = "center">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="a_Left" OnClick = "btnSave_Click" >发    布</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="a_Left" OnClientClick="javascript:window.close();">取    消</asp:LinkButton> 
		            </td>
		          </tr>
	            </table>
            </td>
        </tr>
        
                <div id = "divShow" runat="server">
                
                </div>
    </table>
    </form>
</body>
</html>

