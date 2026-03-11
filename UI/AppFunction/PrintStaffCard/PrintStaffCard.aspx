<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintStaffCard.aspx.cs" Inherits="AppFunction_PrintStaffCard_PrintStaffCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>制作员工卡</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../../common/js/waitProcess.js"></script>
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
 
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
<form id="form1" runat="server">
<table border="0" id="tb1" width="80%"  align="center" style="height:auto">
  <tr>
    <td>
        <table border="0" class="table" align="center" style="height:auto;">
            <tr>
                <td>
                    <asp:Label ID="lbTitle" runat="server" Text="员工卡制作页面，制作完成后再进行员工卡的打印"></asp:Label>
                </td>
                <td align="right">
                    <a href="../../UserFile/Doctool/magicard_driver_v2_5_0_300.exe"  target="_blank"><font color="blue">下载打印机驱动</font></a>
                </td>
            </tr>
        </table>
    </td>
  </tr>
  <tr>
    <td > 
        <table border="0" class="table"  align="center" style="height:auto;">
          <tr>
            <td class="edit_label" align = "center" colspan="2">
                <asp:Label ID="lbldcno" runat="server"></asp:Label>：
                <asp:TextBox runat="server" ID="txtdcno"></asp:TextBox>
                <asp:LinkButton ID="aFilter" runat="server" CssClass="a_Left" Font-Bold="true" OnClick="aFilter_Click">定   位</asp:LinkButton>
            </td>
          </tr>
          <tr>
            <td class="left_bt2" style="color:Blue" colspan="2" align=center>
                <asp:Label ID="Label_DocuInfo" runat="server" Text="员工档案库信息"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="edit_label" align = "center" style="width:35%" >
                <asp:Label ID="Label_DCNO" runat="server" Text="员工编号"></asp:Label>
            </td>
            <td><asp:Label ID="lbDCNO" runat="server" Text="员工编号"></asp:Label></td>
          </tr>
          <tr>
            <td class="edit_label"align = "center">
                <asp:Label ID="Label_Name" runat="server" Text="员工英文名"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbName" runat="server" Text="员工英文名"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="edit_label"align = "center">
                <asp:Label ID="Label_NameCn" runat="server" Text="员工中文名"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbNameCn" runat="server" Text="员工中文名"></asp:Label>
            </td>
          </tr>
          
          <tr height="30" align="center">
            <TD align ="center" colspan="2">
            </TD>
          </tr>
          
          <tr id="trNotHavedOp" runat="server">
            <td colspan="2" align="right">
                <font color="red">*<asp:Label ID="lbHaveTip" runat="server" Text="该员工暂不存在员工卡数据，制作请点击"></asp:Label></font>
                <asp:LinkButton ID="aMake" runat="server" CssClass="a_Left" Font-Bold="true" onclick="aMake_Click">制    作</asp:LinkButton>
            </td>
          </tr>
          <tr id="trHaveOp" runat="server">
            <td colspan="2" align="right">
                <font color="red">*<asp:Label ID="lbNotHaveTip" runat="server" Text="该员工已存在员工卡数据，重新制作请点击"></asp:Label></font>
                <asp:LinkButton ID="aReMake" runat="server" CssClass="a_Left" Font-Bold="true" onclick="aReMake_Click">重新制作</asp:LinkButton>
            </td>
          </tr>
          <tr>
            <td colspan="2" align="right">
                <font color="red">*<asp:Label ID="lbMultiTip" runat="server" Text="如果您想批量制作所有员工卡数组，请点击"></asp:Label></font>
                <asp:LinkButton ID="aMultiMake" runat="server" CssClass="a_Left" Font-Bold="true" onclick="aMultiMake_Click">全部批量制作</asp:LinkButton>
            </td>
          </tr>
  
          <tr>
            <td class="edit_label" align="left" colspan="2">
                <asp:TextBox runat="server" ID="txtErrLog" TextMode="MultiLine" Width="100%" Rows="10" Visible="false"></asp:TextBox>
            </td>
          <tr>
        </table>
    </td>
  </tr>
</table>
</form>
</body>
</html>