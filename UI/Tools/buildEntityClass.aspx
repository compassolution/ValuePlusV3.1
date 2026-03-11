<%@ Page Language="C#" AutoEventWireup="true" CodeFile="buildEntityClass.aspx.cs" Inherits="Tools_buildEntityClass" validateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>根据数据库表结构自动生成相关实体类到指定文件</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/fixAreaStyle.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script src="../common/js/tableStyle.js" type="text/javascript"></script>
    
<script language="javascript">

	function setFileName(){
	    document.getElementById('txtFileName').value = 'Entity_'+document.getElementById('txtTableName').value;
	}
</script> 
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label2" runat="server" Text="请输入数据库表名：" ForeColor="Red" Font-Size=Small></asp:Label>
        <asp:TextBox ID="txtTableName" runat="server" Width="250px" Text="TB_HR_USER"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="实体类名：" ForeColor="Red" Font-Size=Small></asp:Label>
        <asp:TextBox ID="txtFileName" runat="server" Width="271px" Text="Entity_TB_HR_USER"  ReadOnly="true"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="命名空间：" ForeColor="Red" Font-Size=Small></asp:Label>
        <asp:TextBox ID="txtNameSpace" runat="server" Width="271px" Text="Com.ValuePlus.Archive.Entity"  ReadOnly="false"></asp:TextBox>
        <a href="ToolsList.aspx" class="a_Right">返回工具列表</a>
        <p>
        </p>
        <asp:Label ID="Label1" runat="server" Text="生成文件路径：" ForeColor="Red" Font-Size=Small></asp:Label>
        <asp:TextBox ID="txtFilePath" runat="server" Width="600px" Text="" ></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="生成实体类" onclick="Button1_Click" />
        <asp:Button ID="Button3" runat="server" Text="DATAROW填充到实体" onclick="Button3_Click" />
        <asp:Button ID="Button2" runat="server" Text="生成config文件内容" onclick="Button2_Click" />
        <p>
        </p>
        <asp:TextBox ID="txtResult" runat="server" Rows="32" Columns="150" TextMode="MultiLine"></asp:TextBox>
    </form>
</body>
</html>