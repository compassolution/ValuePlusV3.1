
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingList.aspx.cs" Inherits="Archive_Pending_PendingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Pending List</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" />
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<style type="text/css">
    span
    {
	    font-size:12px;
    }
</style>

<script src="../../common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="../../common/js/tableStyle.js" type="text/javascript"></script>

<script src="../../common/js/waitProcess.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
	function OpenArchiveMain(url){
//	    window.location.href = url;
        window.open(url, 'ArchiveMain', 'left=0,top=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-50) +',scrollbars,resizable=yes,toolbar=no');//最大化打开
	}
	
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"-->
    <form id="Form1" method="post" runat="server">
    <table id="tbMain" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <table  class="table" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="../../common/images/icon/event.gif"></asp:Image>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label1"   runat="server" BackColor="Transparent"> Alert- These tasks need be handled</asp:Label>
                        </td>
                        <td align = "right">
                            <div id="div1"  style=" display:">
                                <asp:LinkButton ID="aRefresh_PendingList" runat="server" CssClass="a_Left" OnClick = "aRefresh_Click" Text = "刷新">
                                </asp:LinkButton>
                                <a id="aClose" style="display:none" href="javascript:window.close();" class="a_Right">close</a>
                            </div>
                        </td>
                    </tr>
                </table>
                <table  class="table" id="changecolor" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                    <td colspan = "2">
                        <br />
                        <asp:DataGrid ID="DataGrid1" runat="server"   BorderWidth="0" Width="100%"
                            CellPadding="0" CellSpacing="0" GridLines="Horizontal" AutoGenerateColumns="False"
                            OnEditCommand="DataGrid1_EditCommand" OnItemCreated="DataGrid1_ItemCreated">
                            <ItemStyle CssClass="tableContent" />
                            <HeaderStyle CssClass="tableTitle"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="TDESCRIPTION" ItemStyle-Width= "30%"
                                    ItemStyle-Height="20" HeaderStyle-Height="20"   SortExpression="TDESCRIPTION"
                                    ReadOnly="True" HeaderText="Document"> 
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="RDESCRIPTION" ItemStyle-Width= "25%"
                                    ItemStyle-Height="20" HeaderStyle-Height="20"   SortExpression="RDESCRIPTION"
                                    ReadOnly="True" HeaderText="Role"> 
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SDESCRIPTION" ItemStyle-Width= "30%"
                                    ItemStyle-Height="20" HeaderStyle-Height="20"   SortExpression="SDESCRIPTION"
                                    ReadOnly="True" HeaderText="Scene"> 
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="TCOUNT" ItemStyle-Width= "5%"
                                    ItemStyle-Height="20" HeaderStyle-Height="20"    
                                    SortExpression="TCOUNT" ReadOnly="True" HeaderText="Count"> 
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Edit" ItemStyle-Width = "5%"
                                    ItemStyle-Height="20" HeaderStyle-Height="20"    >
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ImageUrl="../../common/images/icon/proc.gif" AlternateText="Process"
                                            CommandName="Edit" CausesValidation="false" ID="Imagebutton1"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="TID"    
                                    HeaderText="TID" ReadOnly="True" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SID"    
                                    HeaderText="Scene" ReadOnly="True" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="RID"    
                                    HeaderText="RID" ReadOnly="True" Visible="False"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle HorizontalAlign="Right"   Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    
    <%--加载自动上传lic数据的页面--%>
    <iframe id="uploadLicFrame" name="uploadLicFrame" width="0%" height="0" frameborder="0" src="../../AppFunction/UploadToRemote/License/UploadLicense.html" style="border: 0px solid #cecece;" scrolling="auto"></iframe>

</body>
<script type="text/javascript">
    var screenHeight = screen.availHeight;
    $(document).ready(function () {
        //$("#tbMain").height(screenHeight - 140);
    });
</script>

<script language="javascript" type="text/javascript">
	//初始化结果表格
    DefineTableCssNoCursorOver("changecolor");

    if (window.opener != null) {
        document.getElementById('aClose').style.display = "";
    } else {
        document.getElementById('aClose').style.display = "none";
    }
</script>

</html>
