<%@ Page Language="C#" AutoEventWireup="true" CodeFile="crypt.aspx.cs" Inherits="crypt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="ToolsList.aspx" class="a_Right">返回工具列表</a>
    
    </div>
    串：<asp:TextBox ID="TextBox1" runat="server" Width="694px"></asp:TextBox>
&nbsp;<br />
    结果：<asp:TextBox ID="TextBox2" runat="server" Width="678px"></asp:TextBox>
    <p>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="加密" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="解密" />
    </p>
    <p>
        &nbsp;</p>
        <p>
       <asp:DropDownList ID="encodingdrop" runat="server"><asp:ListItem Value="UTF-8">utf-8</asp:ListItem><asp:ListItem Value="GB2312">gb2312</asp:ListItem></asp:DropDownList></p>
    <p>
        密钥：<asp:TextBox ID="TextBox3" runat="server" Width="738px"></asp:TextBox>
    </p>
    <p>
        向量：<asp:TextBox ID="TextBox4" runat="server" Width="738px"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="加密" />
        <asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="解密" />
    </p>
    </form>
</body>
</html>
