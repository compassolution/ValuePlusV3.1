<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormFieldInfo.aspx.cs" Inherits="Flow_FormManage_FormFieldInfor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>表单字段字段明细信息</title>
<link href="../../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../../common/css/topStyle.css" rel="stylesheet"  type="text/css" />
<script  src="../../common/js/waitProcess.js"></script>
<script  src="../../common/js/stringUtil.js"></script>
<script language="javascript">
    var varFkeyFlag = 0;
    function showFkeyArea(){
	    if (document.getElementById("cbIsFkey").checked){
		    this.divFKeyArea.style.display = "inline";
		}
	    else{
		    this.divFKeyArea.style.display = "none";
		}
    }
    
    function EnterTableNameTextBox(){
         if(event.keyCode == 13 && document.all["txtFkTable"].value != "")
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["btnLoadCoulmn"].click();
         }
    }
    
</script>
</head>
<body>
<!--#include   file= "../../common/WaitProccess.htm"--> 
 <form id="form1" runat="server">
    <div>
        <table border="0" class="table" width="75%" id="tb1" align="center" style="height:auto">
		  <tr>
		    <td class="left_bt2" style="color:Blue" colspan="4" align=center>
		        <asp:Label ID="Label1" runat="server" Text="表单字段明细信息"></asp:Label>
            </td>
		  </tr>
		  
		  <tr>
		    <td class="edit_label" align = "center" style="width:15%">
		        <asp:Label ID="Label3" runat="server" Text="表单字段编码"></asp:Label>
		    </td>
		    <td colspan="3" style="width:80%" class="tdNoborder">
		        <asp:TextBox ID="TextBox1" runat="server" Width="100" MaxLength="40"></asp:TextBox><font color=red>*</font>
		        <asp:Label ID="Label10" runat="server" Text="是否外键" CssClass="edit_label"></asp:Label><asp:CheckBox ID="cbIsFkey" runat="server"/>
		        <div id="divFKeyArea" style="display:inline" runat="server">
		            <asp:Label ID="Label8" runat="server" Text="关联表名" CssClass="edit_label"></asp:Label>
		            <asp:TextBox ID="txtFkTable" runat="server" Width="200" MaxLength="50"></asp:TextBox>
                    <asp:LinkButton ID="btnLoadCoulmn" runat="server" CssClass="a_Right" onclick="btnLoadCoulmn_Click">加载字段</asp:LinkButton>
		            <asp:DropDownList id="ddListFkField" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddListFkField_Click">
				    </asp:DropDownList>
				</div>
            </td>
		  </tr>
		  
		  <tr>
		    <td class="edit_label"align = "center" style="width:20%">
		        <asp:Label ID="Label4" runat="server" Text="字段英文名"></asp:Label>    
		    </td>
		    <td style="width:30%">
		        <asp:TextBox ID="TextBox2" runat="server" Width="80%" MaxLength="50"></asp:TextBox><font color=red>*</font>
		    </td>
		    <td class="edit_label"align = "center" style="width:20%">
		        <asp:Label ID="Label5" runat="server" Text="字段中文名"></asp:Label>
		    </td>
		    <td style="width:30%">
		        <asp:TextBox ID="TextBox3" runat="server" Width="80%" MaxLength="50"></asp:TextBox><font color=red>*</font>
		    </td>
		  </tr>
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label6" runat="server" Text="是否主键"></asp:Label>
		    </td>
		    <td>
                <asp:CheckBox ID="CheckBox1" runat="server" />
		    </td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label2" runat="server" Text="字段类型"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList1" runat="server" Width="80%">
				</asp:DropDownList>
		    </td>
		  </tr>
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label12" runat="server" Text="字段长度"></asp:Label>    
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox4" runat="server" Width="80%" MaxLength="9"></asp:TextBox><font color=red>*</font>
		    </td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label13" runat="server" Text="表单字段精度"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox5" runat="server" Width="80%" MaxLength="9"></asp:TextBox><font color=red>*</font>
		    </td>
		  </tr>
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label14" runat="server" Text="是否可为空"></asp:Label>
		    </td>
		    <td>
                <asp:CheckBox ID="CheckBox2" runat="server" Checked="true"/>
		    </td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label15" runat="server" Text="字段默认值"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox6" runat="server" Width="80%" MaxLength="50"></asp:TextBox><font color=red>*</font>
		    </td>
		  </tr>
		  
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label20" runat="server" Text="控件宽度"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="TextBox7" runat="server" Width="80%" MaxLength="9"></asp:TextBox><font color=red>*</font>
		    </td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label21" runat="server" Text="控件显示顺序"></asp:Label>
		    </td>
		    <td>
                <asp:TextBox ID="TextBox8" runat="server" Width="80%" MaxLength="9"></asp:TextBox><font color=red>*</font>
		    </td>
		  </tr>	  
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label17" runat="server" Text="控件类型"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList2" runat="server" Width="80%">
				</asp:DropDownList>
		    </td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label16" runat="server" Text="控件权限类型"></asp:Label>
		    </td>
		    <td>
		        <asp:DropDownList id="DropDownList3" runat="server" Width="80%">
					<asp:ListItem Value="001">可编辑</asp:ListItem>
					<asp:ListItem Value="002">只读</asp:ListItem>
					<asp:ListItem Value="003">隐藏</asp:ListItem>
				</asp:DropDownList>
		    </td>
		  </tr>
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label7" runat="server" Text="控件数据集"></asp:Label>
		    </td>
		    <td colspan="3">
		        <asp:TextBox ID="TextBox9" runat="server" Width="90%" MaxLength="1000" TextMode="MultiLine" Rows = "4"></asp:TextBox>
		    </td>
		  </tr>
		  
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label18" runat="server" Text="是否主显示"></asp:Label>
		    </td>
		    <td>
		        <asp:CheckBox ID="CheckBox3" runat="server" />
		    </td>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label19" runat="server" Text="是否必填"></asp:Label>
		    </td>
		    <td>
                <asp:CheckBox ID="CheckBox4" runat="server" />
		    </td>
		  </tr>
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label22" runat="server" Text="提醒说明（英文）"></asp:Label>
		    </td>
		    <td colspan="3">
		        <asp:TextBox ID="TextBox10" runat="server" Width="90%" MaxLength="200" TextMode="MultiLine" Rows = "3"></asp:TextBox>
		    </td>
		  </tr>
		  <tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="Label23" runat="server" Text="提醒说明（中文）"></asp:Label>
		    </td>
		    <td colspan="3">
		        <asp:TextBox ID="TextBox11" runat="server" Width="90%" MaxLength="200" TextMode="MultiLine" Rows = "3"></asp:TextBox>
		    </td>
		  </tr>
		  
		  
		  <tr height="18" align="center">
            <TD align ="center" colspan="4">
                  <asp:Button ID="Button1" runat="server" Text="保    存" width="100px" height="25px" CssClass="btn_2k3" OnClick="Button1_Click"/>
                  <asp:Button ID="Button2" runat="server" Text="返    回" width="100px" height="25px" CssClass="btn_2k3" OnClick="Button2_Click"/>
            </TD>
           </tr>
	    </table>
    </div>
    </form>
</body>
</html>
