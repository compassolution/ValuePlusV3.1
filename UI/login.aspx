<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title>ValuePlus_Login</title>
<style type="text/css">
body {
}
</style>
<script language="javascript">

if(top.location !== self.location) {
    top.location=self.location;
}
</script>
</head>
<link href="common/css/login.css" rel="stylesheet" type="text/css">

<body>
<form id="form1" runat="server">
<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td valign="middle">
		<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
					<tr><td height="20%"></td></tr>
		  <tr height="60%">
			<td width="100%" height="100%" align="right">
				<table width="98%" height="100%" border="0" cellpadding="0" cellspacing="0">
					<br>
					<br>
					<br>
					<br>
					<br>
					<br>
					<tr height="80%"  valign="bottom">
						<td align="right" ><img src="common/images/clientpic/cm_sofitel.jpg" />
						</td>
						<td align=left>
							<table>
								<tr valign=bottom>
									<td  width="96%" align=center >                 
										<img src="common/images/login/logo1.jpg" alt="" width = "40" height = "30"/>
										<span class="login_txt_bt_big" > &nbsp;&nbsp;A|CARE</span>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr height="20%">
						<td height="164" colspan="2" align="middle">
							<label for="account">Account Name<span class="required">*</span></label>
							<asp:TextBox ID="account" runat="server" class="textbox" width='100'></asp:TextBox>
							<label for="password">Password<span class="required">*</span></label>
							<asp:TextBox ID="pwd" runat="server" TextMode="Password" width=100 class="textbox"></asp:TextBox>
							<asp:ImageButton ID="ImageButton1" src="common/images/login/login_btn.gif" runat="server" onclick="ImageButton1_Click" />
							<asp:CheckBox ID="CheckBox1" runat="server" enabled=false checked=false />
							<label for="password">Is Group User?</label>
						</td>
					</tr>
				</table>
			</td>
		  </tr>
		 
          <tr id="tr_DomainArea" runat="server">
               <td colspan="2" align="middle">
                  <table>
			        <tr>
				        <td colspan="2" align="middle">
					        <label for="lb_DomainLogin">Windows Domain & Account:</label>
					        <asp:label runat="server" id="lb_DomainName"></asp:label>
					        <asp:label runat="server" id="lb_DomainAccount"></asp:label>
				        </td>
			        </tr>
			        <tr>
				        <td colspan="2" align="middle">
					        <label for="lb_DomainLogin">You can:  </label>
                            <asp:LinkButton ID="LinkButton1" runat="server">Windows Login</asp:LinkButton>
				        </td>
			        </tr>
                  </table>
               </td>
          </tr>
		</table>
    </td>

  </tr>
</table>
</form>
   
<%--Group Function Addtional--%>
<script src="common/JQuery/jquery-1.10.2.js" type="text/javascript"></script>
<script src="common/Group/jquery.cookies.js" type="text/javascript"></script>
<script src="common/js/MainUtil.js" type="text/javascript"></script>
<script src="common/JS/base64.js" type="text/javascript"></script>
<script type="text/javascript">
    //var varUserId = GetUrlQueryString("userid");
    //base64解密接收【方法Base64.encode】
    var Base64 = new Base64();
    var varIsGroup = GetUrlQueryString("isgroup");
    var varUserId = Base64.decode($.cookie('vp_userid'));
    var varPwd = Base64.decode($.cookie('vp_pwd'));
    //alert(varIsGroup);
    //alert(varUserId);
    //alert($.cookie('vp_pwd'));

    $(document).ready(function () {
        //alert($.cookie('vp_isDoLogin'));
        if (!(varIsGroup == '1')) {
            if ($.cookie('vp_isDoLogin') == '1') {//登录后退出时的情况
                $.cookie('vp_isDoLogin', "0");
                window.close();
            }
            else {
                //如果不是集团跳转的则清空cookie
                $.cookie('vp_redirectUrl', null);
                $.cookie('vp_userid', null);
                $.cookie('vp_pwd', null);
                $.cookie('vp_isDoLogin', null);
            }

        } else {
            //判断如果是集团登录跳转过来的，则直接登录
            if ((varUserId != '') && (varUserId != null) & (varPwd != '') && (varPwd != null)) {
                $("#account").val(varUserId);
                $("#pwd").val(varPwd);

                ////判断login.aspx页面是否点击过登录，预防登录失败后会一直进行重复登录操作，如果登录过并失败了，则关闭此页面
                if ($.cookie('vp_isDoLogin') != "1") {
                    $.cookie('vp_isDoLogin', "1");
                    //alert($.cookie('vp_isDoLogin'));
                    $("#ImageButton1").click();
                } else {
                    $.cookie('vp_isDoLogin', '0');
                    window.close();
                }
            } else {
                $.cookie('vp_isDoLogin', '0');
            }
        }

        window.attachEvent("onbeforeunload", function () {
            //alert('onbeforeunload' + $.cookie('vp_isDoLogin'));
            if ($.cookie('vp_isDoLogin') == '1') {
                $.cookie('vp_isDoLogin', "0");
            }
        });
    });
</script>
<%--Group Function Addtional--%>

</body>
</html>
