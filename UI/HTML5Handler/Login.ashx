<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Security;

public class Login : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        //string param = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//请求类型参数
        //string strUserID = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();//登录名
        //string strPassword = hsTableUrlQuery["password"] == null ? string.Empty : hsTableUrlQuery["password"].ToString();//密码
        //string strRequestLanguage = hsTableUrlQuery["language"] == null ? string.Empty : hsTableUrlQuery["language"].ToString();//终端请求时的语言

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strUserID = context.Request["userid"] == null ? string.Empty : context.Request["userid"].ToString();//登录名
        string strPassword = context.Request["password"] == null ? string.Empty : context.Request["password"].ToString();//密码
        string strNewPassword = context.Request["newpassword"] == null ? string.Empty : context.Request["newpassword"].ToString();//新密码
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strUserID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserID);
        strPassword = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPassword);
        strNewPassword = SQLInjectionDefense.ReplaceSQLReservedKeyword(strNewPassword);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);

        switch (param.ToLower().ToString())
        {
            case "login":
                this.JudgeLogin(context, strUserID, strPassword);
                break;
            case "hrlogin":
                if (strUserID.ToLower().Equals("admin"))
                {
                    this.JudgeLogin(context, strUserID, strPassword);
                }else
                {
                    this.JudgeHRLogin(context, strUserID, strPassword);
                }
                break;
            case "getcustomer":
                this.GetCustomerInfo(context, strRequestLanguage);
                break;
            case "changepassword":
                this.ChangePassword(context, strUserID, strPassword, strNewPassword);
                break;
            case "getuserinfo":
                this.GetUserInfoByUserId(context, strUserID);
                break;
            default:
                this.JudgeLogin(context, strUserID, strPassword);
                break;
        }
    }

    /// <summary>
    /// 获取客户信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strRequestLanguage"></param>
    public void GetCustomerInfo(HttpContext context,String strRequestLanguage)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (strRequestLanguage.Equals("en-us"))
            {
                sbSql.Append("SELECT (select ParamValue from [VW_Moblie_BasicParam] WHERE ParamName = 'WelcomeString_EN') AS 'WelcomeString'");
                sbSql.Append(",(SELECT ParamValue from [VW_Moblie_BasicParam] WHERE ParamName = 'CustomerString_EN') AS 'CustomerString'");
            }
            else
            {
                sbSql.Append("SELECT (select ParamValue from [VW_Moblie_BasicParam] WHERE ParamName = 'WelcomeString_CN') AS 'WelcomeString'");
                sbSql.Append(",(SELECT ParamValue from [VW_Moblie_BasicParam] WHERE ParamName = 'CustomerString_CN') AS 'CustomerString'");
            }
            DataTable dtResultData = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");

            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    #region 验证登录
    /// <summary>
    /// 验证登录
    /// </summary>
    public void JudgeLogin(HttpContext context, String strUserId, String strPwd)
    {
        String strResult = "0";
        try
        {
            UserLoginBll bllLogin = new UserLoginBll();

            //首先判断该用户是否首次登陆修改过密码，是否需要强制修改密码
            /// <returns>0:不需要修改密码</returns>
            /// <returns>1:首次登陆需要修改密码</returns>
            /// <returns>2:密码已过期,请及时修改密码</returns>
            String isNeedChangePassword = bllLogin.JudgeIsNeedChangePassword(strUserId);
            if (!isNeedChangePassword.Equals("0"))
            {
                if (isNeedChangePassword.Equals("1"))
                {
                    strResult = "-99";//首次登陆需要修改密码
                }
                else if (isNeedChangePassword.Equals("2"))
                {
                    strResult = "-98";//密码已过期,请及时修改密码
                }
            }
            else
            {
                UserInfo userInfo = bllLogin.GetUserInfoFromDBByUserID(strUserId);

                if ((userInfo == null) || (String.IsNullOrEmpty(userInfo.SUSERID)))
                {
                    strResult = "-1";//用户名不存在
                    DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_NoUser);
                }
                else if (!userInfo.SPWD.ToLower().Equals(strPwd.ToLower()))
                {
                    strResult = "-2";//密码不正确
                    DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_ErrorPassword);
                }
                else if (userInfo.BISSTOP.Equals("1"))
                {
                    strResult = "-3";//用户被停用
                    DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_NoUser);
                }
                else
                {
                    DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_Success);
                    //写入全局session和cookie
                    userInfo.Language = Com.ValuePlus.Common.Config.BaseConfig.Instance.GetConfigValueByKey("DefaultLanguage");
                    UserLoginBll.LoginUserInfo = userInfo;

                    strResult = userInfo.SDEPTCODE==null?"":userInfo.SDEPTCODE.ToString();//匹配，登录成功，返回部门编码

                    //执行登录成功后的系统操作
                    this.ExecuteAfterLogin(strUserId, strPwd);
                }
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        context.Response.Write(strResult);
    }
    #endregion

    #region HR App验证登录
    /// <summary>
    /// HR App验证登录
    /// </summary>
    public void JudgeHRLogin(HttpContext context, String strUserId, String strPwd)
    {
        String strResult = "0";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from MBUser_1 where DCNO = '"+strUserId+"'");//员工编号
            sbSql.Append(" or DCID = '"+strUserId+"'");//身份证号
            sbSql.Append(" or DCMOBILE = '"+strUserId+"'");//手机号码
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count ==1))
            {
                DataRow dr = dt.Rows[0];

                if ((dr["SUSERID"] != null) && (!String.IsNullOrEmpty(dr["SUSERID"].ToString())))
                {
                    strUserId = dr["SUSERID"].ToString();
                }
                String strDeptCode = dr["DCDDESCCHS"]==null?"":dr["DCDDESCCHS"].ToString();//部门编码

                if (!dr["BIsValid"].Equals("1"))
                {
                    strResult = "-3";//用户被停用
                    DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_NoUser);
                }else
                {
                    if (!dr["SPWD"].Equals(strPwd))
                    {
                        strResult = "-2";//密码不正确
                        DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_ErrorPassword);
                    }else
                    {
                        DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_Success);

                        strResult = "{'UserId':'"+strUserId+"',DeptCode:'"+strDeptCode+"'}";//匹配，登录成功，返回部门编码

                        //执行登录成功后的系统操作
                        this.ExecuteAfterLogin(strUserId, strPwd);
                    }
                }
            }else
            {
                strResult = "-1";//用户名不存在
                DataLogWriter.Log_Login(strUserId, this.GetClientIPAddress(), Com.ValuePlus.DataLog.Enum.LogActionType.Login_NoUser);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        context.Response.Write(strResult);
    }
    #endregion

    /// <summary>
    /// //登录成功后,执行存储过程,目前主要是写入应用主机地址
    /// </summary>
    private void ExecuteAfterLogin(String strUserId, String strPwd)
    {
        String strSpName = "p_tb_userlogin";
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("SACCOUNTID", strUserId);
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
    /// 获取所有用户信息
    /// </summary>
    /// <param name="context"></param>
    public void GetUserInfoByUserId(HttpContext context, String strUserId)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_User] where SUSERID = '" + strUserId + "'");
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserID"></param>
    /// <param name="strOldPassword"></param>
    /// <param name="strNewPassword"></param>
    private void ChangePassword(HttpContext context, String strUserID, String strOldPassword, String strNewPassword)
    {
        String strResult = "0";
        try
        {
            UserManagerBll bllUser = new UserManagerBll();
            int iCount = bllUser.ChangeUserPwd(strNewPassword, strUserID);
            if (iCount > 0)
            {
                //写修改密码记录
                String strSpName = "USP_Sys_RecordPasswordHis";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("UserId", strUserID);
                hsTableParam.Add("OldPwd", strOldPassword);
                hsTableParam.Add("NewPwd", strNewPassword);

                try
                {
                    iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }


            }
            strResult = iCount.ToString();
        }
        catch (Exception ex)
        {
            strResult = "-1";
            log.Error(ex);
            log.Error("修改密码失败");
        }
        context.Response.Write(strResult);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}