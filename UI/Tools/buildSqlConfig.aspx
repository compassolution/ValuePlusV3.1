<%@ Page Language="C#" AutoEventWireup="true" CodeFile="buildSqlConfig.aspx.cs" Inherits="Tools_buildSqlConfig" validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>根据数据库表结构自动生成配置文件及DAO</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Label ID="Label2" runat="server" 
            Text="请输入数据库表名：" ForeColor="Red" Font-Size=Small></asp:Label>
        <asp:TextBox ID="txtTableName" runat="server" Width="271px" Text="TB_HR_USER" ></asp:TextBox>
        <asp:Label ID="Label3" runat="server" 
            Text="请输入SQL配置文件名：" ForeColor="Red" Font-Size=Small></asp:Label>
        <asp:TextBox ID="txtConfigFile" runat="server" Width="271px" Text="FlowSqlConfig" ></asp:TextBox>
        <a href="ToolsList.aspx" class="a_Right">返回工具列表</a>
    <p>
        <asp:DropDownList ID="dList" runat="server">
            <asp:ListItem>selectAll</asp:ListItem>
            <asp:ListItem>selectByKey</asp:ListItem>
            <asp:ListItem>insert</asp:ListItem>
            <asp:ListItem>updateByKey</asp:ListItem>
            <asp:ListItem>deleteByKey</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="Button1" runat="server" Text="生成config文件内容" onclick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="生成获取config方法" onclick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" Text="生成DAL方法" onclick="Button3_Click" />
        <asp:Button ID="Button4" runat="server" Text="生成实体类" onclick="Button4_Click" />
        <asp:Label ID="Label1" runat="server" 
            Text="提示：select/update/delete生成配置文件的sql语句都是默认根据表的第一个字段来过滤条件" ForeColor="Red" Font-Size=Small></asp:Label>
    </p>
    <asp:TextBox ID="txtResult" runat="server" Rows="32" Columns="150" TextMode="MultiLine"></asp:TextBox>
    </form>
</body>
</html>
