<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Entity;

public class Login : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());
        string param = WebCommon.GetJsonObjectValue(strParamJson,"param").ToString();//请求类型参数        
        string strAccount = WebCommon.GetJsonObjectValue(strParamJson,"account").ToString();//用户编码
        string strPassword = WebCommon.GetJsonObjectValue(strParamJson,"password").ToString();//登录名
        string strNewPassword = WebCommon.GetJsonObjectValue(strParamJson,"newpassword").ToString();//密码
        string strMobileNo = WebCommon.GetJsonObjectValue(strParamJson,"mobileno").ToString();//手机号码
        string strCulture = WebCommon.GetJsonObjectValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "judgeaccountisvalid"://校验登录账号是否有效
                this.JudgeAccountIsValid(context, strAccount, strRequestLanguage);
                break;
            case "login"://手机端验证登录
                this.JudgeLogin(context, strAccount, strPassword,strRequestLanguage);
                break;
            case "getuserinfobymobileno"://通过手机号码获取用户信息
                this.GetUserInfoByMobileNo(context,strMobileNo,strRequestLanguage);
                break;
            case "getcustomerinfo"://获取项目授权信息
                this.GetCustomerInfo(context, strCulture);
                break;
            case "changepassword"://修改密码
                this.ChangePassword(context, strAccount, strPassword, strNewPassword,strRequestLanguage);
                break;
            case "logout"://退出登录
                this.DoLogout(context, strAccount);
                break;
            default:
                this.JudgeAccountIsValid(context, strAccount, strRequestLanguage);
                break;
        }
    }


    /// <summary>
    /// 获取项目授权信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage">0中文1英文</param>
    public void GetCustomerInfo(HttpContext context,String strCulture)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取项目授权信息";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (strCulture.Equals("1"))
            {
                sbSql.Append("SELECT (select ParamValue from [VW_MB_BasicParam] WHERE ParamName = 'WelcomeString_EN') AS 'WelcomeString'");
                sbSql.Append(",(SELECT ParamValue from [VW_MB_BasicParam] WHERE ParamName = 'CustomerString_EN') AS 'CustomerString'");
            }
            else
            {
                sbSql.Append("SELECT (select ParamValue from [VW_MB_BasicParam] WHERE ParamName = 'WelcomeString_CN') AS 'WelcomeString'");
                sbSql.Append(",(SELECT ParamValue from [VW_MB_BasicParam] WHERE ParamName = 'CustomerString_CN') AS 'CustomerString'");
            }
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg =strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 校验登录账号是否有效
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAccount"></param>
    /// <param name="strRequestLanguage">0中文1英文</param>
    public void JudgeAccountIsValid(HttpContext context,String strAccount,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "用户状态异常" : "The account is invalid";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "校验登录账号是否有效";

        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_MB_User] where SUSERID = '"+strAccount+"' AND DCSTATUS <> '3' AND BISSTOP <> '1'");
            //log.Error(strMethodDesc+"SQL:"+sbSql.ToString());
            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            if (dt != null && dt.Rows.Count >= 1)
            {
                strReturnCode = "1";
                strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "用户状态正常" : "The account is valid";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            //strReturnMsg =strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }



    #region 手机端验证登录
    /// <summary>
    /// 手机端验证登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAccount"></param>
    /// <param name="strPwd"></param>
    /// <param name="strRequestLanguage">0中文1英文</param>
    public void JudgeLogin(HttpContext context, String strAccount, String strPwd,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "手机端验证登录";

        StringBuilder sbResult = new StringBuilder();

        try
        {
            //首先判断该用户是否首次登陆修改过密码，是否需要强制修改密码
            /// <returns>0:不需要修改密码</returns>
            /// <returns>1:首次登陆需要修改密码</returns>
            /// <returns>2:密码已过期,请及时修改密码</returns>
            UserLoginBll bllLogin = new UserLoginBll();
            String isNeedChangePassword = bllLogin.JudgeIsNeedChangePassword(strAccount);
            if (!isNeedChangePassword.Equals("0"))
            {
                if (isNeedChangePassword.Equals("1"))
                {
                    strReturnCode = "-9";//
                    strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "首次登陆需要修改密码" : "You need to change your password for the first login";
                }
                else if (isNeedChangePassword.Equals("2"))
                {
                    strReturnCode = "-9";//
                    strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "密码已过期,请及时修改密码" : "The password has expired. Please change the password in time";
                }
            }
            else
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM [VW_MB_User] A");
                sbSql.Append(" where A.SUSERID = '" + strAccount + "'");
                sbSql.Append(" or ISNULL(DCNO,'') = '" + strAccount + "'");//员工编号
                sbSql.Append(" or ISNULL(DCID,'') = '" + strAccount + "'");//身份证号
                sbSql.Append(" or ISNULL(DCMOBILE,'') = '" + strAccount + "'");//手机号码
                String strSql = sbSql.ToString();
                //log.Error(strMethodDesc+"时SQL语句："+strSql);
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count >= 1))
                {
                    DataRow dr = dt.Rows[0];

                    //add by sammen 20260303 为了匹配YINNG项目 可能存在一个工号分开多个不同账号的情况，此时匹配，工号和用户账号相同的那一条记录
                    if(dt.Rows.Count>1){
                        DataRow[] rows1 = dt.Select("SUSERID = ISNULL(DCNO,'')");
                        if(rows1.Length>0){
                            dr = rows1[0];
                        }
                    }

                    if ((dr["SUSERID"] != null) && (!String.IsNullOrEmpty(dr["SUSERID"].ToString())))
                    {
                        strAccount = dr["SUSERID"].ToString();
                    }
                    if (dr["BISSTOP"].Equals("1"))
                    {
                        strReturnCode = "-3";//
                        strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "用户被停用" : "The account was be stopped";
                        DataLogWriter.Log_Login(strAccount, this.GetClientIPAddress(), LogActionType.Login_NoUser);
                    }
                    else
                    {
                        if (!dr["SPWD"].Equals(strPwd))
                        {
                            strReturnCode = "-2";//
                            strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "密码不正确!" : "The password is wrong!";
                            DataLogWriter.Log_Login(strAccount, this.GetClientIPAddress(), LogActionType.Login_ErrorPassword);
                        }
                        else if (!dr["IsMobileUser"].Equals("1"))
                        {
                            strReturnCode = "-4";//
                            strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "您的账号暂不支持手机登录!" : "The account can not login with mobile!";
                            DataLogWriter.Log_Login(strAccount, this.GetClientIPAddress(), LogActionType.Login_ErrorPassword);
                        }
                        else
                        {
                            DataLogWriter.Log_Login(strAccount, this.GetClientIPAddress(), LogActionType.Login_Success);


                            //清空登录错误次数信息 add by sammen 20150604
                            UserLoginError.ClearLoginErrorLog(strAccount);

                            UserBll bllUser = new UserBll();
                            UserInfo userInfo = bllUser.login(strAccount, strPwd, System.Guid.NewGuid().ToString("N"), Com.ValuePlus.Utils.RequestUtils.GetIP());
                            userInfo.Loginip = Com.ValuePlus.Utils.RequestUtils.GetIP();
                            userInfo.Logintiem = System.DateTime.Now;
                            UserLoginBll.LoginUserInfo = userInfo;

                            strReturnCode = "1";//
                            strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "登录成功!" : "Login successfully!";

                            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));

                            //执行登录成功后的系统操作
                            this.ExecuteAfterLogin(strAccount, strPwd);
                        }
                    }
                }
                else
                {
                    strReturnCode = "-1";
                    strReturnMsg = strRequestLanguage.Equals("zh-cn") ? "用户名不存在!" : "The account is not exists!";
                    DataLogWriter.Log_Login(strAccount, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_NoUser);
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg =strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    #endregion

    /// <summary>
    /// 通过手机号码获取用户信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetUserInfoByMobileNo(HttpContext context, String strMobileNo,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "通过手机号码获取用户信息";

        StringBuilder sbResult = new StringBuilder();

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT top 1 * FROM [VW_MB_User] A where ISNULL(DCMOBILE,'') = '" + strMobileNo + "'");//手机号码
            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc+"时SQL语句："+strSql);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg =strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// //登录成功后,执行存储过程,目前主要是写入应用主机地址
    /// </summary>
    private void ExecuteAfterLogin(String strAccount, String strPwd)
    {
        String strSpName = "p_tb_userlogin";
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("SACCOUNTID", strAccount);
        hsTableParam.Add("pwd", strPwd);
        hsTableParam.Add("guid", Guid.NewGuid().ToString());
        hsTableParam.Add("ip", this.GetClientIPAddress());
        try
        {
            if (!String.IsNullOrEmpty(strSpName))
            {
                SqlParamDao.ExcuteSP(strSpName, hsTableParam);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("登录成功后,执行存储过程失败: SPNAME:" + strSpName);
        }

    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAccount"></param>
    /// <param name="strOldPassword"></param>
    /// <param name="strNewPassword"></param>
    /// <param name="strRequestLanguage"></param>
    private void ChangePassword(HttpContext context, String strAccount, String strOldPassword, String strNewPassword,String strRequestLanguage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "修改密码";
        StringBuilder sbResult = new StringBuilder();
        try
        {
            //首先判断旧密码是否正确            
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM [VW_MB_User] A WHERE SPWD = '"+strOldPassword+"'");
            sbSql.Append(" AND (A.SUSERID = '"+strAccount+"'");
            sbSql.Append(" or ISNULL(DCNO,'') = '"+strAccount+"'");//员工编号
            sbSql.Append(" or ISNULL(DCID,'') = '"+strAccount+"'");//身份证号
            sbSql.Append(" or ISNULL(DCMOBILE,'') = '"+strAccount+"')");//手机号码
            String strSql = sbSql.ToString();
            //log.Error(strMethodDesc+"时SQL语句："+strSql);
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if(dt !=null && dt.Rows.Count>0){
                UserManagerBll bllUser = new UserManagerBll();
                int iCount = bllUser.ChangeUserPwd(strNewPassword, strAccount);
                if (iCount > 0)
                {
                    //写修改密码记录
                    String strSpName = "USP_Sys_RecordPasswordHis";
                    Hashtable hsTableParam = new Hashtable();
                    hsTableParam.Add("UserId", strAccount);
                    hsTableParam.Add("OldPwd", strOldPassword);
                    hsTableParam.Add("NewPwd", strNewPassword);
                    hsTableParam.Add("OPUserIP", Com.ValuePlus.Utils.RequestUtils.GetIP());//操作用户IP

                    try
                    {
                        iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    strReturnCode = "1";
                    strReturnMsg =strMethodDesc+"成功";

                }else{

                    strReturnCode = "-99";
                    strReturnMsg =strMethodDesc+"失败";
                }
            }else{
                strReturnCode = "-2";//
                strReturnMsg = strRequestLanguage.Equals("zh-cn")?"旧密码输入不正确!":"The old password is wrong!";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg =strMethodDesc+"出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void DoLogout(HttpContext context, String strAccount)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            UserBll bllUser = new UserBll();
            int iCount = bllUser.ExitLogin(strAccount);

            //注销日志
            DataLogWriter.Log_LogOut(this.GetUserCode(), Com.ValuePlus.Utils.RequestUtils.GetIP(), Com.ValuePlus.DataLog.Enum.LogActionType.LogOut_Success);
            //清除Cookie同时清除所有session
            UserLoginBll.AbandomSession();

            strReturnCode = "1";
            strReturnMsg = "退出登录成功";
            sbResultData.Append("\"ResultData\":{\"UserCode\":\""+strAccount+"\"}");
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "退出登录出错";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error("退出登录时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}