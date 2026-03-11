<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePwd.aspx.cs" Inherits="UserManager_ChangerPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改用户登录密码</title>
<link href="../common/css/main.css" rel="stylesheet" type="text/css" />
<link href="../common/css/topStyle.css" rel="stylesheet"  type="text/css" rev="stylesheet" media="all" />
<link href="../common/css/button.css" type="text/css" rel="stylesheet" /> 
<script  src="../common/js/waitProcess.js"></script>
<script  src="../common/js/stringUtil.js"></script>
<script language="javascript">
    function checkInput(){
        var oldPwd = document.getElementById("txtOldPwd").value;
        var newPwd = document.getElementById("txtNewPwd").value;
        var confirmPwd = document.getElementById("txtNewPwdConfirm").value;
        var oldPwdFromDb = document.getElementById("hfOldPwd").value;
//        alert(oldPwdFromDb);
          
        if(oldPwd!=oldPwdFromDb){
            document.getElementById("txtOldPwd").style.backgroundColor ="Red";
            document.getElementById("txtNewPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwdConfirm").style.backgroundColor ="YellowGreen";
            document.getElementById("txtOldPwd").focus();
            document.getElementById("trErrArea").style.display = "";
            document.getElementById("lbWarning").innerText = document.getElementById("hfWarnTip4").value;
            return false;
        }
        if(!VerifyPwd(newPwd)){
            document.getElementById("txtNewPwd").style.backgroundColor ="Red";
            document.getElementById("txtOldPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwdConfirm").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwd").focus();
            document.getElementById("trErrArea").style.display = "";
            document.getElementById("lbWarning").innerText = document.getElementById("hfWarnTip2").value;
            return false;
        }
        if(newPwd==oldPwdFromDb){
            document.getElementById("txtNewPwd").style.backgroundColor ="Red";
            document.getElementById("txtOldPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwdConfirm").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwd").focus();
            document.getElementById("trErrArea").style.display = "";
            document.getElementById("lbWarning").innerText = document.getElementById("hfWarnTip5").value;
            return false;
        }
        if(!VerifyPwd(confirmPwd)){
            document.getElementById("txtNewPwdConfirm").style.backgroundColor ="Red";
            document.getElementById("txtNewPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtOldPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwdConfirm").focus();
            document.getElementById("trErrArea").style.display = "";
            document.getElementById("lbWarning").innerText = document.getElementById("hfWarnTip2").value;
            return false;
        }
        if(confirmPwd!=newPwd){
            document.getElementById("txtNewPwdConfirm").style.backgroundColor ="Red";
            document.getElementById("txtNewPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtOldPwd").style.backgroundColor ="YellowGreen";
            document.getElementById("txtNewPwdConfirm").focus();
            document.getElementById("trErrArea").style.display = "";
            document.getElementById("lbWarning").innerText = document.getElementById("hfWarnTip3").value;
            return false;
        }
        
        return true;
    }
    
    
    function IsDigit(cCheck) { return (('0'<=cCheck) && (cCheck<='9')); }
    function IsAlpha_Lower(cCheck) { return (('a' <= cCheck) && (cCheck <= 'z'));}
    function IsAlpha_Upper(cCheck) { return (('A' <= cCheck) && (cCheck <= 'Z')); }
    function IsSpecial(cCheck) {
        var containSpecial = "~!@#$%^&*()[]{};:,./<>?_";
        if (containSpecial.indexOf(cCheck) > -1) {
            return true;
        } else {
            return false;
        }
//        var containSpecial = /^[^@\/\'\\\"#$%&\^\*]+$/;    
//        return (containSpecial.test(cCheck) );    
    }   
    function VerifyPwd(pwd)
    {
        var isCan = true;
        var isHaveDigit = false;
        var isHavaAlpha_Lower = false;
        var isHavaAlpha_Upper = false;
        var isHavaSpecial = false;
        if ((pwd.length < document.getElementById("hfMinPasswordLength").value) || (pwd.length > document.getElementById("hfMaxPasswordLength").value)){
            isCan = false;
        }else{
            for (nIndex=0; nIndex<pwd.length; nIndex++)
            {
                cCheck = pwd.charAt(nIndex);

                if (!(IsDigit(cCheck) || IsAlpha_Lower(cCheck) || IsAlpha_Upper(cCheck) || IsSpecial(cCheck)))
                {
                    isCan = false;
                    break;
                }else{
                    if(IsDigit(cCheck)) {isHaveDigit=true; continue;}
                    if(IsAlpha_Lower(cCheck)) {isHavaAlpha_Lower=true; continue;}
                    if (IsAlpha_Upper(cCheck)) { isHavaAlpha_Upper = true; continue; }
                    if (IsSpecial(cCheck)) { isHavaSpecial = true; continue; }
                }
            }
        }
        //判断必须是数字和字母的组合，没有数字或者没有字母都不能修改
        if ((!isHaveDigit) || (!isHavaAlpha_Lower) || (!isHavaAlpha_Upper) || (!isHavaSpecial))
        {
            isCan = false;
        }
        
        return isCan;
    }

