<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExecuteSql.aspx.cs" Inherits="Tools_ExecuteSql" validateRequest="false" enableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>执行sql语句</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        <table class="table" align="center" width="100%">
            <tr>
                <td align=left>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" 
                        OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="File" Selected="True">Execute SQL File</asp:ListItem>
                        <asp:ListItem Value="Sql" >Execute SQL Scripts</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td align =right >
                    <asp:Button ID="Button1" runat="server" Text="Execute" OnClick="Button1_Click" />
                </td>
            </tr>
            <tr id="trFile" runat="server">
                <td colspan="2" width="100%">   
                    <asp:Label ID="Label1" runat="server" Text="Select File：" ForeColor="Red" Font-Size="Small"></asp:Label>
                    <%--<input id="File1" type="file" runat="server" style="width:60%" />--%>
                    <asp:FileUpload ID="File1" runat="server" style="width:60%" onchange="javascript:__doPostBack('previewFile','');"/>
                    <asp:LinkButton ID="previewFile" name = "previewFile" runat="server" OnClick="previewFile_Click" Visible="false">显示SQL语句</asp:LinkButton>  

                </td>
            </tr>
            <tr id="trSql" runat="server">
                <td colspan="2" width="100%"> 
                    <asp:Label ID="Label2" runat="server" Text="Input SQL Scripts：" ForeColor="Red" Font-Size=Small></asp:Label>
                    <asp:TextBox ID="txtSql" runat="server" Rows="20" Width = "100%" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                </td>
            </tr>
            <tr id="trResult">
                <td colspan="2" width="100%">
                    <asp:Label ID="Label3" runat="server" Text="Execute Result:" ForeColor="Red" Font-Size=Small></asp:Label>
                    <asp:TextBox ID="txtResult" runat="server" Rows="5" Width = "100%" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>