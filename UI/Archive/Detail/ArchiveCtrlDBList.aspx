
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArchiveCtrlDBList.aspx.cs" Inherits="Archive_Detail_ArchiveCtrlDBList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>数据列表选择查询主页面</title>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script  src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script  src="../../common/js/tableStyle.js" type="text/javascript"></script>
<script src="../../common/js/stringUtil.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

    window.focus();
    function SetCtrlsValue(strResult)
    {
        if(strResult != null && strResult!="")
        {
            var Arr1 = strResult.split("＆");
            for(var i=0;i<Arr1.length;i++)
            {
                var temp1 = Arr1[i];
                var Arr2 = temp1.split("＄");
                if(Arr2.length==2)
                {
                    var ctrlId = Arr2[0];
                    var ctrlValue = Arr2[1];
                    // add by sammen 20220104 在后台将单引号'替换成”，这里替换回来。
                    ctrlValue = ctrlValue.ReplaceAll("”","'");
                    ////如果手输入后无法找到记录，则保持手输入的字符并跳出循环(Add by sammen 20170807) 
                    ///delete by sammen 20240105 如果输入无法找到记录，则涉及字段也都赋值为空
                    //if (ctrlValue == '' && i == 0) {
                    //    break;
                    //}
                    if(window.opener.document.all(ctrlId)!=null)
                    {
                        window.opener.document.all(ctrlId).value = ctrlValue;
                    }
                }
            }
        }
        //兼容Edge浏览器的特殊设置[由于Edge浏览器不支持onpropertychange事件，在图片选择框选择后触发click事件]
        if ('<%=this.strTextCtrlId%>'&& $('#' + '<%=this.strTextCtrlId%>', window.opener.document)) {
            //alert($('#' + '<%=this.strTextCtrlId%>', window.opener.document).val());
            $('#' + '<%=this.strTextCtrlId%>', window.opener.document).click();
        }

        window.close();
    } 
    
     function SetCheckBoxState()
     {
        var dom=document.all;
        var el=event.srcElement;
        if(el.tagName=="INPUT"&&el.type.toLowerCase()=="checkbox")
        {
          for(i=0;i<dom.length;i++)
          {
              if(dom[i].tagName=="INPUT"&&dom[i].type.toLowerCase()=="checkbox")
              {
                dom[i].checked=false;
              }
          }
        }
        el.checked=!el.checked;
     }
     
     function DbClickCheckBox()
     {
        SetCheckBoxState();
        document.getElementById("btnOK").click();
     }
     
     
    function EnterFilterTextBox()
      {
         if(event.keyCode == 13)
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["btnFilter"].click();
         }
    }
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<asp:HiddenField ID="hfKeyValue" runat="server" />
    <table width="95%" border="0" cellpadding="0" cellspacing="0" >
          <!-- 操作行 -->
          <tr height="30" align="left">
            <td align ="center" valign="middle">
                <div class="topBox"style="vertical-align:middle; float:left; margin:auto">
                    <asp:TextBox ID="txtFilter" runat="server" Width="100"></asp:TextBox>
                    
                    <%--<div style="display:none; float:left; margin:auto">
                        <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="Filter_Click">搜   索</asp:LinkButton>
                    </div>--%>
                    <asp:LinkButton ID="btnFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="Filter_Click">搜   索</asp:LinkButton>
                    <asp:LinkButton ID="btnOK" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="OK_Click">确   定</asp:LinkButton>
                    <asp:LinkButton ID="btnClose" runat="server" CssClass="a_Left" Font-Bold="true" OnClientClick = "javascript:window.close();">关   闭</asp:LinkButton>
                </div>
            </td>
          </tr>
          <!-- 结果列表部分 --> 
          <tr>
            <td align="center" valign="top" bgcolor="#F7F8F9">
                <table id="changecolor" class="table" style="width:90%">
                    <tr width="100%">
                        <td style ="width:98%; white-space:nowrap">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true"  Width="100%"
                                AutoGenerateColumns="true"  onHorizontalAlign="Center" onsortcommand="DataGrid1_SortCommand" OnItemDataBound = "DataGrid1_ItemDataBound"  >
                                <HeaderStyle CssClass="tableTitle"></HeaderStyle>
								<ItemStyle CssClass="tableContent"  />
                                <Columns>
									<asp:TemplateColumn HeaderText="Select">
										<HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle> 
									    <ItemStyle HorizontalAlign="Center" ></ItemStyle> 
										<ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server"/>										            
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                 </table>
             </td>
          </tr>
        <tr id = "trPageArea" valign="middle" runat="server">
		    <td align="right" valign="middle" id="td3" runat="server" class="td_Frame2">
		        <div style="vertical-align:middle; float:right" class="bottomBox">
                    <asp:LinkButton ID="aFirstPage" runat="server" CssClass="a_Right" OnClick="FirstPage_Click"><asp:Label ID="Label_FirstPage" runat="server" Font-Bold = "true" Text="|<"></asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aPrePage" runat="server" CssClass="a_Right" OnClick="PrePage_Click"><asp:Label ID="Label_PrePage" runat="server" Font-Bold = "true" Text="<<"></asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aNextPage" runat="server" CssClass="a_Right" OnClick="NextPage_Click"><asp:Label ID="Label_NextPage" runat="server" Font-Bold = "true" Text=">>"></asp:Label></asp:LinkButton>
                    <asp:LinkButton ID="aLastPage" runat="server" CssClass="a_Right" OnClick="LastPage_Click"><asp:Label ID="Label_LastPage" runat="server" Font-Bold = "true" Text=">|"></asp:Label></asp:LinkButton>
		            <asp:Label ID="Label_Page1" runat="server" Text="Label">Total</asp:Label>
		            <font color="red"><asp:Label ID="Label_AllCount" runat="server" Text="Label"></asp:Label></font>
		            ---<asp:Label ID="Label_Page3" runat="server" Text="Label">Page:</asp:Label>
		                <font color="red"><asp:Label ID="Label_CurPage" runat="server" Text="Label"></asp:Label></font>
		            <asp:Label ID="Label_Page4" runat="server" Text="Label">/</asp:Label>
		                <font color="red"><asp:Label ID="Label_AllPage" runat="server" Text="Label"></asp:Label></font>
		            ------
		        </div>
            </td>
	    </tr>
    </table>
</form>
</body>
</html>
