<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetUserRole.aspx.cs" Inherits="UserManager_SetUserRole" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<title>用户角色分配页面</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<link href="../common/css/topStyle.css" type="text/css" rel="stylesheet" /> 

<script  src="../common/js/waitProcess.js"></script>
</head>
<script language="javascript">
    function dbClickLB1()
    {
         var addOption=document.createElement("option");
         var index1;
         if(document.form1.ListBox1.length==0) return(false);
         index1=document.form1.ListBox1.selectedIndex; 
         if(index1<0)return(false);
         addOption.text=document.form1.ListBox1.options(index1).text;
         addOption.value=document.form1.ListBox1.value;
         document.form1.ListBox2.add(addOption);
         document.form1.ListBox1.remove (index1);
     }
     
     function dbClickLB2()
    {
         var addOption=document.createElement("option");
         var index1;
         if(document.form1.ListBox2.length==0) return(false);
         index1=document.form1.ListBox2.selectedIndex; 
         if(index1<0)return(false);
         addOption.text=document.form1.ListBox2.options(index1).text;
         addOption.value=document.form1.ListBox2.value;
         document.form1.ListBox1.add(addOption);
         document.form1.ListBox2.remove (index1);
     }
     
     function clickImg1()
    {
	     var objList = document.getElementById("ListBox1");
         var lbLength = objList.options.length;
         if(lbLength==0) return(false);
         for(var index1=0;index1<lbLength;index1++){               
             document.getElementById("ListBox2").options.add(new Option(objList.options[index1].text,objList.options[index1].value));
         }
	     for(var index1=lbLength-1;index1>=0;index1--){            
            objList.options.remove(index1);
         }
     }
     
     function clickImg2()
    {
         var objList = document.getElementById("ListBox2");
         var lbLength = objList.options.length;
         if(lbLength==0) return(false);
         for(var index1=0;index1<lbLength;index1++){               
             document.getElementById("ListBox1").options.add(new Option(objList.options[index1].text,objList.options[index1].value));
         }
	     for(var index1=lbLength-1;index1>=0;index1--){            
            objList.options.remove(index1);
         }
     }
     
     
     function GetListBoxValue() {
         var strlist = document.getElementById("ListBox2");//获取Listbox
         var str= "";
         //遍历Listbox，取得选中项的值
         if (strlist.options.length > 0) {
             for (var i = 0; i < strlist.options.length; i++) 
             {
                 var j = strlist.options[i].value;
                 str+=j+"#"; //把Value值串起来

             }
           var strValue=str.replace(/#$/, ""); //去掉最后一个#
           document.getElementById("hidTidRid").value = strValue;
         }
         else {
           document.getElementById("hidTidRid").value = str;
         }
     }
     
    function EnterSearchTextBox()
    {
         if(event.keyCode == 13)
         {
             event.keyCode = 9;
             event.returnValue = false;
             document.all["LinkButton1"].click();
         }
     }
     function openCopyPage() {
         var url = 'CopyUserRole.aspx?userId=' + document.getElementById("hfUserId").value + '&opType=2'
         window.open(url, 'copyUser', 'width=520,height=500,top=100,left=350, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=no,location=no, status=no');
     }
</script>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 

<form id="form1" runat="server">
<asp:HiddenField ID="hfUserId" runat ="server" />
<asp:HiddenField ID="hidTidRid" runat ="server" />
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout:fixed;" >
    <tr>
	    <td width="100%" height="100%" colspan="2">
		    <table width="100%" style="height:100%" cellpadding="0" cellspacing="0" border="0" class="table">
    			<tr height="30" align="center">
	              <TD align ="left" colspan="1">
                    <asp:Label ID="Label3" runat="server" Text="UserId:" class="left_bt2" style="color:red"></asp:Label>
                    <asp:Label ID="lbUserId" runat="server" Text="UserId" class="left_bt2" style="color:red"></asp:Label>
	              </TD>
	              <TD align ="center" colspan="3">
                    <asp:LinkButton ID="Button1" runat="server" CssClass="a_Center" OnClientClick = "GetListBoxValue();" OnClick="Button1_Click">分配角色</asp:LinkButton>
                    <asp:LinkButton ID="Button2" runat="server" CssClass="a_Center" OnClientClick = "openCopyPage();">Copy</asp:LinkButton>
	              </TD>
                </tr>
			    <tr>
				  <td valign="top" align="center" width="45%" height = "100%">
                    <asp:Label ID="Label1" runat="server" Text="温馨提示" class="left_bt2" style="color:red"></asp:Label>
                      <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:LinkButton ID="LinkButton1" runat="server" CssClass="a_Center" OnClick="Button2_Click">search</asp:LinkButton>
		            <asp:ListBox ID="ListBox1" runat="server"  Width="100%" Height="400px" BackColor="#ccffff" AutoPostBack="false" SelectionMode="Multiple">
					</asp:ListBox>
				  </td>
				  <td align="center" width="5%" height = "100%">
				     <img id="img1" src="../common/images/right.png"  style= "cursor:hand " alt="select All left to right" onclick="clickImg1();" />
				     <br></br>
				     <img id="img2" src="../common/images/left.png"  style= "cursor:hand " alt="select All right to left" onclick="clickImg2();" />
				  </td>
				  <td valign="top" align="center" width="45%" height = "100%">
                    <asp:Label ID="Label2" runat="server" Text="温馨提示" class="left_bt2" style="color:red"></asp:Label>
				    <asp:ListBox ID="ListBox2" runat="server"  Width="100%" Height="400px" BackColor="#ccffff" AutoPostBack="false">
					</asp:ListBox>
				  </td>
			    </tr>
			    <tr height="18" align="center">
	              <TD align ="left" colspan="3">
                          
	              </TD>
                </tr>
		    </table>
	    </td>
    </tr>
    </table>
    </form>
</body>
</html>