</script>
</head>
<body>
<!--#include   file= "../common/WaitProccess.htm"--> 
    <form id="form1" runat="server">
<asp:HiddenField ID="hfOldPwd" runat="server" />
<asp:HiddenField ID="hfMinPasswordLength" runat="server" />
<asp:HiddenField ID="hfMaxPasswordLength" runat="server" />
<asp:HiddenField ID="hfWarnTip1" runat="server" />
<asp:HiddenField ID="hfWarnTip2" runat="server" />
<asp:HiddenField ID="hfWarnTip3" runat="server" />
<asp:HiddenField ID="hfWarnTip4" runat="server" />
<asp:HiddenField ID="hfWarnTip5" runat="server" />

    <table border="0" class="table" id="tb1" align="center" style="height:auto;width:70%">
		<tr style="width:50%;" >
		    <td class="left_ts" colspan="2" align="left">
                <img src="../common/images/welcome/ts.gif" width="22" height="22">
		        <asp:Label ID="lbTips" runat="server" Text="" Font-Size="Large" ForeColor="Red"></asp:Label>
            </td>
		</tr>
		<%--<tr style="width:50%;" >
		    <td class="left_bt2" style="color:Blue" colspan="2" align="center">
		        <asp:Label ID="lbChangePassword" runat="server" Text="修改当前用户登录密码"></asp:Label>
            </td>
		</tr>--%>
		<tr>
		    <td class="edit_label" align = "center" style="width:35%">
		        <asp:Label ID="lbLoginName" runat="server" Text="登录名"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtLoginName" runat="server" Width="30%" MaxLength="16" ReadOnly =true></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="edit_label" align = "center" style="width:35%">
		        <asp:Label ID="lbOldPwd" runat="server" Text="原密码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtOldPwd" runat="server" Width="30%" MaxLength="16" TextMode="Password" BackColor="YellowGreen"></asp:TextBox><font color="red">*</font>
            </td>
		</tr>
		<tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="lbNewPwd" runat="server" Text="新密码"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtNewPwd" runat="server" Width="30%" MaxLength="16"  TextMode="Password" BackColor="YellowGreen"></asp:TextBox><font color="red">*</font>
		        <asp:Label ID="lbPwdTips1" runat="server" Text="" CssClass="left_ts"></asp:Label>
            </td>
		</tr>
		<tr>
		    <td class="edit_label"align = "center">
		        <asp:Label ID="lbNewPwdConfirm" runat="server" Text="新密码确认"></asp:Label>
		    </td>
		    <td>
		        <asp:TextBox ID="txtNewPwdConfirm" runat="server" Width="30%" MaxLength="16"  TextMode="Password" BackColor="YellowGreen"></asp:TextBox><font color="red">*</font>
		        <asp:Label ID="lbPwdTips2" runat="server" Text="" CssClass="left_ts"></asp:Label>
            </td>
		</tr>
		<tr id ="trErrArea" style="display:none">
		    <td class="edit_label"align = "center" colspan="2">
		        <font color="red"><asp:Label ID="lbWarning" runat="server" Text="温馨提示" ></asp:Label></font>
		    </td>
		    </tr>
		    <tr height="18" align="center">
            <TD align ="center" colspan="2">
                    <asp:LinkButton ID="btnChangePwd" runat="server" Text="修改密码" width="100px" 
                        height="25px" CssClass="a_Right" OnClick="btnChangePwd_Click" />
                    <asp:LinkButton ID="btnReset" runat="server" Text="重    置" width="100px" height="25px" CssClass="a_Right"   />
                    <asp:LinkButton ID="btnToLoginPage" runat="server" CssClass="a_Right" OnClick="btnToLoginPage_Click">重新登录</asp:LinkButton>
            </TD>
        </tr>
	</table>

    </form>
</body>
</html>
