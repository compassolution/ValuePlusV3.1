<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Server.aspx.cs" Inherits="Server" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Server Information</title>
<link href="common/css/main.css" rel="stylesheet" type="text/css" />
<link href="common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
</head>
<body>
    <form id="form1" runat="server">
        <table class="table" align="center" width="100%">
            <tr id="trFile" runat="server">
                <td colspan="2" width="100%"> 
                    <asp:Label ID="Label2" runat="server" Text="Server Code：" ForeColor="Red" Font-Size=Small></asp:Label>
                    <asp:TextBox ID="txtServerCode" runat="server" Rows="2" Width = "100%" TextMode="MultiLine" Wrap="true" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
